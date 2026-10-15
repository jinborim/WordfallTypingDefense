using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private void SetLanguage(Language selectedLanguage)
    {
        language = selectedLanguage;
        bool korean = language == Language.Korean;

        if (titleText != null) titleText.text = "WORDFALL";
        if (subtitleText != null) subtitleText.text = korean ? "\uD55C\uAD6D\uC5B4 & \uC601\uC5B4 \uD0C0\uC790 \uB514\uD39C\uC2A4" : "KOREAN & ENGLISH TYPING DEFENSE";
        if (descriptionText != null) descriptionText.text = korean ? "\uB5A8\uC5B4\uC9C0\uB294 \uB2E8\uC5B4\uB97C \uBE60\uB974\uAC8C \uC785\uB825\uD574 \uAE30\uC9C0\uB97C \uC9C0\uD0A4\uC138\uC694." : "Type the falling words before they reach the bottom.";
        SetButtonText(mainStartButton, korean ? "\uAC8C\uC784 \uC2DC\uC791" : "START GAME");
        SetButtonText(languageOpenButton, korean ? "LANGUAGE" : "LANGUAGE");
        SetButtonText(homeHubStartButton, korean ? "\uAC8C\uC784 \uC2DC\uC791" : "START GAME");
        SetButtonText(startButton, korean ? "\uAC8C\uC784 \uC2DC\uC791" : "START GAME");
        SetButtonText(setupBackButton, korean ? "\uB4A4\uB85C" : "BACK");
        SetButtonText(languageCloseButton, korean ? "CLOSE" : "CLOSE");
        SetButtonText(languageButtons != null && languageButtons.Length > 0 ? languageButtons[0] : null, "KOREAN");
        SetButtonText(languageButtons != null && languageButtons.Length > 1 ? languageButtons[1] : null, "ENGLISH");
        SetButtonText(menuButton, korean ? "\uC77C\uC2DC\uC815\uC9C0" : "PAUSE");
        SetButtonText(resumeButton, korean ? "\uACC4\uC18D\uD558\uAE30" : "RESUME");
        SetButtonText(pauseMenuButton, korean ? "MENU" : "MENU");
        SetButtonText(pauseMyPageButton, korean ? "\uB9C8\uC774\uD398\uC774\uC9C0" : "MY PAGE");
        SetButtonText(difficultyButtons != null && difficultyButtons.Length > 0 ? difficultyButtons[0] : null, korean ? "\uC26C\uC6C0" : "EASY");
        SetButtonText(difficultyButtons != null && difficultyButtons.Length > 1 ? difficultyButtons[1] : null, korean ? "\uBCF4\uD1B5" : "NORMAL");
        SetButtonText(difficultyButtons != null && difficultyButtons.Length > 2 ? difficultyButtons[2] : null, korean ? "\\uC5B4\\uB824\\uC6C0" : "HARD");
        SetButtonText(modeButtons != null && modeButtons.Length > 0 ? modeButtons[0] : null, korean ? "\uD63C\uD569" : "MIX");
        SetButtonText(modeButtons != null && modeButtons.Length > 1 ? modeButtons[1] : null, korean ? "\uD55C\uAD6D\uC5B4" : "KOR");
        SetButtonText(modeButtons != null && modeButtons.Length > 2 ? modeButtons[2] : null, korean ? "ENG" : "ENG");
        SetButtonText(modeButtons != null && modeButtons.Length > 3 ? modeButtons[3] : null, korean ? "60\uCD08" : "60 SEC");
        SetButtonText(modeButtons != null && modeButtons.Length > 4 ? modeButtons[4] : null, korean ? "\uC624\uB298" : "DAILY");

        if (homeHubTitleText != null) homeHubTitleText.text = korean ? "\uB85C\uBE44" : "LOBBY";
        if (homeHubPlayerText != null) homeHubPlayerText.text = korean ? "\uD50C\uB808\uC774\uC5B4" : "PLAYER";
        if (setupTitleText != null) setupTitleText.text = korean ? "GAME SETTINGS" : "GAME SETTINGS";
        if (languageTitleText != null) languageTitleText.text = korean ? "LANGUAGE" : "LANGUAGE";
        if (profileTitleText != null) profileTitleText.text = korean ? "\uD504\uB85C\uD544 \uB9CC\uB4E4\uAE30" : "CREATE PLAYER";
        if (myPageTitleText != null) myPageTitleText.text = korean ? "\\uB9C8\\uC774\\uD398\\uC774\\uC9C0" : "MY PAGE";
        if (shopTitleText != null) shopTitleText.text = korean ? "COIN SHOP" : "COIN SHOP";
        if (inputPlaceholder != null) inputPlaceholder.text = korean ? "\uC5EC\uAE30\uC5D0 \uC785\uB825..." : "TYPE HERE...";

        UpdateModeDescription();
        UpdateHighScoreText();
        UpdateProfileSummary();
        UpdateCoinText();
        UpdateMyPage();
        UpdateShopText();
        UpdateHud();
    }
}

