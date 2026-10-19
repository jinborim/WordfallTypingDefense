import { build } from "esbuild";
import { cp, mkdir, readFile, writeFile } from "node:fs/promises";
import { existsSync } from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

const root = path.dirname(fileURLToPath(import.meta.url));
const gameSource = process.argv[2] ? path.resolve(process.argv[2]) : null;
const gameTarget = path.join(root, "game");

await mkdir(root, { recursive: true });
await build({ entryPoints: [path.join(root, "src/background.js")], bundle: true, format: "esm", platform: "browser", outfile: path.join(root, "background.js") });
await build({ entryPoints: [path.join(root, "src/offscreen.js")], bundle: true, format: "iife", platform: "browser", outfile: path.join(root, "offscreen.js") });
if (gameSource && existsSync(gameSource)) {
  await cp(gameSource, gameTarget, { recursive: true, force: true });
  const indexPath = path.join(gameTarget, "index.html");
  let index = await readFile(indexPath, "utf8");
  if (!index.includes("firebase-bridge.js")) {
    index = index.replace(/<head>/i, "<head>\n<script src=\"../firebase-bridge.js\"></script>");
    await writeFile(indexPath, index, "utf8");
  }
}

console.log("Wordfall Chrome extension files built.");
