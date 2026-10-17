using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void ShowUpdatePanel(UpdateChecker.UpdateInfo updateInfo)
    {
        if (updateInfo == null || updatePanel == null) return;
        pendingUpdateUrl = string.IsNullOrWhiteSpace(updateInfo.downloadUrl)
            ? UpdateChecker.DefaultManifestUrl
            : updateInfo.downloadUrl;
        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        setupPanel.SetActive(false);
        languagePanel.SetActive(false);
        profilePanel.SetActive(false);
        myPagePanel.SetActive(false);
        attendancePanel.SetActive(false);
        updateMessageText.text = string.IsNullOrWhiteSpace(updateInfo.message)
            ? (language == Language.Korean ? "\uC0C8\uB85C\uC6B4 \uC5C5\uB370\uC774\uD2B8\uAC00 \uC788\uC2B5\uB2C8\uB2E4." : "A new update is available.")
            : updateInfo.message;
        updateVersionText.text = (language == Language.Korean ? "\uC0C8 \uBC84\uC804 " : "NEW VERSION ") + updateInfo.version;
        updatePanel.SetActive(true);
    }

    private void CloseUpdatePanel()
    {
        if (updatePanel != null) updatePanel.SetActive(false);
    }

    private void OpenUpdatePage()
    {
        if (!string.IsNullOrWhiteSpace(pendingUpdateUrl)) Application.OpenURL(pendingUpdateUrl);
        CloseUpdatePanel();
    }

    private void UpdateMyPage()
    {
        if (myPagePanel == null || playerDataManager == null) return;
        string nickname = string.IsNullOrEmpty(playerDataManager.Data.nickname) ? "PLAYER" : playerDataManager.Data.nickname;
        int gamesPlayed = playerDataManager.Data.gamesPlayed;
        int wordsTyped = playerDataManager.Data.wordsTyped;
        int correctWords = playerDataManager.Data.correctWords;
        int accuracy = wordsTyped > 0 ? Mathf.RoundToInt((float)correctWords / wordsTyped * 100f) : 0;
        myPageInfoText.text = nickname + " / " + GetCharacterName();
        myPageCoinText.text = language == Language.Korean ? "\uCF54\uC778 " + coins : "COINS " + coins;
        myPageStatsText.text = language == Language.Korean
            ? $"\uAC8C\uC784 {gamesPlayed} \u00B7 \uC785\uB825 {wordsTyped} \u00B7 \uC815\uD655\uB3C4 {accuracy}%"
            : $"GAMES {gamesPlayed} \u00B7 TYPED {wordsTyped} \u00B7 ACCURACY {accuracy}%";
        var texture = Resources.Load<Texture2D>(GetCharacterResourcePath(false));
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        myPageCharacterImage.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        myPageCharacterImage.preserveAspect = true;
    }

    private void DeleteAccount()
    {
        playerDataManager.DeleteAccount();
        profileCreated = false;
        selectedCharacter = 0;
        coins = 0;
        nicknameTickets = 0;
        ownedCharacterMask = 1;
        UpdateCharacterButtonStates();
        UpdateProfileSummary();
        UpdateCoinText();
        if (myPagePanel != null) myPagePanel.SetActive(false);
        ShowMainMenu();
    }
}
