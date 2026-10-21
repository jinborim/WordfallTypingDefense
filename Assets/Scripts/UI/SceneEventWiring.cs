using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void WireSceneEvents()
    {
        mainStartButton?.onClick.AddListener(ShowLoginOrLobby);
        languageOpenButton?.onClick.AddListener(ShowLanguagePanel);
        homeHubStartButton?.onClick.AddListener(ShowSetupPanel);
        homeHubMyPageButton?.onClick.AddListener(ShowMyPage);
        homeHubShopButton?.onClick.AddListener(ShowShopFromHome);
        announcementButton?.onClick.AddListener(ShowAnnouncementPanel);
        homeHubProfileButton?.onClick.AddListener(ShowProfileEditFromHome);
        privacyButton?.onClick.AddListener(ShowPrivacyPanel);
        withdrawButton?.onClick.AddListener(ShowWithdrawConfirm);
        homeHubLogoutButton?.onClick.AddListener(Logout);
        homeHubMenuToggleButton?.onClick.AddListener(ToggleHomeHubMenu);
        for (var i = 0; i < difficultyButtons.Length; i++) AddDifficultyListener(i);
        for (var i = 0; i < modeButtons.Length; i++)
        {
            var index = i;
            modeButtons[i]?.onClick.AddListener(() => SetGameMode((GameMode)index));
        }
        startButton?.onClick.AddListener(StartGame);
        setupBackButton?.onClick.AddListener(CloseSetupPanel);
        languageButtons?[0]?.onClick.AddListener(() => SelectLanguageAndClose(Language.Korean));
        languageButtons?[1]?.onClick.AddListener(() => SelectLanguageAndClose(Language.English));
        languageCloseButton?.onClick.AddListener(CloseLanguagePanel);
        for (var i = 0; i < characterButtons.Length; i++)
        {
            var index = i;
            characterButtons[i]?.onClick.AddListener(() => SelectCharacter(index));
        }
        createProfileButton?.onClick.AddListener(CreateProfile);
        myPageCloseButton?.onClick.AddListener(CloseMyPage);
        myPageShopButton?.onClick.AddListener(ShowShopPanel);
        myPageProfileButton?.onClick.AddListener(ShowProfileEdit);
        deleteAccountButton?.onClick.AddListener(ShowWithdrawConfirm);
        attendanceCloseButton?.onClick.AddListener(CloseAttendancePanel);
        shopAllCategoryButton?.onClick.AddListener(() => SetShopCategory(0));
        shopProfileCategoryButton?.onClick.AddListener(() => SetShopCategory(1));
        shopItemCategoryButton?.onClick.AddListener(() => SetShopCategory(2));
        nicknameTicketButton?.onClick.AddListener(BuyNicknameTicket);
        knightShopButton?.onClick.AddListener(() => BuyCharacter(0));
        mageShopButton?.onClick.AddListener(() => BuyCharacter(1));
        hackerShopButton?.onClick.AddListener(() => BuyCharacter(2));
        shopCloseButton?.onClick.AddListener(CloseShopPanel);
        insufficientCoinsCloseButton?.onClick.AddListener(CloseInsufficientCoinsPanel);
        purchaseCompleteCloseButton?.onClick.AddListener(ClosePurchaseCompletePanel);
        nicknameTicketCloseButton?.onClick.AddListener(CloseNicknameTicketPanel);
        nicknameTicketShopButton?.onClick.AddListener(GoToShopFromNicknameTicketPanel);
        announcementCloseButton?.onClick.AddListener(CloseAnnouncementPanel);
        privacyCloseButton?.onClick.AddListener(ClosePrivacyPanel);
        withdrawConfirmButton?.onClick.AddListener(ConfirmDeleteAccount);
        withdrawCancelButton?.onClick.AddListener(CloseWithdrawConfirm);
        guestLoginButton?.onClick.AddListener(LoginAsGuest);
        googleLoginButton?.onClick.AddListener(LoginWithGoogle);
        loginCloseButton?.onClick.AddListener(CloseLoginPanel);
        updateLaterButton?.onClick.AddListener(CloseUpdatePanel);
        updateNowButton?.onClick.AddListener(OpenUpdatePage);
        menuButton?.onClick.AddListener(TogglePause);
        inputField?.onEndEdit.AddListener(HandleSubmitted);
        resumeButton?.onClick.AddListener(TogglePause);
        pauseMyPageButton?.onClick.AddListener(ShowMyPage);
        pauseMenuButton?.onClick.AddListener(ShowMainMenu);
        gameOverRetryButton?.onClick.AddListener(StartGame);
        gameOverSetupButton?.onClick.AddListener(ExitToLobbyFromGameOver);
    }

    private void AddDifficultyListener(int index)
    {
        if (difficultyButtons == null || index >= difficultyButtons.Length || difficultyButtons[index] == null) return;
        difficultyButtons[index].onClick.AddListener(() => SetDifficulty(index));
    }
}
