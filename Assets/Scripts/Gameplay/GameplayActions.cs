using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void HandleInput(string value)
    {
        if (!playing || string.IsNullOrWhiteSpace(value)) return;
        string submitted = value.Trim();
        for (int i = fallingWords.Count - 1; i >= 0; i--)
        {
            if (fallingWords[i].word != submitted) continue;
            var wordType = fallingWords[i].type;
            correctTyped++;
            Destroy(fallingWords[i].objectToDestroy);
            fallingWords.RemoveAt(i);
            if (wordType == WordType.Golden) score += 25 + combo * 3;
            else if (wordType == WordType.Heal)
            {
                score += 15 + combo * 2;
                lives = Mathf.Min(5, lives + 1);
            }
            else if (wordType == WordType.Bomb) score += 20 + combo * 2;
            else if (wordType == WordType.Boss) score += 100 + combo * 5;
            else score += 10 + combo * 2;
            combo++;
            inputField.text = string.Empty;
            UpdateHud();
            RefocusInputField();
            return;
        }
    }

    private string GetGameModeName()
    {
        if (gameMode == GameMode.Korean) return language == Language.Korean ? "\uD55C\uAD6D\uC5B4" : "KOREAN";
        if (gameMode == GameMode.English) return language == Language.Korean ? "\\uC601\\uC5B4" : "ENGLISH";
        if (gameMode == GameMode.TimeAttack) return language == Language.Korean ? "60\uCD08" : "60 SEC";
        if (gameMode == GameMode.Daily) return language == Language.Korean ? "\\uC624\\uB298" : "DAILY";
        return language == Language.Korean ? "\\uD63C\\uD569" : "MIXED";
    }

    private void ActivateFreezeSkill()
    {
        freezeTimer = 5f;
        statusText.text = language == Language.Korean ? "\uBE59\uACB0 \uC2A4\uD0AC! 5\uCD08" : "FREEZE SKILL! 5 seconds";
    }

    private void ClearScreenSkill()
    {
        for (int i = fallingWords.Count - 1; i >= 0; i--)
        {
            Destroy(fallingWords[i].objectToDestroy);
            fallingWords.RemoveAt(i);
        }
        score += 25;
        statusText.text = language == Language.Korean ? "\uD654\uBA74 \uC81C\uAC70 \uC2A4\uD0AC! \uBAA8\uB4E0 \uB2E8\uC5B4 \uC81C\uAC70" : "CLEAR SKILL! All words removed";
        UpdateHud();
    }
}
