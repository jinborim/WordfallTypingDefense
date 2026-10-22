import { initializeApp } from "firebase/app";
import { getAuth, GoogleAuthProvider, signInAnonymously, signInWithCredential, signOut, deleteUser } from "firebase/auth/web-extension";
import { collection, doc, getDoc, getDocs, getFirestore, query, where } from "firebase/firestore";
import { firebaseConfig } from "./firebase-config.js";

const app = initializeApp(firebaseConfig);
const auth = getAuth(app);
const db = getFirestore(app);
const OFFSCREEN_PATH = "offscreen.html";
let creatingOffscreen;

// Unity WebGL is more reliable in a full extension tab than in the small action popup.
// Clicking the pinned Wordfall icon now opens the game page directly.
chrome.action.onClicked.addListener(() => {
  chrome.tabs.create({ url: chrome.runtime.getURL("game/index.html") });
});

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
  const result = await chrome.runtime.sendMessage({ target: "offscreen", action: "google" });
  if (!result || result.error) throw new Error(result?.error || "auth/google-sign-in-failed");
  if (!result.googleIdToken && !result.googleAccessToken) {
    throw new Error("auth/missing-google-credential");
  }

  const credential = GoogleAuthProvider.credential(
    result.googleIdToken || null,
    result.googleAccessToken || null
  );
  const signedIn = await signInWithCredential(auth, credential);
  await chrome.storage.local.set({
    wordfallGoogleIdToken: result.googleIdToken || "",
    wordfallGoogleAccessToken: result.googleAccessToken || ""
  });
  return {
    provider: "google",
    uid: signedIn.user.uid,
    displayName: signedIn.user.displayName || result.displayName || "",
    email: signedIn.user.email || result.email || ""
  };
}

async function signOutUser() {
  await signOut(auth);
  await chrome.storage.local.remove(["wordfallGoogleIdToken", "wordfallGoogleAccessToken"]);
  return { signedOut: true };
}

async function deleteAccount(nickname) {
  const user = await ensureAuthenticatedUser();
  if (!user) throw new Error("auth/missing-user");

  const normalized = String(nickname || "").trim().toLowerCase();
  if (normalized) {
    const nicknameId = nicknameDocumentId(normalized);
    const document = await firestoreRestRequest(`/nicknames/${nicknameId}`);
    const ownerUid = document?.fields?.uid?.stringValue || "";
    if (document && ownerUid === user.uid) {
      await firestoreRestRequest(":commit", {
        method: "POST",
        body: JSON.stringify({
          writes: [{
            delete: `projects/${firebaseConfig.projectId}/databases/(default)/documents/nicknames/${nicknameId}`,
            currentDocument: { exists: true }
          }]
        })
      });
    }
  }

  await deleteUser(user);
  await chrome.storage.local.remove(["wordfallGoogleIdToken", "wordfallGoogleAccessToken"]);
  return { deleted: true };
}

async function ensureAuthenticatedUser() {
  if (auth.currentUser) return auth.currentUser;

  const saved = await chrome.storage.local.get([
    "wordfallGoogleIdToken",
    "wordfallGoogleAccessToken"
  ]);
  if (!saved.wordfallGoogleIdToken && !saved.wordfallGoogleAccessToken) return null;

  const credential = GoogleAuthProvider.credential(
    saved.wordfallGoogleIdToken || null,
    saved.wordfallGoogleAccessToken || null
  );
  const signedIn = await signInWithCredential(auth, credential);
  return signedIn.user;
}

function nicknameDocumentId(nickname) {
  return encodeURIComponent(nickname.trim().toLowerCase());
}

async function firestoreRestRequest(path, options = {}) {
  const user = await ensureAuthenticatedUser();
  if (!user) throw new Error("auth/missing-user");

  const token = await user.getIdToken(true);
  const response = await fetch(
    `https://firestore.googleapis.com/v1/projects/${firebaseConfig.projectId}/databases/(default)/documents${path}?key=${firebaseConfig.apiKey}`,
    {
      ...options,
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
        ...(options.headers || {})
      }
    }
  );
  const body = await response.text();
  if (!response.ok) {
    if (response.status === 404) return null;
    throw new Error(`firestore/${response.status}: ${body}`);
  }
  return body ? JSON.parse(body) : {};
}

async function reserveNickname(nickname, oldNickname, uid) {
  const normalized = nickname.trim().toLowerCase();
  const currentUser = await ensureAuthenticatedUser();
  const ownerUid = currentUser?.uid;
  if (!normalized || !ownerUid) throw new Error("auth/missing-user");
  const newId = nicknameDocumentId(normalized);
  const oldNormalized = (oldNickname || "").trim().toLowerCase();
  const oldId = oldNormalized ? nicknameDocumentId(oldNormalized) : "";
  const newPath = `/nicknames/${newId}`;
  const current = await firestoreRestRequest(newPath);
  const currentUid = current?.fields?.uid?.stringValue || "";
  if (current && currentUid !== ownerUid) return false;

  const writes = [{
    update: {
      name: `projects/${firebaseConfig.projectId}/databases/(default)/documents/nicknames/${newId}`,
      fields: {
        uid: { stringValue: ownerUid },
        nickname: { stringValue: nickname.trim() },
        updatedAt: { timestampValue: new Date().toISOString() }
      }
    },
    currentDocument: { exists: current != null }
  }];

  if (oldId && oldId !== newId) {
    const old = await firestoreRestRequest(`/nicknames/${oldId}`);
    const oldUid = old?.fields?.uid?.stringValue || "";
    if (old && oldUid === ownerUid) {
      writes.push({
        delete: `projects/${firebaseConfig.projectId}/databases/(default)/documents/nicknames/${oldId}`,
        currentDocument: { exists: true }
      });
    }
  }

  const commit = await firestoreRestRequest(":commit", {
    method: "POST",
    body: JSON.stringify({ writes })
  });
  return !!commit;
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
        : message.action === "delete-account"
          ? deleteAccount(message.nickname)
        : message.action === "announcements"
          ? loadAnnouncements().then((items) => ({ items }))
        : message.action === "update"
          ? loadUpdateInfo()
        : reserveNickname(message.nickname, message.oldNickname, message.uid).then((available) => ({ available }));
  task.then(sendResponse).catch((error) => {
    console.error(
      "Wordfall Firebase request failed | action=" + message.action
      + " | code=" + (error?.code || "")
      + " | message=" + (error?.message || String(error))
      + " | authUid=" + (auth.currentUser?.uid || "(none)")
    );
    sendResponse({ error: error.code || error.message || "auth/unknown" });
  });
  return true;
});
