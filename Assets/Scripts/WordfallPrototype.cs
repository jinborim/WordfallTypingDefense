using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public sealed partial class WordfallPrototype : MonoBehaviour
{
    private const string RootName = "WordfallPrototypeRuntime";
    private PlayerDataManager playerDataManager;
    private WordDatabase wordDatabase;
    private readonly GameSessionState session = new GameSessionState();
    private readonly GameSettingsData settings = new GameSettingsData();
    private readonly List<FallingWord> fallingWords = new List<FallingWord>();
    private string[] koreanWords;
    private string[] englishWords;
    private Font uiFont;
    private GameObject mainPanel;
    private GameObject gamePanel;
    private GameObject setupPanel;
    private GameObject setupWindow;
    private GameObject homeHubPanel;
    private GameObject homeHubMenuPanel;
    private GameObject homeHubInfoPanel;
    private GameObject lobbyChatPanel;
    private GameObject languagePanel;
    private GameObject profilePanel;
    private GameObject myPagePanel;
    private GameObject updatePanel;
    private GameObject attendancePanel;
    private GameObject shopPanel;
    private GameObject insufficientCoinsPanel;
    private GameObject announcementPanel;
    private GameObject privacyPanel;
    private GameObject withdrawConfirmPanel;
    private RectTransform shopProductContent;
    private ScrollRect shopScrollRect;
    private Text scoreText;
    private Image[] lifeHeartImages;
    private Text comboText;
    private Text statusText;
    private Text difficultyText;
    private Text highScoreText;
    private Text modeText;
    private Text modeDescriptionText;
    private Text waveText;
    private Text timerText;
    private Text statsText;
    private Text titleText;
    private Text subtitleText;
    private Text descriptionText;
    private Text prototypeText;
    private Text inputPlaceholder;
    private Text setupTitleText;
    private Text languageTitleText;
    private Text profileTitleText;
    private Text profilePromptText;
    private Text profileErrorText;
    private Text profileSummaryText;
    private Text homeHubTitleText;
    private Text homeHubPlayerText;
    private Text homeHubCoinText;
    private Text lobbyChatTitleText;
    private Text lobbyChatMessageText;
    private Text homeHubStartLabelText;
    private Text homeHubMyPageLabelText;
    private Text homeHubQuickStartLabelText;
    private Text homeHubShopLabelText;
    private Text homeHubAnnouncementLabelText;
    private Text homeHubProfileLabelText;
    private Text homeHubPrivacyLabelText;
    private Text homeHubWithdrawLabelText;
    private Text homeHubBackLabelText;
    private Text myPageTitleText;
    private Text myPageInfoText;
    private Text myPageCoinText;
    private Text myPageStatsText;
    private Text updateTitleText;
    private Text updateMessageText;
    private Text updateVersionText;
    private Text coinText;
    private Text attendanceTitleText;
    private Text attendanceBodyText;
    private Text attendanceCoinText;
    private Text shopTitleText;
    private Text shopCoinText;
    private Text shopStatusText;
    private Text insufficientCoinsTitleText;
    private Text insufficientCoinsMessageText;
    private Text announcementTitleText;
    private Text announcementMessageText;
    private Text privacyTitleText;
    private Text privacyMessageText;
    private Text withdrawTitleText;
    private Text withdrawMessageText;
    private InputField inputField;
    private RectTransform wordLayer;
    private Button[] difficultyButtons;
    private Button[] modeButtons;
    private Button[] languageButtons;
    private Button mainStartButton;
    private Button languageOpenButton;
    private Button myPageOpenButton;
    private Button updateLaterButton;
    private Button updateNowButton;
    private Button startButton;
    private Button setupBackButton;
    private Button homeHubStartButton;
    private Button homeHubMyPageButton;
    private Button homeHubLanguageButton;
    private Button homeHubBackButton;
    private Button homeHubShopButton;
    private Button homeHubProfileButton;
    private Button homeHubMenuToggleButton;
    private Image homeHubHamburgerIcon;
    private GameObject homeHubCloseIcon;
    private Button languageCloseButton;
    private Button[] characterButtons;
    private Button createProfileButton;
    private Button myPageCloseButton;
    private Button myPageEditButton;
    private Button myPageShopButton;
    private Button myPageProfileButton;
    private Button deleteAccountButton;
    private Button profileResetButton;
    private Button attendanceCloseButton;
    private Button nicknameTicketButton;
    private Button knightShopButton;
    private Button mageShopButton;
    private Button hackerShopButton;
    private Button shopCloseButton;
    private Button insufficientCoinsCloseButton;
    private Button announcementButton;
    private Button announcementCloseButton;
    private Button privacyButton;
    private Button withdrawButton;
    private Button privacyCloseButton;
    private Button withdrawConfirmButton;
    private Button withdrawCancelButton;
    private Button shopAllCategoryButton;
    private Button shopProfileCategoryButton;
    private Button shopItemCategoryButton;
    private Button menuButton;
    private Button resumeButton;
    private Button pauseMyPageButton;
    private Button pauseMenuButton;
    private GameObject pausePanel;
    private GameObject gameOverPanel;
    private Text gameOverTitleText;
    private Text gameOverSummaryText;
    private Button gameOverRetryButton;
    private Button gameOverSetupButton;
    private GameObject characterStatusPanel;
    private InputField nicknameInput;
    private Image activeCharacterImage;
    private Image homeHubCharacterImage;
    private Image myPageCharacterImage;
    private Image shopTicketImage;
    private Image shopKnightImage;
    private Image shopMageImage;
    private Image shopHackerImage;
    private Image homeHubShopImage;
    private Image homeHubAnnouncementImage;
    private GameObject shopTicketCard;
    private GameObject shopKnightCard;
    private GameObject shopMageCard;
    private GameObject shopHackerCard;
    private int shopCategory;
    private PixelAmbientMotion mainAmbientMotion;
    private string pendingUpdateUrl;
    private bool myPageOpenedFromGame;
    private bool languageOpenedFromHomeHub;
    private bool editingProfile;
    private bool profileEditReturnToMyPage;
    private bool profileEditReturnToHomeHub;
    private bool shopOpenedFromMyPage;
    private bool homeHubMenuOpen;
    private Coroutine homeHubMenuCoroutine;
    private int lobbyDialogueIndex;
    private readonly string[] koreanLobbyDialogues =
    {
        "오늘도 단어가 내려오기 전에 기지를 지켜보자.",
        "콤보를 이어가면 더 높은 점수를 얻을 수 있어.",
        "위험한 단어는 빨리 입력해서 기지에 닿지 않게 하자.",
        "코인은 출석 보상과 게임 플레이로 모을 수 있어.",
        "상점에서 새로운 캐릭터와 닉네임 변경권을 확인해 봐.",
        "준비가 끝나면 게임 시작 버튼을 눌러 작전을 시작하자."
    };
    private readonly string[] englishLobbyDialogues =
    {
        "Let's protect the base before the words reach the bottom.",
        "Keep your combo going to earn a higher score.",
        "Type dangerous words quickly before they hit the base.",
        "You can earn coins from attendance rewards and gameplay.",
        "Check the shop for new characters and nickname tickets.",
        "When you are ready, start the mission with the start button."
    };
    private int selectedCharacter;
    private bool profileCreated;
    private int coins;
    private int nicknameTickets;
    private int ownedCharacterMask;
    private float spawnTimer { get => session.SpawnTimer; set => session.SpawnTimer = value; }
    private int score { get => session.Score; set => session.Score = value; }
    private int lives { get => session.Lives; set => session.Lives = value; }
    private int combo { get => session.Combo; set => session.Combo = value; }
    private int highScore;
    private int wave { get => session.Wave; set => session.Wave = value; }
    private int spawnedThisWave { get => session.SpawnedThisWave; set => session.SpawnedThisWave = value; }
    private string lastSpawnedWord = string.Empty;
    private int totalTyped { get => session.TotalTyped; set => session.TotalTyped = value; }
    private int correctTyped { get => session.CorrectTyped; set => session.CorrectTyped = value; }
    private float timeRemaining { get => session.TimeRemaining; set => session.TimeRemaining = value; }
    private float freezeTimer { get => session.FreezeTimer; set => session.FreezeTimer = value; }
    private float characterEmotionTimer { get => session.CharacterEmotionTimer; set => session.CharacterEmotionTimer = value; }
    private bool characterIsAlarm { get => session.CharacterIsAlarm; set => session.CharacterIsAlarm = value; }
    private int displayedCharacter = -1;
    private int difficulty { get => settings.Difficulty; set => settings.Difficulty = value; }
    private bool playing { get => session.Playing; set => session.Playing = value; }
    private bool paused { get => session.Paused; set => session.Paused = value; }

    private Language language { get => settings.Language; set => settings.Language = value; }

    public enum WordType
    {
        Normal,
        Golden,
        Bomb,
        Heal,
        Boss
    }

    private GameMode gameMode { get => settings.Mode; set => settings.Mode = value; }

    #if false
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void LegacyBoot()
    {
        if (GameObject.Find(RootName) != null) return;
        var root = new GameObject(RootName);
        DontDestroyOnLoad(root);
        root.AddComponent<WordfallPrototype>();
    }

    private void LegacyAwake()
    {
        playerDataManager = gameObject.GetComponent<PlayerDataManager>();
        if (playerDataManager == null) playerDataManager = gameObject.AddComponent<PlayerDataManager>();
        playerDataManager.Load();
        wordDatabase = new WordDatabase();
        koreanWords = wordDatabase.KoreanWords;
        englishWords = wordDatabase.EnglishWords;
        uiFont = Resources.Load<Font>("Fonts/Mulmaru");
        if (uiFont == null) uiFont = Font.CreateDynamicFontFromOSFont("Malgun Gothic", 32);
        if (uiFont == null) uiFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        highScore = playerDataManager.Data.highScore;
        profileCreated = playerDataManager.Data.profileCreated;
        selectedCharacter = playerDataManager.Data.selectedCharacter;
        coins = playerDataManager.Data.coins;
        nicknameTickets = playerDataManager.Data.nicknameTickets;
        ownedCharacterMask = playerDataManager.Data.ownedCharacterMask;
        BuildInterface();
        var updateChecker = gameObject.AddComponent<UpdateChecker>();
        updateChecker.UpdateAvailable += ShowUpdatePanel;
        ShowMainMenu();
    }

    private void LegacyUpdate()
    {
        if (!playing) return;
        HandleShortcuts();
        if (paused) return;
        if (characterEmotionTimer > 0f)
        {
            characterEmotionTimer -= Time.deltaTime;
            if (characterEmotionTimer <= 0f) SetCharacterEmotion(false);
        }
        if (freezeTimer > 0f) freezeTimer -= Time.deltaTime;
        if (gameMode == GameMode.TimeAttack)
        {
            timeRemaining -= Time.deltaTime;
            UpdateHud();
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                EndGame();
                return;
            }
        }
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnWord();
            spawnTimer = Mathf.Max(GetMinimumSpawnInterval(), GetBaseSpawnInterval() - score * 0.003f);
        }
        for (int i = fallingWords.Count - 1; i >= 0; i--)
        {
            var word = fallingWords[i];
            if (freezeTimer <= 0f) word.rect.anchoredPosition += Vector2.down * word.speed * Time.deltaTime;
            if (word.rect.anchoredPosition.y < -360f)
            {
                SetCharacterEmotion(true);
                Destroy(word.objectToDestroy);
                fallingWords.RemoveAt(i);
                if (word.type == WordType.Bomb)
                {
                    lives -= 2;
                    statusText.text = language == Language.Korean ? "폭탄을 놓쳤습니다! 목숨 -2" : "BOMB MISSED!  -2 LIVES";
                }
                else
                {
                    lives--;
                    statusText.text = language == Language.Korean ? "단어를 놓쳤습니다!" : "A word got through!";
                }
                combo = 0;
                UpdateHud();
                if (lives <= 0) EndGame();
            }
        }
    }

    private void LegacyHandleShortcuts()
    {
        if (Keyboard.current == null) return;
        if (Keyboard.current.escapeKey.wasPressedThisFrame) TogglePause();
        if (Keyboard.current.f1Key.wasPressedThisFrame && !paused) ActivateFreezeSkill();
        if (Keyboard.current.f2Key.wasPressedThisFrame && !paused) ClearScreenSkill();
    }

    private void LegacyBuildInterface()
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
        subtitleText = CreateText("한글 & 영어 타자 디펜스", mainPanel.transform, 22, new Color32(158, 180, 220, 255), new Vector2(0.5f, 0.53f), new Vector2(0.5f, 0.53f));
        descriptionText = CreateText("떨어지는 단어를 빠르게 입력해 기지를 지키세요.", mainPanel.transform, 18, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.48f), new Vector2(0.5f, 0.48f));
        mainStartButton = CreateButton("게임 시작", mainPanel.transform, new Vector2(0.38f, 0.21f), new Vector2(340f, 78f), new Color32(255, 196, 61, 255));
        mainStartButton.onClick.AddListener(ShowHomeHub);
        languageOpenButton = CreateButton("언어 설정", mainPanel.transform, new Vector2(0.66f, 0.21f), new Vector2(250f, 78f), new Color32(58, 80, 122, 255));
        languageOpenButton.onClick.AddListener(ShowLanguagePanel);
        profileSummaryText = CreateText("", mainPanel.transform, 16, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.105f), new Vector2(0.5f, 0.105f));

        homeHubPanel = CreatePanel("HomeHubPanel", mainPanel.transform, new Color32(0, 0, 0, 0));
        SetRect(homeHubPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelLobbyBackground(homeHubPanel);
        ApplyModalDim(homeHubPanel);
        homeHubTitleText = CreateText("로비", homeHubPanel.transform, 40, Color.white, new Vector2(0.5f, 0.87f), new Vector2(0.5f, 0.87f));
        homeHubCharacterImage = CreateImage("HomeHubCharacter", homeHubPanel.transform, GetLobbyCharacterResourcePath(), new Vector2(0.5f, 0.42f), new Vector2(220f, 360f));
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
        homeHubStartButton = CreateButton("게임 시작", homeHubPanel.transform, new Vector2(0.5f, 0.10f), new Vector2(290f, 58f), new Color32(67, 149, 255, 255));
        homeHubStartButton.onClick.AddListener(ShowSetupPanel);

        lobbyChatPanel = CreatePanel("LobbyCharacterDialogue", homeHubPanel.transform, new Color32(9, 18, 38, 248));
        SetRect(lobbyChatPanel.GetComponent<RectTransform>(), new Vector2(0.73f, 0.54f), new Vector2(0.73f, 0.54f), Vector2.zero, new Vector2(680f, 220f));
        lobbyChatPanel.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        ApplyPixelLobbyDialogueWindow(lobbyChatPanel);
        lobbyChatPanel.GetComponent<Image>().raycastTarget = false;
        lobbyChatMessageText = CreateText("", lobbyChatPanel.transform, 18, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.50f), new Vector2(0.5f, 0.50f));
        SetRect(lobbyChatMessageText.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(540f, 120f));
        lobbyChatMessageText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        lobbyChatMessageText.rectTransform.localPosition = Vector3.zero;
        lobbyChatMessageText.alignment = TextAnchor.MiddleCenter;
        lobbyChatMessageText.horizontalOverflow = HorizontalWrapMode.Wrap;
        lobbyChatMessageText.verticalOverflow = VerticalWrapMode.Truncate;
        lobbyChatPanel.SetActive(false);

        homeHubMenuPanel = CreatePanel("HomeHubSideMenu", homeHubPanel.transform, new Color32(4, 10, 26, 220));
        var homeHubMenuRect = homeHubMenuPanel.GetComponent<RectTransform>();
        SetRect(homeHubMenuRect, new Vector2(0f, 0f), new Vector2(0f, 1f), Vector2.zero, new Vector2(560f, 0f));
        homeHubMenuRect.pivot = new Vector2(0f, 0.5f);
        homeHubMenuRect.anchoredPosition = new Vector2(-560f, 0f);
        var menuOutline = homeHubMenuPanel.AddComponent<Outline>();
        menuOutline.effectColor = new Color32(20, 66, 126, 220);
        menuOutline.effectDistance = new Vector2(3f, 0f);

        homeHubMyPageButton = CreateImageAction("HomeHubMyPageAction", homeHubMenuPanel.transform, "Pixel/score-hud-frame", "기록 보기", new Vector2(0.50f, 0.78f), new Vector2(170f, 130f), new Vector2(82f, 70f), out homeHubMyPageLabelText);
        homeHubMyPageButton.onClick.AddListener(ShowMyPage);
        homeHubShopButton = CreateImageAction("HomeHubShopAction", homeHubMenuPanel.transform, "Pixel/shop-store-icon", "상점", new Vector2(0.80f, 0.78f), new Vector2(170f, 130f), new Vector2(82f, 78f), out homeHubShopLabelText);
        homeHubShopButton.onClick.AddListener(ShowShopFromHome);
        announcementButton = CreateImageAction("HomeHubAnnouncementAction", homeHubMenuPanel.transform, "Pixel/announcement-icon", "공지사항", new Vector2(0.20f, 0.48f), new Vector2(170f, 130f), new Vector2(72f, 72f), out homeHubAnnouncementLabelText);
        announcementButton.onClick.AddListener(ShowAnnouncementPanel);
        homeHubProfileButton = CreateImageAction("HomeHubProfileAction", homeHubMenuPanel.transform, "Pixel/character-profile-frame", "프로필 수정", new Vector2(0.20f, 0.78f), new Vector2(170f, 130f), new Vector2(82f, 82f), out homeHubProfileLabelText);
        homeHubProfileButton.onClick.AddListener(ShowProfileEditFromHome);
        privacyButton = CreateImageAction("HomeHubPrivacyAction", homeHubMenuPanel.transform, "Pixel/announcement-icon", "개인정보 처리방침", new Vector2(0.50f, 0.48f), new Vector2(170f, 130f), new Vector2(72f, 72f), out homeHubPrivacyLabelText);
        privacyButton.onClick.AddListener(ShowPrivacyPanel);
        withdrawButton = CreateImageAction("HomeHubWithdrawAction", homeHubMenuPanel.transform, "Pixel/character-profile-frame", "회원 탈퇴", new Vector2(0.80f, 0.48f), new Vector2(170f, 130f), new Vector2(72f, 72f), out homeHubWithdrawLabelText);
        withdrawButton.onClick.AddListener(ShowWithdrawConfirm);
        homeHubMenuToggleButton = CreateHamburgerButton("HomeHubMenuToggle", homeHubPanel.transform, new Vector2(0.04f, 0.50f), new Vector2(62f, 62f));
        homeHubHamburgerIcon = homeHubMenuToggleButton.transform.Find("HamburgerMenuIcon").GetComponent<Image>();
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
        setupTitleText = CreateText("게임 설정", setupPanel.transform, 38, Color.white, new Vector2(0.5f, 0.84f), new Vector2(0.5f, 0.84f));
        difficultyText = CreateText("난이도  보통", setupPanel.transform, 22, new Color32(255, 211, 94, 255), new Vector2(0.27f, 0.69f), new Vector2(0.27f, 0.69f));
        difficultyText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        difficultyText.rectTransform.sizeDelta = new Vector2(260f, 36f);
        difficultyButtons = new Button[3];
        difficultyButtons[0] = CreateButton("쉬움", setupPanel.transform, new Vector2(0.27f, 0.58f), new Vector2(260f, 58f), new Color32(55, 102, 149, 255));
        difficultyButtons[1] = CreateButton("보통", setupPanel.transform, new Vector2(0.27f, 0.46f), new Vector2(260f, 58f), new Color32(67, 149, 255, 255));
        difficultyButtons[2] = CreateButton("어려움", setupPanel.transform, new Vector2(0.27f, 0.34f), new Vector2(260f, 58f), new Color32(149, 71, 92, 255));
        difficultyButtons[0].onClick.AddListener(() => SetDifficulty(0));
        difficultyButtons[1].onClick.AddListener(() => SetDifficulty(1));
        difficultyButtons[2].onClick.AddListener(() => SetDifficulty(2));
        modeText = CreateText("모드  혼합", setupPanel.transform, 20, new Color32(147, 232, 189, 255), new Vector2(0.70f, 0.69f), new Vector2(0.70f, 0.69f));
        modeText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        modeText.rectTransform.sizeDelta = new Vector2(420f, 36f);
        modeButtons = new Button[5];
        modeButtons[0] = CreateButton("혼합", setupPanel.transform, new Vector2(0.56f, 0.58f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        modeButtons[1] = CreateButton("한글", setupPanel.transform, new Vector2(0.70f, 0.58f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        modeButtons[2] = CreateButton("영어", setupPanel.transform, new Vector2(0.84f, 0.58f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        modeButtons[3] = CreateButton("60초", setupPanel.transform, new Vector2(0.63f, 0.42f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        modeButtons[4] = CreateButton("오늘", setupPanel.transform, new Vector2(0.77f, 0.42f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        for (int i = 0; i < modeButtons.Length; i++)
        {
            int selectedMode = i;
            modeButtons[i].onClick.AddListener(() => SetGameMode((GameMode)selectedMode));
        }
        modeDescriptionText = CreateText("한글과 영어 단어가 함께 등장합니다.", setupPanel.transform, 16, new Color32(171, 190, 220, 255), new Vector2(0.70f, 0.30f), new Vector2(0.70f, 0.30f));
        modeDescriptionText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        modeDescriptionText.rectTransform.sizeDelta = new Vector2(520f, 42f);
        modeDescriptionText.alignment = TextAnchor.MiddleCenter;
        startButton = CreateButton("게임 시작", setupPanel.transform, new Vector2(0.62f, 0.18f), new Vector2(300f, 68f), new Color32(255, 196, 61, 255));
        startButton.onClick.AddListener(StartGame);
        setupBackButton = CreateButton("뒤로", setupPanel.transform, new Vector2(0.38f, 0.18f), new Vector2(220f, 60f), new Color32(78, 160, 255, 255));
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
        languageTitleText = CreateText("언어 설정", languagePanel.transform, 42, Color.white, new Vector2(0.5f, 0.70f), new Vector2(0.5f, 0.70f));
        languageButtons = new Button[2];
        languageButtons[0] = CreateButton("한국어", languagePanel.transform, new Vector2(0.37f, 0.48f), new Vector2(260f, 76f), new Color32(67, 149, 255, 255));
        languageButtons[1] = CreateButton("ENGLISH", languagePanel.transform, new Vector2(0.63f, 0.48f), new Vector2(260f, 76f), new Color32(58, 80, 122, 255));
        languageButtons[0].onClick.AddListener(() => SelectLanguageAndClose(Language.Korean));
        languageButtons[1].onClick.AddListener(() => SelectLanguageAndClose(Language.English));
        languageCloseButton = CreateButton("닫기", languagePanel.transform, new Vector2(0.5f, 0.22f), new Vector2(220f, 60f), new Color32(58, 80, 122, 255));
        languageCloseButton.onClick.AddListener(CloseLanguagePanel);
        languagePanel.SetActive(false);

        profilePanel = CreatePanel("ProfilePanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(profilePanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(profilePanel);
        ApplyModalDim(profilePanel);
        profileTitleText = CreateText("플레이어 만들기", profilePanel.transform, 42, Color.white, new Vector2(0.5f, 0.74f), new Vector2(0.5f, 0.74f));
        profilePromptText = CreateText("닉네임과 캐릭터를 선택하세요", profilePanel.transform, 20, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.65f), new Vector2(0.5f, 0.65f));
        nicknameInput = CreateNicknameInput(profilePanel.transform);
        characterButtons = new Button[3];
        characterButtons[0] = CreateCharacterButton("기사", "Pixel/character-knight", profilePanel.transform, new Vector2(0.28f, 0.40f), new Color32(55, 102, 149, 255));
        characterButtons[1] = CreateCharacterButton("마법사", "Pixel/character-mage", profilePanel.transform, new Vector2(0.50f, 0.40f), new Color32(105, 76, 162, 255));
        characterButtons[2] = CreateCharacterButton("해커", "Pixel/character-hacker", profilePanel.transform, new Vector2(0.72f, 0.40f), new Color32(53, 137, 105, 255));
        for (int i = 0; i < characterButtons.Length; i++)
        {
            int characterIndex = i;
            characterButtons[i].onClick.AddListener(() => SelectCharacter(characterIndex));
        }
        profileErrorText = CreateText("", profilePanel.transform, 16, new Color32(255, 142, 142, 255), new Vector2(0.5f, 0.27f), new Vector2(0.5f, 0.27f));
        createProfileButton = CreateButton("프로필 만들기", profilePanel.transform, new Vector2(0.5f, 0.20f), new Vector2(330f, 68f), new Color32(67, 149, 255, 255));
        createProfileButton.onClick.AddListener(CreateProfile);
        profilePanel.SetActive(false);
        UpdateCharacterButtonStates();
        UpdateHighScoreText();
        UpdateCoinText();

        myPagePanel = CreatePanel("MyPagePanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(myPagePanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(myPagePanel);
        ApplyModalDim(myPagePanel);
        myPageTitleText = CreateText("마이페이지", myPagePanel.transform, 42, Color.white, new Vector2(0.5f, 0.78f), new Vector2(0.5f, 0.78f));
        myPageCharacterImage = CreateImage("MyPageCharacter", myPagePanel.transform, GetCharacterResourcePath(false), new Vector2(0.5f, 0.58f), new Vector2(132f, 132f));
        myPageCharacterImage.raycastTarget = false;
        var myPageFrame = CreateImage("MyPageProfileFrame", myPagePanel.transform, "Pixel/character-profile-frame", new Vector2(0.5f, 0.58f), new Vector2(154f, 154f));
        myPageFrame.raycastTarget = false;
        myPageInfoText = CreateText("닉네임", myPagePanel.transform, 22, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.37f), new Vector2(0.5f, 0.37f));
        myPageCoinText = CreateText("코인 0", myPagePanel.transform, 22, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.30f), new Vector2(0.5f, 0.30f));
        myPageStatsText = CreateText("기록", myPagePanel.transform, 16, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.25f), new Vector2(0.5f, 0.25f));
        myPageCloseButton = CreateButton("닫기", myPagePanel.transform, new Vector2(0.16f, 0.16f), new Vector2(160f, 62f), new Color32(58, 80, 122, 255));
        myPageCloseButton.onClick.AddListener(CloseMyPage);
        myPageShopButton = CreateButton("상점", myPagePanel.transform, new Vector2(0.34f, 0.16f), new Vector2(150f, 62f), new Color32(105, 76, 162, 255));
        myPageShopButton.onClick.AddListener(ShowShopPanel);
        myPageProfileButton = CreateButton("프로필 수정", myPagePanel.transform, new Vector2(0.59f, 0.16f), new Vector2(180f, 62f), new Color32(67, 149, 255, 255));
        myPageProfileButton.onClick.AddListener(ShowProfileEdit);
        deleteAccountButton = CreateButton("탈퇴하기", myPagePanel.transform, new Vector2(0.86f, 0.16f), new Vector2(150f, 62f), new Color32(149, 71, 92, 255));
        deleteAccountButton.onClick.AddListener(ShowWithdrawConfirm);
        myPagePanel.SetActive(false);

        attendancePanel = CreatePanel("AttendancePanel", mainPanel.transform, new Color32(0, 0, 0, 0));
        SetRect(attendancePanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyModalDim(attendancePanel);
        var attendanceWindow = CreatePanel("AttendanceWindow", attendancePanel.transform, new Color32(9, 18, 38, 250));
        SetRect(attendanceWindow.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(680f, 320f));
        ApplyPixelAttendanceWindow(attendanceWindow);
        attendanceTitleText = CreateText("출석 보상", attendanceWindow.transform, 30, Color.white, new Vector2(0.5f, 0.70f), new Vector2(0.5f, 0.70f));
        attendanceBodyText = CreateText("오늘의 출석 보상을 받았습니다!", attendanceWindow.transform, 17, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.50f), new Vector2(0.5f, 0.50f));
        attendanceCoinText = CreateText("+10 COINS", attendanceWindow.transform, 27, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.34f), new Vector2(0.5f, 0.34f));
        attendanceCloseButton = CreateButton("확인", attendanceWindow.transform, new Vector2(0.5f, 0.16f), new Vector2(180f, 52f), new Color32(67, 149, 255, 255));
        attendanceCloseButton.onClick.AddListener(CloseAttendancePanel);
        if (gameOverTitleText != null) gameOverTitleText.text = language == Language.Korean ? "게임 오버" : "GAME OVER";
        SetButtonText(gameOverRetryButton, language == Language.Korean ? "다시하기" : "RETRY");
        SetButtonText(gameOverSetupButton, language == Language.Korean ? "게임 설정" : "GAME SETTINGS");
        attendancePanel.SetActive(false);

        shopPanel = CreatePanel("ShopPanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(shopPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelShopWindow(shopPanel);
        ApplyModalDim(shopPanel);
        shopTitleText = CreateText("코인 상점", shopPanel.transform, 42, Color.white, new Vector2(0.5f, 0.78f), new Vector2(0.5f, 0.78f));
        shopCoinText = CreateText("코인 0", shopPanel.transform, 22, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.68f), new Vector2(0.5f, 0.68f));
        shopAllCategoryButton = CreateButton("전체", shopPanel.transform, new Vector2(0.39f, 0.59f), new Vector2(170f, 46f), new Color32(67, 149, 255, 255));
        shopAllCategoryButton.onClick.AddListener(() => SetShopCategory(0));
        shopProfileCategoryButton = CreateButton("프로필", shopPanel.transform, new Vector2(0.50f, 0.59f), new Vector2(170f, 46f), new Color32(58, 80, 122, 255));
        shopProfileCategoryButton.onClick.AddListener(() => SetShopCategory(1));
        shopItemCategoryButton = CreateButton("아이템", shopPanel.transform, new Vector2(0.61f, 0.59f), new Vector2(170f, 46f), new Color32(58, 80, 122, 255));
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
        nicknameTicketButton = CreateButton("닉네임 변경권 30코인", shopProductContent, new Vector2(0.10f, 0.30f), new Vector2(210f, 58f), new Color32(67, 149, 255, 255));
        nicknameTicketButton.onClick.AddListener(BuyNicknameTicket);
        shopKnightImage = CreateImage("ShopKnightProfile", shopProductContent, "Pixel/character-knight", new Vector2(0.36f, 0.60f), new Vector2(74f, 74f));
        shopKnightImage.raycastTarget = false;
        knightShopButton = CreateButton("기사 프로필 100코인", shopProductContent, new Vector2(0.36f, 0.30f), new Vector2(210f, 58f), new Color32(55, 102, 149, 255));
        knightShopButton.onClick.AddListener(() => BuyCharacter(0));
        shopMageImage = CreateImage("ShopMageProfile", shopProductContent, "Pixel/character-mage", new Vector2(0.62f, 0.60f), new Vector2(74f, 74f));
        shopMageImage.raycastTarget = false;
        shopHackerImage = CreateImage("ShopHackerProfile", shopProductContent, "Pixel/character-hacker", new Vector2(0.88f, 0.60f), new Vector2(74f, 74f));
        shopHackerImage.raycastTarget = false;
        mageShopButton = CreateButton("마법사 프로필 100코인", shopProductContent, new Vector2(0.62f, 0.30f), new Vector2(210f, 58f), new Color32(105, 76, 162, 255));
        mageShopButton.onClick.AddListener(() => BuyCharacter(1));
        hackerShopButton = CreateButton("해커 프로필 100코인", shopProductContent, new Vector2(0.88f, 0.30f), new Vector2(210f, 58f), new Color32(53, 137, 105, 255));
        hackerShopButton.onClick.AddListener(() => BuyCharacter(2));
        shopStatusText = CreateText("", shopPanel.transform, 18, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.27f), new Vector2(0.5f, 0.27f));
        shopCloseButton = CreateButton("닫기", shopPanel.transform, new Vector2(0.5f, 0.16f), new Vector2(220f, 62f), new Color32(58, 80, 122, 255));
        shopCloseButton.onClick.AddListener(CloseShopPanel);
        shopPanel.SetActive(false);

        insufficientCoinsPanel = CreatePanel("InsufficientCoinsPanel", shopPanel.transform, new Color32(0, 0, 0, 0));
        SetRect(insufficientCoinsPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyModalDim(insufficientCoinsPanel);
        var insufficientWindow = CreatePanel("InsufficientCoinsWindow", insufficientCoinsPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(insufficientWindow.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(560f, 290f));
        ApplyPixelShopWindow(insufficientWindow);
        insufficientCoinsTitleText = CreateText("코인 부족", insufficientWindow.transform, 34, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.67f), new Vector2(0.5f, 0.67f));
        insufficientCoinsMessageText = CreateText("코인이 부족합니다.", insufficientWindow.transform, 20, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.49f), new Vector2(0.5f, 0.49f));
        insufficientCoinsCloseButton = CreateButton("확인", insufficientWindow.transform, new Vector2(0.5f, 0.25f), new Vector2(190f, 56f), new Color32(67, 149, 255, 255));
        insufficientCoinsCloseButton.onClick.AddListener(CloseInsufficientCoinsPanel);
        insufficientCoinsPanel.SetActive(false);

        announcementPanel = CreatePanel("AnnouncementPanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(announcementPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(announcementPanel);
        ApplyModalDim(announcementPanel);
        announcementTitleText = CreateText("공지사항", announcementPanel.transform, 42, Color.white, new Vector2(0.5f, 0.70f), new Vector2(0.5f, 0.70f));
        announcementMessageText = CreateText("워드폴에 오신 것을 환영합니다!\n새로운 소식은 이곳에서 확인하세요.", announcementPanel.transform, 22, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.51f), new Vector2(0.5f, 0.51f));
        announcementCloseButton = CreateButton("닫기", announcementPanel.transform, new Vector2(0.5f, 0.25f), new Vector2(220f, 64f), new Color32(67, 149, 255, 255));
        announcementCloseButton.onClick.AddListener(CloseAnnouncementPanel);
        announcementPanel.SetActive(false);

        privacyPanel = CreatePanel("PrivacyPanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(privacyPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(privacyPanel);
        ApplyModalDim(privacyPanel);
        privacyTitleText = CreateText("개인정보 처리방침", privacyPanel.transform, 36, Color.white, new Vector2(0.5f, 0.72f), new Vector2(0.5f, 0.72f));
        privacyMessageText = CreateText("닉네임, 캐릭터, 코인, 게임 기록은 현재 기기 안에만 저장됩니다.\n외부 서버로 전송하지 않으며, 회원 탈퇴 시 저장된 정보가 삭제됩니다.", privacyPanel.transform, 19, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.50f), new Vector2(0.5f, 0.50f));
        privacyMessageText.rectTransform.sizeDelta = new Vector2(760f, 100f);
        privacyMessageText.alignment = TextAnchor.MiddleCenter;
        privacyMessageText.horizontalOverflow = HorizontalWrapMode.Wrap;
        privacyCloseButton = CreateButton("닫기", privacyPanel.transform, new Vector2(0.5f, 0.23f), new Vector2(220f, 62f), new Color32(67, 149, 255, 255));
        privacyCloseButton.onClick.AddListener(ClosePrivacyPanel);
        privacyPanel.SetActive(false);

        withdrawConfirmPanel = CreatePanel("WithdrawConfirmPanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(withdrawConfirmPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(withdrawConfirmPanel);
        ApplyModalDim(withdrawConfirmPanel);
        withdrawTitleText = CreateText("회원 탈퇴", withdrawConfirmPanel.transform, 38, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.67f), new Vector2(0.5f, 0.67f));
        withdrawMessageText = CreateText("회원 탈퇴를 하시겠습니까?\n저장된 닉네임, 캐릭터, 코인, 기록이 모두 삭제됩니다.", withdrawConfirmPanel.transform, 19, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.49f), new Vector2(0.5f, 0.49f));
        withdrawMessageText.rectTransform.sizeDelta = new Vector2(760f, 90f);
        withdrawMessageText.alignment = TextAnchor.MiddleCenter;
        withdrawMessageText.horizontalOverflow = HorizontalWrapMode.Wrap;
        withdrawConfirmButton = CreateButton("확인", withdrawConfirmPanel.transform, new Vector2(0.38f, 0.23f), new Vector2(190f, 58f), new Color32(149, 71, 92, 255));
        withdrawConfirmButton.onClick.AddListener(ConfirmDeleteAccount);
        withdrawCancelButton = CreateButton("취소", withdrawConfirmPanel.transform, new Vector2(0.62f, 0.23f), new Vector2(190f, 58f), new Color32(67, 149, 255, 255));
        withdrawCancelButton.onClick.AddListener(CloseWithdrawConfirm);
        withdrawConfirmPanel.SetActive(false);

        updatePanel = CreatePanel("UpdatePanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(updatePanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(updatePanel);
        ApplyModalDim(updatePanel);
        updateTitleText = CreateText("업데이트", updatePanel.transform, 42, Color.white, new Vector2(0.5f, 0.72f), new Vector2(0.5f, 0.72f));
        updateMessageText = CreateText("새로운 업데이트가 있습니다.", updatePanel.transform, 20, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.57f), new Vector2(0.5f, 0.57f));
        updateVersionText = CreateText("새 버전 0.1.0", updatePanel.transform, 22, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.45f), new Vector2(0.5f, 0.45f));
        updateLaterButton = CreateButton("나중에", updatePanel.transform, new Vector2(0.32f, 0.25f), new Vector2(220f, 64f), new Color32(58, 80, 122, 255));
        updateLaterButton.onClick.AddListener(CloseUpdatePanel);
        updateNowButton = CreateButton("업데이트", updatePanel.transform, new Vector2(0.68f, 0.25f), new Vector2(220f, 64f), new Color32(67, 149, 255, 255));
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
        menuButton = CreateButton("일시정지", gamePanel.transform, new Vector2(0.08f, 0.93f), new Vector2(120f, 46f), new Color32(58, 80, 122, 255));
        menuButton.onClick.AddListener(TogglePause);
        inputField.onEndEdit.AddListener(HandleSubmitted);

        pausePanel = CreatePanel("PausePanel", gamePanel.transform, new Color32(9, 18, 38, 235));
        CreateText("일시정지", pausePanel.transform, 52, Color.white, new Vector2(0.5f, 0.62f), new Vector2(0.5f, 0.62f));
        CreateText("ESC를 누르면 계속", pausePanel.transform, 18, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.53f), new Vector2(0.5f, 0.53f));
        resumeButton = CreateButton("계속하기", pausePanel.transform, new Vector2(0.5f, 0.39f), new Vector2(220f, 58f), new Color32(67, 149, 255, 255));
        resumeButton.onClick.AddListener(TogglePause);
        pauseMyPageButton = CreateButton("마이페이지", pausePanel.transform, new Vector2(0.5f, 0.27f), new Vector2(220f, 52f), new Color32(105, 76, 162, 255));
        pauseMyPageButton.onClick.AddListener(ShowMyPage);
        pauseMenuButton = CreateButton("메뉴", pausePanel.transform, new Vector2(0.5f, 0.15f), new Vector2(220f, 52f), new Color32(58, 80, 122, 255));
        pauseMenuButton.onClick.AddListener(ShowMainMenu);
        pausePanel.SetActive(false);

        gameOverPanel = CreatePanel("GameOverPanel", gamePanel.transform, new Color32(9, 18, 38, 250));
        SetRect(gameOverPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyModalDim(gameOverPanel);
        var gameOverWindow = CreatePanel("GameOverWindow", gameOverPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(gameOverWindow.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(760f, 340f));
        ApplyPixelGameOverWindow(gameOverWindow);
        gameOverTitleText = CreateText("게임 오버", gameOverWindow.transform, 38, Color.white, new Vector2(0.5f, 0.73f), new Vector2(0.5f, 0.73f));
        gameOverSummaryText = CreateText("점수 0000\n정확도 0%", gameOverWindow.transform, 20, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.53f), new Vector2(0.5f, 0.53f));
        gameOverSummaryText.alignment = TextAnchor.MiddleCenter;
        gameOverRetryButton = CreateButton("다시하기", gameOverWindow.transform, new Vector2(0.32f, 0.28f), new Vector2(210f, 56f), new Color32(67, 149, 255, 255));
        gameOverRetryButton.onClick.AddListener(StartGame);
        gameOverSetupButton = CreateButton("게임 설정", gameOverWindow.transform, new Vector2(0.68f, 0.28f), new Vector2(210f, 56f), new Color32(105, 76, 162, 255));
        gameOverSetupButton.onClick.AddListener(ShowSetupFromGameOver);
        gameOverPanel.SetActive(false);
    }

    private void LegacyEnsureInputSystemEventSystem()
    {
        var eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            var eventSystemObject = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            eventSystemObject.transform.SetParent(transform, false);
            return;
        }

        var legacyModule = eventSystem.GetComponent<StandaloneInputModule>();
        if (legacyModule != null) Destroy(legacyModule);
        if (eventSystem.GetComponent<InputSystemUIInputModule>() == null)
        {
            eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
        }
    }

    private void LegacySetLanguage(Language selectedLanguage)
    {
        language = selectedLanguage;
        titleText.text = language == Language.Korean ? "워드폴" : "WORDFALL";
        subtitleText.text = language == Language.Korean ? "한글 & 영어 타자 디펜스" : "KOREAN & ENGLISH TYPING DEFENSE";
        descriptionText.text = language == Language.Korean ? "떨어지는 단어를 빠르게 입력해 기지를 지키세요." : "Type the falling words before they reach the bottom.";
        SetButtonText(mainStartButton, language == Language.Korean ? "게임 시작" : "START GAME");
        SetButtonText(languageOpenButton, language == Language.Korean ? "언어 설정" : "LANGUAGE");
        if (myPageOpenButton != null)
        {
            SetButtonText(myPageOpenButton, language == Language.Korean ? "마이페이지" : "MY PAGE");
        }
        SetButtonText(homeHubStartButton, language == Language.Korean ? "게임 시작" : "START GAME");
        SetLabelText(homeHubMyPageLabelText, language == Language.Korean ? "기록 보기" : "RECORDS");
        SetLabelText(homeHubShopLabelText, language == Language.Korean ? "상점" : "SHOP");
        SetLabelText(homeHubAnnouncementLabelText, language == Language.Korean ? "공지사항" : "ANNOUNCEMENTS");
        SetLabelText(homeHubProfileLabelText, language == Language.Korean ? "프로필 수정" : "EDIT PROFILE");
        SetLabelText(homeHubPrivacyLabelText, language == Language.Korean ? "개인정보 처리방침" : "PRIVACY POLICY");
        SetLabelText(homeHubWithdrawLabelText, language == Language.Korean ? "회원 탈퇴" : "DELETE ACCOUNT");
        SetButtonText(privacyButton, language == Language.Korean ? "개인정보 처리방침" : "PRIVACY POLICY");
        SetButtonText(withdrawButton, language == Language.Korean ? "회원 탈퇴" : "DELETE ACCOUNT");
        privacyTitleText.text = language == Language.Korean ? "개인정보 처리방침" : "PRIVACY POLICY";
        privacyMessageText.text = language == Language.Korean
            ? "닉네임, 캐릭터, 코인, 게임 기록은 현재 기기 안에만 저장됩니다.\n외부 서버로 전송하지 않으며, 회원 탈퇴 시 저장된 정보가 삭제됩니다."
            : "Your nickname, character, coins, and records are stored only on this device.\nNo data is sent to an external server, and deleting your account removes the saved data.";
        SetButtonText(privacyCloseButton, language == Language.Korean ? "닫기" : "CLOSE");
        withdrawTitleText.text = language == Language.Korean ? "회원 탈퇴" : "DELETE ACCOUNT";
        withdrawMessageText.text = language == Language.Korean
            ? "회원 탈퇴를 하시겠습니까?\n저장된 닉네임, 캐릭터, 코인, 기록이 모두 삭제됩니다."
            : "Delete your account?\nYour nickname, character, coins, and records will be removed.";
        SetButtonText(withdrawConfirmButton, language == Language.Korean ? "확인" : "CONFIRM");
        SetButtonText(withdrawCancelButton, language == Language.Korean ? "취소" : "CANCEL");
        homeHubTitleText.text = language == Language.Korean ? "로비" : "LOBBY";
        homeHubPlayerText.text = language == Language.Korean ? "플레이어" : "PLAYER";
        homeHubCoinText.text = language == Language.Korean ? "코인 " + coins : "COINS " + coins;
        SetButtonText(startButton, language == Language.Korean ? "게임 시작" : "START GAME");
        SetButtonText(setupBackButton, language == Language.Korean ? "뒤로" : "BACK");
        SetButtonText(languageCloseButton, language == Language.Korean ? "닫기" : "CLOSE");
        SetButtonText(languageButtons[0], language == Language.Korean ? "한국어" : "KOREAN");
        SetButtonText(languageButtons[1], language == Language.Korean ? "영어" : "ENGLISH");
        SetButtonText(createProfileButton, editingProfile
            ? (language == Language.Korean ? "저장하기" : "SAVE PROFILE")
            : (language == Language.Korean ? "프로필 만들기" : "CREATE PROFILE"));
        SetButtonText(characterButtons[0], language == Language.Korean ? "기사" : "KNIGHT");
        SetButtonText(characterButtons[1], language == Language.Korean ? "마법사" : "MAGE");
        SetButtonText(characterButtons[2], language == Language.Korean ? "해커" : "HACKER");
        SetButtonText(menuButton, language == Language.Korean ? "메뉴" : "MENU");
        SetButtonText(resumeButton, language == Language.Korean ? "계속하기" : "RESUME");
        SetButtonText(pauseMyPageButton, language == Language.Korean ? "마이페이지" : "MY PAGE");
        SetButtonText(pauseMenuButton, language == Language.Korean ? "메뉴" : "MENU");
        SetButtonText(difficultyButtons[0], language == Language.Korean ? "쉬움" : "EASY");
        SetButtonText(difficultyButtons[1], language == Language.Korean ? "보통" : "NORMAL");
        SetButtonText(difficultyButtons[2], language == Language.Korean ? "어려움" : "HARD");
        SetButtonText(modeButtons[0], language == Language.Korean ? "혼합" : "MIX");
        SetButtonText(modeButtons[1], language == Language.Korean ? "한글" : "KOR");
        SetButtonText(modeButtons[2], language == Language.Korean ? "영어" : "ENG");
        SetButtonText(modeButtons[3], language == Language.Korean ? "60초" : "60 SEC");
        SetButtonText(modeButtons[4], language == Language.Korean ? "오늘" : "DAILY");
        setupTitleText.text = language == Language.Korean ? "게임 설정" : "GAME SETTINGS";
        languageTitleText.text = language == Language.Korean ? "언어 설정" : "LANGUAGE";
        profileTitleText.text = language == Language.Korean ? "플레이어 만들기" : "CREATE PLAYER";
        profilePromptText.text = language == Language.Korean ? "닉네임과 캐릭터를 선택하세요" : "Choose a nickname and character";
        myPageTitleText.text = language == Language.Korean ? "마이페이지" : "MY PAGE";
        SetButtonText(myPageCloseButton, language == Language.Korean ? "닫기" : "CLOSE");
        SetButtonText(myPageShopButton, language == Language.Korean ? "상점" : "SHOP");
        SetButtonText(myPageProfileButton, language == Language.Korean ? "프로필 수정" : "EDIT PROFILE");
        SetButtonText(deleteAccountButton, language == Language.Korean ? "탈퇴하기" : "DELETE ACCOUNT");
        SetButtonText(shopCloseButton, language == Language.Korean ? "닫기" : "CLOSE");
        updateTitleText.text = language == Language.Korean ? "업데이트" : "UPDATE";
        SetButtonText(updateLaterButton, language == Language.Korean ? "나중에" : "LATER");
        SetButtonText(updateNowButton, language == Language.Korean ? "업데이트" : "UPDATE");
        difficultyText.text = language == Language.Korean ? "난이도" : "DIFFICULTY";
        modeText.text = language == Language.Korean ? "모드" : "MODE";
        UpdateModeDescription();
        UpdateHighScoreText();
        UpdateProfileSummary();
        UpdateCoinText();
        UpdateMyPage();
        SetButtonText(attendanceCloseButton, language == Language.Korean ? "확인" : "OK");
        attendanceTitleText.text = language == Language.Korean ? "출석 보상" : "DAILY LOGIN";
        attendanceBodyText.text = language == Language.Korean ? "오늘의 출석 보상을 받았습니다!" : "Today's login reward is ready!";
        attendanceCoinText.text = language == Language.Korean ? "+10 코인" : "+10 COINS";
        shopTitleText.text = language == Language.Korean ? "코인 상점" : "COIN SHOP";
        shopCoinText.text = language == Language.Korean ? "코인 " + coins : "COINS " + coins;
        SetButtonText(shopAllCategoryButton, language == Language.Korean ? "전체" : "ALL");
        SetButtonText(shopProfileCategoryButton, language == Language.Korean ? "프로필" : "PROFILES");
        SetButtonText(shopItemCategoryButton, language == Language.Korean ? "아이템" : "ITEMS");
        SetButtonText(nicknameTicketButton, language == Language.Korean ? "닉네임 변경권 30코인" : "NICKNAME TICKET 30 COINS");
        SetButtonText(mageShopButton, language == Language.Korean ? "마법사 프로필 100코인" : "MAGE PROFILE 100 COINS");
        SetButtonText(hackerShopButton, language == Language.Korean ? "해커 프로필 100코인" : "HACKER PROFILE 100 COINS");
        if (inputPlaceholder != null) inputPlaceholder.text = language == Language.Korean ? "여기에 입력..." : "TYPE HERE...";
        UpdateHud();
    }

    private void LegacySetButtonText(Button button, string value)
    {
        if (button == null) return;
        var text = button.GetComponentInChildren<Text>();
        if (text != null) text.text = value;
    }

    private void LegacySetLabelText(Text label, string value)
    {
        if (label != null) label.text = value;
    }

    private void LegacyUpdateCoinText()
    {
        string value = language == Language.Korean ? "코인 " + coins : "COINS " + coins;
        if (coinText != null) coinText.text = value;
        if (homeHubCoinText != null) homeHubCoinText.text = value;
    }

    private void LegacyCheckAttendanceReward()
    {
        if (!profileCreated) return;
        System.DateTime koreaNow = System.DateTime.UtcNow.AddHours(9);
        System.DateTime attendanceDay = koreaNow.Hour < 6 ? koreaNow.Date.AddDays(-1) : koreaNow.Date;
        string attendanceKey = attendanceDay.ToString("yyyy-MM-dd");
        if (playerDataManager.HasAttendance(attendanceKey)) return;

        playerDataManager.GrantAttendance(attendanceKey, 10);
        coins = playerDataManager.Data.coins;
        UpdateCoinText();
        attendancePanel.SetActive(true);
    }

    private void LegacyCloseAttendancePanel()
    {
        attendancePanel.SetActive(false);
    }

    {
        insufficientCoinsTitleText.text = language == Language.Korean ? "코인 부족" : "NOT ENOUGH COINS";
        insufficientCoinsMessageText.text = language == Language.Korean ? "코인이 부족합니다." : "You do not have enough coins.";
        SetButtonText(insufficientCoinsCloseButton, language == Language.Korean ? "확인" : "OK");
        insufficientCoinsPanel.SetActive(true);
    }

    private void LegacyCloseInsufficientCoinsPanel()
    {
        insufficientCoinsPanel.SetActive(false);
    }

    private void LegacyShowAnnouncementPanel()
    {
        if (playing) return;
        homeHubPanel.SetActive(false);
        announcementTitleText.text = language == Language.Korean ? "공지사항" : "ANNOUNCEMENTS";
        announcementMessageText.text = language == Language.Korean
            ? "워드폴에 오신 것을 환영합니다!\n새로운 소식은 이곳에서 확인하세요."
            : "Welcome to Wordfall!\nCheck this panel for the latest news.";
        SetButtonText(announcementCloseButton, language == Language.Korean ? "닫기" : "CLOSE");
        announcementPanel.SetActive(true);
    }

    private void LegacyCloseAnnouncementPanel()
    {
        announcementPanel.SetActive(false);
        ResetHomeHubMenuState();
        homeHubPanel.SetActive(true);
        ShowLobbyDialogue();
    }

    private void LegacyShowPrivacyPanel()
    {
        if (playing) return;
        homeHubMenuPanel.SetActive(false);
        homeHubPanel.SetActive(false);
        privacyPanel.SetActive(true);
    }

    private void LegacyClosePrivacyPanel()
    {
        privacyPanel.SetActive(false);
        ResetHomeHubMenuState();
        homeHubPanel.SetActive(true);
        ShowLobbyDialogue();
    }

    private void LegacyShowWithdrawConfirm()
    {
        if (playing) return;
        homeHubMenuPanel.SetActive(false);
        homeHubPanel.SetActive(false);
        myPagePanel.SetActive(false);
        privacyPanel.SetActive(false);
        withdrawConfirmPanel.SetActive(true);
    }

    private void LegacyCloseWithdrawConfirm()
    {
        withdrawConfirmPanel.SetActive(false);
        if (profileCreated)
        {
            ResetHomeHubMenuState();
            homeHubPanel.SetActive(true);
            ShowLobbyDialogue();
        }
        else
        {
            ShowMainMenu();
        }
    }

    private void LegacyResetHomeHubMenuState()
    {
        homeHubMenuOpen = false;
        if (homeHubMenuCoroutine != null)
        {
            StopCoroutine(homeHubMenuCoroutine);
            homeHubMenuCoroutine = null;
        }
        if (homeHubMenuPanel != null)
        {
            homeHubMenuPanel.SetActive(false);
            homeHubMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-560f, 0f);
        }
        if (homeHubMenuToggleButton != null)
        {
            homeHubMenuToggleButton.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        if (homeHubHamburgerIcon != null) homeHubHamburgerIcon.gameObject.SetActive(true);
        if (homeHubCloseIcon != null) homeHubCloseIcon.SetActive(false);
    }

    private void LegacyConfirmDeleteAccount()
    {
        withdrawConfirmPanel.SetActive(false);
        DeleteAccount();
    }

    private void LegacyUpdateShopText()
    {
        if (shopPanel == null) return;
        shopCoinText.text = language == Language.Korean ? "코인 " + coins + "  ·  변경권 " + nicknameTickets : "COINS " + coins + "  ·  TICKETS " + nicknameTickets;
        shopStatusText.text = language == Language.Korean ? "구매한 상품은 프로필 수정에서 사용할 수 있습니다." : "Purchased items can be used in Edit Profile.";
        SetButtonText(nicknameTicketButton, nicknameTickets > 0
            ? (language == Language.Korean ? "닉네임 변경권 보유 " + nicknameTickets : "NICKNAME TICKETS " + nicknameTickets)
            : (language == Language.Korean ? "닉네임 변경권 30코인" : "NICKNAME TICKET 30 COINS"));
        SetButtonText(knightShopButton, OwnsCharacter(0)
            ? (language == Language.Korean ? "기사 프로필 보유" : "KNIGHT PROFILE OWNED")
            : (language == Language.Korean ? "기사 프로필 100코인" : "KNIGHT PROFILE 100 COINS"));
        SetButtonText(mageShopButton, OwnsCharacter(1)
            ? (language == Language.Korean ? "마법사 프로필 보유" : "MAGE PROFILE OWNED")
            : (language == Language.Korean ? "마법사 프로필 100코인" : "MAGE PROFILE 100 COINS"));
        SetButtonText(hackerShopButton, OwnsCharacter(2)
            ? (language == Language.Korean ? "해커 프로필 보유" : "HACKER PROFILE OWNED")
            : (language == Language.Korean ? "해커 프로필 100코인" : "HACKER PROFILE 100 COINS"));
        SetShopCategory(shopCategory);
    }

    private void LegacyBuyNicknameTicket()
    {
        if (nicknameTickets > 0)
        {
            UpdateShopText();
            return;
        }
        if (coins < 30)
        {
            ShowInsufficientCoinsPanel();
            return;
        }
        coins -= 30;
        nicknameTickets++;
        SaveShopData();
        UpdateCoinText();
        UpdateCharacterButtonStates();
        UpdateShopText();
    }

    private void LegacyBuyCharacter(int characterIndex)
    {
        if (OwnsCharacter(characterIndex))
        {
            shopStatusText.text = language == Language.Korean ? "이미 보유한 프로필입니다." : "You already own this profile.";
            return;
        }
        if (coins < 100)
        {
            ShowInsufficientCoinsPanel();
            return;
        }
        coins -= 100;
        ownedCharacterMask |= 1 << characterIndex;
        SaveShopData();
        UpdateCoinText();
        UpdateCharacterButtonStates();
        UpdateShopText();
    }

    private void LegacyShowUpdatePanel(UpdateChecker.UpdateInfo updateInfo)
    {
        if (updateInfo == null || updatePanel == null) return;
        pendingUpdateUrl = string.IsNullOrWhiteSpace(updateInfo.downloadUrl)
            ? UpdateChecker.DefaultManifestUrl
            : updateInfo.downloadUrl;
        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        setupPanel.SetActive(false);
        languagePanel.SetActive(false);
        profilePanel.SetActive(false);
        myPagePanel.SetActive(false);
        attendancePanel.SetActive(false);
        updateMessageText.text = string.IsNullOrWhiteSpace(updateInfo.message)
            ? (language == Language.Korean ? "새로운 업데이트가 있습니다." : "A new update is available.")
            : updateInfo.message;
        updateVersionText.text = (language == Language.Korean ? "새 버전 " : "NEW VERSION ") + updateInfo.version;
        updatePanel.SetActive(true);
    }

    private void LegacyCloseUpdatePanel()
    {
        updatePanel.SetActive(false);
    }

    private void LegacyOpenUpdatePage()
    {
        if (!string.IsNullOrWhiteSpace(pendingUpdateUrl)) Application.OpenURL(pendingUpdateUrl);
        updatePanel.SetActive(false);
    }

    private void LegacyUpdateMyPage()
    {
        if (myPagePanel == null) return;
        string nickname = string.IsNullOrEmpty(playerDataManager.Data.nickname)
            ? (language == Language.Korean ? "플레이어" : "PLAYER")
            : playerDataManager.Data.nickname;
        int gamesPlayed = playerDataManager.Data.gamesPlayed;
        int wordsTyped = playerDataManager.Data.wordsTyped;
        int correctWords = playerDataManager.Data.correctWords;
        int accuracy = wordsTyped > 0 ? Mathf.RoundToInt((float)correctWords / wordsTyped * 100f) : 0;
        myPageInfoText.text = language == Language.Korean
            ? nickname + "  /  " + GetCharacterName()
            : nickname + "  /  " + GetCharacterName();
        myPageCoinText.text = language == Language.Korean ? "코인 " + coins : "COINS " + coins;
        myPageStatsText.text = language == Language.Korean
            ? "플레이 " + gamesPlayed + "회  ·  타자 " + wordsTyped + "회  ·  정확도 " + accuracy + "%"
            : "GAMES " + gamesPlayed + "  ·  TYPED " + wordsTyped + "  ·  ACCURACY " + accuracy + "%";
        var texture = Resources.Load<Texture2D>(GetCharacterResourcePath(false));
        if (texture != null)
        {
            texture.filterMode = FilterMode.Point;
            myPageCharacterImage.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
            myPageCharacterImage.preserveAspect = true;
        }
    }

    private void LegacyDeleteAccount()
    {
        playerDataManager.DeleteAccount();

        profileCreated = false;
        selectedCharacter = 0;
        coins = 0;
        nicknameTickets = 0;
        ownedCharacterMask = 1;
        UpdateCharacterButtonStates();
        UpdateProfileSummary();
        UpdateCoinText();
        myPagePanel.SetActive(false);
        ShowMainMenu();
    }

    private void LegacySelectLanguageAndClose(Language selectedLanguage)
    {
        SetLanguage(selectedLanguage);
        languagePanel.SetActive(false);
    }

    private void LegacyStartGame()
    {
        session.ResetForNewGame();
        lastSpawnedWord = string.Empty;
        if (gameMode == GameMode.Daily) Random.InitState(System.DateTime.Now.DayOfYear + System.DateTime.Now.Year * 1000);
        spawnTimer = 0.2f;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        SetCharacterEmotion(false);
        statusText.text = language == Language.Korean
            ? GetDifficultyName() + " / " + GetGameModeName() + "  -  ESC 일시정지  F1 빙결  F2 제거"
            : GetDifficultyName() + " / " + GetGameModeName() + "  -  ESC: pause  F1: freeze  F2: clear";
        if (gameMode == GameMode.TimeAttack) statusText.text = language == Language.Korean ? "60초 도전 - 최대한 많은 단어를 입력하세요!" : "60 second challenge - Type as many words as possible!";
        if (gameMode == GameMode.Daily) statusText.text = language == Language.Korean ? "오늘의 도전 - 모두 같은 단어를 만나요!" : "TODAY'S CHALLENGE - Same seed for everyone!";
        inputField.text = string.Empty;
        UpdateHud();
        inputField.ActivateInputField();
    }

    private void LegacyEndGame()
    {
        session.ResetToIdle();
        if (pausePanel != null) pausePanel.SetActive(false);
        int gameCoinReward = Mathf.Max(1, score / 50);
        playerDataManager.RecordGame(score, totalTyped, correctTyped, gameCoinReward);
        coins = playerDataManager.Data.coins;
        UpdateCoinText();
        string accuracy = totalTyped > 0 ? Mathf.RoundToInt((float)correctTyped / totalTyped * 100f) + "% accuracy" : "0% accuracy";
        if (score > highScore)
        {
            highScore = score;
            playerDataManager.Data.highScore = highScore;
            playerDataManager.Save();
            UpdateHighScoreText();
            statusText.text = language == Language.Korean ? "최고 기록!  " + score + "  " + accuracy + "  -  메뉴를 눌러 다시 시작하세요" : "NEW BEST!  " + score + "  " + accuracy + "  -  Press MENU to try again";
        }
        else
        {
            statusText.text = language == Language.Korean ? "게임 종료  점수 " + score + "  " + accuracy + "  -  메뉴를 눌러 다시 시작하세요" : "GAME OVER  SCORE " + score + "  " + accuracy + "  -  Press MENU to try again";
        }
        inputField.DeactivateInputField();
        if (gameOverSummaryText != null)
        {
            gameOverSummaryText.text = language == Language.Korean
                ? "점수 " + score + "\n정확도 " + accuracy.Replace(" accuracy", "") + "\n획득 코인 +" + gameCoinReward
                : "SCORE " + score + "\n" + accuracy + "\nCOINS +" + gameCoinReward;
        }
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    private void LegacyShowSetupFromGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        ShowSetupPanel();
    }

    private void LegacyHandleInput(string value)
    {
        if (!playing || string.IsNullOrWhiteSpace(value)) return;
        for (int i = fallingWords.Count - 1; i >= 0; i--)
        {
            if (fallingWords[i].word != value.Trim()) continue;
            var wordType = fallingWords[i].type;
            correctTyped++;
            Destroy(fallingWords[i].objectToDestroy);
            fallingWords.RemoveAt(i);
            if (wordType == WordType.Golden)
            {
                score += 25 + combo * 3;
                statusText.text = language == Language.Korean ? "골든 단어! 보너스 점수!" : "GOLDEN WORD!  Bonus points!";
            }
            else if (wordType == WordType.Heal)
            {
                score += 15 + combo * 2;
                lives = Mathf.Min(5, lives + 1);
                statusText.text = language == Language.Korean ? "회복 단어! 목숨 +1" : "HEAL WORD!  +1 LIFE";
            }
            else if (wordType == WordType.Bomb)
            {
                score += 20 + combo * 2;
                statusText.text = language == Language.Korean ? "폭탄 해제!" : "BOMB DEFUSED!";
            }
            else if (wordType == WordType.Boss)
            {
                score += 100 + combo * 5;
                statusText.text = language == Language.Korean ? "보스 처치! 큰 보너스!" : "BOSS DEFEATED!  Huge bonus!";
            }
            else
            {
                score += 10 + combo * 2;
                statusText.text = language == Language.Korean ? "정답! 계속 입력하세요." : "NICE! Keep typing.";
            }
            combo++;
            inputField.text = string.Empty;
            UpdateHud();
            RefocusInputField();
            return;
        }
    }

    private void LegacySetGameMode(GameMode selectedMode)
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

    private void LegacyUpdateModeDescription()
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

    private string LegacyGetGameModeName()
    {
        if (gameMode == GameMode.Korean) return language == Language.Korean ? "한글" : "KOREAN";
        if (gameMode == GameMode.English) return language == Language.Korean ? "영어" : "ENGLISH";
        if (gameMode == GameMode.TimeAttack) return language == Language.Korean ? "60초" : "60 SEC";
        if (gameMode == GameMode.Daily) return language == Language.Korean ? "오늘" : "DAILY";
        return language == Language.Korean ? "혼합" : "MIXED";
    }

    private void LegacyActivateFreezeSkill()
    {
        freezeTimer = 5f;
        statusText.text = language == Language.Korean ? "빙결 스킬! 5초" : "FREEZE SKILL!  5 seconds";
    }

    private void LegacyClearScreenSkill()
    {
        for (int i = fallingWords.Count - 1; i >= 0; i--)
        {
            Destroy(fallingWords[i].objectToDestroy);
            fallingWords.RemoveAt(i);
        }
        score += 25;
        statusText.text = language == Language.Korean ? "삭제 스킬! 모든 단어 제거" : "CLEAR SKILL!  All words removed";
        UpdateHud();
    }

    private void LegacySpawnWord()
    {
        bool useKorean = gameMode == GameMode.Korean || (gameMode != GameMode.English && Random.value > 0.5f);
        string[] source = useKorean ? koreanWords : englishWords;
        bool isBoss = wave > 1 && wave % 5 == 0 && spawnedThisWave == 0;
        string word = string.Empty;
        for (int attempt = 0; attempt < source.Length * 3; attempt++)
        {
            string candidate = source[Random.Range(0, source.Length)];
            if (isBoss) candidate += useKorean ? source[Random.Range(0, source.Length)] : " " + source[Random.Range(0, source.Length)];
            if (candidate == lastSpawnedWord || IsWordAlreadyFalling(candidate)) continue;
            word = candidate;
            break;
        }

        if (string.IsNullOrEmpty(word)) return;
        lastSpawnedWord = word;
        var type = isBoss ? WordType.Boss : GetRandomWordType();
        var text = CreateText(word, wordLayer, 30, GetWordColor(type, useKorean), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
        var rect = text.rectTransform;
        rect.sizeDelta = new Vector2(190f, 55f);
        rect.anchoredPosition = GetNonOverlappingSpawnPosition();
        fallingWords.Add(new FallingWord(word, type, rect, text.gameObject, (type == WordType.Boss ? GetMinimumWordSpeed() * 0.8f : Random.Range(GetMinimumWordSpeed(), GetMaximumWordSpeed())) + score * 0.03f));
        spawnedThisWave++;
        if (spawnedThisWave >= 10)
        {
            wave++;
            spawnedThisWave = 0;
            statusText.text = language == Language.Korean ? "웨이브 " + wave + " 도착!" : "WAVE " + wave + " INCOMING!";
            UpdateHud();
        }
    }

    private bool LegacyIsWordAlreadyFalling(string word)
    {
        for (int i = 0; i < fallingWords.Count; i++)
        {
            if (fallingWords[i].word == word) return true;
        }

        return false;
    }

    private Vector2 LegacyGetNonOverlappingSpawnPosition()
    {
        for (int attempt = 0; attempt < 40; attempt++)
        {
            var candidate = new Vector2(Random.Range(-500f, 500f), Random.Range(250f, 330f));
            bool overlaps = false;
            for (int i = 0; i < fallingWords.Count; i++)
            {
                var existing = fallingWords[i].rect.anchoredPosition;
                if (Mathf.Abs(candidate.x - existing.x) < 205f && Mathf.Abs(candidate.y - existing.y) < 72f)
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps) return candidate;
        }

        return new Vector2(Random.Range(-500f, 500f), 330f);
    }

    private WordType LegacyGetRandomWordType()
    {
        return GameRules.GetRandomWordType();
    }

    private Color LegacyGetWordColor(WordType type, bool useKorean)
    {
        return GameRules.GetWordColor(type, useKorean);
    }

    private void LegacySetDifficulty(int selectedDifficulty)
    {
        difficulty = selectedDifficulty;
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

    private string LegacyGetDifficultyName()
    {
        if (difficulty == 0) return language == Language.Korean ? "쉬움" : "EASY";
        if (difficulty == 2) return language == Language.Korean ? "어려움" : "HARD";
        return language == Language.Korean ? "보통" : "NORMAL";
    }

    private float LegacyGetBaseSpawnInterval()
    {
        return GameRules.GetBaseSpawnInterval(difficulty);
    }

    private float LegacyGetMinimumSpawnInterval()
    {
        return GameRules.GetMinimumSpawnInterval(difficulty);
    }

    private float LegacyGetMinimumWordSpeed()
    {
        return GameRules.GetMinimumWordSpeed(difficulty);
    }

    private float LegacyGetMaximumWordSpeed()
    {
        return GameRules.GetMaximumWordSpeed(difficulty);
    }

    private void LegacyUpdateHud()
    {
        scoreText.text = string.Format(language == Language.Korean ? "점수 {0:0000}" : "SCORE {0:0000}", score);
        if (lifeHeartImages != null)
        {
            for (int i = 0; i < lifeHeartImages.Length; i++)
            {
                lifeHeartImages[i].gameObject.SetActive(i < lives);
            }
        }
        comboText.text = (language == Language.Korean ? "콤보 x" : "COMBO x") + combo;
        waveText.text = (language == Language.Korean ? "웨이브 " : "WAVE ") + wave;
        timerText.text = gameMode == GameMode.TimeAttack ? (language == Language.Korean ? "시간 " : "TIME ") + Mathf.CeilToInt(timeRemaining) : "";
    }

    private void LegacyHandleSubmitted(string value)
    {
        if (!playing || paused) return;
        if (string.IsNullOrWhiteSpace(value))
        {
            RefocusInputField();
            return;
        }

        totalTyped++;
        bool matched = false;
        string submitted = value.Trim();
        for (int i = fallingWords.Count - 1; i >= 0; i--)
        {
            if (fallingWords[i].word != submitted) continue;
            matched = true;
            var wordType = fallingWords[i].type;
            correctTyped++;
            Destroy(fallingWords[i].objectToDestroy);
            fallingWords.RemoveAt(i);
            if (wordType == WordType.Golden)
            {
                score += 25 + combo * 3;
                statusText.text = language == Language.Korean ? "골든 단어! 보너스 점수!" : "GOLDEN WORD! Bonus points!";
            }
            else if (wordType == WordType.Heal)
            {
                score += 15 + combo * 2;
                lives = Mathf.Min(5, lives + 1);
                statusText.text = language == Language.Korean ? "회복 단어! 목숨 +1" : "HEAL WORD! +1 LIFE";
            }
            else if (wordType == WordType.Bomb)
            {
                score += 20 + combo * 2;
                statusText.text = language == Language.Korean ? "폭탄 해제!" : "BOMB DEFUSED!";
            }
            else if (wordType == WordType.Boss)
            {
                score += 100 + combo * 5;
                statusText.text = language == Language.Korean ? "보스 처치! 큰 보너스!" : "BOSS DEFEATED! Huge bonus!";
            }
            else
            {
                score += 10 + combo * 2;
                statusText.text = language == Language.Korean ? "정답! 계속 입력하세요." : "NICE! Keep typing.";
            }
            combo++;
            SetCharacterEmotion(false);
            break;
        }

        if (!matched)
        {
            combo = 0;
            SetCharacterEmotion(true);
            statusText.text = language == Language.Korean ? "오답입니다. 다시 입력하세요." : "WRONG WORD. Try again.";
        }

        inputField.text = string.Empty;
        UpdateHud();
        RefocusInputField();
    }

    private string LegacyGetCharacterResourcePath(bool alarm)
    {
        string[] normalPaths = { "Pixel/character-knight", "Pixel/character-mage", "Pixel/character-hacker" };
        string[] alarmPaths = { "Pixel/character-knight-alarm", "Pixel/character-mage-alarm", "Pixel/character-hacker-alarm" };
        int index = Mathf.Clamp(selectedCharacter, 0, normalPaths.Length - 1);
        return alarm ? alarmPaths[index] : normalPaths[index];
    }

    private string LegacyGetLobbyCharacterResourcePath()
    {
        string[] fullBodyPaths = { "Pixel/character-knight-full", "Pixel/character-mage-full", "Pixel/character-hacker-full" };
        int index = Mathf.Clamp(selectedCharacter, 0, fullBodyPaths.Length - 1);
        return Resources.Load<Texture2D>(fullBodyPaths[index]) != null
            ? fullBodyPaths[index]
            : GetCharacterResourcePath(false);
    }

    private void LegacySetCharacterEmotion(bool alarm)
    {
        if (activeCharacterImage == null) return;
        characterEmotionTimer = alarm ? 1.5f : 0f;
        if (characterIsAlarm == alarm && displayedCharacter == selectedCharacter && activeCharacterImage.sprite != null) return;
        characterIsAlarm = alarm;
        displayedCharacter = selectedCharacter;
        var texture = Resources.Load<Texture2D>(GetCharacterResourcePath(alarm));
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        activeCharacterImage.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        activeCharacterImage.preserveAspect = true;
        activeCharacterImage.color = Color.white;
    }

    private Image LegacyCreateCharacterProfile(Transform parent)
    {
        characterStatusPanel = new GameObject("CharacterProfile", typeof(RectTransform), typeof(Image), typeof(Mask));
        characterStatusPanel.transform.SetParent(parent, false);
        var rootRect = characterStatusPanel.GetComponent<RectTransform>();
        SetRect(rootRect, new Vector2(0.88f, 0.22f), new Vector2(0.88f, 0.22f), Vector2.zero, new Vector2(170f, 170f));

        var profileMaskImage = characterStatusPanel.GetComponent<Image>();
        profileMaskImage.sprite = CreateCircleSprite(256);
        profileMaskImage.color = Color.white;
        profileMaskImage.raycastTarget = false;
        characterStatusPanel.GetComponent<Mask>().showMaskGraphic = false;

        var portrait = CreateImage("ActiveCharacter", characterStatusPanel.transform, GetCharacterResourcePath(false), new Vector2(0.5f, 0.5f), new Vector2(116f, 116f));
        portrait.raycastTarget = false;
        var frame = CreateImage("ProfileFrame", characterStatusPanel.transform, "Pixel/character-profile-frame", new Vector2(0.5f, 0.5f), new Vector2(170f, 170f));
        frame.raycastTarget = false;
        return portrait;
    }

    private Sprite LegacyCreateCircleSprite(int size)
    {
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        var center = (size - 1) * 0.5f;
        var radius = size * 0.5f;
        var pixels = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x - center;
                float dy = y - center;
                pixels[y * size + x] = dx * dx + dy * dy <= radius * radius
                    ? new Color32(255, 255, 255, 255)
                    : new Color32(255, 255, 255, 0);
            }
        }
        texture.SetPixels32(pixels);
        texture.Apply();
        return Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), 100f);
    }

    private void LegacyUpdateHighScoreText()
    {
        if (highScoreText != null) highScoreText.text = string.Format(language == Language.Korean ? "최고 점수 {0:0000}" : "BEST SCORE {0:0000}", highScore);
    }

    private void LegacyClearWords()
    {
        for (int i = 0; i < fallingWords.Count; i++) Destroy(fallingWords[i].objectToDestroy);
        fallingWords.Clear();
        if (inputField != null) inputField.text = string.Empty;
    }

    private GameObject LegacyCreatePanel(string name, Transform parent, Color color)
    {
        var objectToCreate = new GameObject(name, typeof(RectTransform), typeof(Image));
        objectToCreate.transform.SetParent(parent, false);
        objectToCreate.GetComponent<Image>().color = color;
        var rect = objectToCreate.GetComponent<RectTransform>();
        SetRect(rect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        return objectToCreate;
    }

    private GameObject LegacyCreateShopItemCard(string name, Transform parent, Vector2 anchor, Vector2 size)
    {
        var card = CreatePanel(name, parent, new Color32(0, 0, 0, 0));
        SetRect(card.GetComponent<RectTransform>(), anchor, anchor, Vector2.zero, size);
        card.GetComponent<Image>().raycastTarget = false;
        return card;
    }

    private void LegacyApplyPixelInputFrame(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/wordfall-input");
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, texture.height * 0.16f, texture.width, texture.height * 0.70f), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, new Vector4(110f, 110f, 110f, 110f));
        image.type = Image.Type.Sliced;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void LegacyApplyPixelRectBackground(GameObject panel, Color fillColor, Color borderColor)
    {
        var image = panel.GetComponent<Image>();
        image.sprite = null;
        image.type = Image.Type.Simple;
        image.color = fillColor;
        image.raycastTarget = false;

        var outline = panel.GetComponent<Outline>();
        if (outline == null) outline = panel.AddComponent<Outline>();
        outline.effectColor = borderColor;
        outline.effectDistance = new Vector2(4f, 4f);
        outline.useGraphicAlpha = true;
    }

    private void LegacyApplyCompactPixelRectImage(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/compact-pixel-rect");
        if (texture == null)
        {
            ApplyPixelRectBackground(panel, new Color32(5, 18, 48, 235), new Color32(41, 162, 235, 255));
            return;
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, new Vector4(120f, 120f, 120f, 120f));
        image.type = Image.Type.Sliced;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void LegacyApplyPixelPlayerInfoCard(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/lobby-player-info-card");
        if (texture == null)
        {
            ApplyCompactPixelRectImage(panel);
            return;
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        image.type = Image.Type.Simple;
        image.preserveAspect = true;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void LegacyApplyPixelHudStrip(GameObject panel, string resourcePath)
    {
        var texture = Resources.Load<Texture2D>(resourcePath);
        if (texture == null)
        {
            ApplyCompactPixelRectImage(panel);
            return;
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        var cropHeight = texture.height * 0.20f;
        var cropY = texture.height * 0.40f;
        image.sprite = Sprite.Create(texture, new Rect(0f, cropY, texture.width, cropHeight), new Vector2(0.5f, 0.5f), 100f);
        image.type = Image.Type.Simple;
        image.preserveAspect = false;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void LegacyApplyPixelScoreHud(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/score-hud-frame");
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, texture.height * 0.14f, texture.width, texture.height * 0.72f), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, new Vector4(130f, 130f, 130f, 130f));
        image.type = Image.Type.Sliced;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void LegacyApplyPixelLobbyBackground(GameObject panel)
    {
        var background = Resources.Load<Texture2D>("Pixel/lobby-background");
        if (background == null) return;
        background.filterMode = FilterMode.Point;
        background.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(background, new Rect(0f, 0f, background.width, background.height), new Vector2(0.5f, 0.5f), 100f);
        image.type = Image.Type.Simple;
        image.preserveAspect = false;
        image.color = Color.white;
    }

    private void LegacyApplyPixelShopWindow(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/coin-shop-window");
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, new Vector4(180f, 180f, 180f, 180f));
        image.type = Image.Type.Sliced;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void LegacyApplyPixelBackground(GameObject panel)
    {
        var background = Resources.Load<Texture2D>("Pixel/wordfall-background");
        if (background == null) return;
        background.filterMode = FilterMode.Point;
        background.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(background, new Rect(0f, 0f, background.width, background.height), new Vector2(0.5f, 0.5f), 100f);
        image.type = Image.Type.Simple;
        image.preserveAspect = false;
        image.color = Color.white;
    }

    private LegacyPixelAmbientMotion LegacyCreateMainAmbientMotion(GameObject panel)
    {
        var motionObject = new GameObject("MainAmbientMotion", typeof(RectTransform));
        motionObject.transform.SetParent(panel.transform, false);
        SetRect(motionObject.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        motionObject.transform.SetAsFirstSibling();
        return motionObject.AddComponent<LegacyPixelAmbientMotion>();
    }

    private void LegacyApplyPixelWindow(GameObject panel)
    {
        var windowTexture = Resources.Load<Texture2D>("Pixel/wordfall-window");
        if (windowTexture == null) return;
        windowTexture.filterMode = FilterMode.Point;
        windowTexture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(
            windowTexture,
            new Rect(0f, 0f, windowTexture.width, windowTexture.height),
            new Vector2(0.5f, 0.5f),
            100f,
            0,
            SpriteMeshType.FullRect,
            new Vector4(180f, 180f, 180f, 180f));
        image.type = Image.Type.Sliced;
        image.preserveAspect = false;
        image.color = Color.white;
    }

    private void LegacyApplyPixelLobbyDialogueWindow(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/lobby-dialogue-window-clean");
        if (texture == null)
            texture = Resources.Load<Texture2D>("Pixel/lobby-dialogue-window");
        if (texture == null)
        {
            ApplyPixelWindow(panel);
            return;
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        image.type = Image.Type.Simple;
        image.preserveAspect = true;
        image.color = Color.white;
    }

    private void LegacyApplyPixelSettingsWindow(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/settings-window");
        if (texture == null)
        {
            ApplyPixelWindow(panel);
            ApplyModalDim(panel);
            return;
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(
            texture,
            new Rect(0f, 0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100f);
        image.type = Image.Type.Simple;
        image.preserveAspect = false;
        image.color = Color.white;
        image.raycastTarget = true;
    }

    private void LegacyApplyPixelGameOverWindow(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/game-over-window");
        if (texture == null)
        {
            ApplyPixelWindow(panel);
            return;
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        image.type = Image.Type.Simple;
        image.preserveAspect = true;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void LegacyApplyPixelAttendanceWindow(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/attendance-reward-window");
        if (texture == null)
        {
            ApplyPixelWindow(panel);
            return;
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        image.type = Image.Type.Simple;
        image.preserveAspect = true;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void LegacyApplyModalDim(GameObject panel)
    {
        var dimObject = new GameObject("ModalDim", typeof(RectTransform), typeof(Image));
        dimObject.transform.SetParent(panel.transform, false);
        var dimImage = dimObject.GetComponent<Image>();
        dimImage.color = new Color32(0, 0, 0, 230);
        dimImage.raycastTarget = true;
        SetRect(dimObject.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        dimObject.transform.SetAsFirstSibling();
    }

    private Image LegacyCreateImage(string name, Transform parent, string resourcePath, Vector2 anchor, Vector2 size)
    {
        var objectToCreate = new GameObject(name, typeof(RectTransform), typeof(Image));
        objectToCreate.transform.SetParent(parent, false);
        var image = objectToCreate.GetComponent<Image>();
        var texture = Resources.Load<Texture2D>(resourcePath);
        if (texture != null)
        {
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
            image.preserveAspect = true;
        }
        SetRect(image.rectTransform, anchor, anchor, Vector2.zero, size);
        return image;
    }

    private Text LegacyCreateText(string content, Transform parent, int size, Color color, Vector2 anchor, Vector2 pivot)
    {
        var objectToCreate = new GameObject("Text", typeof(RectTransform), typeof(Text));
        objectToCreate.transform.SetParent(parent, false);
        var text = objectToCreate.GetComponent<Text>();
        text.text = content;
        text.font = uiFont;
        text.fontSize = size;
        text.color = color;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        var rect = text.rectTransform;
        SetRect(rect, anchor, anchor, Vector2.zero, new Vector2(700f, size + 24f));
        rect.pivot = pivot;
        return text;
    }

    private Button LegacyCreateButton(string label, Transform parent, Vector2 anchor, Vector2 size, Color color)
    {
        var objectToCreate = new GameObject(label + "Button", typeof(RectTransform), typeof(Image), typeof(Button));
        objectToCreate.transform.SetParent(parent, false);
        var image = objectToCreate.GetComponent<Image>();
        var buttonTexture = Resources.Load<Texture2D>("Pixel/wordfall-button");
        if (buttonTexture != null)
        {
            buttonTexture.filterMode = FilterMode.Point;
            image.sprite = Sprite.Create(buttonTexture, new Rect(0f, 0f, buttonTexture.width, buttonTexture.height), new Vector2(0.5f, 0.5f), 100f);
            image.type = Image.Type.Simple;
            image.color = Color.white;
        }
        else
        {
            image.color = color;
        }
        var outline = objectToCreate.AddComponent<Outline>();
        outline.effectColor = new Color32(3, 8, 18, 255);
        outline.effectDistance = new Vector2(4f, -4f);
        outline.useGraphicAlpha = false;
        var button = objectToCreate.GetComponent<Button>();
        button.transition = Selectable.Transition.ColorTint;
        var navigation = button.navigation;
        navigation.mode = Navigation.Mode.None;
        button.navigation = navigation;
        var colors = button.colors;
        colors.normalColor = color;
        colors.highlightedColor = new Color(Mathf.Min(color.r + 0.08f, 1f), Mathf.Min(color.g + 0.08f, 1f), Mathf.Min(color.b + 0.08f, 1f), 1f);
        colors.pressedColor = new Color(color.r * 0.8f, color.g * 0.8f, color.b * 0.8f, 1f);
        colors.selectedColor = color;
        colors.disabledColor = new Color(color.r * 0.45f, color.g * 0.45f, color.b * 0.45f, 0.75f);
        button.colors = colors;
        objectToCreate.AddComponent<ButtonPressFeedback>();
        SetRect(objectToCreate.GetComponent<RectTransform>(), anchor, anchor, Vector2.zero, size);
        var text = CreateText(label, objectToCreate.transform, 22, Color.white, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        text.rectTransform.sizeDelta = size;
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = Mathf.Clamp(Mathf.RoundToInt(size.y * 0.42f), 12, 20);
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = 10;
        text.resizeTextMaxSize = text.fontSize;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        return button;
    }

    private Button LegacyCreateImageAction(string name, Transform parent, string iconPath, string label, Vector2 anchor, Vector2 size, Vector2 iconSize, out Text labelText)
    {
        var objectToCreate = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        objectToCreate.transform.SetParent(parent, false);
        SetRect(objectToCreate.GetComponent<RectTransform>(), anchor, anchor, Vector2.zero, size);

        var hitImage = objectToCreate.GetComponent<Image>();
        hitImage.color = new Color(1f, 1f, 1f, 0f);
        hitImage.raycastTarget = true;

        var button = objectToCreate.GetComponent<Button>();
        button.targetGraphic = hitImage;
        button.transition = Selectable.Transition.ColorTint;
        var navigation = button.navigation;
        navigation.mode = Navigation.Mode.None;
        button.navigation = navigation;
        var colors = button.colors;
        colors.normalColor = new Color(1f, 1f, 1f, 0f);
        colors.highlightedColor = new Color(1f, 1f, 1f, 0f);
        colors.pressedColor = new Color(1f, 1f, 1f, 0f);
        colors.selectedColor = new Color(1f, 1f, 1f, 0f);
        colors.disabledColor = new Color(1f, 1f, 1f, 0f);
        button.colors = colors;
        objectToCreate.AddComponent<ButtonPressFeedback>();

        var icon = CreateImage(name + "Icon", objectToCreate.transform, iconPath, new Vector2(0.5f, 0.65f), iconSize);
        icon.raycastTarget = false;
        labelText = CreateText(label, objectToCreate.transform, 14, new Color32(222, 235, 250, 255), new Vector2(0.5f, 0.09f), new Vector2(0.5f, 0.09f));
        labelText.rectTransform.sizeDelta = new Vector2(size.x, 26f);
        labelText.fontStyle = FontStyle.Normal;
        labelText.fontSize = 14;
        labelText.resizeTextForBestFit = true;
        labelText.resizeTextMinSize = 9;
        labelText.resizeTextMaxSize = 14;
        labelText.horizontalOverflow = HorizontalWrapMode.Overflow;
        labelText.verticalOverflow = VerticalWrapMode.Truncate;
        labelText.raycastTarget = false;
        return button;
    }

    private Button LegacyCreateHamburgerButton(string name, Transform parent, Vector2 anchor, Vector2 size)
    {
        var objectToCreate = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        objectToCreate.transform.SetParent(parent, false);
        SetRect(objectToCreate.GetComponent<RectTransform>(), anchor, anchor, Vector2.zero, size);

        var hitImage = objectToCreate.GetComponent<Image>();
        hitImage.color = new Color(1f, 1f, 1f, 0f);
        hitImage.raycastTarget = true;
        var button = objectToCreate.GetComponent<Button>();
        button.targetGraphic = hitImage;
        button.transition = Selectable.Transition.ColorTint;
        var navigation = button.navigation;
        navigation.mode = Navigation.Mode.None;
        button.navigation = navigation;
        var colors = button.colors;
        colors.normalColor = new Color(1f, 1f, 1f, 0f);
        colors.highlightedColor = new Color(1f, 1f, 1f, 0f);
        colors.pressedColor = new Color(1f, 1f, 1f, 0f);
        colors.selectedColor = new Color(1f, 1f, 1f, 0f);
        colors.disabledColor = new Color(1f, 1f, 1f, 0f);
        button.colors = colors;
        objectToCreate.AddComponent<ButtonPressFeedback>();

        var icon = CreateImage("HamburgerMenuIcon", objectToCreate.transform, "Pixel/menu-hamburger-icon", new Vector2(0.5f, 0.5f), new Vector2(58f, 58f));
        icon.raycastTarget = false;

        return button;
    }

    private GameObject LegacyCreateMenuCloseIcon(Transform parent)
    {
        var closeIcon = new GameObject("MenuCloseIcon", typeof(RectTransform), typeof(Image));
        closeIcon.transform.SetParent(parent, false);
        SetRect(closeIcon.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(58f, 58f));
        var background = closeIcon.GetComponent<Image>();
        background.sprite = null;
        background.type = Image.Type.Simple;
        background.color = new Color(1f, 1f, 1f, 0f);
        background.raycastTarget = false;

        CreateCloseBar(closeIcon.transform, 45f);
        CreateCloseBar(closeIcon.transform, -45f);
        return closeIcon;
    }

    private void LegacyCreateCloseBar(Transform parent, float angle)
    {
        var bar = new GameObject("CloseBar", typeof(RectTransform), typeof(Image));
        bar.transform.SetParent(parent, false);
        SetRect(bar.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(34f, 5f));
        bar.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        var image = bar.GetComponent<Image>();
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private Button LegacyCreateCharacterButton(string label, string portraitPath, Transform parent, Vector2 anchor, Color color)
    {
        var button = CreateButton(label, parent, anchor, new Vector2(210f, 130f), color);
        var portrait = CreateImage("CharacterPortrait", button.transform, portraitPath, new Vector2(0.5f, 0.64f), new Vector2(78f, 78f));
        portrait.raycastTarget = false;
        var labelText = button.GetComponentInChildren<Text>();
        if (labelText != null)
        {
            labelText.rectTransform.anchoredPosition = new Vector2(0f, -43f);
            labelText.rectTransform.sizeDelta = new Vector2(190f, 28f);
            labelText.fontSize = 16;
        }
        return button;
    }

    private InputField LegacyCreateInputField(Transform parent)
    {
        var objectToCreate = new GameObject("TypingInput", typeof(RectTransform), typeof(Image), typeof(InputField));
        objectToCreate.transform.SetParent(parent, false);
        objectToCreate.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        var rect = objectToCreate.GetComponent<RectTransform>();
        SetRect(rect, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(650f, 76f));
        var input = objectToCreate.GetComponent<InputField>();
        input.lineType = InputField.LineType.SingleLine;
        input.caretColor = new Color32(255, 211, 94, 255);
        input.textComponent = CreateText("", objectToCreate.transform, 30, new Color32(255, 255, 255, 255), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        input.textComponent.rectTransform.sizeDelta = new Vector2(620f, 54f);
        inputPlaceholder = CreateText("여기에 입력...", objectToCreate.transform, 22, new Color32(126, 164, 215, 255), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        input.placeholder = inputPlaceholder;
        input.placeholder.rectTransform.sizeDelta = new Vector2(620f, 54f);
        return input;
    }

    private InputField LegacyCreateNicknameInput(Transform parent)
    {
        var objectToCreate = new GameObject("NicknameInput", typeof(RectTransform), typeof(Image), typeof(InputField));
        objectToCreate.transform.SetParent(parent, false);
        objectToCreate.GetComponent<Image>().color = new Color32(245, 248, 255, 255);
        var rect = objectToCreate.GetComponent<RectTransform>();
        SetRect(rect, new Vector2(0.5f, 0.57f), new Vector2(0.5f, 0.57f), Vector2.zero, new Vector2(560f, 58f));
        var input = objectToCreate.GetComponent<InputField>();
        input.lineType = InputField.LineType.SingleLine;
        input.contentType = InputField.ContentType.Name;
        input.characterLimit = 16;
        input.caretColor = new Color32(40, 80, 140, 255);
        input.textComponent = CreateText("", objectToCreate.transform, 24, new Color32(30, 48, 80, 255), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        input.textComponent.rectTransform.sizeDelta = new Vector2(520f, 50f);
        var placeholder = CreateText(language == Language.Korean ? "닉네임 입력..." : "ENTER NICKNAME...", objectToCreate.transform, 20, new Color32(143, 161, 190, 255), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        placeholder.rectTransform.sizeDelta = new Vector2(520f, 50f);
        input.placeholder = placeholder;
        return input;
    }

    private static void LegacySetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size)
    {
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = position;
        if (anchorMin == anchorMax) rect.sizeDelta = size;
        else { rect.offsetMin = position; rect.offsetMax = position + size; }
    }

    private sealed class LegacyButtonPressFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Vector3 baseScale;

        private void Awake()
        {
            baseScale = transform.localScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.localScale = baseScale * 0.94f;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.localScale = baseScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = baseScale * 1.02f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = baseScale;
        }
    }

    private sealed class LegacyPixelAmbientMotion : MonoBehaviour
    {
        private Image backgroundLayer;
        private readonly List<RectTransform> motes = new List<RectTransform>();
        private readonly List<Vector2> offsets = new List<Vector2>();
        private readonly List<float> speeds = new List<float>();
        private readonly List<float> phases = new List<float>();
        private readonly List<float> twinkleSpeeds = new List<float>();
        private readonly List<float> baseAlphas = new List<float>();

        private static readonly Vector2[] Positions =
        {
            new Vector2(0.08f, 0.82f), new Vector2(0.18f, 0.63f), new Vector2(0.29f, 0.88f),
            new Vector2(0.41f, 0.73f), new Vector2(0.57f, 0.84f), new Vector2(0.68f, 0.62f),
            new Vector2(0.79f, 0.79f), new Vector2(0.91f, 0.57f), new Vector2(0.13f, 0.34f),
            new Vector2(0.33f, 0.29f), new Vector2(0.72f, 0.35f), new Vector2(0.87f, 0.24f)
        };

        private void Awake()
        {
            CreateDriftingBackgroundLayer();
            for (int i = 0; i < Positions.Length; i++)
            {
                var moteObject = new GameObject("Mote" + i, typeof(RectTransform), typeof(Image));
                moteObject.transform.SetParent(transform, false);
                var rect = moteObject.GetComponent<RectTransform>();
                rect.anchorMin = Positions[i];
                rect.anchorMax = Positions[i];
                rect.pivot = new Vector2(0.5f, 0.5f);
                float size = i % 3 == 0 ? 7f : 4f;
                rect.sizeDelta = new Vector2(size, size);
                rect.anchoredPosition = Vector2.zero;

                var image = moteObject.GetComponent<Image>();
                image.color = i % 3 == 0
                    ? new Color32(255, 220, 130, 125)
                    : new Color32(162, 215, 255, 100);
                image.raycastTarget = false;

                motes.Add(rect);
                offsets.Add(new Vector2(18f + (i % 3) * 8f, 8f + (i % 2) * 5f));
                speeds.Add(0.12f + (i % 4) * 0.035f);
                phases.Add(i * 0.83f);
                twinkleSpeeds.Add(0.8f + (i % 3) * 0.25f);
                baseAlphas.Add(image.color.a);
            }
        }

        private void CreateDriftingBackgroundLayer()
        {
            var texture = Resources.Load<Texture2D>("Pixel/wordfall-background");
            if (texture == null) return;

            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            var viewportObject = new GameObject("SkyMotionViewport", typeof(RectTransform), typeof(RectMask2D));
            viewportObject.transform.SetParent(transform, false);
            viewportObject.transform.SetAsFirstSibling();
            var viewport = viewportObject.GetComponent<RectTransform>();
            viewport.anchorMin = new Vector2(0f, 0.24f);
            viewport.anchorMax = Vector2.one;
            viewport.offsetMin = Vector2.zero;
            viewport.offsetMax = Vector2.zero;

            var layerObject = new GameObject("DriftingSkyLayer", typeof(RectTransform), typeof(Image));
            layerObject.transform.SetParent(viewportObject.transform, false);

            var rect = layerObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(1440f, 820f);

            backgroundLayer = layerObject.GetComponent<Image>();
            backgroundLayer.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
            backgroundLayer.type = Image.Type.Simple;
            backgroundLayer.preserveAspect = false;
            backgroundLayer.color = new Color(0.42f, 0.62f, 1f, 0.20f);
            backgroundLayer.raycastTarget = false;
        }

        private void Update()
        {
            float time = Time.unscaledTime;
            if (backgroundLayer != null)
            {
                backgroundLayer.rectTransform.anchoredPosition = new Vector2(
                    Mathf.Sin(time * 0.10f) * 30f,
                    Mathf.Cos(time * 0.075f) * 12f);
            }

            for (int i = 0; i < motes.Count; i++)
            {
                float phase = phases[i] + time * speeds[i];
                motes[i].anchoredPosition = new Vector2(
                    Mathf.Sin(phase) * offsets[i].x,
                    Mathf.Cos(phase * 0.83f) * offsets[i].y);

                var image = motes[i].GetComponent<Image>();
                Color color = image.color;
                color.a = baseAlphas[i] * (0.72f + Mathf.Sin(time * twinkleSpeeds[i] + phases[i]) * 0.28f);
                image.color = color;
            }
        }
    }

    private sealed class LegacyFallingWord
    {
        public readonly string word;
        public readonly WordType type;
        public readonly RectTransform rect;
        public readonly GameObject objectToDestroy;
        public readonly float speed;
        public FallingWord(string word, WordType type, RectTransform rect, GameObject objectToDestroy, float speed)
        {
            this.word = word;
            this.type = type;
            this.rect = rect;
            this.objectToDestroy = objectToDestroy;
            this.speed = speed;
        }
    }
    #endif
}
