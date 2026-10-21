(function () {
  function request(action) {
    return new Promise(function (resolve, reject) {
      if (!window.chrome || !chrome.runtime || !chrome.runtime.sendMessage) {
        reject(new Error("Chrome extension runtime is unavailable"));
        return;
      }
      chrome.runtime.sendMessage({ type: "firebase-auth", action: action }, function (result) {
        if (chrome.runtime.lastError) {
          reject(new Error(chrome.runtime.lastError.message));
          return;
        }
        if (!result || result.error) {
          reject(new Error(result && result.error ? result.error : "Firebase authentication failed"));
          return;
        }
        resolve(result);
      });
    });
  }

  window.WordfallFirebaseAuth = {
    signInAnonymously: function () { return request("guest"); },
    signInWithGoogle: function () { return request("google"); },
    signOut: function () { return request("logout"); },
    getAnnouncements: function () { return request("announcements"); },
    getUpdateInfo: function () { return request("update"); },
    checkNickname: function (nickname, oldNickname, uid) {
      return new Promise(function (resolve, reject) {
        chrome.runtime.sendMessage({
          type: "firebase-auth",
          action: "nickname",
          nickname: nickname,
          oldNickname: oldNickname || "",
          uid: uid || ""
        }, function (result) {
          if (chrome.runtime.lastError) {
            reject(new Error(chrome.runtime.lastError.message));
            return;
          }
          if (!result || result.error) {
            reject(new Error(result && result.error ? result.error : "Nickname check failed"));
            return;
          }
          resolve(result.available === true);
        });
      });
    }
  };
})();
