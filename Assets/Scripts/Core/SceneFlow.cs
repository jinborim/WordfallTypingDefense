using UnityEngine.SceneManagement;

public sealed partial class WordfallPrototype
{
    private const string MainMenuSceneName = "MainMenuScene";
    private const string LobbySceneName = "LobbyScene";
    private const string GameSceneName = "GameScene";

    private void EnsureScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name == sceneName) return;
        SceneManager.LoadScene(sceneName);
    }

    private void LoadMainMenuScene() => EnsureScene(MainMenuSceneName);
    private void LoadLobbyScene() => EnsureScene(LobbySceneName);
    private void LoadGameScene() => EnsureScene(GameSceneName);
}
