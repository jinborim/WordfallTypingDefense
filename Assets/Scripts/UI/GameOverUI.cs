public sealed partial class WordfallPrototype
{
    private void ExitToLobbyFromGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        ShowHomeHub();
    }
}
