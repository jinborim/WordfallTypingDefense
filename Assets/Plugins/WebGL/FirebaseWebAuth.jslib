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
  }
});
