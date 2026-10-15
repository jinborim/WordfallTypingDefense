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
        if (gameMode == GameMode.Korean) return language == Language.Korean ? "TEXT" : "KOREAN";
        if (gameMode == GameMode.English) return language == Language.Korean ? "?곸뼱" : "ENGLISH";
        if (gameMode == GameMode.TimeAttack) return language == Language.Korean ? "TEXT" : "60 SEC";
        if (gameMode == GameMode.Daily) return language == Language.Korean ? "?ㅻ뒛" : "DAILY";
        return language == Language.Korean ? "?쇳빀" : "MIXED";
    }

    private void ActivateFreezeSkill()
    {
        freezeTimer = 5f;
        statusText.text = language == Language.Korean ? "TEXT" : "FREEZE SKILL! 5 seconds";
    }

    private void ClearScreenSkill()
    {
        for (int i = fallingWords.Count - 1; i >= 0; i--)
        {
            Destroy(fallingWords[i].objectToDestroy);
            fallingWords.RemoveAt(i);
        }
        score += 25;
        statusText.text = language == Language.Korean ? "?쒓굅 ?ㅽ궗! 紐⑤뱺 ?⑥뼱 ?쒓굅" : "CLEAR SKILL! All words removed";
        UpdateHud();
    }
}
