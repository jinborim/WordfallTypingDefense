public enum Language
{
    Korean,
    English
}

public enum GameMode
{
    Mixed,
    Korean,
    English,
    TimeAttack,
    Daily
}

public sealed class GameSettingsData
{
    public int Difficulty = 1;
    public Language Language = global::Language.Korean;
    public GameMode Mode = GameMode.Mixed;
}
