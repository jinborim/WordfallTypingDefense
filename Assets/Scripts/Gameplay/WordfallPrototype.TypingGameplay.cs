public sealed partial class WordfallPrototype
{
    private void HandleSubmitted(string value)
    {
        if (!playing || paused) return;
        if (string.IsNullOrWhiteSpace(value))
        {
            RefocusInputField();
            return;
        }

        totalTyped++;
        bool matched = false;
        string submitted = value.Trim();
        for (int i = fallingWords.Count - 1; i >= 0; i--)
        {
            if (fallingWords[i].word != submitted) continue;
            matched = true;
            var wordType = fallingWords[i].type;
            correctTyped++;
            Destroy(fallingWords[i].objectToDestroy);
            fallingWords.RemoveAt(i);
            if (wordType == WordType.Golden)
            {
                score += 25 + combo * 3;
                statusText.text = language == Language.Korean ? "골든 단어! 보너스 점수!" : "GOLDEN WORD! Bonus points!";
            }
            else if (wordType == WordType.Heal)
            {
                score += 15 + combo * 2;
                lives = Mathf.Min(5, lives + 1);
                statusText.text = language == Language.Korean ? "회복 단어! 목숨 +1" : "HEAL WORD! +1 LIFE";
            }
            else if (wordType == WordType.Bomb)
            {
                score += 20 + combo * 2;
                statusText.text = language == Language.Korean ? "폭탄 해제!" : "BOMB DEFUSED!";
            }
            else if (wordType == WordType.Boss)
            {
                score += 100 + combo * 5;
                statusText.text = language == Language.Korean ? "보스 처치! 큰 보너스!" : "BOSS DEFEATED! Huge bonus!";
            }
            else
            {
                score += 10 + combo * 2;
                statusText.text = language == Language.Korean ? "정답! 계속 입력하세요." : "NICE! Keep typing.";
            }
            combo++;
            SetCharacterEmotion(false);
            break;
        }

        if (!matched)
        {
            combo = 0;
            SetCharacterEmotion(true);
            statusText.text = language == Language.Korean ? "오답입니다. 다시 입력하세요." : "WRONG WORD. Try again.";
        }

        inputField.text = string.Empty;
        UpdateHud();
        RefocusInputField();
    }
}
