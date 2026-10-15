using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private string GetCharacterResourcePath(bool alarm)
    {
        string[] normalPaths = { "Pixel/character-knight", "Pixel/character-mage", "Pixel/character-hacker" };
        string[] alarmPaths = { "Pixel/character-knight-alarm", "Pixel/character-mage-alarm", "Pixel/character-hacker-alarm" };
        int index = Mathf.Clamp(selectedCharacter, 0, normalPaths.Length - 1);
        return alarm ? alarmPaths[index] : normalPaths[index];
    }

    private string GetLobbyCharacterResourcePath()
    {
        string[] fullBodyPaths = { "Pixel/character-knight-full", "Pixel/character-mage-full", "Pixel/character-hacker-full" };
        int index = Mathf.Clamp(selectedCharacter, 0, fullBodyPaths.Length - 1);
        return Resources.Load<Texture2D>(fullBodyPaths[index]) != null ? fullBodyPaths[index] : GetCharacterResourcePath(false);
    }

    private void SetCharacterEmotion(bool alarm)
    {
        if (activeCharacterImage == null) return;
        characterEmotionTimer = alarm ? 1.5f : 0f;
        if (characterIsAlarm == alarm && displayedCharacter == selectedCharacter && activeCharacterImage.sprite != null) return;
        characterIsAlarm = alarm;
        displayedCharacter = selectedCharacter;
        var texture = Resources.Load<Texture2D>(GetCharacterResourcePath(alarm));
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        activeCharacterImage.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        activeCharacterImage.preserveAspect = true;
        activeCharacterImage.color = Color.white;
    }

    private Image CreateCharacterProfile(Transform parent)
    {
        characterStatusPanel = new GameObject("CharacterProfile", typeof(RectTransform), typeof(Image), typeof(Mask));
        characterStatusPanel.transform.SetParent(parent, false);
        SetRect(characterStatusPanel.GetComponent<RectTransform>(), new Vector2(0.88f, 0.22f), new Vector2(0.88f, 0.22f), Vector2.zero, new Vector2(170f, 170f));

        var profileMaskImage = characterStatusPanel.GetComponent<Image>();
        profileMaskImage.sprite = CreateCircleSprite(256);
        profileMaskImage.color = Color.white;
        profileMaskImage.raycastTarget = false;
        characterStatusPanel.GetComponent<Mask>().showMaskGraphic = false;

        var portrait = CreateImage("ActiveCharacter", characterStatusPanel.transform, GetCharacterResourcePath(false), new Vector2(0.5f, 0.5f), new Vector2(116f, 116f));
        portrait.raycastTarget = false;
        var frame = CreateImage("ProfileFrame", characterStatusPanel.transform, "Pixel/character-profile-frame", new Vector2(0.5f, 0.5f), new Vector2(170f, 170f));
        frame.raycastTarget = false;
        return portrait;
    }

    private Sprite CreateCircleSprite(int size)
    {
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        var center = (size - 1) * 0.5f;
        var radius = size * 0.5f;
        var pixels = new Color32[size * size];
        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            float dx = x - center;
            float dy = y - center;
            pixels[y * size + x] = dx * dx + dy * dy <= radius * radius ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 0);
        }
        texture.SetPixels32(pixels);
        texture.Apply();
        return Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), 100f);
    }

    private Button CreateImageAction(string name, Transform parent, string iconPath, string label, Vector2 anchor, Vector2 size, Vector2 iconSize, out Text labelText)
    {
        var objectToCreate = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        objectToCreate.transform.SetParent(parent, false);
        SetRect(objectToCreate.GetComponent<RectTransform>(), anchor, anchor, Vector2.zero, size);
        var hitImage = objectToCreate.GetComponent<Image>();
        hitImage.color = new Color(1f, 1f, 1f, 0f);
        hitImage.raycastTarget = true;
        var button = objectToCreate.GetComponent<Button>();
        button.targetGraphic = hitImage;
        button.transition = Selectable.Transition.ColorTint;
        var navigation = button.navigation;
        navigation.mode = Navigation.Mode.None;
        button.navigation = navigation;
        var colors = button.colors;
        colors.normalColor = colors.highlightedColor = colors.pressedColor = colors.selectedColor = colors.disabledColor = new Color(1f, 1f, 1f, 0f);
        button.colors = colors;
        objectToCreate.AddComponent<ButtonPressFeedback>();
        var icon = CreateImage(name + "Icon", objectToCreate.transform, iconPath, new Vector2(0.5f, 0.65f), iconSize);
        icon.raycastTarget = false;
        labelText = CreateText(label, objectToCreate.transform, 14, new Color32(222, 235, 250, 255), new Vector2(0.5f, 0.09f), new Vector2(0.5f, 0.09f));
        labelText.rectTransform.sizeDelta = new Vector2(size.x, 26f);
        labelText.fontStyle = FontStyle.Normal;
        labelText.fontSize = 14;
        labelText.resizeTextForBestFit = true;
        labelText.resizeTextMinSize = 9;
        labelText.resizeTextMaxSize = 14;
        labelText.horizontalOverflow = HorizontalWrapMode.Overflow;
        labelText.verticalOverflow = VerticalWrapMode.Truncate;
        labelText.raycastTarget = false;
        return button;
    }

    private Button CreateHamburgerButton(string name, Transform parent, Vector2 anchor, Vector2 size)
    {
        var button = CreateImageAction(name, parent, "Pixel/menu-hamburger-icon", "", anchor, size, new Vector2(58f, 58f), out _);
        return button;
    }

    private GameObject CreateMenuCloseIcon(Transform parent)
    {
        var closeIcon = new GameObject("MenuCloseIcon", typeof(RectTransform), typeof(Image));
        closeIcon.transform.SetParent(parent, false);
        SetRect(closeIcon.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(58f, 58f));
        var background = closeIcon.GetComponent<Image>();
        background.color = new Color(1f, 1f, 1f, 0f);
        background.raycastTarget = false;
        CreateCloseBar(closeIcon.transform, 45f);
        CreateCloseBar(closeIcon.transform, -45f);
        return closeIcon;
    }

    private void CreateCloseBar(Transform parent, float angle)
    {
        var bar = new GameObject("CloseBar", typeof(RectTransform), typeof(Image));
        bar.transform.SetParent(parent, false);
        SetRect(bar.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(34f, 5f));
        bar.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        bar.GetComponent<Image>().color = Color.white;
        bar.GetComponent<Image>().raycastTarget = false;
    }

    private Button CreateCharacterButton(string label, string portraitPath, Transform parent, Vector2 anchor, Color color)
    {
        var button = CreateButton(label, parent, anchor, new Vector2(210f, 130f), color);
        var portrait = CreateImage("CharacterPortrait", button.transform, portraitPath, new Vector2(0.5f, 0.64f), new Vector2(78f, 78f));
        portrait.raycastTarget = false;
        var labelText = button.GetComponentInChildren<Text>();
        if (labelText != null)
        {
            labelText.rectTransform.anchoredPosition = new Vector2(0f, -43f);
            labelText.rectTransform.sizeDelta = new Vector2(190f, 28f);
            labelText.fontSize = 16;
        }
        return button;
    }

    private InputField CreateInputField(Transform parent)
    {
        var objectToCreate = new GameObject("TypingInput", typeof(RectTransform), typeof(Image), typeof(InputField));
        objectToCreate.transform.SetParent(parent, false);
        objectToCreate.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        SetRect(objectToCreate.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(650f, 76f));
        var input = objectToCreate.GetComponent<InputField>();
        input.lineType = InputField.LineType.SingleLine;
        input.caretColor = new Color32(255, 211, 94, 255);
        input.textComponent = CreateText("", objectToCreate.transform, 30, Color.white, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        input.textComponent.rectTransform.sizeDelta = new Vector2(620f, 54f);
        inputPlaceholder = CreateText("여기에 입력...", objectToCreate.transform, 22, new Color32(126, 164, 215, 255), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        input.placeholder = inputPlaceholder;
        input.placeholder.rectTransform.sizeDelta = new Vector2(620f, 54f);
        return input;
    }

    private InputField CreateNicknameInput(Transform parent)
    {
        var objectToCreate = new GameObject("NicknameInput", typeof(RectTransform), typeof(Image), typeof(InputField));
        objectToCreate.transform.SetParent(parent, false);
        objectToCreate.GetComponent<Image>().color = new Color32(245, 248, 255, 255);
        SetRect(objectToCreate.GetComponent<RectTransform>(), new Vector2(0.5f, 0.57f), new Vector2(0.5f, 0.57f), Vector2.zero, new Vector2(560f, 58f));
        var input = objectToCreate.GetComponent<InputField>();
        input.lineType = InputField.LineType.SingleLine;
        input.contentType = InputField.ContentType.Name;
        input.characterLimit = 16;
        input.caretColor = new Color32(40, 80, 140, 255);
        input.textComponent = CreateText("", objectToCreate.transform, 24, new Color32(30, 48, 80, 255), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        input.textComponent.rectTransform.sizeDelta = new Vector2(520f, 50f);
        var placeholder = CreateText(language == Language.Korean ? "닉네임 입력..." : "ENTER NICKNAME...", objectToCreate.transform, 20, new Color32(143, 161, 190, 255), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        placeholder.rectTransform.sizeDelta = new Vector2(520f, 50f);
        input.placeholder = placeholder;
        return input;
    }
}
