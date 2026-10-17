using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class SceneObjectBuilder
{
    private const string RootName = "EditableSceneUI";
    private const string MainBackgroundPath = "Pixel/wordfall-background";
    private const string LobbyBackgroundPath = "Pixel/lobby-background";

    [MenuItem("Wordfall/Build Editable Scene Objects")]
    public static void BuildAll()
    {
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        BuildMainMenu();
        BuildLobby();
        BuildGame();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Wordfall editable scene objects created.");
    }

    private static void PrepareScene(string path, string title, out Transform root)
    {
        var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
        var previous = GameObject.Find(RootName);
        if (previous != null) Object.DestroyImmediate(previous);
        var canvasObject = new GameObject(RootName, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.pixelPerfect = true;
        var scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280f, 720f);
        scaler.matchWidthOrHeight = 0.5f;
        var cameraObject = new GameObject("Main Camera", typeof(Camera));
        cameraObject.tag = "MainCamera";
        cameraObject.transform.position = new Vector3(0f, 0f, -10f);
        cameraObject.GetComponent<Camera>().orthographic = true;
        cameraObject.GetComponent<Camera>().orthographicSize = 5f;
        root = canvasObject.transform;
        CreateText(title, root, "SceneTitle", 36, new Vector2(0.5f, 0.88f), new Vector2(500f, 60f));
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }

    private static void BuildMainMenu()
    {
        PrepareScene("Assets/Scenes/MainMenuScene.unity", "MAIN MENU", out var root);
        root.GetComponent<Canvas>().sortingOrder = 10;
        CreateImage("MainMenuBackground", root, MainBackgroundPath, new Vector2(0.5f, 0.5f), new Vector2(1280f, 720f));
        CreateImage("WordfallLogo", root, "Pixel/wordfall-logo-en", new Vector2(0.5f, 0.66f), new Vector2(430f, 92f));
        CreateText("한국어 & 영어 타자 디펜스", root, "SubtitleText", 22, new Vector2(0.5f, 0.53f), new Vector2(720f, 40f));
        CreateText("떨어지는 단어를 빠르게 입력해 기지를 지키세요.", root, "DescriptionText", 18, new Vector2(0.5f, 0.48f), new Vector2(860f, 34f));
        CreateButton("GameStartButton", root, "게임 시작", new Vector2(0.38f, 0.22f), new Vector2(340f, 78f), "ShowHomeHub");
        CreateButton("LanguageButton", root, "언어", new Vector2(0.66f, 0.22f), new Vector2(250f, 78f), "ShowLanguagePanel");
        CreateText("게스트 플레이어", root, "ProfileSummaryText", 16, new Vector2(0.5f, 0.105f), new Vector2(520f, 30f));
        SaveCurrentScene();
    }

    private static void BuildLobby()
    {
        PrepareScene("Assets/Scenes/LobbyScene.unity", "로비", out var root);
        CreateImage("LobbyBackground", root, LobbyBackgroundPath, new Vector2(0.5f, 0.5f), new Vector2(1280f, 720f));
        CreateImage("LobbyCharacter", root, "Pixel/character-hacker-full", new Vector2(0.5f, 0.42f), new Vector2(188f, 308f));
        CreatePanel("PlayerInfoPanel", root, new Vector2(0.18f, 0.78f), new Vector2(500f, 150f));
        CreateText("플레이어 보림림", root, "PlayerInfoText", 22, new Vector2(0.18f, 0.795f), new Vector2(440f, 36f));
        CreateText("코인 0", root, "CoinText", 20, new Vector2(0.18f, 0.735f), new Vector2(440f, 32f));
        CreatePanel("DialogueWindow", root, new Vector2(0.73f, 0.54f), new Vector2(560f, 182f));
        CreateText("오늘도 기지를 지켜보자.", root, "DialogueText", 18, new Vector2(0.73f, 0.54f), new Vector2(480f, 90f));
        CreateButton("LobbyGameStartButton", root, "게임 시작", new Vector2(0.5f, 0.10f), new Vector2(290f, 58f), "ShowSetupPanel");
        CreateButton("LobbyMenuButton", root, "메뉴", new Vector2(0.04f, 0.50f), new Vector2(72f, 72f), "ToggleHomeHubMenu");
        SaveCurrentScene();
    }

    private static void BuildGame()
    {
        PrepareScene("Assets/Scenes/GameScene.unity", "게임", out var root);
        CreateImage("GameBackground", root, MainBackgroundPath, new Vector2(0.5f, 0.5f), new Vector2(1280f, 720f));
        CreatePanel("ScoreHUD", root, new Vector2(0.5f, 0.94f), new Vector2(700f, 64f));
        CreateText("점수 0000", root, "ScoreText", 24, new Vector2(0.31f, 0.94f), new Vector2(190f, 36f));
        CreateText("웨이브 1", root, "WaveText", 18, new Vector2(0.46f, 0.94f), new Vector2(130f, 32f));
        CreateText("콤보 x0", root, "ComboText", 20, new Vector2(0.61f, 0.94f), new Vector2(150f, 34f));
        var hearts = CreatePanel("Lives", root, new Vector2(0.89f, 0.94f), new Vector2(300f, 54f));
        for (var i = 0; i < 5; i++) CreateImage("Heart" + (i + 1), hearts.transform, "Pixel/life-heart", new Vector2(0.1f + i * 0.2f, 0.5f), new Vector2(42f, 40f));
        CreatePanel("InputPanel", root, new Vector2(0.5f, 0.16f), new Vector2(720f, 112f));
        CreateInputField(root, "TypingInputField", new Vector2(0.5f, 0.16f), new Vector2(620f, 58f));
        CreateButton("PauseButton", root, "일시정지", new Vector2(0.08f, 0.93f), new Vector2(120f, 46f), "TogglePause");
        var gameOver = CreatePanel("GameOverPanel", root, new Vector2(0.5f, 0.5f), new Vector2(760f, 340f));
        CreateText("게임 오버", gameOver.transform, "GameOverTitle", 38, new Vector2(0.5f, 0.73f), new Vector2(500f, 52f));
        CreateText("점수 0\n정확도 0%", gameOver.transform, "GameOverSummary", 20, new Vector2(0.5f, 0.53f), new Vector2(500f, 80f));
        CreateButton("RetryButton", gameOver.transform, "다시하기", new Vector2(0.32f, 0.28f), new Vector2(210f, 56f), "StartGame");
        CreateButton("ExitButton", gameOver.transform, "나가기", new Vector2(0.68f, 0.28f), new Vector2(210f, 56f), "ExitToLobbyFromGameOver");
        gameOver.SetActive(false);
        SaveCurrentScene();
    }

    private static GameObject CreatePanel(string name, Transform parent, Vector2 anchor, Vector2 size)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = anchor;
        rect.sizeDelta = size;
        go.GetComponent<Image>().color = new Color(0.03f, 0.08f, 0.18f, 0.88f);
        return go;
    }

    private static Image CreateImage(string name, Transform parent, string resource, Vector2 anchor, Vector2 size)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = anchor;
        rect.sizeDelta = size;
        var image = go.GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>(resource);
        image.preserveAspect = true;
        image.raycastTarget = false;
        return image;
    }

    private static Button CreateButton(string name, Transform parent, string label, Vector2 anchor, Vector2 size, string actionName)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = anchor;
        rect.sizeDelta = size;
        go.GetComponent<Image>().color = new Color32(37, 95, 210, 255);
        var bridge = go.AddComponent<SceneButtonBridge>();
        var actionField = typeof(SceneButtonBridge).GetField("actionName", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        actionField.SetValue(bridge, actionName);
        CreateText(label, go.transform, "Label", 22, new Vector2(0.5f, 0.5f), size - new Vector2(20f, 10f));
        return go.GetComponent<Button>();
    }

    private static InputField CreateInputField(Transform parent, string name, Vector2 anchor, Vector2 size)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(InputField));
        go.transform.SetParent(parent, false);
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = anchor;
        rect.sizeDelta = size;
        go.GetComponent<Image>().color = new Color32(5, 18, 48, 235);
        var text = CreateText("", go.transform, "Text", 22, new Vector2(0.5f, 0.5f), size - new Vector2(30f, 10f));
        var placeholder = CreateText("여기에 입력...", go.transform, "Placeholder", 22, new Vector2(0.5f, 0.5f), size - new Vector2(30f, 10f));
        placeholder.color = new Color32(150, 175, 220, 255);
        var input = go.GetComponent<InputField>();
        input.textComponent = text;
        input.placeholder = placeholder;
        return input;
    }

    private static Text CreateText(string value, Transform parent, string name, int size, Vector2 anchor, Vector2 dimensions)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Text));
        go.transform.SetParent(parent, false);
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = anchor;
        rect.sizeDelta = dimensions;
        var text = go.GetComponent<Text>();
        text.text = value;
        text.font = Resources.Load<Font>("Fonts/Mulmaru");
        if (text.font == null) text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = size;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;
        return text;
    }

    private static void SaveCurrentScene()
    {
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
    }
}
