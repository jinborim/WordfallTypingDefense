import { initializeApp } from "firebase/app";
import { getAuth, GoogleAuthProvider, signInWithPopup } from "firebase/auth";
import { firebaseConfig } from "./firebase-config.js";

const app = initializeApp(firebaseConfig);
const auth = getAuth(app);
const provider = new GoogleAuthProvider();

chrome.runtime.onMessage.addListener((message, sender, sendResponse) => {
  if (message?.target !== "offscreen" || message.action !== "google") return false;

  signInWithPopup(auth, provider)
    .then((result) => {
      sendResponse({
        provider: "google",
        uid: result.user.uid,
        displayName: result.user.displayName || ""
      });
    })
    .catch((error) => {
      sendResponse({ error: error.code || error.message || "auth/google-sign-in-failed" });
    });
  return true;
});
