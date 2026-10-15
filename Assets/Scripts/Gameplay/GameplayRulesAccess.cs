using UnityEngine;

public sealed partial class WordfallPrototype
{
    private string GetDifficultyName()
    {
        if (difficulty == 0) return language == Language.Korean ? "쉬움" : "EASY";
        if (difficulty == 2) return language == Language.Korean ? "어려움" : "HARD";
        return language == Language.Korean ? "보통" : "NORMAL";
    }

    private WordType GetRandomWordType()
    {
        return GameRules.GetRandomWordType();
    }

    private Color GetWordColor(WordType type, bool useKorean)
    {
        return GameRules.GetWordColor(type, useKorean);
    }

    private float GetBaseSpawnInterval()
    {
        return GameRules.GetBaseSpawnInterval(difficulty);
    }

    private float GetMinimumSpawnInterval()
    {
        return GameRules.GetMinimumSpawnInterval(difficulty);
    }

    private float GetMinimumWordSpeed()
    {
        return GameRules.GetMinimumWordSpeed(difficulty);
    }

    private float GetMaximumWordSpeed()
    {
        return GameRules.GetMaximumWordSpeed(difficulty);
    }

    private void ClearWords()
    {
        for (int i = 0; i < fallingWords.Count; i++) Destroy(fallingWords[i].objectToDestroy);
        fallingWords.Clear();
        if (inputField != null) inputField.text = string.Empty;
    }
}
