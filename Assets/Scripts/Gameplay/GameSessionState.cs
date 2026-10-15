public sealed class GameSessionState
{
    public float SpawnTimer;
    public int Score;
    public int Lives;
    public int Combo;
    public int Wave;
    public int SpawnedThisWave;
    public int TotalTyped;
    public int CorrectTyped;
    public float TimeRemaining;
    public float FreezeTimer;
    public float CharacterEmotionTimer;
    public bool CharacterIsAlarm;
    public bool Playing;
    public bool Paused;

    public void ResetForNewGame()
    {
        SpawnTimer = 0f;
        Score = 0;
        Lives = 5;
        Combo = 0;
        Wave = 1;
        SpawnedThisWave = 0;
        TotalTyped = 0;
        CorrectTyped = 0;
        TimeRemaining = 60f;
        FreezeTimer = 0f;
        CharacterEmotionTimer = 0f;
        CharacterIsAlarm = false;
        Playing = true;
        Paused = false;
    }

    public void ResetToIdle()
    {
        Playing = false;
        Paused = false;
    }
}
