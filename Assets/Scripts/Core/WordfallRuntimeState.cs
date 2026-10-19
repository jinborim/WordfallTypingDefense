using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
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
    private GameObject purchaseCompletePanel;
    private GameObject nicknameTicketPanel;
    private GameObject announcementPanel;
    private GameObject privacyPanel;
    private GameObject withdrawConfirmPanel;
    private RectTransform shopProductContent;
    private ScrollRect shopScrollRect;
    private Scrollbar shopVerticalScrollbar;
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
    private Text purchaseCompleteTitleText;
    private Text purchaseCompleteMessageText;
    private Text nicknameTicketTitleText;
    private Text nicknameTicketMessageText;
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
    private Button purchaseCompleteCloseButton;
    private Button nicknameTicketCloseButton;
    private Button nicknameTicketShopButton;
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
        "\uC624\uB298\uB3C4 \uB2E8\uC5B4\uAC00 \uB0B4\uB824\uC624\uAE30 \uC804\uC5D0 \uAE30\uC9C0\uB97C \uC9C0\uCF1C\uBCF4\uC790.",
        "\uCF64\uBCF4\uB97C \uC774\uC5B4\uAC00\uBA74 \uB354 \uB192\uC740 \uC810\uC218\uB97C \uC5BB\uC744 \uC218 \uC788\uC5B4.",
        "\uC704\uD5D8\uD55C \uB2E8\uC5B4\uB294 \uBE60\uB974\uAC8C \uC785\uB825\uD574\uC11C \uAE30\uC9C0\uB97C \uC548\uC804\uD558\uAC8C \uC9C0\uD0A4\uC790.",
        "\uCD9C\uC11D \uBCF4\uC0C1\uACFC \uAC8C\uC784 \uD50C\uB808\uC774\uB85C \uCF54\uC778\uC744 \uBAA8\uC744 \uC218 \uC788\uC5B4.",
        "\uC0C1\uC810\uC5D0\uC11C \uC0C8\uB85C\uC6B4 \uCE90\uB9AD\uD130\uC640 \uB2C9\uB124\uC784 \uBCC0\uACBD\uAD8C\uC744 \uD655\uC778\uD574\uBD10.",
        "\uC900\uBE44\uAC00 \uB05D\uB098\uBA74 \uAC8C\uC784 \uC2DC\uC791 \uBC84\uD2BC\uC744 \uB20C\uB7EC \uC791\uC804\uC744 \uC2DC\uC791\uD558\uC790."
    };

    #if false
    private readonly string[] legacyKoreanLobbyDialogues =
    {
        "??삳뮎????λ선揶쎛 ?????븍┛ ?袁⑸퓠 疫꿸퀣???筌왖?녹뮆???",
        "?꾠끇?ョ몴???곷선揶쎛筌????誘? ?癒?땾????댁뱽 ????됰선.",
        "?袁る퓮????λ선????뫀????낆젾??곴퐣 疫꿸퀣?????? ??꾩쓺 ??륁쁽.",
        "?꾨뗄??? ?곗뮇苑?癰귣똻湲썸?野껊슣?????쟿??以?筌뤴뫁??????됰선.",
        "?怨몄젎?癒?퐣 ??덉쨮??筌?Ŧ??怨? ??곌퐬??癰궰野껋럡????類ㅼ뵥????",
        "餓Β??쑨? ??멸돌筌?野껊슣????뽰삂 甕곌쑵??????쑎 ?臾믪읈????뽰삂??륁쁽."
    };
    #endif
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

    private GameMode gameMode { get => settings.Mode; set => settings.Mode = value; }

}
