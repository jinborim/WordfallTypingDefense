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
            ? GetDifficultyName() + " / " + GetGameModeName() + " - ESC ?쇱떆?뺤?  F1 鍮숆껐  F2 ?쒓굅"
            : GetDifficultyName() + " / " + GetGameModeName() + " - ESC: pause  F1: freeze  F2: clear";
        if (gameMode == GameMode.TimeAttack)
        {
            statusText.text = language == Language.Korean ? "60珥??꾩쟾 - 理쒕? ?먯닔???꾩쟾?섏꽭??" : "60 second challenge - Type as many words as possible!";
        }
        if (gameMode == GameMode.Daily)
        {
            statusText.text = language == Language.Korean ? "?ㅻ뒛???꾩쟾 - 紐⑤몢 媛숈? ?⑥뼱瑜?留뚮굹??" : "TODAY'S CHALLENGE - Same seed for everyone!";
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
                ? "?먯닔 " + score + "\n?뺥솗??" + accuracy.Replace(" accuracy", "") + "\n?띾뱷 肄붿씤 +" + gameCoinReward
                : "SCORE " + score + "\n" + accuracy + "\nCOINS +" + gameCoinReward;
        }
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }
}
