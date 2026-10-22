const AUTH_HELPER_URL =
  "https://typing-c9b14.web.app/signInWithPopup.html";

const AUTH_HELPER_ORIGIN = new URL(AUTH_HELPER_URL).origin;

const iframe = document.createElement("iframe");
iframe.src = AUTH_HELPER_URL;
iframe.style.display = "none";
let helperLoaded = false;
let helperLoadError = null;
let helperLoadPromise = new Promise((resolve, reject) => {
  iframe.addEventListener("load", () => {
    helperLoaded = true;
    resolve();
  }, { once: true });
  iframe.addEventListener("error", () => {
    helperLoadError = "auth/google-helper-load-failed";
    reject(new Error(helperLoadError));
  }, { once: true });
});
document.documentElement.appendChild(iframe);

chrome.runtime.onMessage.addListener((message, sender, sendResponse) => {
  if (message?.target !== "offscreen" || message.action !== "google") {
    return false;
  }

  let completed = false;

  function handleAuthResult(event) {
    if (event.origin !== AUTH_HELPER_ORIGIN) return;

    try {
      // Firebase/Google may send internal string messages through the same
      // window. They are not Wordfall auth results and must be ignored.
      if (typeof event.data === "string" && !event.data.trim().startsWith("{")) return;
      const result = typeof event.data === "string"
        ? JSON.parse(event.data)
        : event.data;
      if (!result || result.wordfallAuthResult !== true) return;

      if (result && result.error) console.error("Wordfall Google auth helper error", result.error);

      completed = true;
      window.removeEventListener("message", handleAuthResult);
      sendResponse(result);
    } catch (error) {
      completed = true;
      window.removeEventListener("message", handleAuthResult);
      sendResponse({ error: error.message || "auth/invalid-response" });
    }
  }

  window.addEventListener("message", handleAuthResult);

  const sendInitMessage = () => {
    if (completed) return;
    if (helperLoadError) {
      completed = true;
      window.removeEventListener("message", handleAuthResult);
      sendResponse({ error: helperLoadError });
      return;
    }
    if (!iframe.contentWindow) {
      completed = true;
      window.removeEventListener("message", handleAuthResult);
      sendResponse({ error: "auth/google-helper-unavailable" });
      return;
    }
    iframe.contentWindow.postMessage({ initAuth: true }, AUTH_HELPER_ORIGIN);
  };

  // Do not send the init message before the Firebase helper has installed its
  // message listener. That race can make the request wait until the timeout.
  if (helperLoaded) {
    sendInitMessage();
  } else {
    // Wait for the helper's load event. Sending earlier can lose the message
    // because signInWithPopup has not installed its listener yet.
    helperLoadPromise.then(sendInitMessage).catch(() => sendInitMessage());
  }

  setTimeout(() => {
    if (completed) return;
    window.removeEventListener("message", handleAuthResult);
    console.error("Wordfall Google auth helper timed out");
    sendResponse({ error: "auth/google-login-timeout" });
  }, 120000);

  return true;
});
