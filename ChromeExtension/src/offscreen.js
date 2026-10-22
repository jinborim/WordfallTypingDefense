const AUTH_HELPER_URL =
  "https://typing-c9b14.web.app/signInWithPopup.html";

const AUTH_HELPER_ORIGIN = new URL(AUTH_HELPER_URL).origin;

const iframe = document.createElement("iframe");
iframe.src = AUTH_HELPER_URL;
iframe.style.display = "none";
document.documentElement.appendChild(iframe);

chrome.runtime.onMessage.addListener((message, sender, sendResponse) => {
  if (message?.target !== "offscreen" || message.action !== "google") {
    return false;
  }

  let completed = false;

  function handleAuthResult(event) {
    if (event.origin !== AUTH_HELPER_ORIGIN) return;

    try {
      if (typeof event.data !== "string" || !event.data.trim().startsWith("{")) return;
      const result = JSON.parse(event.data);
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
  iframe.contentWindow.postMessage({ initAuth: true }, AUTH_HELPER_ORIGIN);

  setTimeout(() => {
    if (completed) return;
    window.removeEventListener("message", handleAuthResult);
    console.error("Wordfall Google auth helper timed out");
    sendResponse({ error: "auth/google-login-timeout" });
  }, 60000);

  return true;
});
