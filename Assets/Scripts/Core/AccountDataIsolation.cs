using UnityEngine;

public sealed class AccountDataIsolation : MonoBehaviour
{
    private const string AccountPrefix = "WordfallTypingDefense.Account.";
    private const string DefaultAccount = "local";
    private static readonly string[] DataKeys =
    {
        "HighScore", "ProfileCreated", "Nickname", "Character", "Coins",
        "NicknameTickets", "OwnedCharacters", "LastAttendanceDay", "GamesPlayed",
        "WordsTyped", "CorrectWords", "LastScore"
    };

    private static string activeAccount = DefaultAccount;
    private static bool initialized;
    private float syncTimer;

    public static void Ensure(GameObject host)
    {
        if (host != null && host.GetComponent<AccountDataIsolation>() == null)
            host.AddComponent<AccountDataIsolation>();
    }

    public static void Initialize(string userId)
    {
        activeAccount = Normalize(userId);
        initialized = true;
        if (activeAccount == DefaultAccount && !PlayerPrefs.HasKey(AccountKey(activeAccount, "ProfileCreated")))
            SaveSharedKeysToAccount(activeAccount);
        LoadAccountIntoSharedKeys(activeAccount);
    }

    public static void SwitchToAccount(string userId)
    {
        if (!initialized) Initialize(DefaultAccount);
        SaveSharedKeysToAccount(activeAccount);
        activeAccount = Normalize(userId);
        LoadAccountIntoSharedKeys(activeAccount);
    }

    public static void DeleteActiveAccount()
    {
        foreach (var key in DataKeys)
            PlayerPrefs.DeleteKey(AccountKey(activeAccount, key));
        PlayerPrefs.Save();
    }

    private void Update()
    {
        if (!initialized) return;
        syncTimer += Time.unscaledDeltaTime;
        if (syncTimer < 0.5f) return;
        syncTimer = 0f;
        SaveSharedKeysToAccount(activeAccount);
    }

    private static void LoadAccountIntoSharedKeys(string account)
    {
        foreach (var key in DataKeys)
        {
            var storedKey = AccountKey(account, key);
            var sharedKey = SharedKey(key);
            if (!PlayerPrefs.HasKey(storedKey))
            {
                PlayerPrefs.DeleteKey(sharedKey);
                continue;
            }

            if (key == "Nickname" || key == "LastAttendanceDay")
                PlayerPrefs.SetString(sharedKey, PlayerPrefs.GetString(storedKey, string.Empty));
            else
                PlayerPrefs.SetInt(sharedKey, PlayerPrefs.GetInt(storedKey, 0));
        }
        PlayerPrefs.Save();
    }

    private static void SaveSharedKeysToAccount(string account)
    {
        foreach (var key in DataKeys)
        {
            var sharedKey = SharedKey(key);
            var storedKey = AccountKey(account, key);
            if (!PlayerPrefs.HasKey(sharedKey))
            {
                PlayerPrefs.DeleteKey(storedKey);
                continue;
            }

            if (key == "Nickname" || key == "LastAttendanceDay")
                PlayerPrefs.SetString(storedKey, PlayerPrefs.GetString(sharedKey, string.Empty));
            else
                PlayerPrefs.SetInt(storedKey, PlayerPrefs.GetInt(sharedKey, 0));
        }
        PlayerPrefs.Save();
    }

    private static string SharedKey(string key) => "WordfallTypingDefense." + key;
    private static string AccountKey(string account, string key) => AccountPrefix + account + "." + key;

    private static string Normalize(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId)) return DefaultAccount;
        var characters = userId.Trim().ToCharArray();
        for (var i = 0; i < characters.Length; i++)
        {
            if (!char.IsLetterOrDigit(characters[i]) && characters[i] != '-' && characters[i] != '_')
                characters[i] = '_';
        }
        return new string(characters);
    }
}
