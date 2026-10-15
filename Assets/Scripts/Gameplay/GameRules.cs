using UnityEngine;

public static class GameRules
{
    public static WordfallPrototype.WordType GetRandomWordType()
    {
        float roll = Random.value;
        if (roll < 0.10f) return WordfallPrototype.WordType.Golden;
        if (roll < 0.17f) return WordfallPrototype.WordType.Bomb;
        if (roll < 0.24f) return WordfallPrototype.WordType.Heal;
        return WordfallPrototype.WordType.Normal;
    }

    public static Color GetWordColor(WordfallPrototype.WordType type, bool useKorean)
    {
        if (type == WordfallPrototype.WordType.Golden) return new Color32(255, 211, 94, 255);
        if (type == WordfallPrototype.WordType.Bomb) return new Color32(255, 105, 115, 255);
        if (type == WordfallPrototype.WordType.Heal) return new Color32(121, 235, 169, 255);
        if (type == WordfallPrototype.WordType.Boss) return new Color32(211, 137, 255, 255);
        return useKorean ? new Color32(255, 220, 130, 255) : new Color32(133, 212, 255, 255);
    }

    public static float GetBaseSpawnInterval(int difficulty)
    {
        if (difficulty == 0) return 2.35f;
        if (difficulty == 2) return 1.35f;
        return 1.8f;
    }

    public static float GetMinimumSpawnInterval(int difficulty)
    {
        if (difficulty == 0) return 1.2f;
        if (difficulty == 2) return 0.55f;
        return 0.75f;
    }

    public static float GetMinimumWordSpeed(int difficulty)
    {
        if (difficulty == 0) return 35f;
        if (difficulty == 2) return 65f;
        return 45f;
    }

    public static float GetMaximumWordSpeed(int difficulty)
    {
        if (difficulty == 0) return 55f;
        if (difficulty == 2) return 105f;
        return 75f;
    }
}
