using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RuntimeUiSceneExporter
{
    private static readonly string[] ScenePaths =
    {
        "Assets/Scenes/MainMenuScene.unity",
        "Assets/Scenes/LobbyScene.unity",
        "Assets/Scenes/GameScene.unity"
    };

    [MenuItem("Wordfall/Export Runtime UI To Scenes")]
    public static void ExportAll()
    {
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        foreach (var scenePath in ScenePaths) ExportScene(scenePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Runtime UI exported into all Wordfall scenes.");
    }

    private static void ExportScene(string path)
    {
        var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
        var oldCanvas = GameObject.Find("EditableSceneUI");
        if (oldCanvas != null) Object.DestroyImmediate(oldCanvas);
        var oldRuntimeCanvas = GameObject.Find("WordfallCanvas");
        if (oldRuntimeCanvas != null) Object.DestroyImmediate(oldRuntimeCanvas);
        foreach (var bridge in Object.FindObjectsByType<SceneButtonBridge>(FindObjectsSortMode.None))
            Object.DestroyImmediate(bridge);

        var generator = new GameObject("SceneUiGenerator");
        var prototype = generator.AddComponent<WordfallPrototype>();
        var fontField = typeof(WordfallPrototype).GetField("uiFont", BindingFlags.Instance | BindingFlags.NonPublic);
        fontField.SetValue(prototype, Resources.Load<Font>("Fonts/Mulmaru"));
        var buildMethod = typeof(WordfallPrototype).GetMethod("BuildInterface", BindingFlags.Instance | BindingFlags.NonPublic);
        buildMethod.Invoke(prototype, null);

        var canvas = generator.transform.Find("WordfallCanvas");
        if (canvas == null) throw new System.InvalidOperationException("WordfallCanvas was not generated.");
        canvas.SetParent(null, true);
        canvas.name = "SceneUI";
        var runtime = new GameObject("SceneRuntime");
        SceneManager.MoveGameObjectToScene(runtime, scene);
        var references = runtime.AddComponent<SceneUiReferences>();
        references.CaptureFrom(prototype);
        runtime.AddComponent<WordfallPrototype>();
        Object.DestroyImmediate(generator);

        var canvasComponent = canvas.GetComponent<Canvas>();
        canvasComponent.sortingOrder = 10;
        var mainPanel = canvas.Find("MainPanel");
        var gamePanel = canvas.Find("GamePanel");
        if (mainPanel != null) mainPanel.gameObject.SetActive(path.Contains("MainMenu"));
        if (gamePanel != null) gamePanel.gameObject.SetActive(path.Contains("GameScene"));
        var lobbyPanel = canvas.Find("MainPanel/HomeHubPanel");
        if (lobbyPanel != null) lobbyPanel.gameObject.SetActive(path.Contains("Lobby"));

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }
}
