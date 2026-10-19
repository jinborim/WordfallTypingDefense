import { initializeApp } from "firebase/app";
import { getAuth, signInAnonymously } from "firebase/auth/web-extension";
import { firebaseConfig } from "./firebase-config.js";

const app = initializeApp(firebaseConfig);
const auth = getAuth(app);
const OFFSCREEN_PATH = "offscreen.html";
let creatingOffscreen;

async function hasOffscreenDocument() {
  const clients = await self.clients.matchAll();
  return clients.some((client) => client.url === chrome.runtime.getURL(OFFSCREEN_PATH));
}

async function ensureOffscreenDocument() {
  if (await hasOffscreenDocument()) return;
  if (!creatingOffscreen) {
    creatingOffscreen = chrome.offscreen.createDocument({
      url: OFFSCREEN_PATH,
      reasons: ["IFRAME_SCRIPTING"],
      justification: "Firebase Google authentication"
    });
  }
  await creatingOffscreen;
  creatingOffscreen = null;
}

async function signInGuest() {
  const credential = await signInAnonymously(auth);
  return { provider: "guest", uid: credential.user.uid, displayName: "" };
}

async function signInGoogle() {
  await ensureOffscreenDocument();
  return await chrome.runtime.sendMessage({ target: "offscreen", action: "google" });
}

chrome.runtime.onMessage.addListener((message, sender, sendResponse) => {
  if (message?.target === "offscreen") return false;
  if (message?.type !== "firebase-auth") return false;

  const task = message.action === "guest" ? signInGuest() : signInGoogle();
  task.then(sendResponse).catch((error) => {
    sendResponse({ error: error.code || error.message || "auth/unknown" });
  });
  return true;
});
