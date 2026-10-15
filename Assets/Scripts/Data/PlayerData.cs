using System;

[Serializable]
public sealed class PlayerData
{
    public bool profileCreated;
    public string nickname = string.Empty;
    public int selectedCharacter;
    public int coins;
    public int nicknameTickets;
    public int ownedCharacterMask = 1;
    public string lastAttendanceDay = string.Empty;
    public int gamesPlayed;
    public int wordsTyped;
    public int correctWords;
    public int lastScore;
    public int highScore;
}
