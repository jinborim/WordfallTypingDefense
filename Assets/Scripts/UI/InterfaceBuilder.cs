using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    // Syntax repair pass.
    private void BuildInterface()
    {
        var canvasObject = new GameObject("WordfallCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasObject.transform.SetParent(transform, false);
        var canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.pixelPerfect = true;
        var scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280f, 720f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
        EnsureInputSystemEventSystem();

        mainPanel = CreatePanel("MainPanel", canvasObject.transform, new Color32(16, 27, 52, 255));
        gamePanel = CreatePanel("GamePanel", canvasObject.transform, new Color32(9, 18, 38, 255));
        ApplyPixelBackground(mainPanel);
        mainAmbientMotion = CreateMainAmbientMotion(mainPanel);
        ApplyPixelBackground(gamePanel);
        titleText = CreateText("WORDFALL", mainPanel.transform, 64, new Color32(241, 247, 255, 0), new Vector2(0.5f, 0.65f), new Vector2(0.5f, 0.65f));
        CreateImage("WordfallEnglishLogo", mainPanel.transform, "Pixel/wordfall-logo-en", new Vector2(0.5f, 0.65f), new Vector2(430f, 92f));
        subtitleText = CreateText("\uD55C\uAD6D\uC5B4 & \uC601\uC5B4 \uD0C0\uC790 \uB514\uD39C\uC2A4", mainPanel.transform, 22, new Color32(158, 180, 220, 255), new Vector2(0.5f, 0.53f), new Vector2(0.5f, 0.53f));
        descriptionText = CreateText("\uB5A8\uC5B4\uC9C0\uB294 \uB2E8\uC5B4\uB97C \uBE60\uB974\uAC8C \uC785\uB825\uD574 \uAE30\uC9C0\uB97C \uC9C0\uD0A4\uC138\uC694.", mainPanel.transform, 18, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.48f), new Vector2(0.5f, 0.48f));
        mainStartButton = CreateButton("\uAC8C\uC784 \uC2DC\uC791", mainPanel.transform, new Vector2(0.38f, 0.21f), new Vector2(340f, 78f), new Color32(255, 196, 61, 255));
        mainStartButton.onClick.AddListener(ShowHomeHub);
        languageOpenButton = CreateButton("LANGUAGE", mainPanel.transform, new Vector2(0.66f, 0.21f), new Vector2(250f, 78f), new Color32(58, 80, 122, 255));
        languageOpenButton.onClick.AddListener(ShowLanguagePanel);
        profileSummaryText = CreateText("", mainPanel.transform, 16, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.105f), new Vector2(0.5f, 0.105f));

        homeHubPanel = CreatePanel("HomeHubPanel", mainPanel.transform, new Color32(0, 0, 0, 0));
        SetRect(homeHubPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelLobbyBackground(homeHubPanel);
        ApplyModalDim(homeHubPanel);
        homeHubTitleText = CreateText("\uB85C\uBE44", homeHubPanel.transform, 40, Color.white, new Vector2(0.5f, 0.87f), new Vector2(0.5f, 0.87f));
        homeHubCharacterImage = CreateImage("HomeHubCharacter", homeHubPanel.transform, GetLobbyCharacterResourcePath(), new Vector2(0.5f, 0.42f), new Vector2(188f, 308f));
        homeHubCharacterImage.rectTransform.anchoredPosition = new Vector2(0f, -26f);
        homeHubCharacterImage.raycastTarget = false;
        homeHubInfoPanel = CreatePanel("HomeHubPlayerInfo", homeHubPanel.transform, new Color32(5, 18, 48, 245));
        SetRect(homeHubInfoPanel.GetComponent<RectTransform>(), new Vector2(0.18f, 0.78f), new Vector2(0.18f, 0.78f), Vector2.zero, new Vector2(500f, 167f));
        ApplyPixelPlayerInfoCard(homeHubInfoPanel);
        homeHubPlayerText = CreateText("PLAYER", homeHubPanel.transform, 22, new Color32(235, 241, 255, 255), new Vector2(0.18f, 0.795f), new Vector2(0.18f, 0.795f));
        SetRect(homeHubPlayerText.rectTransform, new Vector2(0.18f, 0.795f), new Vector2(0.18f, 0.795f), Vector2.zero, new Vector2(440f, 34f));
        homeHubPlayerText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        homeHubPlayerText.alignment = TextAnchor.MiddleCenter;
        homeHubCoinText = CreateText("COINS 0", homeHubPanel.transform, 20, new Color32(255, 211, 94, 255), new Vector2(0.18f, 0.735f), new Vector2(0.18f, 0.735f));
        SetRect(homeHubCoinText.rectTransform, new Vector2(0.18f, 0.735f), new Vector2(0.18f, 0.735f), Vector2.zero, new Vector2(440f, 30f));
        homeHubCoinText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        homeHubCoinText.alignment = TextAnchor.MiddleCenter;
        homeHubStartButton = CreateButton("\uAC8C\uC784 \uC2DC\uC791", homeHubPanel.transform, new Vector2(0.5f, 0.10f), new Vector2(290f, 58f), new Color32(67, 149, 255, 255));
        homeHubStartButton.onClick.AddListener(ShowSetupPanel);

        lobbyChatPanel = CreatePanel("LobbyCharacterDialogue", homeHubPanel.transform, new Color32(9, 18, 38, 248));
        SetRect(lobbyChatPanel.GetComponent<RectTransform>(), new Vector2(0.73f, 0.54f), new Vector2(0.73f, 0.54f), Vector2.zero, new Vector2(560f, 182f));
        lobbyChatPanel.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        ApplyPixelLobbyDialogueWindow(lobbyChatPanel);
        lobbyChatPanel.GetComponent<Image>().raycastTarget = false;
        lobbyChatMessageText = CreateText("", lobbyChatPanel.transform, 18, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.50f), new Vector2(0.5f, 0.50f));
        SetRect(lobbyChatMessageText.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(448f, 96f));
        lobbyChatMessageText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        lobbyChatMessageText.rectTransform.localPosition = Vector3.zero;
        lobbyChatMessageText.alignment = TextAnchor.MiddleCenter;
        lobbyChatMessageText.horizontalOverflow = HorizontalWrapMode.Wrap;
        lobbyChatMessageText.verticalOverflow = VerticalWrapMode.Overflow;
        lobbyChatMessageText.resizeTextForBestFit = true;
        lobbyChatMessageText.resizeTextMinSize = 14;
        lobbyChatMessageText.resizeTextMaxSize = 18;
        lobbyChatPanel.SetActive(false);

        homeHubMenuPanel = CreatePanel("HomeHubSideMenu", homeHubPanel.transform, new Color32(4, 10, 26, 220));
        var homeHubMenuRect = homeHubMenuPanel.GetComponent<RectTransform>();
        SetRect(homeHubMenuRect, new Vector2(0f, 0f), new Vector2(0f, 1f), Vector2.zero, new Vector2(560f, 0f));
        homeHubMenuRect.pivot = new Vector2(0f, 0.5f);
        homeHubMenuRect.anchoredPosition = new Vector2(-560f, 0f);
        var menuOutline = homeHubMenuPanel.AddComponent<Outline>();
        menuOutline.effectColor = new Color32(20, 66, 126, 220);
        menuOutline.effectDistance = new Vector2(3f, 0f);

        homeHubMyPageButton = CreateImageAction("HomeHubMyPageAction", homeHubMenuPanel.transform, "Pixel/score-hud-frame", "\uB9C8\uC774\uD398\uC774\uC9C0", new Vector2(0.50f, 0.78f), new Vector2(170f, 130f), new Vector2(82f, 70f), out homeHubMyPageLabelText);
        homeHubMyPageButton.onClick.AddListener(ShowMyPage);
        homeHubShopButton = CreateImageAction("HomeHubShopAction", homeHubMenuPanel.transform, "Pixel/shop-store-icon", "SHOP", new Vector2(0.80f, 0.78f), new Vector2(170f, 130f), new Vector2(82f, 78f), out homeHubShopLabelText);
        homeHubShopButton.onClick.AddListener(ShowShopFromHome);
        announcementButton = CreateImageAction("HomeHubAnnouncementAction", homeHubMenuPanel.transform, "Pixel/announcement-icon", "ANNOUNCEMENTS", new Vector2(0.20f, 0.48f), new Vector2(170f, 130f), new Vector2(72f, 72f), out homeHubAnnouncementLabelText);
        announcementButton.onClick.AddListener(ShowAnnouncementPanel);
        homeHubProfileButton = CreateImageAction("HomeHubProfileAction", homeHubMenuPanel.transform, "Pixel/character-profile-frame", "PROFILE", new Vector2(0.20f, 0.78f), new Vector2(170f, 130f), new Vector2(82f, 82f), out homeHubProfileLabelText);
        homeHubProfileButton.onClick.AddListener(ShowProfileEditFromHome);
        privacyButton = CreateImageAction("HomeHubPrivacyAction", homeHubMenuPanel.transform, "Pixel/announcement-icon", "PRIVACY", new Vector2(0.50f, 0.48f), new Vector2(170f, 130f), new Vector2(72f, 72f), out homeHubPrivacyLabelText);
        privacyButton.onClick.AddListener(ShowPrivacyPanel);
        withdrawButton = CreateImageAction("HomeHubWithdrawAction", homeHubMenuPanel.transform, "Pixel/character-profile-frame", "\uD68C\uC6D0 \uD0C8\uD1F4", new Vector2(0.80f, 0.48f), new Vector2(170f, 130f), new Vector2(72f, 72f), out homeHubWithdrawLabelText);
        withdrawButton.onClick.AddListener(ShowWithdrawConfirm);
        homeHubMenuToggleButton = CreateHamburgerButton("HomeHubMenuToggle", homeHubPanel.transform, new Vector2(0.04f, 0.50f), new Vector2(62f, 62f));
        var hamburgerIconTransform = homeHubMenuToggleButton.transform.Find("HomeHubMenuToggleIcon");
        homeHubHamburgerIcon = hamburgerIconTransform != null ? hamburgerIconTransform.GetComponent<Image>() : null;
        homeHubCloseIcon = CreateMenuCloseIcon(homeHubMenuToggleButton.transform);
        homeHubCloseIcon.SetActive(false);
        homeHubMenuToggleButton.onClick.AddListener(ToggleHomeHubMenu);
        homeHubMenuPanel.SetActive(false);
        homeHubPanel.SetActive(false);

        setupPanel = CreatePanel("SetupPanel", mainPanel.transform, new Color32(9, 18, 38, 245));
        SetRect(setupPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        setupPanel.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        ApplyModalDim(setupPanel);
        setupWindow = CreatePanel("SettingsWindow", setupPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(setupWindow.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(1220f, 680f));
        ApplyPixelSettingsWindow(setupWindow);
        setupTitleText = CreateText("GAME SETTINGS", setupPanel.transform, 38, Color.white, new Vector2(0.5f, 0.84f), new Vector2(0.5f, 0.84f));
        difficultyText = CreateText("\uB09C\uC774\uB3C4 \uBCF4\uD1B5", setupPanel.transform, 22, new Color32(255, 211, 94, 255), new Vector2(0.27f, 0.69f), new Vector2(0.27f, 0.69f));
        difficultyText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        difficultyText.rectTransform.sizeDelta = new Vector2(260f, 36f);
        difficultyButtons = new Button[3];
        difficultyButtons[0] = CreateButton("\uC26C\uC6C0", setupPanel.transform, new Vector2(0.27f, 0.58f), new Vector2(260f, 58f), new Color32(55, 102, 149, 255));
        difficultyButtons[1] = CreateButton("\uBCF4\uD1B5", setupPanel.transform, new Vector2(0.27f, 0.46f), new Vector2(260f, 58f), new Color32(67, 149, 255, 255));
        difficultyButtons[2] = CreateButton("\uC5B4\uB824\uC6C0", setupPanel.transform, new Vector2(0.27f, 0.34f), new Vector2(260f, 58f), new Color32(149, 71, 92, 255));
        difficultyButtons[0].onClick.AddListener(() => SetDifficulty(0));
        difficultyButtons[1].onClick.AddListener(() => SetDifficulty(1));
        difficultyButtons[2].onClick.AddListener(() => SetDifficulty(2));
        modeText = CreateText("\uBAA8\uB4DC \uD63C\uD569", setupPanel.transform, 20, new Color32(147, 232, 189, 255), new Vector2(0.70f, 0.69f), new Vector2(0.70f, 0.69f));
        modeText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        modeText.rectTransform.sizeDelta = new Vector2(420f, 36f);
        modeButtons = new Button[5];
        modeButtons[0] = CreateButton("\uD63C\uD569", setupPanel.transform, new Vector2(0.56f, 0.58f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        modeButtons[1] = CreateButton("\uD55C\uAD6D\uC5B4", setupPanel.transform, new Vector2(0.70f, 0.58f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        modeButtons[2] = CreateButton("ENG", setupPanel.transform, new Vector2(0.84f, 0.58f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        modeButtons[3] = CreateButton("60\uCD08", setupPanel.transform, new Vector2(0.63f, 0.42f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        modeButtons[4] = CreateButton("DAILY", setupPanel.transform, new Vector2(0.77f, 0.42f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        for (int i = 0; i < modeButtons.Length; i++)
        {
            int selectedMode = i;
            modeButtons[i].onClick.AddListener(() => SetGameMode((GameMode)selectedMode));
        }
        modeDescriptionText = CreateText("\uD55C\uAD6D\uC5B4\uC640 \uC601\uC5B4 \uB2E8\uC5B4\uAC00 \uD568\uAED8 \uB4F1\uC7A5\uD569\uB2C8\uB2E4.", setupPanel.transform, 16, new Color32(171, 190, 220, 255), new Vector2(0.70f, 0.30f), new Vector2(0.70f, 0.30f));
        modeDescriptionText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        modeDescriptionText.rectTransform.sizeDelta = new Vector2(520f, 42f);
        modeDescriptionText.alignment = TextAnchor.MiddleCenter;
        startButton = CreateButton("\uAC8C\uC784 \uC2DC\uC791", setupPanel.transform, new Vector2(0.62f, 0.18f), new Vector2(300f, 68f), new Color32(255, 196, 61, 255));
        startButton.onClick.AddListener(StartGame);
        setupBackButton = CreateButton("\uB4A4\uB85C", setupPanel.transform, new Vector2(0.38f, 0.18f), new Vector2(220f, 60f), new Color32(78, 160, 255, 255));
        setupBackButton.onClick.AddListener(CloseSetupPanel);
        for (int i = setupPanel.transform.childCount - 1; i >= 0; i--)
        {
            var child = setupPanel.transform.GetChild(i);
            if (child != setupWindow.transform && child.name != "ModalDim") child.SetParent(setupWindow.transform, false);
        }
        setupPanel.SetActive(false);

        languagePanel = CreatePanel("LanguagePanel", mainPanel.transform, new Color32(9, 18, 38, 245));
        SetRect(languagePanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(languagePanel);
        ApplyModalDim(languagePanel);
        languageTitleText = CreateText("LANGUAGE", languagePanel.transform, 42, Color.white, new Vector2(0.5f, 0.70f), new Vector2(0.5f, 0.70f));
        languageButtons = new Button[2];
        languageButtons[0] = CreateButton("\uD55C\uAD6D\uC5B4", languagePanel.transform, new Vector2(0.37f, 0.48f), new Vector2(260f, 76f), new Color32(67, 149, 255, 255));
        languageButtons[1] = CreateButton("ENGLISH", languagePanel.transform, new Vector2(0.63f, 0.48f), new Vector2(260f, 76f), new Color32(58, 80, 122, 255));
        languageButtons[0].onClick.AddListener(() => SelectLanguageAndClose(Language.Korean));
        languageButtons[1].onClick.AddListener(() => SelectLanguageAndClose(Language.English));
        languageCloseButton = CreateButton("CLOSE", languagePanel.transform, new Vector2(0.5f, 0.22f), new Vector2(220f, 60f), new Color32(58, 80, 122, 255));
        languageCloseButton.onClick.AddListener(CloseLanguagePanel);
        languagePanel.SetActive(false);

        profilePanel = CreatePanel("ProfilePanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(profilePanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(profilePanel);
        ApplyModalDim(profilePanel);
        profileTitleText = CreateText("\uD50C\uB808\uC774\uC5B4 \uB9CC\uB4E4\uAE30", profilePanel.transform, 42, Color.white, new Vector2(0.5f, 0.74f), new Vector2(0.5f, 0.74f));
        profilePromptText = CreateText("\uB2C9\uB124\uC784\uACFC \uCE90\uB9AD\uD130\uB97C \uC120\uD0DD\uD558\uC138\uC694", profilePanel.transform, 20, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.65f), new Vector2(0.5f, 0.65f));
        nicknameInput = CreateNicknameInput(profilePanel.transform);
        characterButtons = new Button[3];
        characterButtons[0] = CreateCharacterButton("\uAE30\uC0AC", "Pixel/character-knight", profilePanel.transform, new Vector2(0.28f, 0.40f), new Color32(55, 102, 149, 255));
        characterButtons[1] = CreateCharacterButton("\uB9C8\uBC95\uC0AC", "Pixel/character-mage", profilePanel.transform, new Vector2(0.50f, 0.40f), new Color32(105, 76, 162, 255));
        characterButtons[2] = CreateCharacterButton("\uD574\uCEE4", "Pixel/character-hacker", profilePanel.transform, new Vector2(0.72f, 0.40f), new Color32(53, 137, 105, 255));
        for (int i = 0; i < characterButtons.Length; i++)
        {
            int characterIndex = i;
            characterButtons[i].onClick.AddListener(() => SelectCharacter(characterIndex));
        }
        profileErrorText = CreateText("", profilePanel.transform, 16, new Color32(255, 142, 142, 255), new Vector2(0.5f, 0.27f), new Vector2(0.5f, 0.27f));
        createProfileButton = CreateButton("\uD504\uB85C\uD544 \uC800\uC7A5", profilePanel.transform, new Vector2(0.5f, 0.20f), new Vector2(330f, 68f), new Color32(67, 149, 255, 255));
        createProfileButton.onClick.AddListener(CreateProfile);
        profilePanel.SetActive(false);
        UpdateCharacterButtonStates();
        UpdateHighScoreText();
        UpdateCoinText();

        myPagePanel = CreatePanel("MyPagePanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(myPagePanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(myPagePanel);
        ApplyModalDim(myPagePanel);
        myPageTitleText = CreateText("\uB9C8\uC774\uD398\uC774\uC9C0", myPagePanel.transform, 42, Color.white, new Vector2(0.5f, 0.78f), new Vector2(0.5f, 0.78f));
        myPageCharacterImage = CreateImage("MyPageCharacter", myPagePanel.transform, GetCharacterResourcePath(false), new Vector2(0.5f, 0.58f), new Vector2(132f, 132f));
        myPageCharacterImage.raycastTarget = false;
        var myPageFrame = CreateImage("MyPageProfileFrame", myPagePanel.transform, "Pixel/character-profile-frame", new Vector2(0.5f, 0.58f), new Vector2(154f, 154f));
        myPageFrame.raycastTarget = false;
        myPageInfoText = CreateText("PROFILE INFO", myPagePanel.transform, 22, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.37f), new Vector2(0.5f, 0.37f));
        myPageCoinText = CreateText("\uCF54\uC778 0", myPagePanel.transform, 22, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.30f), new Vector2(0.5f, 0.30f));
        myPageStatsText = CreateText("\uD50C\uB808\uC774 \uAE30\uB85D", myPagePanel.transform, 16, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.25f), new Vector2(0.5f, 0.25f));
        myPageCloseButton = CreateButton("CLOSE", myPagePanel.transform, new Vector2(0.16f, 0.16f), new Vector2(160f, 62f), new Color32(58, 80, 122, 255));
        myPageCloseButton.onClick.AddListener(CloseMyPage);
        myPageShopButton = CreateButton("SHOP", myPagePanel.transform, new Vector2(0.34f, 0.16f), new Vector2(150f, 62f), new Color32(105, 76, 162, 255));
        myPageShopButton.onClick.AddListener(ShowShopPanel);
        myPageProfileButton = CreateButton("\uD504\uB85C\uD544 \uC218\uC815", myPagePanel.transform, new Vector2(0.59f, 0.16f), new Vector2(180f, 62f), new Color32(67, 149, 255, 255));
        myPageProfileButton.onClick.AddListener(ShowProfileEdit);
        deleteAccountButton = CreateButton("DELETE ACCOUNT", myPagePanel.transform, new Vector2(0.86f, 0.16f), new Vector2(150f, 62f), new Color32(149, 71, 92, 255));
        deleteAccountButton.onClick.AddListener(ShowWithdrawConfirm);
        myPagePanel.SetActive(false);

        attendancePanel = CreatePanel("AttendancePanel", mainPanel.transform, new Color32(0, 0, 0, 0));
        SetRect(attendancePanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyModalDim(attendancePanel);
        var attendanceWindow = CreatePanel("AttendanceWindow", attendancePanel.transform, new Color32(9, 18, 38, 250));
        SetRect(attendanceWindow.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(680f, 320f));
        ApplyPixelAttendanceWindow(attendanceWindow);
        attendanceTitleText = CreateText("\uCD9C\uC11D \uBCF4\uC0C1", attendanceWindow.transform, 30, Color.white, new Vector2(0.5f, 0.70f), new Vector2(0.5f, 0.70f));
        attendanceBodyText = CreateText("\uC624\uB298\uC758 \uCD9C\uC11D \uCF54\uC778\uC744 \uBC1B\uC558\uC2B5\uB2C8\uB2E4!", attendanceWindow.transform, 17, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.50f), new Vector2(0.5f, 0.50f));
        attendanceCoinText = CreateText("+10 COINS", attendanceWindow.transform, 27, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.34f), new Vector2(0.5f, 0.34f));
        attendanceCloseButton = CreateButton("OK", attendanceWindow.transform, new Vector2(0.5f, 0.16f), new Vector2(180f, 52f), new Color32(67, 149, 255, 255));
        attendanceCloseButton.onClick.AddListener(CloseAttendancePanel);
        if (gameOverTitleText != null) gameOverTitleText.text = language == Language.Korean ? "\uAC8C\uC784 \uC624\uBC84" : "GAME OVER";
        SetButtonText(gameOverRetryButton, language == Language.Korean ? "\uB2E4\uC2DC\uD558\uAE30" : "RETRY");
        SetButtonText(gameOverSetupButton, language == Language.Korean ? "\uAC8C\uC784 \uC124\uC815" : "GAME SETTINGS");
        attendancePanel.SetActive(false);

        shopPanel = CreatePanel("ShopPanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(shopPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelShopWindow(shopPanel);
        ApplyModalDim(shopPanel);
        shopTitleText = CreateText("COIN SHOP", shopPanel.transform, 42, Color.white, new Vector2(0.5f, 0.78f), new Vector2(0.5f, 0.78f));
        shopCoinText = CreateText("\uCF54\uC778 0", shopPanel.transform, 22, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.68f), new Vector2(0.5f, 0.68f));
        shopAllCategoryButton = CreateButton("ALL", shopPanel.transform, new Vector2(0.39f, 0.59f), new Vector2(170f, 46f), new Color32(67, 149, 255, 255));
        shopAllCategoryButton.onClick.AddListener(() => SetShopCategory(0));
        shopProfileCategoryButton = CreateButton("\uD504\uB85C\uD544", shopPanel.transform, new Vector2(0.50f, 0.59f), new Vector2(170f, 46f), new Color32(58, 80, 122, 255));
        shopProfileCategoryButton.onClick.AddListener(() => SetShopCategory(1));
        shopItemCategoryButton = CreateButton("\uC544\uC774\uD15C", shopPanel.transform, new Vector2(0.61f, 0.59f), new Vector2(170f, 46f), new Color32(58, 80, 122, 255));
        shopItemCategoryButton.onClick.AddListener(() => SetShopCategory(2));

        var shopViewportObject = CreatePanel("ShopProductViewport", shopPanel.transform, new Color32(0, 0, 0, 0));
        SetRect(shopViewportObject.GetComponent<RectTransform>(), new Vector2(0.5f, 0.41f), new Vector2(0.5f, 0.41f), Vector2.zero, new Vector2(1160f, 275f));
        var viewport = shopViewportObject.GetComponent<RectTransform>();
        viewport.gameObject.AddComponent<RectMask2D>();
        var contentObject = new GameObject("ShopProductContent", typeof(RectTransform));
        contentObject.transform.SetParent(viewport, false);
        shopProductContent = contentObject.GetComponent<RectTransform>();
        SetRect(shopProductContent, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), Vector2.zero, new Vector2(1420f, 260f));
        shopProductContent.pivot = new Vector2(0f, 0.5f);
        shopScrollRect = shopViewportObject.AddComponent<ScrollRect>();
        shopScrollRect.viewport = viewport;
        shopScrollRect.content = shopProductContent;
        shopScrollRect.horizontal = true;
        shopScrollRect.vertical = false;
        shopScrollRect.movementType = ScrollRect.MovementType.Clamped;
        shopScrollRect.inertia = true;
        shopScrollRect.scrollSensitivity = 35f;

        shopTicketCard = CreateShopItemCard("ShopTicketCard", shopProductContent, new Vector2(0.10f, 0.5f), new Vector2(230f, 230f));
        shopKnightCard = CreateShopItemCard("ShopKnightCard", shopProductContent, new Vector2(0.36f, 0.5f), new Vector2(230f, 230f));
        shopMageCard = CreateShopItemCard("ShopMageCard", shopProductContent, new Vector2(0.62f, 0.5f), new Vector2(230f, 230f));
        shopHackerCard = CreateShopItemCard("ShopHackerCard", shopProductContent, new Vector2(0.88f, 0.5f), new Vector2(230f, 230f));
        shopTicketImage = CreateImage("ShopNicknameTicket", shopProductContent, "Pixel/nickname-ticket", new Vector2(0.10f, 0.60f), new Vector2(66f, 66f));
        shopTicketImage.raycastTarget = false;
        nicknameTicketButton = CreateButton("\uB2C9\uB124\uC784 \uBCC0\uACBD\uAD8C 30\uCF54\uC778", shopProductContent, new Vector2(0.10f, 0.30f), new Vector2(210f, 58f), new Color32(67, 149, 255, 255));
        nicknameTicketButton.onClick.AddListener(BuyNicknameTicket);
        shopKnightImage = CreateImage("ShopKnightProfile", shopProductContent, "Pixel/character-knight", new Vector2(0.36f, 0.60f), new Vector2(74f, 74f));
        shopKnightImage.raycastTarget = false;
        knightShopButton = CreateButton("\uAE30\uC0AC \uD504\uB85C\uD544 \uBCF4\uC720", shopProductContent, new Vector2(0.36f, 0.30f), new Vector2(210f, 58f), new Color32(55, 102, 149, 255));
        knightShopButton.onClick.AddListener(() => BuyCharacter(0));
        shopMageImage = CreateImage("ShopMageProfile", shopProductContent, "Pixel/character-mage", new Vector2(0.62f, 0.60f), new Vector2(74f, 74f));
        shopMageImage.raycastTarget = false;
        shopHackerImage = CreateImage("ShopHackerProfile", shopProductContent, "Pixel/character-hacker", new Vector2(0.88f, 0.60f), new Vector2(74f, 74f));
        shopHackerImage.raycastTarget = false;
        mageShopButton = CreateButton("\uB9C8\uBC95\uC0AC \uD504\uB85C\uD544 100\uCF54\uC778", shopProductContent, new Vector2(0.62f, 0.30f), new Vector2(210f, 58f), new Color32(105, 76, 162, 255));
        mageShopButton.onClick.AddListener(() => BuyCharacter(1));
        hackerShopButton = CreateButton("\uD574\uCEE4 \uD504\uB85C\uD544 \uBCF4\uC720", shopProductContent, new Vector2(0.88f, 0.30f), new Vector2(210f, 58f), new Color32(53, 137, 105, 255));
        hackerShopButton.onClick.AddListener(() => BuyCharacter(2));
        shopStatusText = CreateText("", shopPanel.transform, 18, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.27f), new Vector2(0.5f, 0.27f));
        shopCloseButton = CreateButton("CLOSE", shopPanel.transform, new Vector2(0.5f, 0.16f), new Vector2(220f, 62f), new Color32(58, 80, 122, 255));
        shopCloseButton.onClick.AddListener(CloseShopPanel);
        shopPanel.SetActive(false);

        insufficientCoinsPanel = CreatePanel("InsufficientCoinsPanel", shopPanel.transform, new Color32(0, 0, 0, 0));
        SetRect(insufficientCoinsPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyModalDim(insufficientCoinsPanel);
        var insufficientWindow = CreatePanel("InsufficientCoinsWindow", insufficientCoinsPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(insufficientWindow.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(640f, 320f));
        ApplyPixelInsufficientCoinsWindow(insufficientWindow);
        insufficientCoinsTitleText = CreateText("\uCF54\uC778 \uBD80\uC871", insufficientWindow.transform, 34, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.67f), new Vector2(0.5f, 0.67f));
        SetRect(insufficientCoinsTitleText.rectTransform, new Vector2(0.5f, 0.67f), new Vector2(0.5f, 0.67f), Vector2.zero, new Vector2(500f, 52f));
        insufficientCoinsTitleText.alignment = TextAnchor.MiddleCenter;
        insufficientCoinsTitleText.resizeTextForBestFit = true;
        insufficientCoinsTitleText.resizeTextMinSize = 22;
        insufficientCoinsTitleText.resizeTextMaxSize = 34;
        insufficientCoinsMessageText = CreateText("\uCF54\uC778\uC774 \uBD80\uC871\uD569\uB2C8\uB2E4.", insufficientWindow.transform, 20, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.49f), new Vector2(0.5f, 0.49f));
        SetRect(insufficientCoinsMessageText.rectTransform, new Vector2(0.5f, 0.49f), new Vector2(0.5f, 0.49f), Vector2.zero, new Vector2(500f, 48f));
        insufficientCoinsMessageText.alignment = TextAnchor.MiddleCenter;
        insufficientCoinsMessageText.resizeTextForBestFit = true;
        insufficientCoinsMessageText.resizeTextMinSize = 14;
        insufficientCoinsMessageText.resizeTextMaxSize = 22;
        insufficientCoinsMessageText.horizontalOverflow = HorizontalWrapMode.Wrap;
        insufficientCoinsMessageText.verticalOverflow = VerticalWrapMode.Truncate;
        insufficientCoinsCloseButton = CreateButton("OK", insufficientWindow.transform, new Vector2(0.5f, 0.25f), new Vector2(190f, 56f), new Color32(67, 149, 255, 255));
        insufficientCoinsCloseButton.onClick.AddListener(CloseInsufficientCoinsPanel);
        insufficientCoinsPanel.SetActive(false);

        announcementPanel = CreatePanel("AnnouncementPanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(announcementPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(announcementPanel);
        ApplyModalDim(announcementPanel);
        announcementTitleText = CreateText("\uACF5\uC9C0\uC0AC\uD56D", announcementPanel.transform, 42, Color.white, new Vector2(0.5f, 0.70f), new Vector2(0.5f, 0.70f));
        announcementMessageText = CreateText("\uC0C8\uB85C\uC6B4 \uC18C\uC2DD\uC774 \uC788\uC2B5\uB2C8\uB2E4.\n\uACF5\uC9C0\uC0AC\uD56D\uC744 \uD655\uC778\uD574\uC8FC\uC138\uC694.", announcementPanel.transform, 22, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.51f), new Vector2(0.5f, 0.51f));
        announcementCloseButton = CreateButton("CLOSE", announcementPanel.transform, new Vector2(0.5f, 0.25f), new Vector2(220f, 64f), new Color32(67, 149, 255, 255));
        announcementCloseButton.onClick.AddListener(CloseAnnouncementPanel);
        announcementPanel.SetActive(false);

        privacyPanel = CreatePanel("PrivacyPanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(privacyPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(privacyPanel);
        ApplyModalDim(privacyPanel);
        privacyTitleText = CreateText("\uAC1C\uC778\uC815\uBCF4 \uCC98\uB9AC\uBC29\uCE68", privacyPanel.transform, 36, Color.white, new Vector2(0.5f, 0.72f), new Vector2(0.5f, 0.72f));
        privacyMessageText = CreateText("\uB2C9\uB124\uC784, \uCE90\uB9AD\uD130, \uCF54\uC778, \uAC8C\uC784 \uAE30\uB85D\uC740 \uC774 \uAE30\uAE30\uC5D0 \uC800\uC7A5\uB429\uB2C8\uB2E4.\n\uC11C\uBC84 \uC804\uC1A1\uC774\uB098 \uAD11\uACE0 \uBAA9\uC801\uC758 \uC218\uC9D1\uC740 \uD558\uC9C0 \uC54A\uC2B5\uB2C8\uB2E4.", privacyPanel.transform, 19, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.50f), new Vector2(0.5f, 0.50f));
        privacyMessageText.rectTransform.sizeDelta = new Vector2(760f, 100f);
        privacyMessageText.alignment = TextAnchor.MiddleCenter;
        privacyMessageText.horizontalOverflow = HorizontalWrapMode.Wrap;
        privacyCloseButton = CreateButton("CLOSE", privacyPanel.transform, new Vector2(0.5f, 0.23f), new Vector2(220f, 62f), new Color32(67, 149, 255, 255));
        privacyCloseButton.onClick.AddListener(ClosePrivacyPanel);
        privacyPanel.SetActive(false);

        withdrawConfirmPanel = CreatePanel("WithdrawConfirmPanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(withdrawConfirmPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(withdrawConfirmPanel);
        ApplyModalDim(withdrawConfirmPanel);
        withdrawTitleText = CreateText("\uD68C\uC6D0 \uD0C8\uD1F4", withdrawConfirmPanel.transform, 38, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.67f), new Vector2(0.5f, 0.67f));
        withdrawMessageText = CreateText("\uC815\uB9D0 \uD0C8\uD1F4\uD558\uC2DC\uACA0\uC2B5\uB2C8\uAE4C?\n\uC800\uC7A5\uB41C \uACC4\uC815 \uB370\uC774\uD130\uAC00 \uC0AD\uC81C\uB429\uB2C8\uB2E4.", withdrawConfirmPanel.transform, 19, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.49f), new Vector2(0.5f, 0.49f));
        withdrawMessageText.rectTransform.sizeDelta = new Vector2(760f, 90f);
        withdrawMessageText.alignment = TextAnchor.MiddleCenter;
        withdrawMessageText.horizontalOverflow = HorizontalWrapMode.Wrap;
        withdrawConfirmButton = CreateButton("CONFIRM", withdrawConfirmPanel.transform, new Vector2(0.38f, 0.23f), new Vector2(190f, 58f), new Color32(149, 71, 92, 255));
        withdrawConfirmButton.onClick.AddListener(ConfirmDeleteAccount);
        withdrawCancelButton = CreateButton("\uCDE8\uC18C", withdrawConfirmPanel.transform, new Vector2(0.62f, 0.23f), new Vector2(190f, 58f), new Color32(67, 149, 255, 255));
        withdrawCancelButton.onClick.AddListener(CloseWithdrawConfirm);
        withdrawConfirmPanel.SetActive(false);

        updatePanel = CreatePanel("UpdatePanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(updatePanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(updatePanel);
        ApplyModalDim(updatePanel);
        updateTitleText = CreateText("\uC5C5\uB370\uC774\uD2B8 \uC548\uB0B4", updatePanel.transform, 42, Color.white, new Vector2(0.5f, 0.72f), new Vector2(0.5f, 0.72f));
        updateMessageText = CreateText("\uC0C8\uB85C\uC6B4 \uBC84\uC804\uC774 \uC900\uBE44\uB418\uC5C8\uC2B5\uB2C8\uB2E4.", updatePanel.transform, 20, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.57f), new Vector2(0.5f, 0.57f));
        updateVersionText = CreateText("\uBC84\uC804 0.1.0", updatePanel.transform, 22, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.45f), new Vector2(0.5f, 0.45f));
        updateLaterButton = CreateButton("\uB098\uC911\uC5D0", updatePanel.transform, new Vector2(0.32f, 0.25f), new Vector2(220f, 64f), new Color32(58, 80, 122, 255));
        updateLaterButton.onClick.AddListener(CloseUpdatePanel);
        updateNowButton = CreateButton("\uC5C5\uB370\uC774\uD2B8", updatePanel.transform, new Vector2(0.68f, 0.25f), new Vector2(220f, 64f), new Color32(67, 149, 255, 255));
        updateNowButton.onClick.AddListener(OpenUpdatePage);
        updatePanel.SetActive(false);

        var header = CreatePanel("Header", gamePanel.transform, new Color32(0, 0, 0, 0));
        SetRect(header.GetComponent<RectTransform>(), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -36f), new Vector2(1280f, 72f));
        var scoreHud = CreatePanel("ScoreHud", header.transform, new Color32(0, 0, 0, 0));
        SetRect(scoreHud.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(700f, 56f));
        ApplyPixelHudStrip(scoreHud, "Pixel/score-hud-strip");
        scoreText = CreateText("SCORE 0000", header.transform, 24, Color.white, new Vector2(0.31f, 0.5f), new Vector2(0.31f, 0.5f));
        scoreText.rectTransform.sizeDelta = new Vector2(190f, 36f);
        lifeHeartImages = new Image[5];
        var heartTexture = Resources.Load<Texture2D>("Pixel/life-heart");
        Sprite heartSprite = null;
        if (heartTexture != null)
        {
            heartTexture.filterMode = FilterMode.Point;
            heartTexture.wrapMode = TextureWrapMode.Clamp;
            heartSprite = Sprite.Create(heartTexture, new Rect(heartTexture.width * 0.20f, heartTexture.height * 0.23f, heartTexture.width * 0.60f, heartTexture.height * 0.54f), new Vector2(0.5f, 0.5f), 100f);
        }
        for (int i = 0; i < lifeHeartImages.Length; i++)
        {
            lifeHeartImages[i] = CreateImage("LifeHeart" + (i + 1), header.transform, "Pixel/life-heart", new Vector2(0.89f, 0.5f), new Vector2(42f, 40f));
            if (heartSprite != null) lifeHeartImages[i].sprite = heartSprite;
            lifeHeartImages[i].preserveAspect = true;
            lifeHeartImages[i].rectTransform.anchoredPosition = new Vector2(-100f + i * 50f, 0f);
            lifeHeartImages[i].raycastTarget = false;
        }
        comboText = CreateText("COMBO x0", header.transform, 20, new Color32(147, 232, 189, 255), new Vector2(0.61f, 0.5f), new Vector2(0.61f, 0.5f));
        comboText.rectTransform.sizeDelta = new Vector2(150f, 34f);
        waveText = CreateText("WAVE 1", header.transform, 18, new Color32(193, 205, 230, 255), new Vector2(0.46f, 0.5f), new Vector2(0.46f, 0.5f));
        waveText.rectTransform.sizeDelta = new Vector2(110f, 32f);
        timerText = CreateText("", header.transform, 18, new Color32(255, 142, 142, 255), new Vector2(0.75f, 0.5f), new Vector2(0.75f, 0.5f));
        timerText.rectTransform.sizeDelta = new Vector2(130f, 32f);
        activeCharacterImage = CreateCharacterProfile(gamePanel.transform);
        var wordLayerObject = CreatePanel("WordLayer", gamePanel.transform, new Color32(0, 0, 0, 0));
        wordLayer = wordLayerObject.GetComponent<RectTransform>();
        SetRect(wordLayer, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(26f, 92f), new Vector2(-26f, -190f));
        wordLayerObject.AddComponent<RectMask2D>();
        var inputBackground = CreatePanel("InputBackground", gamePanel.transform, new Color32(0, 0, 0, 0));
        SetRect(inputBackground.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 116f), new Vector2(720f, 112f));
        ApplyPixelHudStrip(inputBackground, "Pixel/typing-input-line");
        inputBackground.transform.SetAsLastSibling();
        inputField = CreateInputField(inputBackground.transform);
        var statusBackground = CreatePanel("StatusBackground", gamePanel.transform, new Color32(3, 10, 28, 205));
        SetRect(statusBackground.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 34f), new Vector2(620f, 34f));
        statusText = CreateText("Type a word to defend your base!", gamePanel.transform, 18, new Color32(224, 232, 248, 255), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f));
        statusText.rectTransform.sizeDelta = new Vector2(600f, 30f);
        statusText.rectTransform.anchoredPosition = new Vector2(0f, 34f);
        var statusOutline = statusText.gameObject.AddComponent<Outline>();
        statusOutline.effectColor = new Color32(0, 0, 0, 230);
        statusOutline.effectDistance = new Vector2(2f, -2f);
        menuButton = CreateButton("\uC77C\uC2DC\uC815\uC9C0", gamePanel.transform, new Vector2(0.08f, 0.93f), new Vector2(120f, 46f), new Color32(58, 80, 122, 255));
        menuButton.onClick.AddListener(TogglePause);
        inputField.onEndEdit.AddListener(HandleSubmitted);

        pausePanel = CreatePanel("PausePanel", gamePanel.transform, new Color32(9, 18, 38, 235));
        CreateText("\uC77C\uC2DC\uC815\uC9C0", pausePanel.transform, 52, Color.white, new Vector2(0.5f, 0.62f), new Vector2(0.5f, 0.62f));
        CreateText("ESC\uB97C \uB204\uB974\uBA74 \uACC4\uC18D\uD560 \uC218 \uC788\uC2B5\uB2C8\uB2E4.", pausePanel.transform, 18, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.53f), new Vector2(0.5f, 0.53f));
        resumeButton = CreateButton("RESUME", pausePanel.transform, new Vector2(0.5f, 0.39f), new Vector2(220f, 58f), new Color32(67, 149, 255, 255));
        resumeButton.onClick.AddListener(TogglePause);
        pauseMyPageButton = CreateButton("\uB9C8\uC774\uD398\uC774\uC9C0", pausePanel.transform, new Vector2(0.5f, 0.27f), new Vector2(220f, 52f), new Color32(105, 76, 162, 255));
        pauseMyPageButton.onClick.AddListener(ShowMyPage);
        pauseMenuButton = CreateButton("\uBA54\uC778\uC73C\uB85C", pausePanel.transform, new Vector2(0.5f, 0.15f), new Vector2(220f, 52f), new Color32(58, 80, 122, 255));
        pauseMenuButton.onClick.AddListener(ShowMainMenu);
        pausePanel.SetActive(false);

        gameOverPanel = CreatePanel("GameOverPanel", gamePanel.transform, new Color32(9, 18, 38, 250));
        SetRect(gameOverPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyModalDim(gameOverPanel);
        var gameOverWindow = CreatePanel("GameOverWindow", gameOverPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(gameOverWindow.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(760f, 340f));
        ApplyPixelGameOverWindow(gameOverWindow);
        gameOverTitleText = CreateText("GAME OVER", gameOverWindow.transform, 38, Color.white, new Vector2(0.5f, 0.73f), new Vector2(0.5f, 0.73f));
        gameOverSummaryText = CreateText("\uC810\uC218 0\n\uC815\uD655\uB3C4 0%\n\uD68D\uB4DD \uCF54\uC778 +0", gameOverWindow.transform, 20, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.53f), new Vector2(0.5f, 0.53f));
        gameOverSummaryText.alignment = TextAnchor.MiddleCenter;
        gameOverRetryButton = CreateButton("RETRY", gameOverWindow.transform, new Vector2(0.32f, 0.28f), new Vector2(210f, 56f), new Color32(67, 149, 255, 255));
        gameOverRetryButton.onClick.AddListener(StartGame);
        gameOverSetupButton = CreateButton("GAME SETTINGS", gameOverWindow.transform, new Vector2(0.68f, 0.28f), new Vector2(210f, 56f), new Color32(105, 76, 162, 255));
        gameOverSetupButton.onClick.AddListener(ShowSetupFromGameOver);
        gameOverPanel.SetActive(false);
    }
}


