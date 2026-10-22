using UnityEngine;

public sealed partial class WordfallPrototype
{
    private const string AuthSignedInKey = "Wordfall.AuthSignedIn";
    private const string AuthProviderKey = "Wordfall.AuthProvider";
    private const string AuthUidKey = "Wordfall.AuthUid";

    private void ShowLoginOrLobby()
    {
        if (playing) return;
        if (authSignedIn)
        {
            ShowHomeHub();
            return;
        }
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
        SetLoginButtonsInteractable(true);
        PreparePopupBackgroundOnly(loginPanel);
    }

    private void SetLoginButtonsInteractable(bool interactable)
    {
        if (guestLoginButton != null) guestLoginButton.interactable = interactable;
        if (googleLoginButton != null) googleLoginButton.interactable = interactable;
        if (loginCloseButton != null) loginCloseButton.interactable = interactable;
    }

    private void CloseLoginPanel()
    {
        if (loginPanel != null) loginPanel.SetActive(false);
        if (!authSignedIn) ShowMainMenu();
    }

    private void LoginAsGuest()
    {
        if (authInProgress) return;
        authInProgress = true;
        SetLoginButtonsInteractable(false);
#if UNITY_WEBGL && !UNITY_EDITOR
        loginStatusText.text = language == Language.Korean ? "게스트 로그인 중..." : "Signing in as guest...";
        FirebaseWebAuthBridge.SignInAnonymously(gameObject.name);
#else
        CompleteAuth("guest", "editor-guest");
#endif
    }

    private void LoginWithGoogle()
    {
        if (authInProgress) return;
        authInProgress = true;
        SetLoginButtonsInteractable(false);
#if UNITY_WEBGL && !UNITY_EDITOR
        loginStatusText.text = language == Language.Korean ? "Google 로그인 중..." : "Signing in with Google...";
        FirebaseWebAuthBridge.SignInWithGoogle(gameObject.name);
#else
        loginStatusText.text = language == Language.Korean
            ? "Google 로그인은 WebGL 빌드에서 사용할 수 있습니다."
            : "Google login is available in the WebGL build.";
        authInProgress = false;
        SetLoginButtonsInteractable(true);
#endif
    }

    // Called by the WebGL Firebase bridge through Unity SendMessage.
    private void OnFirebaseAuthResult(string result)
    {
        if (string.IsNullOrEmpty(result) || result.StartsWith("error|"))
        {
            UnityEngine.Debug.LogError("Wordfall Google login failed: " + (result ?? "empty-result"));
            if (loginStatusText != null)
                loginStatusText.text = language == Language.Korean ? "로그인에 실패했습니다." : "Login failed.";
            if (loginStatusText != null && !string.IsNullOrEmpty(result) && result.StartsWith("error|"))
                loginStatusText.text += "\n" + result.Substring("error|".Length);
            authInProgress = false;
            SetLoginButtonsInteractable(true);
            return;
        }

        var parts = result.Split('|');
        var provider = parts.Length > 0 ? parts[0] : "guest";
        var uid = parts.Length > 1 ? parts[1] : string.Empty;
        CompleteAuth(provider, uid);
    }

    private void CompleteAuth(string provider, string uid)
    {
        authInProgress = false;
        SetLoginButtonsInteractable(true);
        authSignedIn = true;
        PlayerPrefs.SetInt(AuthSignedInKey, 1);
        PlayerPrefs.SetString(AuthProviderKey, provider);
        PlayerPrefs.SetString(AuthUidKey, uid ?? string.Empty);
        PlayerPrefs.Save();
        AccountDataIsolation.SwitchToAccount(uid);
        playerDataManager.Load();
        // SwitchToAccount restores this user's isolated PlayerPrefs values.
        // Refresh the runtime fields before deciding whether this is a new
        // profile; otherwise every successful login looks like first launch.
        profileCreated = playerDataManager.Data.profileCreated;
        selectedCharacter = playerDataManager.Data.selectedCharacter;
        coins = playerDataManager.Data.coins;
        nicknameTickets = playerDataManager.Data.nicknameTickets;
        ownedCharacterMask = playerDataManager.Data.ownedCharacterMask;
        highScore = playerDataManager.Data.highScore;
        UpdateProfileSummary();
        CloseLoginPanel();
        ShowHomeHub();
    }

    private void Logout()
    {
        ShowLogoutConfirm();
    }

    private void ExecuteLogout()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        FirebaseWebAuthBridge.SignOut(gameObject.name);
#else
        CompleteLogout();
#endif
    }

    private void OnFirebaseSignOutResult(string result)
    {
        if (string.IsNullOrEmpty(result) || result.StartsWith("error|"))
        {
            if (loginStatusText != null)
                loginStatusText.text = language == Language.Korean ? "\uB85C\uADF8\uC544\uC6C3\uC5D0 \uC2E4\uD328\uD588\uC2B5\uB2C8\uB2E4." : "Log out failed.";
            return;
        }
        CompleteLogout();
    }

    private void CompleteLogout()
    {
        AccountDataIsolation.SwitchToAccount("local");
        authSignedIn = false;
        PlayerPrefs.SetInt(AuthSignedInKey, 0);
        PlayerPrefs.DeleteKey(AuthProviderKey);
        PlayerPrefs.DeleteKey(AuthUidKey);
        PlayerPrefs.Save();
        playerDataManager.Load();
        ShowMainMenu();
    }
}
