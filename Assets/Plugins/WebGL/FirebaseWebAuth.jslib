mergeInto(LibraryManager.library, {
  WordfallFirebaseSignInAnonymously: function (gameObjectNamePtr) {
    var gameObjectName = UTF8ToString(gameObjectNamePtr);
    var send = function (payload) {
      SendMessage(gameObjectName, 'OnFirebaseAuthResult', payload);
    };
    if (!window.WordfallFirebaseAuth || !window.WordfallFirebaseAuth.signInAnonymously) {
      send('error|Firebase Web Auth bridge is not loaded');
      return;
    }
    window.WordfallFirebaseAuth.signInAnonymously()
      .then(function (user) { send('guest|' + user.uid); })
      .catch(function (error) {
        send('error|' + (error.code || error.message || 'anonymous'));
      });
  },
  WordfallFirebaseSignInGoogle: function (gameObjectNamePtr) {
    var gameObjectName = UTF8ToString(gameObjectNamePtr);
    var send = function (payload) {
      SendMessage(gameObjectName, 'OnFirebaseAuthResult', payload);
    };
    if (!window.WordfallFirebaseAuth || !window.WordfallFirebaseAuth.signInWithGoogle) {
      send('error|Firebase Web Auth bridge is not loaded');
      return;
    }
    window.WordfallFirebaseAuth.signInWithGoogle()
      .then(function (user) {
        send('google|' + user.uid + '|' + (user.displayName || ''));
      })
      .catch(function (error) {
        send('error|' + (error.code || error.message || 'google'));
      });
  },
  WordfallFirebaseCheckNickname: function (gameObjectNamePtr, nicknamePtr, oldNicknamePtr, uidPtr) {
    var gameObjectName = UTF8ToString(gameObjectNamePtr);
    var nickname = UTF8ToString(nicknamePtr);
    var oldNickname = UTF8ToString(oldNicknamePtr);
    var uid = UTF8ToString(uidPtr);
    var send = function (payload) {
      SendMessage(gameObjectName, 'OnNicknameAvailabilityResult', payload);
    };
    if (!window.WordfallFirebaseAuth || !window.WordfallFirebaseAuth.checkNickname) {
      send('error|Firebase nickname bridge is not loaded');
      return;
    }
    window.WordfallFirebaseAuth.checkNickname(nickname, oldNickname, uid)
      .then(function (available) {
        send((available ? 'available|' : 'taken|') + nickname);
      })
      .catch(function (error) {
        send('error|' + (error.code || error.message || 'nickname'));
      });
  },
  WordfallFirebaseSignOut: function (gameObjectNamePtr) {
    var gameObjectName = UTF8ToString(gameObjectNamePtr);
    var send = function (payload) {
      SendMessage(gameObjectName, 'OnFirebaseSignOutResult', payload);
    };
    if (!window.WordfallFirebaseAuth || !window.WordfallFirebaseAuth.signOut) {
      send('error|Firebase sign-out bridge is not loaded');
      return;
    }
    window.WordfallFirebaseAuth.signOut()
      .then(function () { send('ok'); })
      .catch(function (error) {
        send('error|' + (error.code || error.message || 'signout'));
      });
  },
  WordfallFirebaseDeleteAccount: function (gameObjectNamePtr, nicknamePtr) {
    var gameObjectName = UTF8ToString(gameObjectNamePtr);
    var nickname = UTF8ToString(nicknamePtr);
    var send = function (payload) {
      SendMessage(gameObjectName, 'OnFirebaseAccountDeleteResult', payload);
    };
    if (!window.WordfallFirebaseAuth || !window.WordfallFirebaseAuth.deleteAccount) {
      send('error|Firebase account deletion bridge is not loaded');
      return;
    }
    window.WordfallFirebaseAuth.deleteAccount(nickname)
      .then(function () { send('ok'); })
      .catch(function (error) {
        send('error|' + (error.code || error.message || 'delete-account'));
      });
  },
  WordfallFirebaseLoadAnnouncements: function (gameObjectNamePtr) {
    var gameObjectName = UTF8ToString(gameObjectNamePtr);
    var send = function (payload) {
      SendMessage(gameObjectName, 'OnFirebaseAnnouncementsResult', payload);
    };
    if (!window.WordfallFirebaseAuth || !window.WordfallFirebaseAuth.getAnnouncements) {
      send('{"ok":false,"error":"Firebase announcements bridge is not loaded"}');
      return;
    }
    window.WordfallFirebaseAuth.getAnnouncements()
      .then(function (result) {
        send(JSON.stringify({ ok: true, items: result && result.items ? result.items : [] }));
      })
      .catch(function (error) {
        send(JSON.stringify({ ok: false, error: error.code || error.message || 'announcements' }));
      });
  },
  WordfallFirebaseLoadUpdateInfo: function (gameObjectNamePtr) {
    var gameObjectName = UTF8ToString(gameObjectNamePtr);
    var send = function (payload) {
      SendMessage(gameObjectName, 'OnFirebaseUpdateInfoResult', payload);
    };
    if (!window.WordfallFirebaseAuth || !window.WordfallFirebaseAuth.getUpdateInfo) {
      send('{"ok":false,"error":"Firebase update bridge is not loaded"}');
      return;
    }
    window.WordfallFirebaseAuth.getUpdateInfo()
      .then(function (result) {
        send(JSON.stringify({ ok: true, update: result || {} }));
      })
      .catch(function (error) {
        send(JSON.stringify({ ok: false, error: error.code || error.message || 'update' }));
      });
  }
});
