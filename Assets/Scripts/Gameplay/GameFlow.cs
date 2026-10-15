using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void StartGame()
    {
        session.ResetForNewGame();
        lastSpawnedWord = string.Empty;
        if (gameMode == GameMode.Daily)
        {
            Random.InitState(System.DateTime.Now.DayOfYear + System.DateTime.Now.Year * 1000);
        }

        spawnTimer = 0.2f;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        SetCharacterEmotion(false);
        statusText.text = language == Language.Korean
            ? GetDifficultyName() + " / " + GetGameModeName() + " - ESC: \uC77C\uC2DC\uC815\uC9C0  F1: \uBE59\uACB0  F2: \uC81C\uAC70"
            : GetDifficultyName() + " / " + GetGameModeName() + " - ESC: pause  F1: freeze  F2: clear";
        if (gameMode == GameMode.TimeAttack)
        {
            statusText.text = language == Language.Korean ? "60\uCD08 \uB3C4\uC804 - \uCD5C\uB300\uD55C \uB9CE\uC740 \uC810\uC218\uB97C \uC5BB\uC73C\uC138\uC694!" : "60 second challenge - Type as many words as possible!";
        }
        if (gameMode == GameMode.Daily)
        {
            statusText.text = language == Language.Korean ? "\uC624\uB298\uC758 \uB3C4\uC804 - \uBAA8\uB450 \uAC19\uC740 \uB2E8\uC5B4\uB97C \uB9CC\uB098\uC694!" : "TODAY'S CHALLENGE - Same seed for everyone!";
        }

        inputField.text = string.Empty;
        UpdateHud();
        inputField.ActivateInputField();
    }

    private void EndGame()
    {
        session.ResetToIdle();
        if (pausePanel != null) pausePanel.SetActive(false);

        int gameCoinReward = Mathf.Max(1, score / 50);
        playerDataManager.RecordGame(score, totalTyped, correctTyped, gameCoinReward);
        coins = playerDataManager.Data.coins;
        UpdateCoinText();

        string accuracy = totalTyped > 0
            ? Mathf.RoundToInt((float)correctTyped / totalTyped * 100f) + "% accuracy"
            : "0% accuracy";
        if (score > highScore)
        {
            highScore = score;
            playerDataManager.Data.highScore = highScore;
            playerDataManager.Save();
            UpdateHighScoreText();
            statusText.text = language == Language.Korean
                ? "BEST " + score + " " + accuracy
                : "NEW BEST!  " + score + "  " + accuracy + " - Press MENU to try again";
        }
        else
        {
            statusText.text = language == Language.Korean
                ? "GAME OVER " + score + " " + accuracy
                : "GAME OVER  SCORE " + score + "  " + accuracy + " - Press MENU to try again";
        }

        inputField.DeactivateInputField();
        if (gameOverSummaryText != null)
        {
            gameOverSummaryText.text = language == Language.Korean
                ? "\uC810\uC218 " + score + "\\n\uC815\uD655\uB3C4 " + accuracy.Replace(" accuracy", "") + "\\n\uD68D\uB4DD \uCF54\uC778 +" + gameCoinReward
                : "SCORE " + score + "\n" + accuracy + "\nCOINS +" + gameCoinReward;
        }
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }
}
