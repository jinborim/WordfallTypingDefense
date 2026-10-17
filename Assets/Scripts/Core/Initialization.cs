using UnityEngine;

public sealed partial class WordfallPrototype
{
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
        SetLanguage(language);

        var updateChecker = gameObject.AddComponent<UpdateChecker>();
        updateChecker.UpdateAvailable += ShowUpdatePanel;
        ShowMainMenu();
    }
}
