using System.Runtime.InteropServices;

public static class FirebaseWebAuthBridge
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void WordfallFirebaseSignInAnonymously(string gameObjectName);

    [DllImport("__Internal")]
    private static extern void WordfallFirebaseSignInGoogle(string gameObjectName);

    [DllImport("__Internal")]
    private static extern void WordfallFirebaseCheckNickname(string gameObjectName, string nickname, string oldNickname, string uid);
#endif

    public static void SignInAnonymously(string gameObjectName)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        WordfallFirebaseSignInAnonymously(gameObjectName);
#endif
    }

    public static void SignInWithGoogle(string gameObjectName)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        WordfallFirebaseSignInGoogle(gameObjectName);
#endif
    }

    public static void CheckNickname(string gameObjectName, string nickname, string oldNickname, string uid)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        WordfallFirebaseCheckNickname(gameObjectName, nickname, oldNickname, uid);
#endif
    }
}
