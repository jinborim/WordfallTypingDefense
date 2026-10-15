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
            ? GetDifficultyName() + " / " + GetGameModeName() + " - ESC 일시정지  F1 빙결  F2 제거"
            : GetDifficultyName() + " / " + GetGameModeName() + " - ESC: pause  F1: freeze  F2: clear";
        if (gameMode == GameMode.TimeAttack)
        {
            statusText.text = language == Language.Korean ? "60초 도전 - 최대 점수에 도전하세요!" : "60 second challenge - Type as many words as possible!";
        }
        if (gameMode == GameMode.Daily)
        {
            statusText.text = language == Language.Korean ? "오늘의 도전 - 모두 같은 단어를 만나요!" : "TODAY'S CHALLENGE - Same seed for everyone!";
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
                ? "최고 기록!  " + score + "  " + accuracy + " - 메뉴를 눌러 다시 시작하세요"
                : "NEW BEST!  " + score + "  " + accuracy + " - Press MENU to try again";
        }
        else
        {
            statusText.text = language == Language.Korean
                ? "게임 종료  점수 " + score + "  " + accuracy + " - 메뉴를 눌러 다시 시작하세요"
                : "GAME OVER  SCORE " + score + "  " + accuracy + " - Press MENU to try again";
        }

        inputField.DeactivateInputField();
        if (gameOverSummaryText != null)
        {
            gameOverSummaryText.text = language == Language.Korean
                ? "점수 " + score + "\n정확도 " + accuracy.Replace(" accuracy", "") + "\n획득 코인 +" + gameCoinReward
                : "SCORE " + score + "\n" + accuracy + "\nCOINS +" + gameCoinReward;
        }
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }
}
