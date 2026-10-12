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
    private readonly List<FallingWord> fallingWords = new List<FallingWord>();
    private readonly string[] koreanWords = { "사과", "하늘", "컴퓨터", "게임", "방어", "키보드", "단어", "우주" };
    private readonly string[] englishWords = { "apple", "sky", "computer", "game", "defense", "keyboard", "word", "space" };
    private Font uiFont;
    private GameObject mainPanel;
    private GameObject gamePanel;
    private GameObject setupPanel;
    private GameObject languagePanel;
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
    private InputField inputField;
    private RectTransform wordLayer;
    private Button[] difficultyButtons;
    private Button[] modeButtons;
    private Button[] languageButtons;
    private Button mainStartButton;
    private Button languageOpenButton;
    private Button startButton;
    private Button setupBackButton;
    private Button languageCloseButton;
    private Button menuButton;
    private Button pauseButton;
    private Button resumeButton;
    private Button pauseMenuButton;
    private GameObject pausePanel;
    private float spawnTimer;
    private int score;
    private int lives;
    private int combo;
    private int highScore;
    private int wave;
    private int spawnedThisWave;
    private int totalTyped;
    private int correctTyped;
    private float timeRemaining;
    private float freezeTimer;
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
        uiFont = Resources.Load<Font>("Fonts/Mulmaru");
        if (uiFont == null) uiFont = Font.CreateDynamicFontFromOSFont("Malgun Gothic", 32);
        if (uiFont == null) uiFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        BuildInterface();
        ShowMainMenu();
    }

    private void Update()
    {
        if (!playing) return;
        HandleShortcuts();
        if (paused) return;
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
                Destroy(word.objectToDestroy);
                fallingWords.RemoveAt(i);
                if (word.type == WordType.Bomb)
                {
                    lives -= 2;
                    statusText.text = "BOMB MISSED!  -2 LIVES";
                }
                else
                {
                    lives--;
                    statusText.text = "A word got through!";
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
        mainStartButton = CreateButton("게임 시작", mainPanel.transform, new Vector2(0.38f, 0.21f), new Vector2(340f, 78f), new Color32(67, 149, 255, 255));
        mainStartButton.onClick.AddListener(ShowSetupPanel);
        languageOpenButton = CreateButton("언어 설정", mainPanel.transform, new Vector2(0.66f, 0.21f), new Vector2(250f, 78f), new Color32(58, 80, 122, 255));
        languageOpenButton.onClick.AddListener(ShowLanguagePanel);
        highScoreText = CreateText("최고 점수 0000", mainPanel.transform, 18, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.07f), new Vector2(0.5f, 0.07f));
        prototypeText = CreateText("픽셀 프로토타입", mainPanel.transform, 14, new Color32(120, 140, 180, 255), new Vector2(0.5f, 0.035f), new Vector2(0.5f, 0.035f));

        setupPanel = CreatePanel("SetupPanel", mainPanel.transform, new Color32(9, 18, 38, 245));
        SetRect(setupPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
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
        startButton = CreateButton("게임 시작", setupPanel.transform, new Vector2(0.5f, 0.12f), new Vector2(360f, 72f), new Color32(67, 149, 255, 255));
        startButton.onClick.AddListener(StartGame);
        setupBackButton = CreateButton("뒤로", setupPanel.transform, new Vector2(0.12f, 0.12f), new Vector2(180f, 60f), new Color32(58, 80, 122, 255));
        setupBackButton.onClick.AddListener(CloseSetupPanel);
        setupPanel.SetActive(false);

        languagePanel = CreatePanel("LanguagePanel", mainPanel.transform, new Color32(9, 18, 38, 245));
        SetRect(languagePanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        languageTitleText = CreateText("언어 설정", languagePanel.transform, 42, Color.white, new Vector2(0.5f, 0.70f), new Vector2(0.5f, 0.70f));
        languageButtons = new Button[2];
        languageButtons[0] = CreateButton("한국어", languagePanel.transform, new Vector2(0.37f, 0.48f), new Vector2(260f, 76f), new Color32(67, 149, 255, 255));
        languageButtons[1] = CreateButton("ENGLISH", languagePanel.transform, new Vector2(0.63f, 0.48f), new Vector2(260f, 76f), new Color32(58, 80, 122, 255));
        languageButtons[0].onClick.AddListener(() => SelectLanguageAndClose(Language.Korean));
        languageButtons[1].onClick.AddListener(() => SelectLanguageAndClose(Language.English));
        languageCloseButton = CreateButton("닫기", languagePanel.transform, new Vector2(0.5f, 0.25f), new Vector2(220f, 60f), new Color32(58, 80, 122, 255));
        languageCloseButton.onClick.AddListener(CloseLanguagePanel);
        languagePanel.SetActive(false);
        UpdateHighScoreText();
        UpdateHighScoreText();

        var header = CreatePanel("Header", gamePanel.transform, new Color32(21, 35, 66, 255));
        SetRect(header.GetComponent<RectTransform>(), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -36f), new Vector2(1280f, 72f));
        scoreText = CreateText("SCORE 0000", header.transform, 24, Color.white, new Vector2(0.12f, 0.5f), new Vector2(0.12f, 0.5f));
        livesText = CreateText("LIVES 5", header.transform, 24, new Color32(255, 211, 94, 255), new Vector2(0.76f, 0.5f), new Vector2(0.76f, 0.5f));
        comboText = CreateText("COMBO x0", header.transform, 20, new Color32(147, 232, 189, 255), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        waveText = CreateText("WAVE 1", header.transform, 18, new Color32(193, 205, 230, 255), new Vector2(0.34f, 0.5f), new Vector2(0.34f, 0.5f));
        timerText = CreateText("", header.transform, 18, new Color32(255, 142, 142, 255), new Vector2(0.90f, 0.5f), new Vector2(0.90f, 0.5f));
        wordLayer = CreatePanel("WordLayer", gamePanel.transform, new Color32(0, 0, 0, 0)).GetComponent<RectTransform>();
        SetRect(wordLayer, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(26f, 92f), new Vector2(-26f, -168f));
        var inputBackground = CreatePanel("InputBackground", gamePanel.transform, new Color32(24, 43, 76, 255));
        SetRect(inputBackground.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 110f), new Vector2(720f, 70f));
        inputField = CreateInputField(inputBackground.transform);
        statusText = CreateText("Type a word to defend your base!", gamePanel.transform, 18, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.08f), new Vector2(0.5f, 0.08f));
        menuButton = CreateButton("메뉴", gamePanel.transform, new Vector2(0.08f, 0.93f), new Vector2(120f, 46f), new Color32(58, 80, 122, 255));
        menuButton.onClick.AddListener(ShowMainMenu);
        pauseButton = CreateButton("일시정지", gamePanel.transform, new Vector2(0.92f, 0.93f), new Vector2(120f, 46f), new Color32(58, 80, 122, 255));
        pauseButton.onClick.AddListener(TogglePause);
        inputField.onValueChanged.AddListener(HandleInput);
        inputField.onEndEdit.AddListener(HandleSubmitted);

        pausePanel = CreatePanel("PausePanel", gamePanel.transform, new Color32(9, 18, 38, 235));
        CreateText("일시정지", pausePanel.transform, 52, Color.white, new Vector2(0.5f, 0.62f), new Vector2(0.5f, 0.62f));
        CreateText("ESC를 누르면 계속", pausePanel.transform, 18, new Color32(171, 190, 220, 255), new Vector2(0.5f, 0.53f), new Vector2(0.5f, 0.53f));
        resumeButton = CreateButton("계속하기", pausePanel.transform, new Vector2(0.5f, 0.39f), new Vector2(220f, 58f), new Color32(67, 149, 255, 255));
        resumeButton.onClick.AddListener(TogglePause);
        pauseMenuButton = CreateButton("메뉴", pausePanel.transform, new Vector2(0.5f, 0.27f), new Vector2(220f, 52f), new Color32(58, 80, 122, 255));
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
        SetButtonText(startButton, language == Language.Korean ? "게임 시작" : "START GAME");
        SetButtonText(setupBackButton, language == Language.Korean ? "뒤로" : "BACK");
        SetButtonText(languageCloseButton, language == Language.Korean ? "닫기" : "CLOSE");
        SetButtonText(languageButtons[0], language == Language.Korean ? "한국어" : "KOREAN");
        SetButtonText(languageButtons[1], language == Language.Korean ? "영어" : "ENGLISH");
        SetButtonText(menuButton, language == Language.Korean ? "메뉴" : "MENU");
        SetButtonText(pauseButton, language == Language.Korean ? "일시정지" : "PAUSE");
        SetButtonText(resumeButton, language == Language.Korean ? "계속하기" : "RESUME");
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
        difficultyText.text = (language == Language.Korean ? "난이도  " : "DIFFICULTY  ") + GetDifficultyName();
        modeText.text = (language == Language.Korean ? "모드  " : "MODE  ") + GetGameModeName();
        UpdateHighScoreText();
        if (inputPlaceholder != null) inputPlaceholder.text = language == Language.Korean ? "여기에 입력..." : "TYPE HERE...";
        UpdateHud();
    }

    private void SetButtonText(Button button, string value)
    {
        if (button == null) return;
        var text = button.GetComponentInChildren<Text>();
        if (text != null) text.text = value;
    }

    private void ShowMainMenu()
    {
        playing = false;
        paused = false;
        ClearWords();
        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        if (setupPanel != null) setupPanel.SetActive(false);
        if (languagePanel != null) languagePanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    private void ShowSetupPanel()
    {
        if (playing) return;
        languagePanel.SetActive(false);
        setupPanel.SetActive(true);
        SetDifficulty(difficulty);
        SetGameMode(gameMode);
    }

    private void CloseSetupPanel()
    {
        setupPanel.SetActive(false);
    }

    private void ShowLanguagePanel()
    {
        if (playing) return;
        setupPanel.SetActive(false);
        languagePanel.SetActive(true);
    }

    private void CloseLanguagePanel()
    {
        languagePanel.SetActive(false);
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
        string accuracy = totalTyped > 0 ? Mathf.RoundToInt((float)correctTyped / totalTyped * 100f) + "% accuracy" : "0% accuracy";
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
            UpdateHighScoreText();
            statusText.text = "NEW BEST!  " + score + "  " + accuracy + "  -  Press MENU to try again";
        }
        else
        {
            statusText.text = "GAME OVER  SCORE " + score + "  " + accuracy + "  -  Press MENU to try again";
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
                statusText.text = "GOLDEN WORD!  Bonus points!";
            }
            else if (wordType == WordType.Heal)
            {
                score += 15 + combo * 2;
                lives = Mathf.Min(5, lives + 1);
                statusText.text = "HEAL WORD!  +1 LIFE";
            }
            else if (wordType == WordType.Bomb)
            {
                score += 20 + combo * 2;
                statusText.text = "BOMB DEFUSED!";
            }
            else if (wordType == WordType.Boss)
            {
                score += 100 + combo * 5;
                statusText.text = "BOSS DEFEATED!  Huge bonus!";
            }
            else
            {
                score += 10 + combo * 2;
                statusText.text = "NICE! Keep typing.";
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
        statusText.text = "FREEZE SKILL!  5 seconds";
    }

    private void ClearScreenSkill()
    {
        for (int i = fallingWords.Count - 1; i >= 0; i--)
        {
            Destroy(fallingWords[i].objectToDestroy);
            fallingWords.RemoveAt(i);
        }
        score += 25;
        statusText.text = "CLEAR SKILL!  All words removed";
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
        string word = source[Random.Range(0, source.Length)];
        bool isBoss = wave > 1 && wave % 5 == 0 && spawnedThisWave == 0;
        if (isBoss) word += useKorean ? source[Random.Range(0, source.Length)] : " " + source[Random.Range(0, source.Length)];
        var type = isBoss ? WordType.Boss : GetRandomWordType();
        var text = CreateText(word, wordLayer, 30, GetWordColor(type, useKorean), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
        var rect = text.rectTransform;
        rect.sizeDelta = new Vector2(190f, 55f);
        rect.anchoredPosition = new Vector2(Random.Range(-470f, 470f), 300f);
        fallingWords.Add(new FallingWord(word, type, rect, text.gameObject, (type == WordType.Boss ? GetMinimumWordSpeed() * 0.8f : Random.Range(GetMinimumWordSpeed(), GetMaximumWordSpeed())) + score * 0.03f));
        spawnedThisWave++;
        if (spawnedThisWave >= 10)
        {
            wave++;
            spawnedThisWave = 0;
            statusText.text = "WAVE " + wave + " INCOMING!";
            UpdateHud();
        }
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
        if (playing && !paused && !string.IsNullOrWhiteSpace(value)) totalTyped++;
        RefocusInputField();
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

    private InputField CreateInputField(Transform parent)
    {
        var objectToCreate = new GameObject("TypingInput", typeof(RectTransform), typeof(Image), typeof(InputField));
        objectToCreate.transform.SetParent(parent, false);
        objectToCreate.GetComponent<Image>().color = new Color32(245, 248, 255, 255);
        var rect = objectToCreate.GetComponent<RectTransform>();
        SetRect(rect, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(650f, 56f));
        var input = objectToCreate.GetComponent<InputField>();
        input.lineType = InputField.LineType.SingleLine;
        input.caretColor = new Color32(40, 80, 140, 255);
        input.textComponent = CreateText("", objectToCreate.transform, 26, new Color32(30, 48, 80, 255), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        input.textComponent.rectTransform.sizeDelta = new Vector2(620f, 50f);
        inputPlaceholder = CreateText("여기에 입력...", objectToCreate.transform, 22, new Color32(143, 161, 190, 255), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        input.placeholder = inputPlaceholder;
        input.placeholder.rectTransform.sizeDelta = new Vector2(620f, 50f);
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
