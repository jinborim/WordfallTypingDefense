using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public sealed class WordfallPrototype : MonoBehaviour
{
    private const string RootName = "WordfallPrototypeRuntime";
    private const string HighScoreKey = "WordfallTypingDefense.HighScore";
    private const string ProfileCreatedKey = "WordfallTypingDefense.ProfileCreated";
    private const string NicknameKey = "WordfallTypingDefense.Nickname";
    private const string CharacterKey = "WordfallTypingDefense.Character";
    private const string CoinsKey = "WordfallTypingDefense.Coins";
    private const string AttendanceDayKey = "WordfallTypingDefense.LastAttendanceDay";
    private const string GamesPlayedKey = "WordfallTypingDefense.GamesPlayed";
    private const string WordsTypedKey = "WordfallTypingDefense.WordsTyped";
    private const string CorrectWordsKey = "WordfallTypingDefense.CorrectWords";
    private const string LastScoreKey = "WordfallTypingDefense.LastScore";
    private readonly List<FallingWord> fallingWords = new List<FallingWord>();
    private readonly string[] defaultKoreanWords = { "사과", "하늘", "컴퓨터", "게임", "방어", "키보드", "단어", "우주" };
    private readonly string[] defaultEnglishWords = { "apple", "sky", "computer", "game", "defense", "keyboard", "word", "space" };
    private string[] koreanWords;
    private string[] englishWords;
    private Font uiFont;
    private GameObject mainPanel;
    private GameObject gamePanel;
    private GameObject setupPanel;
    private GameObject homeHubPanel;
    private GameObject languagePanel;
    private GameObject profilePanel;
    private GameObject myPagePanel;
    private GameObject updatePanel;
    private GameObject attendancePanel;
    private Text scoreText;
    private Text livesText;
    private Text comboText;
    private Text statusText;
    private Text difficultyText;
    private Text highScoreText;
    private Text modeText;
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
    private Button languageCloseButton;
    private Button[] characterButtons;
    private Button createProfileButton;
    private Button myPageCloseButton;
    private Button deleteAccountButton;
    private Button profileResetButton;
    private Button attendanceCloseButton;
    private Button menuButton;
    private Button pauseButton;
    private Button resumeButton;
    private Button pauseMyPageButton;
    private Button pauseMenuButton;
    private GameObject pausePanel;
    private GameObject characterStatusPanel;
    private InputField nicknameInput;
    private Image activeCharacterImage;
    private Image homeHubCharacterImage;
    private Image myPageCharacterImage;
    private string pendingUpdateUrl;
    private bool myPageOpenedFromGame;
    private bool languageOpenedFromHomeHub;
    private int selectedCharacter;
    private bool profileCreated;
    private int coins;
    private float spawnTimer;
    private int score;
    private int lives;
    private int combo;
    private int highScore;
    private int wave;
    private int spawnedThisWave;
    private string lastSpawnedWord = string.Empty;
    private int totalTyped;
    private int correctTyped;
    private float timeRemaining;
    private float freezeTimer;
    private float characterEmotionTimer;
    private bool characterIsAlarm;
    private int displayedCharacter = -1;
    private int difficulty = 1;
    private bool playing;
    private bool paused;

    private enum Language
    {
        Korean,
        English
    }

    private Language language = Language.Korean;

    private enum WordType
    {
        Normal,
        Golden,
        Bomb,
        Heal,
        Boss
    }

    private enum GameMode
    {
        Mixed,
        Korean,
        English,
        TimeAttack,
        Daily
    }

    private GameMode gameMode = GameMode.Mixed;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Boot()
    {
        if (GameObject.Find(RootName) != null) return;
        var root = new GameObject(RootName);
        DontDestroyOnLoad(root);
        root.AddComponent<WordfallPrototype>();
    }

    private void Awake()
    {
        koreanWords = LoadWords("Words/korean", defaultKoreanWords);
        englishWords = LoadWords("Words/english", defaultEnglishWords);
        uiFont = Resources.Load<Font>("Fonts/Mulmaru");
        if (uiFont == null) uiFont = Font.CreateDynamicFontFromOSFont("Malgun Gothic", 32);
        if (uiFont == null) uiFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        profileCreated = PlayerPrefs.GetInt(ProfileCreatedKey, 0) == 1;
        selectedCharacter = Mathf.Clamp(PlayerPrefs.GetInt(CharacterKey, 0), 0, 2);
        coins = PlayerPrefs.GetInt(CoinsKey, 0);
        BuildInterface();
        var updateChecker = gameObject.AddComponent<UpdateChecker>();
        updateChecker.UpdateAvailable += ShowUpdatePanel;
        ShowMainMenu();
        if (profileCreated) CheckAttendanceReward();
    }

    private string[] LoadWords(string resourcePath, string[] fallback)
    {
        var wordAsset = Resources.Load<TextAsset>(resourcePath);
        if (wordAsset == null) return fallback;

        var lines = wordAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        var words = new List<string>();
        for (int i = 0; i < lines.Length; i++)
        {
            string word = lines[i].Trim();
            if (string.IsNullOrEmpty(word) || word.StartsWith("#")) continue;
            words.Add(word);
        }

        return words.Count > 0 ? words.ToArray() : fallback;
    }

    private void Update()
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

    private void HandleShortcuts()
    {
        if (Keyboard.current == null) return;
        if (Keyboard.current.escapeKey.wasPressedThisFrame) TogglePause();
        if (Keyboard.current.f1Key.wasPressedThisFrame && !paused) ActivateFreezeSkill();
        if (Keyboard.current.f2Key.wasPressedThisFrame && !paused) ClearScreenSkill();
    }

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
        ApplyPixelBackground(gamePanel);
        CreateImage("PixelShieldLogo", mainPanel.transform, "Pixel/wordfall-shield", new Vector2(0.5f, 0.80f), new Vector2(150f, 150f));
        titleText = CreateText("워드폴", mainPanel.transform, 64, new Color32(241, 247, 255, 255), new Vector2(0.5f, 0.65f), new Vector2(0.5f, 0.65f));
        subtitleText = CreateText("한글 & 영어 타자 디펜스", mainPanel.transform, 22, new Color32(158, 180, 220, 255), new Vector2(0.5f, 0.57f), new Vector2(0.5f, 0.57f));
        descriptionText = CreateText("떨어지는 단어를 빠르게 입력해 기지를 지키세요.", mainPanel.transform, 18, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.48f), new Vector2(0.5f, 0.48f));
        mainStartButton = CreateButton("게임 시작", mainPanel.transform, new Vector2(0.38f, 0.21f), new Vector2(340f, 78f), new Color32(255, 196, 61, 255));
        mainStartButton.onClick.AddListener(ShowHomeHub);
        languageOpenButton = CreateButton("언어 설정", mainPanel.transform, new Vector2(0.66f, 0.21f), new Vector2(250f, 78f), new Color32(58, 80, 122, 255));
        languageOpenButton.onClick.AddListener(ShowLanguagePanel);
        profileSummaryText = CreateText("", mainPanel.transform, 16, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.105f), new Vector2(0.5f, 0.105f));

        homeHubPanel = CreatePanel("HomeHubPanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(homeHubPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(homeHubPanel);
        ApplyModalDim(homeHubPanel);
        homeHubTitleText = CreateText("플레이어 홈", homeHubPanel.transform, 42, Color.white, new Vector2(0.5f, 0.78f), new Vector2(0.5f, 0.78f));
        homeHubCharacterImage = CreateImage("HomeHubCharacter", homeHubPanel.transform, GetCharacterResourcePath(false), new Vector2(0.5f, 0.61f), new Vector2(112f, 112f));
        var homeHubPortrait = homeHubCharacterImage;
        homeHubPortrait.raycastTarget = false;
        var homeHubFrame = CreateImage("HomeHubProfileFrame", homeHubPanel.transform, "Pixel/character-profile-frame", new Vector2(0.5f, 0.61f), new Vector2(136f, 136f));
        homeHubFrame.raycastTarget = false;
        homeHubPlayerText = CreateText("PLAYER", homeHubPanel.transform, 22, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.42f), new Vector2(0.5f, 0.42f));
        homeHubCoinText = CreateText("COINS 0", homeHubPanel.transform, 20, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.36f), new Vector2(0.5f, 0.36f));
        homeHubStartButton = CreateButton("게임 시작", homeHubPanel.transform, new Vector2(0.5f, 0.27f), new Vector2(330f, 68f), new Color32(255, 196, 61, 255));
        homeHubStartButton.onClick.AddListener(ShowSetupPanel);
        homeHubMyPageButton = CreateButton("마이페이지", homeHubPanel.transform, new Vector2(0.34f, 0.15f), new Vector2(230f, 58f), new Color32(105, 76, 162, 255));
        homeHubMyPageButton.onClick.AddListener(ShowMyPage);
        homeHubLanguageButton = CreateButton("언어 설정", homeHubPanel.transform, new Vector2(0.66f, 0.15f), new Vector2(230f, 58f), new Color32(58, 80, 122, 255));
        homeHubLanguageButton.onClick.AddListener(ShowLanguagePanel);
        homeHubBackButton = CreateButton("뒤로", homeHubPanel.transform, new Vector2(0.12f, 0.15f), new Vector2(150f, 58f), new Color32(78, 160, 255, 255));
        homeHubBackButton.onClick.AddListener(CloseHomeHub);
        homeHubPanel.SetActive(false);

        setupPanel = CreatePanel("SetupPanel", mainPanel.transform, new Color32(9, 18, 38, 245));
        SetRect(setupPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(setupPanel);
        ApplyModalDim(setupPanel);
        setupTitleText = CreateText("게임 설정", setupPanel.transform, 38, Color.white, new Vector2(0.5f, 0.78f), new Vector2(0.5f, 0.78f));
        difficultyText = CreateText("난이도  보통", setupPanel.transform, 22, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.65f), new Vector2(0.5f, 0.65f));
        difficultyButtons = new Button[3];
        difficultyButtons[0] = CreateButton("쉬움", setupPanel.transform, new Vector2(0.29f, 0.55f), new Vector2(210f, 62f), new Color32(55, 102, 149, 255));
        difficultyButtons[1] = CreateButton("보통", setupPanel.transform, new Vector2(0.5f, 0.55f), new Vector2(210f, 62f), new Color32(67, 149, 255, 255));
        difficultyButtons[2] = CreateButton("어려움", setupPanel.transform, new Vector2(0.71f, 0.55f), new Vector2(210f, 62f), new Color32(149, 71, 92, 255));
        difficultyButtons[0].onClick.AddListener(() => SetDifficulty(0));
        difficultyButtons[1].onClick.AddListener(() => SetDifficulty(1));
        difficultyButtons[2].onClick.AddListener(() => SetDifficulty(2));
        modeText = CreateText("모드  혼합", setupPanel.transform, 20, new Color32(147, 232, 189, 255), new Vector2(0.5f, 0.38f), new Vector2(0.5f, 0.38f));
        modeButtons = new Button[5];
        modeButtons[0] = CreateButton("혼합", setupPanel.transform, new Vector2(0.12f, 0.28f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        modeButtons[1] = CreateButton("한글", setupPanel.transform, new Vector2(0.31f, 0.28f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        modeButtons[2] = CreateButton("영어", setupPanel.transform, new Vector2(0.50f, 0.28f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        modeButtons[3] = CreateButton("60초", setupPanel.transform, new Vector2(0.69f, 0.28f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        modeButtons[4] = CreateButton("오늘", setupPanel.transform, new Vector2(0.88f, 0.28f), new Vector2(150f, 52f), new Color32(58, 80, 122, 255));
        for (int i = 0; i < modeButtons.Length; i++)
        {
            int selectedMode = i;
            modeButtons[i].onClick.AddListener(() => SetGameMode((GameMode)selectedMode));
        }
        startButton = CreateButton("게임 시작", setupPanel.transform, new Vector2(0.5f, 0.14f), new Vector2(360f, 72f), new Color32(255, 196, 61, 255));
        startButton.onClick.AddListener(StartGame);
        setupBackButton = CreateButton("뒤로", setupPanel.transform, new Vector2(0.12f, 0.14f), new Vector2(180f, 60f), new Color32(78, 160, 255, 255));
        setupBackButton.onClick.AddListener(CloseSetupPanel);
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
        myPageCloseButton = CreateButton("닫기", myPagePanel.transform, new Vector2(0.30f, 0.16f), new Vector2(200f, 62f), new Color32(58, 80, 122, 255));
        myPageCloseButton.onClick.AddListener(CloseMyPage);
        deleteAccountButton = CreateButton("탈퇴하기", myPagePanel.transform, new Vector2(0.70f, 0.16f), new Vector2(220f, 62f), new Color32(149, 71, 92, 255));
        deleteAccountButton.onClick.AddListener(DeleteAccount);
        myPagePanel.SetActive(false);

        attendancePanel = CreatePanel("AttendancePanel", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(attendancePanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(attendancePanel);
        ApplyModalDim(attendancePanel);
        attendanceTitleText = CreateText("출석 보상", attendancePanel.transform, 42, Color.white, new Vector2(0.5f, 0.70f), new Vector2(0.5f, 0.70f));
        attendanceBodyText = CreateText("오늘의 출석 보상을 받았습니다!", attendancePanel.transform, 20, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.58f), new Vector2(0.5f, 0.58f));
        attendanceCoinText = CreateText("+10 COINS", attendancePanel.transform, 34, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.43f), new Vector2(0.5f, 0.43f));
        attendanceCloseButton = CreateButton("확인", attendancePanel.transform, new Vector2(0.5f, 0.22f), new Vector2(220f, 64f), new Color32(67, 149, 255, 255));
        attendanceCloseButton.onClick.AddListener(CloseAttendancePanel);
        attendancePanel.SetActive(false);

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
        SetRect(scoreHud.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(760f, 64f));
        ApplyPixelScoreHud(scoreHud);
        scoreText = CreateText("SCORE 0000", header.transform, 24, Color.white, new Vector2(0.31f, 0.5f), new Vector2(0.31f, 0.5f));
        livesText = CreateText("LIVES 5", header.transform, 24, new Color32(255, 211, 94, 255), new Vector2(0.76f, 0.5f), new Vector2(0.76f, 0.5f));
        comboText = CreateText("COMBO x0", header.transform, 20, new Color32(147, 232, 189, 255), new Vector2(0.61f, 0.5f), new Vector2(0.61f, 0.5f));
        waveText = CreateText("WAVE 1", header.transform, 18, new Color32(193, 205, 230, 255), new Vector2(0.46f, 0.5f), new Vector2(0.46f, 0.5f));
        timerText = CreateText("", header.transform, 18, new Color32(255, 142, 142, 255), new Vector2(0.88f, 0.5f), new Vector2(0.88f, 0.5f));
        activeCharacterImage = CreateCharacterProfile(gamePanel.transform);
        var wordLayerObject = CreatePanel("WordLayer", gamePanel.transform, new Color32(0, 0, 0, 0));
        wordLayer = wordLayerObject.GetComponent<RectTransform>();
        SetRect(wordLayer, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(26f, 92f), new Vector2(-26f, -190f));
        wordLayerObject.AddComponent<RectMask2D>();
        var inputBackground = CreatePanel("InputBackground", gamePanel.transform, new Color32(0, 0, 0, 0));
        SetRect(inputBackground.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 122f), new Vector2(780f, 88f));
        ApplyPixelInputFrame(inputBackground);
        inputBackground.transform.SetAsLastSibling();
        inputField = CreateInputField(inputBackground.transform);
        statusText = CreateText("Type a word to defend your base!", gamePanel.transform, 18, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f));
        statusText.rectTransform.anchoredPosition = new Vector2(0f, 34f);
        menuButton = CreateButton("메뉴", gamePanel.transform, new Vector2(0.08f, 0.93f), new Vector2(120f, 46f), new Color32(58, 80, 122, 255));
        menuButton.onClick.AddListener(ShowMainMenu);
        pauseButton = CreateButton("일시정지", gamePanel.transform, new Vector2(0.92f, 0.93f), new Vector2(120f, 46f), new Color32(58, 80, 122, 255));
        pauseButton.onClick.AddListener(TogglePause);
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
    }

    private void EnsureInputSystemEventSystem()
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

    private void SetLanguage(Language selectedLanguage)
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
        SetButtonText(homeHubMyPageButton, language == Language.Korean ? "마이페이지" : "MY PAGE");
        SetButtonText(homeHubLanguageButton, language == Language.Korean ? "언어 설정" : "LANGUAGE");
        SetButtonText(homeHubBackButton, language == Language.Korean ? "뒤로" : "BACK");
        homeHubTitleText.text = language == Language.Korean ? "플레이어 홈" : "PLAYER HOME";
        homeHubPlayerText.text = language == Language.Korean ? "플레이어" : "PLAYER";
        homeHubCoinText.text = language == Language.Korean ? "코인 " + coins : "COINS " + coins;
        SetButtonText(startButton, language == Language.Korean ? "게임 시작" : "START GAME");
        SetButtonText(setupBackButton, language == Language.Korean ? "뒤로" : "BACK");
        SetButtonText(languageCloseButton, language == Language.Korean ? "닫기" : "CLOSE");
        SetButtonText(languageButtons[0], language == Language.Korean ? "한국어" : "KOREAN");
        SetButtonText(languageButtons[1], language == Language.Korean ? "영어" : "ENGLISH");
        SetButtonText(createProfileButton, language == Language.Korean ? "프로필 만들기" : "CREATE PROFILE");
        SetButtonText(characterButtons[0], language == Language.Korean ? "기사" : "KNIGHT");
        SetButtonText(characterButtons[1], language == Language.Korean ? "마법사" : "MAGE");
        SetButtonText(characterButtons[2], language == Language.Korean ? "해커" : "HACKER");
        SetButtonText(menuButton, language == Language.Korean ? "메뉴" : "MENU");
        SetButtonText(pauseButton, language == Language.Korean ? "일시정지" : "PAUSE");
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
        SetButtonText(deleteAccountButton, language == Language.Korean ? "탈퇴하기" : "DELETE ACCOUNT");
        updateTitleText.text = language == Language.Korean ? "업데이트" : "UPDATE";
        SetButtonText(updateLaterButton, language == Language.Korean ? "나중에" : "LATER");
        SetButtonText(updateNowButton, language == Language.Korean ? "업데이트" : "UPDATE");
        difficultyText.text = (language == Language.Korean ? "난이도  " : "DIFFICULTY  ") + GetDifficultyName();
        modeText.text = (language == Language.Korean ? "모드  " : "MODE  ") + GetGameModeName();
        UpdateHighScoreText();
        UpdateProfileSummary();
        UpdateCoinText();
        UpdateMyPage();
        SetButtonText(attendanceCloseButton, language == Language.Korean ? "확인" : "OK");
        attendanceTitleText.text = language == Language.Korean ? "출석 보상" : "DAILY LOGIN";
        attendanceBodyText.text = language == Language.Korean ? "오늘의 출석 보상을 받았습니다!" : "Today's login reward is ready!";
        attendanceCoinText.text = language == Language.Korean ? "+10 코인" : "+10 COINS";
        if (inputPlaceholder != null) inputPlaceholder.text = language == Language.Korean ? "여기에 입력..." : "TYPE HERE...";
        UpdateHud();
    }

    private void SetButtonText(Button button, string value)
    {
        if (button == null) return;
        var text = button.GetComponentInChildren<Text>();
        if (text != null) text.text = value;
    }

    private void ShowProfilePanel()
    {
        setupPanel.SetActive(false);
        languagePanel.SetActive(false);
        profilePanel.SetActive(true);
        nicknameInput.text = string.Empty;
        profileErrorText.text = string.Empty;
        SelectCharacter(selectedCharacter);
        EventSystem.current.SetSelectedGameObject(nicknameInput.gameObject);
        nicknameInput.ActivateInputField();
    }

    private void SelectCharacter(int characterIndex)
    {
        selectedCharacter = Mathf.Clamp(characterIndex, 0, characterButtons.Length - 1);
        UpdateCharacterButtonStates();
    }

    private void UpdateCharacterButtonStates()
    {
        if (characterButtons == null) return;
        for (int i = 0; i < characterButtons.Length; i++)
        {
            var colors = characterButtons[i].colors;
            colors.normalColor = i == selectedCharacter ? new Color32(255, 211, 94, 255) : Color.white;
            colors.highlightedColor = new Color32(255, 232, 150, 255);
            colors.pressedColor = new Color32(216, 173, 68, 255);
            characterButtons[i].colors = colors;
        }
    }

    private void CreateProfile()
    {
        var nickname = nicknameInput.text.Trim();
        if (nickname.Length < 2)
        {
            profileErrorText.text = language == Language.Korean ? "닉네임을 2글자 이상 입력하세요." : "Enter at least 2 characters.";
            nicknameInput.ActivateInputField();
            return;
        }

        PlayerPrefs.SetString(NicknameKey, nickname);
        PlayerPrefs.SetInt(CharacterKey, selectedCharacter);
        PlayerPrefs.SetInt(ProfileCreatedKey, 1);
        PlayerPrefs.Save();
        profileCreated = true;
        UpdateProfileSummary();
        profilePanel.SetActive(false);
        ShowSetupPanel();
        CheckAttendanceReward();
    }

    private string GetCharacterName()
    {
        if (selectedCharacter == 1) return language == Language.Korean ? "마법사" : "MAGE";
        if (selectedCharacter == 2) return language == Language.Korean ? "해커" : "HACKER";
        return language == Language.Korean ? "기사" : "KNIGHT";
    }

    private void UpdateProfileSummary()
    {
        if (!profileCreated)
        {
            if (profileSummaryText != null) profileSummaryText.text = string.Empty;
            if (homeHubPlayerText != null) homeHubPlayerText.text = string.Empty;
            return;
        }

        string nickname = PlayerPrefs.GetString(NicknameKey, "PLAYER");
        string summary = language == Language.Korean
            ? "플레이어  " + nickname + "  /  " + GetCharacterName()
            : "PLAYER  " + nickname + "  /  " + GetCharacterName();
        if (profileSummaryText != null) profileSummaryText.text = summary;
        if (homeHubPlayerText != null) homeHubPlayerText.text = summary;
    }

    private void UpdateCoinText()
    {
        string value = language == Language.Korean ? "코인 " + coins : "COINS " + coins;
        if (coinText != null) coinText.text = value;
        if (homeHubCoinText != null) homeHubCoinText.text = value;
    }

    private void CheckAttendanceReward()
    {
        if (!profileCreated) return;
        System.DateTime koreaNow = System.DateTime.UtcNow.AddHours(9);
        System.DateTime attendanceDay = koreaNow.Hour < 6 ? koreaNow.Date.AddDays(-1) : koreaNow.Date;
        string attendanceKey = attendanceDay.ToString("yyyy-MM-dd");
        if (PlayerPrefs.GetString(AttendanceDayKey, string.Empty) == attendanceKey) return;

        coins += 10;
        PlayerPrefs.SetInt(CoinsKey, coins);
        PlayerPrefs.SetString(AttendanceDayKey, attendanceKey);
        PlayerPrefs.Save();
        UpdateCoinText();
        attendancePanel.SetActive(true);
    }

    private void CloseAttendancePanel()
    {
        attendancePanel.SetActive(false);
    }

    private void ShowMainMenu()
    {
        playing = false;
        paused = false;
        ClearWords();
        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        if (setupPanel != null) setupPanel.SetActive(false);
        if (homeHubPanel != null) homeHubPanel.SetActive(false);
        if (languagePanel != null) languagePanel.SetActive(false);
        if (profilePanel != null) profilePanel.SetActive(false);
        if (myPagePanel != null) myPagePanel.SetActive(false);
        if (attendancePanel != null) attendancePanel.SetActive(false);
        if (updatePanel != null) updatePanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    private void ShowHomeHub()
    {
        if (playing) return;
        if (!profileCreated)
        {
            ShowProfilePanel();
            return;
        }

        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        setupPanel.SetActive(false);
        languagePanel.SetActive(false);
        profilePanel.SetActive(false);
        myPagePanel.SetActive(false);
        UpdateProfileSummary();
        UpdateCoinText();
        UpdateHomeHubCharacter();
        homeHubPanel.SetActive(true);
    }

    private void CloseHomeHub()
    {
        homeHubPanel.SetActive(false);
    }

    private void UpdateHomeHubCharacter()
    {
        if (homeHubCharacterImage == null) return;
        var texture = Resources.Load<Texture2D>(GetCharacterResourcePath(false));
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        homeHubCharacterImage.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        homeHubCharacterImage.preserveAspect = true;
    }

    private void ShowSetupPanel()
    {
        if (playing) return;
        if (!profileCreated)
        {
            ShowProfilePanel();
            return;
        }
        homeHubPanel.SetActive(false);
        languagePanel.SetActive(false);
        setupPanel.SetActive(true);
        SetDifficulty(difficulty);
        SetGameMode(gameMode);
    }

    private void CloseSetupPanel()
    {
        setupPanel.SetActive(false);
        if (profileCreated) homeHubPanel.SetActive(true);
    }

    private void ShowLanguagePanel()
    {
        if (playing) return;
        languageOpenedFromHomeHub = homeHubPanel != null && homeHubPanel.activeSelf;
        if (languageOpenedFromHomeHub) homeHubPanel.SetActive(false);
        setupPanel.SetActive(false);
        languagePanel.SetActive(true);
    }

    private void CloseLanguagePanel()
    {
        languagePanel.SetActive(false);
        if (languageOpenedFromHomeHub && profileCreated) homeHubPanel.SetActive(true);
        languageOpenedFromHomeHub = false;
    }

    private void ShowMyPage()
    {
        if (!profileCreated || (playing && !paused)) return;
        myPageOpenedFromGame = playing;
        if (myPageOpenedFromGame)
        {
            gamePanel.SetActive(false);
            mainPanel.SetActive(true);
        }
        setupPanel.SetActive(false);
        languagePanel.SetActive(false);
        profilePanel.SetActive(false);
        UpdateMyPage();
        myPagePanel.SetActive(true);
    }

    private void CloseMyPage()
    {
        myPagePanel.SetActive(false);
        if (myPageOpenedFromGame)
        {
            mainPanel.SetActive(false);
            gamePanel.SetActive(true);
            pausePanel.SetActive(true);
        }
        myPageOpenedFromGame = false;
    }

    private void ShowUpdatePanel(UpdateChecker.UpdateInfo updateInfo)
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

    private void CloseUpdatePanel()
    {
        updatePanel.SetActive(false);
    }

    private void OpenUpdatePage()
    {
        if (!string.IsNullOrWhiteSpace(pendingUpdateUrl)) Application.OpenURL(pendingUpdateUrl);
        updatePanel.SetActive(false);
    }

    private void UpdateMyPage()
    {
        if (myPagePanel == null) return;
        string nickname = PlayerPrefs.GetString(NicknameKey, language == Language.Korean ? "플레이어" : "PLAYER");
        int gamesPlayed = PlayerPrefs.GetInt(GamesPlayedKey, 0);
        int wordsTyped = PlayerPrefs.GetInt(WordsTypedKey, 0);
        int correctWords = PlayerPrefs.GetInt(CorrectWordsKey, 0);
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

    private void DeleteAccount()
    {
        PlayerPrefs.DeleteKey(ProfileCreatedKey);
        PlayerPrefs.DeleteKey(NicknameKey);
        PlayerPrefs.DeleteKey(CharacterKey);
        PlayerPrefs.DeleteKey(CoinsKey);
        PlayerPrefs.DeleteKey(AttendanceDayKey);
        PlayerPrefs.DeleteKey(GamesPlayedKey);
        PlayerPrefs.DeleteKey(WordsTypedKey);
        PlayerPrefs.DeleteKey(CorrectWordsKey);
        PlayerPrefs.DeleteKey(LastScoreKey);
        PlayerPrefs.DeleteKey(HighScoreKey);
        PlayerPrefs.Save();

        profileCreated = false;
        selectedCharacter = 0;
        coins = 0;
        UpdateCharacterButtonStates();
        UpdateProfileSummary();
        UpdateCoinText();
        myPagePanel.SetActive(false);
        ShowMainMenu();
    }

    private void SelectLanguageAndClose(Language selectedLanguage)
    {
        SetLanguage(selectedLanguage);
        languagePanel.SetActive(false);
    }

    private void StartGame()
    {
        score = 0;
        lives = 5;
        combo = 0;
        wave = 1;
        spawnedThisWave = 0;
        lastSpawnedWord = string.Empty;
        totalTyped = 0;
        correctTyped = 0;
        paused = false;
        freezeTimer = 0f;
        timeRemaining = 60f;
        if (gameMode == GameMode.Daily) Random.InitState(System.DateTime.Now.DayOfYear + System.DateTime.Now.Year * 1000);
        spawnTimer = 0.2f;
        playing = true;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
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

    private void EndGame()
    {
        playing = false;
        paused = false;
        if (pausePanel != null) pausePanel.SetActive(false);
        PlayerPrefs.SetInt(GamesPlayedKey, PlayerPrefs.GetInt(GamesPlayedKey, 0) + 1);
        PlayerPrefs.SetInt(WordsTypedKey, PlayerPrefs.GetInt(WordsTypedKey, 0) + totalTyped);
        PlayerPrefs.SetInt(CorrectWordsKey, PlayerPrefs.GetInt(CorrectWordsKey, 0) + correctTyped);
        PlayerPrefs.SetInt(LastScoreKey, score);
        PlayerPrefs.Save();
        string accuracy = totalTyped > 0 ? Mathf.RoundToInt((float)correctTyped / totalTyped * 100f) + "% accuracy" : "0% accuracy";
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
            UpdateHighScoreText();
            statusText.text = language == Language.Korean ? "최고 기록!  " + score + "  " + accuracy + "  -  메뉴를 눌러 다시 시작하세요" : "NEW BEST!  " + score + "  " + accuracy + "  -  Press MENU to try again";
        }
        else
        {
            statusText.text = language == Language.Korean ? "게임 종료  점수 " + score + "  " + accuracy + "  -  메뉴를 눌러 다시 시작하세요" : "GAME OVER  SCORE " + score + "  " + accuracy + "  -  Press MENU to try again";
        }
        inputField.DeactivateInputField();
    }

    private void HandleInput(string value)
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

    private void SetGameMode(GameMode selectedMode)
    {
        gameMode = selectedMode;
        modeText.text = (language == Language.Korean ? "모드  " : "MODE  ") + GetGameModeName();
        for (int i = 0; i < modeButtons.Length; i++)
        {
            var colors = modeButtons[i].colors;
            colors.normalColor = i == (int)gameMode ? new Color32(255, 211, 94, 255) : new Color32(58, 80, 122, 255);
            colors.highlightedColor = new Color32(255, 232, 150, 255);
            colors.pressedColor = new Color32(216, 173, 68, 255);
            modeButtons[i].colors = colors;
        }
    }

    private string GetGameModeName()
    {
        if (gameMode == GameMode.Korean) return language == Language.Korean ? "한글" : "KOREAN";
        if (gameMode == GameMode.English) return language == Language.Korean ? "영어" : "ENGLISH";
        if (gameMode == GameMode.TimeAttack) return language == Language.Korean ? "60초" : "60 SEC";
        if (gameMode == GameMode.Daily) return language == Language.Korean ? "오늘" : "DAILY";
        return language == Language.Korean ? "혼합" : "MIXED";
    }

    private void TogglePause()
    {
        if (!playing) return;
        paused = !paused;
        pausePanel.SetActive(paused);
        if (paused) inputField.DeactivateInputField();
        else RefocusInputField();
    }

    private void ActivateFreezeSkill()
    {
        freezeTimer = 5f;
        statusText.text = language == Language.Korean ? "빙결 스킬! 5초" : "FREEZE SKILL!  5 seconds";
    }

    private void ClearScreenSkill()
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

    private void RefocusInputField()
    {
        if (!playing) return;
        StartCoroutine(RefocusInputFieldNextFrame());
    }

    private IEnumerator RefocusInputFieldNextFrame()
    {
        yield return null;
        if (!playing || inputField == null) yield break;
        EventSystem.current.SetSelectedGameObject(inputField.gameObject);
        inputField.ActivateInputField();
        inputField.caretPosition = inputField.text.Length;
    }

    private void SpawnWord()
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

    private bool IsWordAlreadyFalling(string word)
    {
        for (int i = 0; i < fallingWords.Count; i++)
        {
            if (fallingWords[i].word == word) return true;
        }

        return false;
    }

    private Vector2 GetNonOverlappingSpawnPosition()
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

    private WordType GetRandomWordType()
    {
        var roll = Random.value;
        if (roll < 0.10f) return WordType.Golden;
        if (roll < 0.17f) return WordType.Bomb;
        if (roll < 0.24f) return WordType.Heal;
        return WordType.Normal;
    }

    private Color GetWordColor(WordType type, bool useKorean)
    {
        if (type == WordType.Golden) return new Color32(255, 211, 94, 255);
        if (type == WordType.Bomb) return new Color32(255, 105, 115, 255);
        if (type == WordType.Heal) return new Color32(121, 235, 169, 255);
        if (type == WordType.Boss) return new Color32(211, 137, 255, 255);
        return useKorean ? new Color32(255, 220, 130, 255) : new Color32(133, 212, 255, 255);
    }

    private void SetDifficulty(int selectedDifficulty)
    {
        difficulty = selectedDifficulty;
        difficultyText.text = (language == Language.Korean ? "난이도  " : "DIFFICULTY  ") + GetDifficultyName();
        for (int i = 0; i < difficultyButtons.Length; i++)
        {
            var colors = difficultyButtons[i].colors;
            colors.normalColor = i == difficulty ? new Color32(255, 211, 94, 255) : new Color32(58, 80, 122, 255);
            colors.highlightedColor = new Color32(255, 232, 150, 255);
            colors.pressedColor = new Color32(216, 173, 68, 255);
            difficultyButtons[i].colors = colors;
        }
    }

    private string GetDifficultyName()
    {
        if (difficulty == 0) return language == Language.Korean ? "쉬움" : "EASY";
        if (difficulty == 2) return language == Language.Korean ? "어려움" : "HARD";
        return language == Language.Korean ? "보통" : "NORMAL";
    }

    private float GetBaseSpawnInterval()
    {
        if (difficulty == 0) return 2.35f;
        if (difficulty == 2) return 1.35f;
        return 1.8f;
    }

    private float GetMinimumSpawnInterval()
    {
        if (difficulty == 0) return 1.2f;
        if (difficulty == 2) return 0.55f;
        return 0.75f;
    }

    private float GetMinimumWordSpeed()
    {
        if (difficulty == 0) return 35f;
        if (difficulty == 2) return 65f;
        return 45f;
    }

    private float GetMaximumWordSpeed()
    {
        if (difficulty == 0) return 55f;
        if (difficulty == 2) return 105f;
        return 75f;
    }

    private void UpdateHud()
    {
        scoreText.text = string.Format(language == Language.Korean ? "점수 {0:0000}" : "SCORE {0:0000}", score);
        livesText.text = (language == Language.Korean ? "목숨 " : "LIVES ") + lives;
        comboText.text = (language == Language.Korean ? "콤보 x" : "COMBO x") + combo;
        waveText.text = (language == Language.Korean ? "웨이브 " : "WAVE ") + wave;
        timerText.text = gameMode == GameMode.TimeAttack ? (language == Language.Korean ? "시간 " : "TIME ") + Mathf.CeilToInt(timeRemaining) : "";
    }

    private void HandleSubmitted(string value)
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

    private string GetCharacterResourcePath(bool alarm)
    {
        string[] normalPaths = { "Pixel/character-knight", "Pixel/character-mage", "Pixel/character-hacker" };
        string[] alarmPaths = { "Pixel/character-knight-alarm", "Pixel/character-mage-alarm", "Pixel/character-hacker-alarm" };
        int index = Mathf.Clamp(selectedCharacter, 0, normalPaths.Length - 1);
        return alarm ? alarmPaths[index] : normalPaths[index];
    }

    private void SetCharacterEmotion(bool alarm)
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

    private Image CreateCharacterProfile(Transform parent)
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

    private Sprite CreateCircleSprite(int size)
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

    private void UpdateHighScoreText()
    {
        if (highScoreText != null) highScoreText.text = string.Format(language == Language.Korean ? "최고 점수 {0:0000}" : "BEST SCORE {0:0000}", highScore);
    }

    private void ClearWords()
    {
        for (int i = 0; i < fallingWords.Count; i++) Destroy(fallingWords[i].objectToDestroy);
        fallingWords.Clear();
        if (inputField != null) inputField.text = string.Empty;
    }

    private GameObject CreatePanel(string name, Transform parent, Color color)
    {
        var objectToCreate = new GameObject(name, typeof(RectTransform), typeof(Image));
        objectToCreate.transform.SetParent(parent, false);
        objectToCreate.GetComponent<Image>().color = color;
        var rect = objectToCreate.GetComponent<RectTransform>();
        SetRect(rect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        return objectToCreate;
    }

    private void ApplyPixelInputFrame(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/wordfall-input");
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, new Vector4(150f, 150f, 150f, 150f));
        image.type = Image.Type.Sliced;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void ApplyPixelScoreHud(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/score-hud-frame");
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, new Vector4(180f, 180f, 180f, 180f));
        image.type = Image.Type.Sliced;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void ApplyPixelBackground(GameObject panel)
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

    private void ApplyPixelWindow(GameObject panel)
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

    private void ApplyModalDim(GameObject panel)
    {
        var dimObject = new GameObject("ModalDim", typeof(RectTransform), typeof(Image));
        dimObject.transform.SetParent(panel.transform, false);
        var dimImage = dimObject.GetComponent<Image>();
        dimImage.color = new Color32(0, 0, 0, 230);
        dimImage.raycastTarget = true;
        SetRect(dimObject.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        dimObject.transform.SetAsFirstSibling();
    }

    private Image CreateImage(string name, Transform parent, string resourcePath, Vector2 anchor, Vector2 size)
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

    private Text CreateText(string content, Transform parent, int size, Color color, Vector2 anchor, Vector2 pivot)
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

    private Button CreateButton(string label, Transform parent, Vector2 anchor, Vector2 size, Color color)
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
        var colors = button.colors;
        colors.normalColor = color;
        colors.highlightedColor = new Color(Mathf.Min(color.r + 0.08f, 1f), Mathf.Min(color.g + 0.08f, 1f), Mathf.Min(color.b + 0.08f, 1f), 1f);
        colors.pressedColor = new Color(color.r * 0.8f, color.g * 0.8f, color.b * 0.8f, 1f);
        button.colors = colors;
        SetRect(objectToCreate.GetComponent<RectTransform>(), anchor, anchor, Vector2.zero, size);
        var text = CreateText(label, objectToCreate.transform, 22, Color.white, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        text.rectTransform.sizeDelta = size;
        text.fontSize = Mathf.Clamp(Mathf.RoundToInt(size.y * 0.42f), 12, 20);
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = 10;
        text.resizeTextMaxSize = text.fontSize;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        return button;
    }

    private Button CreateCharacterButton(string label, string portraitPath, Transform parent, Vector2 anchor, Color color)
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

    private InputField CreateInputField(Transform parent)
    {
        var objectToCreate = new GameObject("TypingInput", typeof(RectTransform), typeof(Image), typeof(InputField));
        objectToCreate.transform.SetParent(parent, false);
        objectToCreate.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        var rect = objectToCreate.GetComponent<RectTransform>();
        SetRect(rect, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(720f, 68f));
        var input = objectToCreate.GetComponent<InputField>();
        input.lineType = InputField.LineType.SingleLine;
        input.caretColor = new Color32(255, 211, 94, 255);
        input.textComponent = CreateText("", objectToCreate.transform, 30, new Color32(255, 255, 255, 255), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        input.textComponent.rectTransform.sizeDelta = new Vector2(690f, 62f);
        inputPlaceholder = CreateText("여기에 입력...", objectToCreate.transform, 22, new Color32(126, 164, 215, 255), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        input.placeholder = inputPlaceholder;
        input.placeholder.rectTransform.sizeDelta = new Vector2(690f, 62f);
        return input;
    }

    private InputField CreateNicknameInput(Transform parent)
    {
        var objectToCreate = new GameObject("NicknameInput", typeof(RectTransform), typeof(Image), typeof(InputField));
        objectToCreate.transform.SetParent(parent, false);
        objectToCreate.GetComponent<Image>().color = new Color32(245, 248, 255, 255);
        var rect = objectToCreate.GetComponent<RectTransform>();
        SetRect(rect, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 100f), new Vector2(560f, 58f));
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

    private static void SetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size)
    {
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = position;
        if (anchorMin == anchorMax) rect.sizeDelta = size;
        else { rect.offsetMin = position; rect.offsetMax = position + size; }
    }

    private sealed class FallingWord
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
}
