#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal static class LobbyCharacterSceneLayout
{
    private const string AnchorName = "HomeHubCharacter";

    [InitializeOnLoadMethod]
    private static void ApplyWhenMainMenuIsOpen()
    {
        EditorApplication.delayCall += () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (IsTargetScene(SceneManager.GetActiveScene()))
                CreateLobbyCharacterPreviews();
        };
    }

    [MenuItem("Wordfall/로비 캐릭터 중앙 배치")]
    private static void CreateLobbyCharacterPreviews()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        if (!IsTargetScene(SceneManager.GetActiveScene()))
        {
            Debug.LogWarning("MainMenuScene 또는 LobbyScene을 연 뒤 다시 실행해주세요.");
            return;
        }

        var anchor = FindSceneObject(AnchorName);
        if (anchor == null)
        {
            Debug.LogWarning("HomeHubCharacter 오브젝트를 찾을 수 없습니다.");
            return;
        }

        CreatePreview(anchor, "HomeHubMageCharacterPreview", "Pixel/character-mage-full");
        CreatePreview(anchor, "HomeHubHackerCharacterPreview", "Pixel/character-hacker-full");
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        Debug.Log("마법사와 해커 미리보기를 기사와 같은 중앙 위치에 배치했습니다.");
    }

    private static void CreatePreview(GameObject anchor, string objectName, string resourcePath)
    {
        var existing = anchor.transform.parent.Find(objectName);
        var preview = existing != null ? existing.gameObject : new GameObject(objectName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        if (existing == null)
            preview.transform.SetParent(anchor.transform.parent, false);

        var rect = preview.GetComponent<RectTransform>();
        var anchorRect = anchor.GetComponent<RectTransform>();
        rect.anchorMin = anchorRect.anchorMin;
        rect.anchorMax = anchorRect.anchorMax;
        rect.pivot = anchorRect.pivot;
        rect.anchoredPosition = anchorRect.anchoredPosition;
        rect.sizeDelta = anchorRect.sizeDelta;

        var image = preview.GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>(resourcePath);
        image.preserveAspect = true;
        image.raycastTarget = false;

        if (preview.GetComponent<LobbyCharacterScenePreview>() == null)
            preview.AddComponent<LobbyCharacterScenePreview>();
        preview.SetActive(true);

    }

    private static bool IsTargetScene(Scene scene)
    {
        return scene.name == "MainMenuScene" || scene.name == "LobbyScene";
    }

    private static GameObject FindSceneObject(string objectName)
    {
        foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            var transforms = root.GetComponentsInChildren<Transform>(true);
            foreach (var item in transforms)
                if (item.name == objectName) return item.gameObject;
        }
        return null;
    }
}
#endif
