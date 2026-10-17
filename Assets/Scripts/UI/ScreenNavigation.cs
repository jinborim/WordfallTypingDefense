using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void ShowMainMenu()
    {
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

        // 기사와 마법사는 머리 장비와 망토가 넓게 그려져 있어
        // 해커와 같은 프레임을 사용하면 상대적으로 크게 보입니다.
        // 폭을 캐릭터별로 보정하고 발끝은 같은 바닥선에 맞춥니다.
        if (selectedCharacter == 0)
        {
            characterRect.sizeDelta = new Vector2(150f, 320f);
            characterRect.anchoredPosition = new Vector2(0f, -20f);
            homeHubCharacterImage.preserveAspect = false;
        }
        else if (selectedCharacter == 1)
        {
            characterRect.sizeDelta = new Vector2(160f, 330f);
            characterRect.anchoredPosition = new Vector2(0f, -15f);
            homeHubCharacterImage.preserveAspect = false;
        }
        else
        {
            characterRect.sizeDelta = new Vector2(188f, 308f);
            characterRect.anchoredPosition = new Vector2(0f, -26f);
            homeHubCharacterImage.preserveAspect = true;
        }
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
