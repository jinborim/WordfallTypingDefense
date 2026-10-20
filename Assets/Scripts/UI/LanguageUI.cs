using UnityEngine.UI;
using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void SetLanguage(Language selectedLanguage)
    {
        language = selectedLanguage;
        PlayerPrefs.SetInt("Wordfall.Language", (int)selectedLanguage);
        PlayerPrefs.Save();
        bool korean = language == Language.Korean;

        if (titleText != null) titleText.text = "WORDFALL";
        if (subtitleText != null) subtitleText.text = korean ? "\uD55C\uAD6D\uC5B4 & \uC601\uC5B4 \uD0C0\uC790 \uB514\uD39C\uC2A4" : "KOREAN & ENGLISH TYPING DEFENSE";
        if (descriptionText != null) descriptionText.text = korean ? "\uB5A8\uC5B4\uC9C0\uB294 \uB2E8\uC5B4\uB97C \uBE60\uB974\uAC8C \uC785\uB825\uD574 \uAE30\uC9C0\uB97C \uC9C0\uD0A4\uC138\uC694." : "Type the falling words before they reach the bottom.";
        SetButtonText(mainStartButton, korean ? "\uAC8C\uC784 \uC2DC\uC791" : "START GAME");
        SetButtonText(languageOpenButton, korean ? "\uC5B8\uC5B4 \uC124\uC815" : "LANGUAGE");
        if (loginTitleText != null) loginTitleText.text = korean ? "\uB85C\uADF8\uC778" : "LOGIN";
        if (loginMessageText != null) loginMessageText.text = korean ? "\uAC8C\uC2A4\uD2B8 \uB610\uB294 Google\uB85C \uB85C\uADF8\uC778\uD558\uC138\uC694." : "Choose Guest or Google to continue.";
        SetButtonText(guestLoginButton, korean ? "\uAC8C\uC2A4\uD2B8 \uB85C\uADF8\uC778" : "GUEST LOGIN");
        SetButtonText(googleLoginButton, korean ? "Google \uB85C\uADF8\uC778" : "GOOGLE LOGIN");
        SetButtonText(loginCloseButton, korean ? "\uB2EB\uAE30" : "CLOSE");
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
        if (updateTitleText != null) updateTitleText.text = korean ? "\uC5C5\uB370\uC774\uD2B8 \uC548\uB0B4" : "UPDATE NOTICE";
        if (attendanceTitleText != null) attendanceTitleText.text = korean ? "\uCD9C\uC11D \uBCF4\uC0C1" : "ATTENDANCE REWARD";
        if (attendanceBodyText != null) attendanceBodyText.text = korean ? "\uC624\uB298\uC758 \uCD9C\uC11D \uCF54\uC778\uC744 \uBC1B\uC558\uC2B5\uB2C8\uB2E4!" : "TODAY'S ATTENDANCE COINS RECEIVED!";
        if (attendanceCoinText != null) attendanceCoinText.text = "+10 COINS";
        if (privacyTitleText != null) privacyTitleText.text = korean ? "\uAC1C\uC778\uC815\uBCF4 \uCC98\uB9AC\uBC29\uCE68" : "PRIVACY POLICY";
        if (privacyMessageText != null) privacyMessageText.text = korean
            ? "\uC2DC\uD589\uC77C: 2026\uB144 10\uC6D4 20\uC77C\n\n1. \uCC98\uB9AC\uBAA9\uC801\nWordfall\uC740 \uAC8C\uC784 \uC9C4\uD589, \uB85C\uADF8\uC778 \uC0C1\uD0DC \uC720\uC9C0, \uAC8C\uC784 \uC815\uBCF4 \uC800\u7AE0, \uC0C1\uC810\u00B7\uD504\uB85C\uD30C\uC77C \uAE30\uB2A5 \uC81C\uACF5\uC744 \uC704\uD574 \uC815\uBCF4\uB97C \uCC98\uB9AC\uD569\uB2C8\uB2E4.\n\n2. \uCC98\uB9AC\uD558\uB294 \uC815\uBCF4\n\u00B7 \uAC8C\uC2A4\uD2B8 \uB85C\uADF8\uC778: Firebase\uAC00 \uBC1C\uAE09\uD558\uB294 \uC775\uBA85 UID\n\u00B7 Google \uB85C\uADF8\uC778: Google/Firebase \uACC4\uC815 UID\uC640 \uD45C\uC2DC \uC774\uB984 \uB4F1 \uC778\uC99D\uC5D0 \uD544\uC694\uD55C \uAE30\uBCF8 \uD504\uB85C\uD30C\uC77C \uC815\uBCF4\n\u00B7 \uB85C\uCEEC \uC800\u7AE0: \uB2C9\uB124\uC784, \uC120\uD0DD \uCE90\uB9AD\uD130, \uCF54\uC778, \uBCC0\uACBD\uAD8C, \uAC8C\uC784 \uD1B5\uACC4, \uCD5C\uACE0\uC810, \uC5B8\uC5B4\uC640 \uC124\uC815\n\u00B7 \uC11C\uBE44\uC2A4 \uC0AC\uC6A9 \uC815\uBCF4: Firebase \uC778\uC99D \uC11C\uBE44\uC2A4\uC640 \uC624\uB958\u00B7\uBCF4\uC548 \uAE30\uB85D\n\n3. \uCC98\uB9AC\uBC0F \uBCF4\uC720\uAE30\uAC04\n\uB85C\uCEEC \uB370\uC774\uD130\uB294 \uC571 \uC0AD\uC81C, \uBE0C\uB77C\uC6B0\uC800 \uB370\uC774\uD130 \uC0AD\uC81C, \uD68C\uC6D0 \uD0C8\uD1F4 \uCC98\uB9AC \uC2DC\uAE4C\uC9C0 \uBCF4\uC720\uD569\uB2C8\uB2E4. Firebase \uC778\uC99D \uACC4\uC815\uC740 \uC0AC\uC6A9\uC790\uC758 \uD68C\uC6D0 \uD0C8\uD1F4 \uC694\uCCAD\uC774\uB098 \uAD00\uB828 \uBC95\uB839\uC5D0 \uB530\uB77C \uC0AD\uC81C\uD569\uB2C8\uB2E4.\n\n4. \uC81C3\uC790 \uC81C\uACF5\u00B7\uCC98\uB9AC\nGoogle Firebase\uC758 \uC778\uC99D \uC11C\uBE44\uC2A4\uB97C \uC0AC\uC6A9\uD558\uBA70, \uC778\uC99D\uACFC \uC11C\uBE44\uC2A4 \uC6B4\uC601\uC5D0 \uD544\uC694\uD55C \uBC94\uC704\uC5D0\uC11C\uB9CC \uC815\uBCF4\uAC00 \uCC98\uB9AC\uB429\uB2C8\uB2E4. \uC815\uBCF4\uB97C \uD310\uB9E4\uD558\uAC70\uB098 \uAD11\uACE0\uB300\uC0C1 \uD504\uB85C\uD30C\uC77C\uB9C1\uC5D0 \uC0AC\uC6A9\uD558\uC9C0 \uC54A\uC2B5\uB2C8\uB2E4. \uAD6D\uC678 \uC11C\uBC84\uC5D0\uC11C \uCC98\uB9AC\uB420 \uC218 \uC788\uC73C\uBA70, \uC804\uC1A1\uC740 HTTPS \uB4F1 \uBCF4\uC548 \uC5F0\uACB0\uC744 \uC0AC\uC6A9\uD569\uB2C8\uB2E4.\n\n5. \uC815\uBCF4\uC8FC\uCCB4\uC758 \uAD8C\uB9AC\n\uC774\uC6A9\uC790\uB294 \uC790\uC2E0\uC758 \uC815\uBCF4\uC5D0 \uB300\uD574 \uC5F4\uB78C, \uC815\uC815, \uC0AD\uC81C, \uCC98\uB9AC\uC815\uC9C0\uB97C \uC694\uCCAD\uD560 \uC218 \uC788\uC2B5\uB2C8\uB2E4. \uC571 \uB0B4 \uD68C\uC6D0 \uD0C8\uD1F4\uB97C \uC0AC\uC6A9\uD558\uAC70\uB098 Chrome Web Store \uAC8C\uC2DC \uD398\uC774\uC9C0\uC758 \uAC1C\uBC1C\uC790 \uBB38\uC758\uCC98\uB85C \uC694\uCCAD\uD560 \uC218 \uC788\uC2B5\uB2C8\uB2E4.\n\n6. \uC544\uB3D9 \uBC0F \uBC95\uC815\uB300\uB9AC\uC778\n\uB9CC 14\uC138 \uBBF8\uB9CC\uC758 \uC774\uC6A9\uC790\uB294 \uBC95\uC815\uB300\uB9AC\uC778\uC758 \uB3D9\uC758\uAC00 \uD544\uC694\uD560 \uC218 \uC788\uC2B5\uB2C8\uB2E4.\n\n7. \uBCC0\uACBD\n\uCC98\uB9AC\uBC29\uCE68\uC744 \uBCC0\uACBD\uD558\uBA74 \uC571 \uB0B4 \uACF5\uC9C0\uC640 \uCD5C\uC2E0 \uBC84\uC804\uC5D0 \uBC18\uC601\uD569\uB2C8\uB2E4.\n\n\uBB38\uC758: Chrome Web Store \uAC8C\uC2DC \uD398\uC774\uC9C0\uC5D0 \uD45C\uC2DC\uD55C \uAC1C\uBC1C\uC790 \uBB38\uC758\uCC98"
            : "Effective date: October 20, 2026\n\n1. Purpose\nWordfall processes information to provide gameplay, sign-in status, local game-data storage, shop, and profile features.\n\n2. Information we process\n- Guest sign-in: the anonymous UID issued by Firebase.\n- Google sign-in: the Google/Firebase account UID and basic profile information needed for authentication, such as display name.\n- Local data: nickname, selected character, coins, nickname tickets, game statistics, high score, language, and settings.\n- Service information: Firebase authentication and security/error records.\n\n3. Retention\nLocal data is retained until the app data is deleted, browser data is cleared, or account deletion is processed. Firebase authentication accounts are deleted upon a user request or as required by applicable law.\n\n4. Sharing and processing\nWe use Google Firebase for authentication and related service operations. Information is processed only as necessary to provide authentication and the game. We do not sell information or use it for targeted advertising. Data may be processed on servers outside your country and is transmitted over secure HTTPS connections.\n\n5. Your rights\nYou may request access, correction, deletion, or suspension of processing. Use the in-game account deletion feature or contact the developer through the contact information shown on the Chrome Web Store listing.\n\n6. Children\nUsers under 14 may need consent from a legal representative under applicable law.\n\n7. Changes\nChanges to this policy will be announced in the app and reflected in a future version.\n\nContact: the developer contact shown on the Chrome Web Store listing.";
        ApplyExpandedPrivacyPolicy(korean);
        if (withdrawTitleText != null) withdrawTitleText.text = korean ? "\uD68C\uC6D0 \uD0C8\uD1F4" : "DELETE ACCOUNT";
        if (withdrawMessageText != null) withdrawMessageText.text = korean
            ? "\uC815\uB9D0 \uD0C8\uD1F4\uD558\uC2DC\uACA0\uC2B5\uB2C8\uAE4C?\n\uC800\uC7A5\uB41C \uACC4\uC815 \uB370\uC774\uD130\uAC00 \uC0AD\uC81C\uB429\uB2C8\uB2E4."
            : "Are you sure you want to delete your account?\nAll saved account data will be deleted.";
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
        if (nicknameTicketTitleText != null) nicknameTicketTitleText.text = korean ? "\uB2C9\uB124\uC784 \uBCC0\uACBD\uAD8C \uBD80\uC871" : "NICKNAME TICKET REQUIRED";
        if (nicknameTicketMessageText != null) nicknameTicketMessageText.text = korean ? "\uB2C9\uB124\uC784\uC744 \uBCC0\uACBD\uD558\uB824\uBA74 \uBCC0\uACBD\uAD8C\uC774 \uD544\uC694\uD569\uB2C8\uB2E4." : "You need a nickname ticket to change your nickname.";
        SetButtonText(nicknameTicketCloseButton, korean ? "\uD655\uC778" : "OK");
        SetButtonText(nicknameTicketShopButton, korean ? "\uC0C1\uC810 \uAC00\uAE30" : "GO TO SHOP");
        SetButtonText(announcementCloseButton, korean ? "\uB2EB\uAE30" : "CLOSE");
        SetButtonText(gameOverRetryButton, korean ? "\uB2E4\uC2DC\uD558\uAE30" : "RETRY");
        SetButtonText(gameOverSetupButton, korean ? "\uB098\uAC00\uAE30" : "EXIT");
        UpdateModeDescription();
        UpdateHighScoreText();
        UpdateProfileSummary();
        UpdateCoinText();
        UpdateMyPage();
        UpdateShopText();
        UpdateHud();
    }
}

