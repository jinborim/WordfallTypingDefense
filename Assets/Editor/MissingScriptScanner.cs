using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MissingScriptScanner
{
    private static readonly string[] ScenePaths =
    {
        "Assets/Scenes/MainMenuScene.unity",
        "Assets/Scenes/LobbyScene.unity",
        "Assets/Scenes/GameScene.unity"
    };

    [MenuItem("Wordfall/Scan And Remove Missing Scripts")]
    public static void ScanAndRemove()
    {
        foreach (var path in ScenePaths)
        {
            var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            var removed = 0;
            foreach (var gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (gameObject == null || gameObject.scene != scene || gameObject.hideFlags != HideFlags.None) continue;
                var count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(gameObject);
                if (count <= 0) continue;
                Debug.LogWarning("Removing " + count + " missing script(s) from " + path + " / " + GetPath(gameObject));
                removed += count;
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
            }
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log("Missing script scan complete: " + path + " removed=" + removed);
        }
        AssetDatabase.SaveAssets();
    }

    private static string GetPath(GameObject gameObject)
    {
        var path = gameObject.name;
        var current = gameObject.transform.parent;
        while (current != null)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }
        return path;
    }
}
