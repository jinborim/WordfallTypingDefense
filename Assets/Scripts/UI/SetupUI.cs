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
        modeText.text = language == Language.Korean ? "MODE" : "MODE";
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
            if (gameMode == GameMode.Korean) modeDescriptionText.text = "??? ?????????궕???????롮쾸?椰?????轅붽틓??????";
            else if (gameMode == GameMode.English) modeDescriptionText.text = "???????살몖???????????궕???????롮쾸?椰?????轅붽틓??????";
            else if (gameMode == GameMode.TimeAttack) modeDescriptionText.text = "60??????????뉖즵 ?耀붾굝??????鶯?????? ???????????獄쏅챶留????轅붽틓??????";
            else if (gameMode == GameMode.Daily) modeDescriptionText.text = "????癲ル슢???????????????諛몃마????????????????????썹땟戮녹??諭????????獄쏅챶留???????筌?캉??";
            else modeDescriptionText.text = "????????????살몖????????됰슦???濡???????쎛 ???????????ル꺄???????롮쾸?椰?????轅붽틓??????";
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
        difficultyText.text = language == Language.Korean ? "DIFFICULTY" : "DIFFICULTY";
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

