using UnityEngine;

public sealed partial class WordfallPrototype
{
    private const string AuthSignedInKey = "Wordfall.AuthSignedIn";
    private const string AuthProviderKey = "Wordfall.AuthProvider";
    private const string AuthUidKey = "Wordfall.AuthUid";

    private void ShowLoginOrLobby()
    {
        if (playing) return;
        if (loginPanel == null)
        {
            ShowHomeHub();
            return;
        }

        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        setupPanel.SetActive(false);
        languagePanel.SetActive(false);
        profilePanel.SetActive(false);
        homeHubPanel.SetActive(false);
        loginStatusText.text = authSignedIn
            ? (language == Language.Korean ? "이미 로그인되어 있습니다." : "You are already signed in.")
            : string.Empty;
        loginPanel.SetActive(true);
    }

    private void CloseLoginPanel()
    {
        if (loginPanel != null) loginPanel.SetActive(false);
    }

    private void LoginAsGuest()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        loginStatusText.text = language == Language.Korean ? "게스트 로그인 중..." : "Signing in as guest...";
        FirebaseWebAuthBridge.SignInAnonymously(gameObject.name);
#else
        CompleteAuth("guest", "editor-guest");
#endif
    }

    private void LoginWithGoogle()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        loginStatusText.text = language == Language.Korean ? "Google 로그인 중..." : "Signing in with Google...";
        FirebaseWebAuthBridge.SignInWithGoogle(gameObject.name);
#else
        loginStatusText.text = language == Language.Korean
            ? "Google 로그인은 WebGL 빌드에서 사용할 수 있습니다."
            : "Google login is available in the WebGL build.";
#endif
    }

    // Called by the WebGL Firebase bridge through Unity SendMessage.
    private void OnFirebaseAuthResult(string result)
    {
        if (string.IsNullOrEmpty(result) || result.StartsWith("error|"))
        {
            if (loginStatusText != null)
                loginStatusText.text = language == Language.Korean ? "로그인에 실패했습니다." : "Login failed.";
            return;
        }

        var parts = result.Split('|');
        var provider = parts.Length > 0 ? parts[0] : "guest";
        var uid = parts.Length > 1 ? parts[1] : string.Empty;
        CompleteAuth(provider, uid);
    }

    private void CompleteAuth(string provider, string uid)
    {
        authSignedIn = true;
        PlayerPrefs.SetInt(AuthSignedInKey, 1);
        PlayerPrefs.SetString(AuthProviderKey, provider);
        PlayerPrefs.SetString(AuthUidKey, uid ?? string.Empty);
        PlayerPrefs.Save();
        CloseLoginPanel();
        ShowHomeHub();
    }
}
