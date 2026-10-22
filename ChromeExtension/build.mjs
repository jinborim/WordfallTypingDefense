import { build } from "esbuild";
import { cp, mkdir, readFile, writeFile } from "node:fs/promises";
import { existsSync } from "node:fs";
import { brotliDecompressSync } from "node:zlib";
import path from "node:path";
import { fileURLToPath } from "node:url";

const root = path.dirname(fileURLToPath(import.meta.url));
const gameSource = process.argv[2] ? path.resolve(process.argv[2]) : null;
const gameTarget = path.join(root, "game");
const firebaseConfigPath = path.join(root, "src/firebase-config.js");

if (!existsSync(firebaseConfigPath)) {
  throw new Error("Missing ChromeExtension/src/firebase-config.js. Copy firebase-config.example.js to firebase-config.js and fill in your Firebase web config locally.");
}

await mkdir(root, { recursive: true });
await build({ entryPoints: [path.join(root, "src/background.js")], bundle: true, format: "esm", platform: "browser", outfile: path.join(root, "background.js") });
await build({ entryPoints: [path.join(root, "src/offscreen.js")], bundle: true, format: "iife", platform: "browser", outfile: path.join(root, "offscreen.js") });
if (gameSource && existsSync(gameSource)) {
  await cp(gameSource, gameTarget, { recursive: true, force: true });
  const indexPath = path.join(gameTarget, "index.html");
  const templateDataPath = path.join(gameTarget, "TemplateData");
  const loadingAssetPath = path.join(root, "assets/wordfall-loading-background.png");
  if (existsSync(loadingAssetPath)) {
    await cp(loadingAssetPath, path.join(templateDataPath, "wordfall-loading-background.png"), { force: true });
  }
  const logoAssetPath = path.join(root, "assets/wordfall-logo-en.png");
  const fontAssetPath = path.join(root, "assets/Mulmaru.ttf");
  if (existsSync(logoAssetPath)) {
    await cp(logoAssetPath, path.join(templateDataPath, "wordfall-logo-en.png"), { force: true });
  }
  if (existsSync(fontAssetPath)) {
    await cp(fontAssetPath, path.join(templateDataPath, "Mulmaru.ttf"), { force: true });
  }
  let index = await readFile(indexPath, "utf8");

  // Chrome extension URLs do not provide the HTTP Content-Encoding header
  // that Unity's .br files require. Decompress the build files locally and
  // point the generated loader at the uncompressed versions.
  const buildPath = path.join(gameTarget, "Build");
  for (const fileName of ["WebGLBuild.data.br", "WebGLBuild.framework.js.br", "WebGLBuild.wasm.br"]) {
    const compressedPath = path.join(buildPath, fileName);
    if (existsSync(compressedPath)) {
      const outputPath = path.join(buildPath, fileName.slice(0, -3));
      await writeFile(outputPath, brotliDecompressSync(await readFile(compressedPath)));
    }
  }
  index = index.replace(/\.br\b/g, "");

  if (!index.includes("firebase-bridge.js")) {
    index = index.replace(/<head>/i, "<head>\n<script src=\"../firebase-bridge.js\"></script>");
  }

  // Chrome extension pages block inline scripts under Manifest V3 CSP.
  // Unity's generated WebGL page contains one inline bootstrap script, so
  // move it to a separate file before loading the game in the extension.
  const inlineScript = index.match(/<script>\s*([\s\S]*?)\s*<\/script>/i);
  if (inlineScript) {
    const bootstrap = inlineScript[1]
      .replaceAll("unity-loading-bar", "wordfall-loading-screen")
      .replace("document.querySelector(\"#wordfall-loading-screen\").style.display = \"block\";", "document.querySelector(\"#wordfall-loading-screen\").style.display = \"flex\";")
      .replace(
        "document.querySelector(\"#unity-progress-bar-full\").style.width = 100 * progress + \"%\";",
        "const progressBar = document.querySelector(\"#unity-progress-bar-full\"); if (!progressBar.dataset.wordfallAnimated) { progressBar.dataset.wordfallAnimated = \"true\"; progressBar.style.width = \"0%\"; const animation = progressBar.animate([{ width: \"0%\" }, { width: \"100%\" }], { duration: 900, easing: \"ease-out\", fill: \"forwards\" }); animation.onfinish = () => { progressBar.style.width = \"100%\"; }; }"
      );
    await writeFile(path.join(gameTarget, "unity-bootstrap.js"), bootstrap, "utf8");
    index = index.replace(inlineScript[0], '<script src="unity-bootstrap.js"></script>');
  }

  index = index.replace(
    /\s*<div id="unity-loading-bar">[\s\S]*?<\/div>\s*<div id="unity-warning">/i,
    `
      <div id="wordfall-loading-screen">
        <div class="wordfall-loading-card">
          <img class="wordfall-loading-logo" src="TemplateData/wordfall-logo-en.png" alt="WORDFALL">
          <div class="wordfall-loading-subtitle">픽셀 타이핑 디펜스</div>
          <div class="wordfall-loading-progress-track">
            <div id="unity-progress-bar-full"></div>
          </div>
          <div class="wordfall-loading-status">게임 준비 중...</div>
        </div>
      </div>
      <div id="unity-warning">`
  );

  const stylePath = path.join(templateDataPath, "style.css");
  let style = await readFile(stylePath, "utf8");
  style += `

/* Wordfall custom WebGL loading screen */
html, body { width: 100%; height: 100%; overflow: hidden; background: #050b1c; }
#unity-container.unity-desktop { width: 960px; height: 600px; }
#wordfall-loading-screen {
  position: fixed; inset: 0; z-index: 20; display: flex; align-items: center; justify-content: center;
  background: #050b1c url('wordfall-loading-background.png') center / cover no-repeat;
  color: #eaf7ff; font-family: Consolas, 'Courier New', monospace;
}
.wordfall-loading-card {
  width: min(520px, 78vw); padding: 34px 42px 30px; text-align: center;
  background: rgba(3, 14, 37, .88); border: 3px solid #18d9ff;
  box-shadow: 0 0 0 3px #07234b, 0 0 32px rgba(24, 217, 255, .5);
}
@font-face { font-family: 'WordfallPixel'; src: url('Mulmaru.ttf') format('truetype'); font-display: swap; }
.wordfall-loading-logo { display: block; width: min(380px, 70vw); height: auto; margin: 0 auto 14px; image-rendering: pixelated; }
.wordfall-loading-subtitle { margin-top: 10px; color: #9edcff; font-family: 'WordfallPixel', monospace; font-size: 17px; letter-spacing: 2px; }
.wordfall-loading-progress-track { position: relative; height: 18px; margin: 28px 0 12px; padding: 3px; overflow: hidden; background: #020817; border: 2px solid #f5c84b; }
#unity-progress-bar-full { position: relative; z-index: 1; width: 0%; height: 100%; background: #08bfff; box-shadow: 0 0 12px #19dfff; }
#unity-progress-bar-full.wordfall-loading-progress-active { animation: wordfall-loading-fill .75s ease-out forwards; }
.wordfall-loading-progress-track::after { content: ''; position: absolute; z-index: 2; top: 3px; bottom: 3px; left: -28%; width: 22%; background: rgba(255,255,255,.58); filter: blur(3px); animation: wordfall-loading-glint 1s linear infinite; pointer-events: none; }
@keyframes wordfall-loading-fill { from { width: 0%; } to { width: 100%; } }
@keyframes wordfall-loading-glint { from { transform: translateX(0); } to { transform: translateX(590%); } }
.wordfall-loading-status { color: #f5c84b; font-family: 'WordfallPixel', monospace; font-size: 15px; letter-spacing: 1px; }
#unity-container.unity-desktop { left: 50%; top: 50%; transform: translate(-50%, -50%); width: 960px; height: 600px; }
#unity-canvas { width: 960px !important; height: 600px !important; }
#wordfall-loading-screen { position: absolute; inset: 0; }
#unity-logo, #unity-logo-title-footer, #unity-build-title { display: none; }
#unity-container { overflow: visible; }
#unity-footer { position: absolute; left: 0; top: calc(100% + 8px); width: 100%; height: 38px; z-index: 3; }
#unity-fullscreen-button { float: right; }
`;
  await writeFile(stylePath, style, "utf8");
  await writeFile(indexPath, index, "utf8");
}

console.log("Wordfall Chrome extension files built.");
