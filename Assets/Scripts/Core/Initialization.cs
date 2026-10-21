using UnityEngine;

public sealed partial class WordfallPrototype
{
    private const string LanguagePreferenceKey = "Wordfall.Language";

    private void Awake()
    {
        playerDataManager = gameObject.GetComponent<PlayerDataManager>();
        if (playerDataManager == null) playerDataManager = gameObject.AddComponent<PlayerDataManager>();
        var savedAuthUid = PlayerPrefs.GetString("Wordfall.AuthUid", string.Empty);
        AccountDataIsolation.Initialize(savedAuthUid);
        AccountDataIsolation.Ensure(gameObject);
        playerDataManager.Load();
        InitializeSoundEffects();

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
        if (!profileCreated)
        {
            // A new profile starts with no owned character; the character chosen
            // during profile creation becomes the only initially owned character.
            ownedCharacterMask = 0;
            playerDataManager.Data.ownedCharacterMask = 0;
        }
        authSignedIn = PlayerPrefs.GetInt("Wordfall.AuthSignedIn", 0) == 1;
        language = (Language)Mathf.Clamp(PlayerPrefs.GetInt(LanguagePreferenceKey, (int)language), 0, 1);

        var sceneReferences = GetComponent<SceneUiReferences>();
        if (sceneReferences == null || !sceneReferences.ApplyTo(this)) BuildInterface();
        else
        {
            EnsurePrivacyPolicySceneLayout();
            EnsureHomeHubLogoutButton();
            EnsureLogoutConfirmPanel();
            EnsureLobbyDialogueTypingControl();
            EnsureLobbyAudioButtons();
            EnsureMainAmbientMotion();
            EnsureLobbyAmbientMotion();
            WireSceneEvents();
        }
        SetLanguage(language);

        var updateChecker = gameObject.AddComponent<UpdateChecker>();
        updateChecker.UpdateAvailable += ShowUpdatePanel;
        var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (sceneName == "MainMenuScene")
        {
            PlayLobbyMusic();
            ShowMainMenu();
        }
        else if (sceneName == "LobbyScene")
        {
            PlayLobbyMusic();
            ShowHomeHub();
        }
        else if (sceneName == "GameScene" && SceneTransitionState.StartGameRequested)
        {
            gameMode = SceneTransitionState.RequestedMode;
            difficulty = SceneTransitionState.RequestedDifficulty;
            language = SceneTransitionState.RequestedLanguage;
            PlayGameMusic();
            SceneTransitionState.StartGameRequested = false;
            StartGame();
        }
        else if (sceneName == "GameScene")
        {
            PlayGameMusic();
        }
    }
}
