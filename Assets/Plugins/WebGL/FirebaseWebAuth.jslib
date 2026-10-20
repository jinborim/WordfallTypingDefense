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
  }
});
