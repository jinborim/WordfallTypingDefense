using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private void SetLanguage(Language selectedLanguage)
    {
        language = selectedLanguage;
        bool korean = language == Language.Korean;

        if (titleText != null) titleText.text = korean ? "WORDFALL" : "WORDFALL";
        if (subtitleText != null) subtitleText.text = korean ? "TEXT" : "KOREAN & ENGLISH TYPING DEFENSE";
        if (descriptionText != null) descriptionText.text = korean ? "?????????亦????嚥싲갭큔?????????????????????????????ㅻ쑋雍???????????????븐뼐????????????얜궙??뷀떐???" : "Type the falling words before they reach the bottom.";
        SetButtonText(mainStartButton, korean ? "TEXT" : "START GAME");
        SetButtonText(languageOpenButton, korean ? "LANGUAGE" : "LANGUAGE");
        SetButtonText(homeHubStartButton, korean ? "TEXT" : "START GAME");
        SetButtonText(startButton, korean ? "TEXT" : "START GAME");
        SetButtonText(setupBackButton, korean ? "TEXT" : "BACK");
        SetButtonText(languageCloseButton, korean ? "CLOSE" : "CLOSE");
        SetButtonText(languageButtons != null && languageButtons.Length > 0 ? languageButtons[0] : null, "KOREAN");
        SetButtonText(languageButtons != null && languageButtons.Length > 1 ? languageButtons[1] : null, "ENGLISH");
        SetButtonText(menuButton, korean ? "??????濡?씀?濾????ㅿ폍筌ㅺ래????" : "PAUSE");
        SetButtonText(resumeButton, korean ? "??節뚮쳮?????꾨탿" : "RESUME");
        SetButtonText(pauseMenuButton, korean ? "MENU" : "MENU");
        SetButtonText(pauseMyPageButton, korean ? "????븐뼐?????????????⑤슢堉??곕????????????" : "MY PAGE");
        SetButtonText(difficultyButtons != null && difficultyButtons.Length > 0 ? difficultyButtons[0] : null, korean ? "???" : "EASY");
        SetButtonText(difficultyButtons != null && difficultyButtons.Length > 1 ? difficultyButtons[1] : null, korean ? "NORMAL" : "NORMAL");
        SetButtonText(difficultyButtons != null && difficultyButtons.Length > 2 ? difficultyButtons[2] : null, korean ? "??????" : "HARD");
        SetButtonText(modeButtons != null && modeButtons.Length > 0 ? modeButtons[0] : null, korean ? "????" : "MIX");
        SetButtonText(modeButtons != null && modeButtons.Length > 1 ? modeButtons[1] : null, korean ? "TEXT" : "KOR");
        SetButtonText(modeButtons != null && modeButtons.Length > 2 ? modeButtons[2] : null, korean ? "ENG" : "ENG");
        SetButtonText(modeButtons != null && modeButtons.Length > 3 ? modeButtons[3] : null, korean ? "TEXT" : "60 SEC");
        SetButtonText(modeButtons != null && modeButtons.Length > 4 ? modeButtons[4] : null, korean ? "????몄툜" : "DAILY");

        if (homeHubTitleText != null) homeHubTitleText.text = korean ? "LOBBY" : "LOBBY";
        if (homeHubPlayerText != null) homeHubPlayerText.text = korean ? "TEXT" : "PLAYER";
        if (setupTitleText != null) setupTitleText.text = korean ? "GAME SETTINGS" : "GAME SETTINGS";
        if (languageTitleText != null) languageTitleText.text = korean ? "LANGUAGE" : "LANGUAGE";
        if (profileTitleText != null) profileTitleText.text = korean ? "TEXT" : "CREATE PLAYER";
        if (myPageTitleText != null) myPageTitleText.text = korean ? "????븐뼐?????????????⑤슢堉??곕????????????" : "MY PAGE";
        if (shopTitleText != null) shopTitleText.text = korean ? "COIN SHOP" : "COIN SHOP";
        if (inputPlaceholder != null) inputPlaceholder.text = korean ? "??????????????ㅻ쑋雍?.." : "TYPE HERE...";

        UpdateModeDescription();
        UpdateHighScoreText();
        UpdateProfileSummary();
        UpdateCoinText();
        UpdateMyPage();
        UpdateShopText();
        UpdateHud();
    }
}

