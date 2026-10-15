using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void ShowSetupPanel()
    {
        if (playing) return;
        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        homeHubPanel.SetActive(false);
        setupPanel.SetActive(true);
        languagePanel.SetActive(false);
        profilePanel.SetActive(false);
        SetDifficulty(difficulty);
        SetGameMode(gameMode);
    }

    private void CloseSetupPanel()
    {
        setupPanel.SetActive(false);
        if (profileCreated) ShowHomeHub();
        else ShowMainMenu();
    }

    private void ShowLanguagePanel()
    {
        languageOpenedFromHomeHub = homeHubPanel != null && homeHubPanel.activeSelf;
        mainPanel.SetActive(true);
        setupPanel.SetActive(false);
        languagePanel.SetActive(true);
    }

    private void CloseLanguagePanel()
    {
        languagePanel.SetActive(false);
        if (languageOpenedFromHomeHub) homeHubPanel.SetActive(true);
        else if (profileCreated) ShowHomeHub();
        else ShowMainMenu();
        languageOpenedFromHomeHub = false;
    }

    private void SetGameMode(GameMode selectedMode)
    {
        gameMode = selectedMode;
        modeText.text = language == Language.Korean ? "모드" : "MODE";
        UpdateModeDescription();
        for (int i = 0; i < modeButtons.Length; i++)
        {
            var colors = modeButtons[i].colors;
            colors.normalColor = i == (int)gameMode ? new Color32(255, 211, 94, 255) : new Color32(58, 80, 122, 255);
            colors.highlightedColor = new Color32(255, 232, 150, 255);
            colors.pressedColor = new Color32(216, 173, 68, 255);
            modeButtons[i].colors = colors;
        }
    }

    private void UpdateModeDescription()
    {
        if (modeDescriptionText == null) return;
        if (language == Language.Korean)
        {
            if (gameMode == GameMode.Korean) modeDescriptionText.text = "한글 단어만 등장합니다.";
            else if (gameMode == GameMode.English) modeDescriptionText.text = "영어 단어만 등장합니다.";
            else if (gameMode == GameMode.TimeAttack) modeDescriptionText.text = "60초 동안 최대한 높은 점수에 도전합니다.";
            else if (gameMode == GameMode.Daily) modeDescriptionText.text = "오늘만 플레이할 수 있는 특별 도전입니다.";
            else modeDescriptionText.text = "한글과 영어 단어가 함께 등장합니다.";
        }
        else
        {
            if (gameMode == GameMode.Korean) modeDescriptionText.text = "Korean words only.";
            else if (gameMode == GameMode.English) modeDescriptionText.text = "English words only.";
            else if (gameMode == GameMode.TimeAttack) modeDescriptionText.text = "Score as high as possible in 60 seconds.";
            else if (gameMode == GameMode.Daily) modeDescriptionText.text = "A special challenge available today.";
            else modeDescriptionText.text = "Korean and English words appear together.";
        }
    }

    private void SetDifficulty(int selectedDifficulty)
    {
        difficulty = Mathf.Clamp(selectedDifficulty, 0, 2);
        difficultyText.text = language == Language.Korean ? "난이도" : "DIFFICULTY";
        for (int i = 0; i < difficultyButtons.Length; i++)
        {
            var colors = difficultyButtons[i].colors;
            colors.normalColor = i == difficulty ? new Color32(255, 211, 94, 255) : new Color32(58, 80, 122, 255);
            colors.highlightedColor = new Color32(255, 232, 150, 255);
            colors.pressedColor = new Color32(216, 173, 68, 255);
            difficultyButtons[i].colors = colors;
        }
    }

    private void SelectLanguageAndClose(Language selectedLanguage)
    {
        SetLanguage(selectedLanguage);
        if (languagePanel != null) languagePanel.SetActive(false);
    }
}
