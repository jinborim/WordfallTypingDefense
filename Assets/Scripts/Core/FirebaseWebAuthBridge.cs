using System.Runtime.InteropServices;

public static class FirebaseWebAuthBridge
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void WordfallFirebaseSignInAnonymously(string gameObjectName);

    [DllImport("__Internal")]
    private static extern void WordfallFirebaseSignInGoogle(string gameObjectName);
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
}
