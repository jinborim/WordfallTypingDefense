using UnityEngine;

public sealed class PlayerDataManager : MonoBehaviour
{
    private const string HighScoreKey = "WordfallTypingDefense.HighScore";
    private const string ProfileCreatedKey = "WordfallTypingDefense.ProfileCreated";
    private const string NicknameKey = "WordfallTypingDefense.Nickname";
    private const string CharacterKey = "WordfallTypingDefense.Character";
    private const string CoinsKey = "WordfallTypingDefense.Coins";
    private const string NicknameTicketsKey = "WordfallTypingDefense.NicknameTickets";
    private const string OwnedCharactersKey = "WordfallTypingDefense.OwnedCharacters";
    private const string AttendanceDayKey = "WordfallTypingDefense.LastAttendanceDay";
    private const string GamesPlayedKey = "WordfallTypingDefense.GamesPlayed";
    private const string WordsTypedKey = "WordfallTypingDefense.WordsTyped";
    private const string CorrectWordsKey = "WordfallTypingDefense.CorrectWords";
    private const string LastScoreKey = "WordfallTypingDefense.LastScore";

    public PlayerData Data { get; private set; } = new PlayerData();

    public void Load()
    {
        Data.profileCreated = PlayerPrefs.GetInt(ProfileCreatedKey, 0) == 1;
        Data.nickname = PlayerPrefs.GetString(NicknameKey, string.Empty);
        Data.selectedCharacter = Mathf.Clamp(PlayerPrefs.GetInt(CharacterKey, 0), 0, 2);
        Data.coins = PlayerPrefs.GetInt(CoinsKey, 0);
        Data.nicknameTickets = PlayerPrefs.GetInt(NicknameTicketsKey, 0);
        Data.ownedCharacterMask = PlayerPrefs.GetInt(OwnedCharactersKey, 1);
        if (Data.ownedCharacterMask == 0) Data.ownedCharacterMask = 1;
        if (Data.profileCreated) Data.ownedCharacterMask |= 1 << Data.selectedCharacter;
        Data.lastAttendanceDay = PlayerPrefs.GetString(AttendanceDayKey, string.Empty);
        Data.gamesPlayed = PlayerPrefs.GetInt(GamesPlayedKey, 0);
        Data.wordsTyped = PlayerPrefs.GetInt(WordsTypedKey, 0);
        Data.correctWords = PlayerPrefs.GetInt(CorrectWordsKey, 0);
        Data.lastScore = PlayerPrefs.GetInt(LastScoreKey, 0);
        Data.highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    public void Save()
    {
        PlayerPrefs.SetInt(ProfileCreatedKey, Data.profileCreated ? 1 : 0);
        PlayerPrefs.SetString(NicknameKey, Data.nickname ?? string.Empty);
        PlayerPrefs.SetInt(CharacterKey, Data.selectedCharacter);
        PlayerPrefs.SetInt(CoinsKey, Data.coins);
        PlayerPrefs.SetInt(NicknameTicketsKey, Data.nicknameTickets);
        PlayerPrefs.SetInt(OwnedCharactersKey, Data.ownedCharacterMask);
        PlayerPrefs.SetString(AttendanceDayKey, Data.lastAttendanceDay ?? string.Empty);
        PlayerPrefs.SetInt(GamesPlayedKey, Data.gamesPlayed);
        PlayerPrefs.SetInt(WordsTypedKey, Data.wordsTyped);
        PlayerPrefs.SetInt(CorrectWordsKey, Data.correctWords);
        PlayerPrefs.SetInt(LastScoreKey, Data.lastScore);
        PlayerPrefs.SetInt(HighScoreKey, Data.highScore);
        PlayerPrefs.Save();
    }

    public void SaveProfile(string nickname, int characterIndex)
    {
        Data.nickname = nickname;
        Data.selectedCharacter = Mathf.Clamp(characterIndex, 0, 2);
        Data.profileCreated = true;
        Data.ownedCharacterMask |= 1 << Data.selectedCharacter;
        Save();
    }

    public void SaveShopData(int coins, int nicknameTickets, int ownedCharacterMask)
    {
        Data.coins = coins;
        Data.nicknameTickets = nicknameTickets;
        Data.ownedCharacterMask = ownedCharacterMask;
        Save();
    }

    public bool HasAttendance(string attendanceDay) => Data.lastAttendanceDay == attendanceDay;

    public void GrantAttendance(string attendanceDay, int amount)
    {
        Data.coins += amount;
        Data.lastAttendanceDay = attendanceDay;
        Save();
    }

    public void RecordGame(int score, int typed, int correct, int coinReward)
    {
        Data.gamesPlayed++;
        Data.wordsTyped += typed;
        Data.correctWords += correct;
        Data.lastScore = score;
        Data.coins += coinReward;
        if (score > Data.highScore) Data.highScore = score;
        Save();
    }

    public void DeleteAccount()
    {
        PlayerPrefs.DeleteKey(ProfileCreatedKey);
        PlayerPrefs.DeleteKey(NicknameKey);
        PlayerPrefs.DeleteKey(CharacterKey);
        PlayerPrefs.DeleteKey(CoinsKey);
        PlayerPrefs.DeleteKey(NicknameTicketsKey);
        PlayerPrefs.DeleteKey(OwnedCharactersKey);
        PlayerPrefs.DeleteKey(AttendanceDayKey);
        PlayerPrefs.DeleteKey(GamesPlayedKey);
        PlayerPrefs.DeleteKey(WordsTypedKey);
        PlayerPrefs.DeleteKey(CorrectWordsKey);
        PlayerPrefs.DeleteKey(LastScoreKey);
        PlayerPrefs.DeleteKey(HighScoreKey);
        PlayerPrefs.Save();
        Data = new PlayerData();
    }
}
