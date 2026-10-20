import { initializeApp } from "firebase/app";
import { getAuth, signInAnonymously } from "firebase/auth/web-extension";
import { doc, getFirestore, runTransaction, serverTimestamp } from "firebase/firestore";
import { firebaseConfig } from "./firebase-config.js";

const app = initializeApp(firebaseConfig);
const auth = getAuth(app);
const db = getFirestore(app);
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

function nicknameDocumentId(nickname) {
  return encodeURIComponent(nickname.trim().toLowerCase());
}

async function reserveNickname(nickname, oldNickname, uid) {
  const normalized = nickname.trim().toLowerCase();
  const ownerUid = uid || auth.currentUser?.uid;
  if (!normalized || !ownerUid) throw new Error("auth/missing-user");
  const newRef = doc(db, "nicknames", nicknameDocumentId(normalized));
  const oldNormalized = (oldNickname || "").trim().toLowerCase();
  const oldRef = oldNormalized ? doc(db, "nicknames", nicknameDocumentId(oldNormalized)) : null;
  return await runTransaction(db, async (transaction) => {
    const current = await transaction.get(newRef);
    const oldSnapshot = oldRef && oldRef.path !== newRef.path ? await transaction.get(oldRef) : null;
    if (current.exists() && current.data().uid !== ownerUid) return false;
    transaction.set(newRef, { uid: ownerUid, nickname: nickname.trim(), updatedAt: serverTimestamp() }, { merge: true });
    if (oldSnapshot && oldSnapshot.exists() && oldSnapshot.data().uid === ownerUid) transaction.delete(oldRef);
    return true;
  });
}

chrome.runtime.onMessage.addListener((message, sender, sendResponse) => {
  if (message?.target === "offscreen") return false;
  if (message?.type !== "firebase-auth") return false;

  const task = message.action === "guest"
    ? signInGuest()
    : message.action === "google"
      ? signInGoogle()
      : reserveNickname(message.nickname, message.oldNickname, message.uid).then((available) => ({ available }));
  task.then(sendResponse).catch((error) => {
    sendResponse({ error: error.code || error.message || "auth/unknown" });
  });
  return true;
});
