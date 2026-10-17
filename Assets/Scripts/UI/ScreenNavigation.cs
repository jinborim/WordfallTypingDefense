using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void ShowMainMenu()
    {
        LoadMainMenuScene();
        playing = false;
        paused = false;
        ClearWords();
        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        if (setupPanel != null) setupPanel.SetActive(false);
        if (homeHubPanel != null) homeHubPanel.SetActive(false);
        if (languagePanel != null) languagePanel.SetActive(false);
        if (profilePanel != null) profilePanel.SetActive(false);
        if (myPagePanel != null) myPagePanel.SetActive(false);
        if (shopPanel != null) shopPanel.SetActive(false);
        if (insufficientCoinsPanel != null) insufficientCoinsPanel.SetActive(false);
        if (announcementPanel != null) announcementPanel.SetActive(false);
        if (privacyPanel != null) privacyPanel.SetActive(false);
        if (withdrawConfirmPanel != null) withdrawConfirmPanel.SetActive(false);
        if (insufficientCoinsPanel != null) insufficientCoinsPanel.SetActive(false);
        if (attendancePanel != null) attendancePanel.SetActive(false);
        if (updatePanel != null) updatePanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void UpdateHomeHubCharacter()
    {
        if (homeHubCharacterImage == null) return;
        var texture = Resources.Load<Texture2D>(GetLobbyCharacterResourcePath());
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        homeHubCharacterImage.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        ApplyLobbyCharacterLayout(homeHubCharacterImage.rectTransform);
    }

    private void ApplyLobbyCharacterLayout(RectTransform characterRect)
    {
        if (characterRect == null) return;

        // 캐릭터 원본 이미지 자체의 비율을 유지하고, 모든 캐릭터가
        // 해커와 같은 전신 표시 영역 안에서 자연스럽게 보이도록 합니다.
        homeHubCharacterImage.preserveAspect = true;
        characterRect.sizeDelta = new Vector2(188f, 308f);
        characterRect.anchoredPosition = new Vector2(0f, -26f);
    }

    private void ShowMyPage()
    {
        if (!profileCreated || (playing && !paused)) return;
        myPageOpenedFromGame = playing;
        if (myPageOpenedFromGame)
        {
            gamePanel.SetActive(false);
            mainPanel.SetActive(true);
        }
        setupPanel.SetActive(false);
        languagePanel.SetActive(false);
        profilePanel.SetActive(false);
        UpdateMyPage();
        myPagePanel.SetActive(true);
    }

    private void CloseMyPage()
    {
        myPagePanel.SetActive(false);
        if (myPageOpenedFromGame)
        {
            mainPanel.SetActive(false);
            gamePanel.SetActive(true);
            pausePanel.SetActive(true);
        }
        else
        {
            ResetHomeHubMenuState();
            ShowLobbyDialogue();
        }
        myPageOpenedFromGame = false;
    }
}
