using UnityEngine;
using UnityEngine.InputSystem;

public sealed partial class WordfallPrototype
{
    private void Update()
    {
        if (!playing) return;
        HandleShortcuts();
        if (paused) return;
        if (characterEmotionTimer > 0f)
        {
            characterEmotionTimer -= Time.deltaTime;
            if (characterEmotionTimer <= 0f) SetCharacterEmotion(false);
        }
        if (freezeTimer > 0f) freezeTimer -= Time.deltaTime;
        if (gameMode == GameMode.TimeAttack)
        {
            timeRemaining -= Time.deltaTime;
            UpdateHud();
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                EndGame();
                return;
            }
        }
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnWord();
            spawnTimer = Mathf.Max(GetMinimumSpawnInterval(), GetBaseSpawnInterval() - score * 0.003f);
        }
        for (int i = fallingWords.Count - 1; i >= 0; i--)
        {
            var word = fallingWords[i];
            if (freezeTimer <= 0f) word.rect.anchoredPosition += Vector2.down * word.speed * Time.deltaTime;
            if (word.rect.anchoredPosition.y < -360f)
            {
                SetCharacterEmotion(true);
                Destroy(word.objectToDestroy);
                fallingWords.RemoveAt(i);

                // Time Attack is a score challenge for the full 60 seconds.
                // Missing words resets the combo, but must not end the run
                // early by consuming the normal-mode lives.
                if (gameMode == GameMode.TimeAttack)
                {
                    combo = 0;
                    statusText.text = language == Language.Korean ? "단어를 놓쳤습니다!" : "A word got through!";
                    UpdateHud();
                    continue;
                }

                if (word.type == WordType.Bomb)
                {
                    lives -= 2;
                    statusText.text = language == Language.Korean ? "폭탄을 놓쳤습니다! 목숨 -2" : "BOMB MISSED! -2 LIVES";
                }
                else
                {
                    lives--;
                    statusText.text = language == Language.Korean ? "단어를 놓쳤습니다!" : "A word got through!";
                }
                combo = 0;
                UpdateHud();
                if (lives <= 0) EndGame();
            }
        }
    }

    private void HandleShortcuts()
    {
        if (Keyboard.current == null) return;
        if (Keyboard.current.escapeKey.wasPressedThisFrame) TogglePause();
        if (Keyboard.current.f1Key.wasPressedThisFrame && !paused) ActivateFreezeSkill();
        if (Keyboard.current.f2Key.wasPressedThisFrame && !paused) ClearScreenSkill();
    }
}
