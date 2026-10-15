public sealed partial class WordfallPrototype
{
    private void ShowSetupFromGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        ShowSetupPanel();
    }
}
