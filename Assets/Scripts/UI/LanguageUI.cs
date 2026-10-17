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
        SetButtonText(languageOpenButton, korean ? "\uC5B8\uC5B4 \uC124\uC815" : "LANGUAGE");
        SetButtonText(homeHubStartButton, korean ? "\uAC8C\uC784 \uC2DC\uC791" : "START GAME");
        SetButtonText(startButton, korean ? "\uAC8C\uC784 \uC2DC\uC791" : "START GAME");
        SetButtonText(setupBackButton, korean ? "\uB4A4\uB85C" : "BACK");
        SetButtonText(languageCloseButton, korean ? "\uB2EB\uAE30" : "CLOSE");
        SetButtonText(languageButtons != null && languageButtons.Length > 0 ? languageButtons[0] : null, korean ? "\uD55C\uAD6D\uC5B4" : "KOREAN");
        SetButtonText(languageButtons != null && languageButtons.Length > 1 ? languageButtons[1] : null, korean ? "\uC601\uC5B4" : "ENGLISH");
        SetButtonText(menuButton, korean ? "\uC77C\uC2DC\uC815\uC9C0" : "PAUSE");
        SetButtonText(resumeButton, korean ? "\uACC4\uC18D\uD558\uAE30" : "RESUME");
        SetButtonText(pauseMenuButton, korean ? "\uBA54\uC778\uC73C\uB85C" : "MENU");
        SetButtonText(pauseMyPageButton, korean ? "\uB9C8\uC774\uD398\uC774\uC9C0" : "MY PAGE");
        SetButtonText(difficultyButtons != null && difficultyButtons.Length > 0 ? difficultyButtons[0] : null, korean ? "\uC26C\uC6C0" : "EASY");
        SetButtonText(difficultyButtons != null && difficultyButtons.Length > 1 ? difficultyButtons[1] : null, korean ? "\uBCF4\uD1B5" : "NORMAL");
        SetButtonText(difficultyButtons != null && difficultyButtons.Length > 2 ? difficultyButtons[2] : null, korean ? "\uC5B4\uB824\uC6C0" : "HARD");
        SetButtonText(modeButtons != null && modeButtons.Length > 0 ? modeButtons[0] : null, korean ? "\uD63C\uD569" : "MIX");
        SetButtonText(modeButtons != null && modeButtons.Length > 1 ? modeButtons[1] : null, korean ? "\uD55C\uAD6D\uC5B4" : "KOR");
        SetButtonText(modeButtons != null && modeButtons.Length > 2 ? modeButtons[2] : null, korean ? "\uC601\uC5B4" : "ENG");
        SetButtonText(modeButtons != null && modeButtons.Length > 3 ? modeButtons[3] : null, korean ? "60\uCD08" : "60 SEC");
        SetButtonText(modeButtons != null && modeButtons.Length > 4 ? modeButtons[4] : null, korean ? "\uC624\uB298" : "DAILY");

        if (homeHubTitleText != null) homeHubTitleText.text = korean ? "\uB85C\uBE44" : "LOBBY";
        if (homeHubPlayerText != null) homeHubPlayerText.text = korean ? "\uD50C\uB808\uC774\uC5B4" : "PLAYER";
        if (homeHubStartLabelText != null) homeHubStartLabelText.text = korean ? "\uAC8C\uC784 \uC2DC\uC791" : "START GAME";
        if (homeHubQuickStartLabelText != null) homeHubQuickStartLabelText.text = korean ? "\uBE60\uB978 \uC2DC\uC791" : "QUICK START";
        if (homeHubBackLabelText != null) homeHubBackLabelText.text = korean ? "\uB4A4\uB85C" : "BACK";
        if (setupTitleText != null) setupTitleText.text = korean ? "\uAC8C\uC784 \uC124\uC815" : "GAME SETTINGS";
        if (difficultyText != null) difficultyText.text = korean ? "\uB09C\uC774\uB3C4" : "DIFFICULTY";
        if (modeText != null) modeText.text = korean ? "\uBAA8\uB4DC" : "MODE";
        if (languageTitleText != null) languageTitleText.text = korean ? "\uC5B8\uC5B4 \uC124\uC815" : "LANGUAGE";
        if (profileTitleText != null) profileTitleText.text = korean ? "\uD504\uB85C\uD544 \uB9CC\uB4E4\uAE30" : "CREATE PLAYER";
        if (myPageTitleText != null) myPageTitleText.text = korean ? "\uB9C8\uC774\uD398\uC774\uC9C0" : "MY PAGE";
        if (shopTitleText != null) shopTitleText.text = korean ? "\uCF54\uC778 \uC0C1\uC810" : "COIN SHOP";
        if (homeHubMyPageLabelText != null) homeHubMyPageLabelText.text = korean ? "\uB9C8\uC774\uD398\uC774\uC9C0" : "MY PAGE";
        if (homeHubShopLabelText != null) homeHubShopLabelText.text = korean ? "\uC0C1\uC810" : "SHOP";
        if (homeHubAnnouncementLabelText != null) homeHubAnnouncementLabelText.text = korean ? "\uACF5\uC9C0\uC0AC\uD56D" : "ANNOUNCEMENTS";
        if (homeHubProfileLabelText != null) homeHubProfileLabelText.text = korean ? "\uD504\uB85C\uD544 \uC218\uC815" : "PROFILE";
        if (homeHubPrivacyLabelText != null) homeHubPrivacyLabelText.text = korean ? "\uAC1C\uC778\uC815\uBCF4" : "PRIVACY";
        if (homeHubWithdrawLabelText != null) homeHubWithdrawLabelText.text = korean ? "\uD68C\uC6D0 \uD0C8\uD1F4" : "WITHDRAW";
        SetButtonText(myPageCloseButton, korean ? "\uB2EB\uAE30" : "CLOSE");
        SetButtonText(myPageShopButton, korean ? "\uC0C1\uC810" : "SHOP");
        SetButtonText(myPageProfileButton, korean ? "\uD504\uB85C\uD544 \uC218\uC815" : "EDIT PROFILE");
        SetButtonText(deleteAccountButton, korean ? "\uD68C\uC6D0 \uD0C8\uD1F4" : "DELETE ACCOUNT");
        SetButtonText(shopAllCategoryButton, korean ? "\uC804\uCCB4" : "ALL");
        SetButtonText(shopProfileCategoryButton, korean ? "\uD504\uB85C\uD544" : "PROFILE");
        SetButtonText(shopItemCategoryButton, korean ? "\uC544\uC774\uD15C" : "ITEMS");
        SetButtonText(shopCloseButton, korean ? "\uB2EB\uAE30" : "CLOSE");
        SetButtonText(privacyCloseButton, korean ? "\uB2EB\uAE30" : "CLOSE");
        SetButtonText(withdrawConfirmButton, korean ? "\uD655\uC778" : "CONFIRM");
        SetButtonText(withdrawCancelButton, korean ? "\uCDE8\uC18C" : "CANCEL");
        SetButtonText(updateLaterButton, korean ? "\uB098\uC911\uC5D0" : "LATER");
        SetButtonText(updateNowButton, korean ? "\uC5C5\uB370\uC774\uD2B8" : "UPDATE");
        if (inputPlaceholder != null) inputPlaceholder.text = korean ? "\uC5EC\uAE30\uC5D0 \uC785\uB825..." : "TYPE HERE...";

        SetButtonText(createProfileButton, korean ? "\uD504\uB85C\uD544 \uC800\uC7A5" : "SAVE PROFILE");
        SetButtonText(attendanceCloseButton, korean ? "\uD655\uC778" : "OK");
        SetButtonText(insufficientCoinsCloseButton, korean ? "\uD655\uC778" : "OK");
        if (purchaseCompleteTitleText != null) purchaseCompleteTitleText.text = korean ? "\uAD6C\uB9E4 \uC644\uB8CC" : "PURCHASE COMPLETE";
        if (purchaseCompleteMessageText != null) purchaseCompleteMessageText.text = korean ? "\uC0C1\uD488\uC744 \uAD6C\uB9E4\uD588\uC2B5\uB2C8\uB2E4." : "Purchase complete.";
        SetButtonText(purchaseCompleteCloseButton, korean ? "\uD655\uC778" : "OK");
        SetButtonText(announcementCloseButton, korean ? "\uB2EB\uAE30" : "CLOSE");
        SetButtonText(gameOverRetryButton, korean ? "\uB2E4\uC2DC\uD558\uAE30" : "RETRY");
        SetButtonText(gameOverSetupButton, korean ? "\uAC8C\uC784 \uC124\uC815" : "GAME SETTINGS");
        UpdateModeDescription();
        UpdateHighScoreText();
        UpdateProfileSummary();
        UpdateCoinText();
        UpdateMyPage();
        UpdateShopText();
        UpdateHud();
    }
}

