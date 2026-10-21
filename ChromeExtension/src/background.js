import { initializeApp } from "firebase/app";
import { getAuth, signInAnonymously, signOut } from "firebase/auth/web-extension";
import { collection, doc, getDoc, getDocs, getFirestore, query, runTransaction, serverTimestamp, where } from "firebase/firestore";
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

async function signOutUser() {
  await signOut(auth);
  return { signedOut: true };
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

async function loadAnnouncements() {
  const snapshot = await getDocs(query(collection(db, "announcements"), where("isActive", "==", true)));
  return snapshot.docs
    .map((item) => {
      const data = item.data() || {};
      const publishedAt = data.publishedAt && typeof data.publishedAt.toMillis === "function"
        ? data.publishedAt.toMillis()
        : Number(data.publishedAt || 0);
      return {
        id: item.id,
        title: String(data.title || ""),
        body: String(data.body || ""),
        language: String(data.language || "all").toLowerCase(),
        isActive: data.isActive === true,
        publishedAt: Number.isFinite(publishedAt) ? publishedAt : 0
      };
    })
    .sort((a, b) => b.publishedAt - a.publishedAt);
}

async function loadUpdateInfo() {
  const snapshot = await getDoc(doc(db, "appConfig", "update"));
  if (!snapshot.exists()) return { available: false };

  const data = snapshot.data() || {};
  return {
    available: data.isActive === true,
    isActive: data.isActive === true,
    version: String(data.version || ""),
    message: String(data.message || ""),
    messageKo: String(data.messageKo || ""),
    messageEn: String(data.messageEn || ""),
    downloadUrl: String(data.downloadUrl || ""),
    forceUpdate: data.forceUpdate === true
  };
}

chrome.runtime.onMessage.addListener((message, sender, sendResponse) => {
  if (message?.target === "offscreen") return false;
  if (message?.type !== "firebase-auth") return false;

  const task = message.action === "guest"
    ? signInGuest()
    : message.action === "google"
      ? signInGoogle()
      : message.action === "logout"
        ? signOutUser()
        : message.action === "announcements"
          ? loadAnnouncements().then((items) => ({ items }))
        : message.action === "update"
          ? loadUpdateInfo()
        : reserveNickname(message.nickname, message.oldNickname, message.uid).then((available) => ({ available }));
  task.then(sendResponse).catch((error) => {
    sendResponse({ error: error.code || error.message || "auth/unknown" });
  });
  return true;
});
