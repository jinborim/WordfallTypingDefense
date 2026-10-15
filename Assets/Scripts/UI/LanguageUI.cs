using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private void SetLanguage(Language selectedLanguage)
    {
        language = selectedLanguage;
        bool korean = language == Language.Korean;

        if (titleText != null) titleText.text = korean ? "워드폴" : "WORDFALL";
        if (subtitleText != null) subtitleText.text = korean ? "한국어 & 영어 타자 디펜스" : "KOREAN & ENGLISH TYPING DEFENSE";
        if (descriptionText != null) descriptionText.text = korean ? "떨어지는 단어를 빠르게 입력해 기지를 지키세요." : "Type the falling words before they reach the bottom.";
        SetButtonText(mainStartButton, korean ? "게임 시작" : "START GAME");
        SetButtonText(languageOpenButton, korean ? "언어 설정" : "LANGUAGE");
        SetButtonText(homeHubStartButton, korean ? "게임 시작" : "START GAME");
        SetButtonText(startButton, korean ? "게임 시작" : "START GAME");
        SetButtonText(setupBackButton, korean ? "뒤로" : "BACK");
        SetButtonText(languageCloseButton, korean ? "닫기" : "CLOSE");
        SetButtonText(languageButtons != null && languageButtons.Length > 0 ? languageButtons[0] : null, "한국어");
        SetButtonText(languageButtons != null && languageButtons.Length > 1 ? languageButtons[1] : null, "ENGLISH");
        SetButtonText(menuButton, korean ? "일시정지" : "PAUSE");
        SetButtonText(resumeButton, korean ? "계속하기" : "RESUME");
        SetButtonText(pauseMenuButton, korean ? "메뉴" : "MENU");
        SetButtonText(pauseMyPageButton, korean ? "마이페이지" : "MY PAGE");
        SetButtonText(difficultyButtons != null && difficultyButtons.Length > 0 ? difficultyButtons[0] : null, korean ? "쉬움" : "EASY");
        SetButtonText(difficultyButtons != null && difficultyButtons.Length > 1 ? difficultyButtons[1] : null, korean ? "보통" : "NORMAL");
        SetButtonText(difficultyButtons != null && difficultyButtons.Length > 2 ? difficultyButtons[2] : null, korean ? "어려움" : "HARD");
        SetButtonText(modeButtons != null && modeButtons.Length > 0 ? modeButtons[0] : null, korean ? "혼합" : "MIX");
        SetButtonText(modeButtons != null && modeButtons.Length > 1 ? modeButtons[1] : null, korean ? "한국어" : "KOR");
        SetButtonText(modeButtons != null && modeButtons.Length > 2 ? modeButtons[2] : null, korean ? "영어" : "ENG");
        SetButtonText(modeButtons != null && modeButtons.Length > 3 ? modeButtons[3] : null, korean ? "60초" : "60 SEC");
        SetButtonText(modeButtons != null && modeButtons.Length > 4 ? modeButtons[4] : null, korean ? "오늘" : "DAILY");

        if (homeHubTitleText != null) homeHubTitleText.text = korean ? "로비" : "LOBBY";
        if (homeHubPlayerText != null) homeHubPlayerText.text = korean ? "플레이어" : "PLAYER";
        if (setupTitleText != null) setupTitleText.text = korean ? "게임 설정" : "GAME SETTINGS";
        if (languageTitleText != null) languageTitleText.text = korean ? "언어 설정" : "LANGUAGE";
        if (profileTitleText != null) profileTitleText.text = korean ? "프로필 만들기" : "CREATE PLAYER";
        if (myPageTitleText != null) myPageTitleText.text = korean ? "마이페이지" : "MY PAGE";
        if (shopTitleText != null) shopTitleText.text = korean ? "코인 상점" : "COIN SHOP";
        if (inputPlaceholder != null) inputPlaceholder.text = korean ? "여기에 입력..." : "TYPE HERE...";

        UpdateModeDescription();
        UpdateHighScoreText();
        UpdateProfileSummary();
        UpdateCoinText();
        UpdateMyPage();
        UpdateShopText();
        UpdateHud();
    }
}
