using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private GameObject CreatePanel(string name, Transform parent, Color color)
    {
        var objectToCreate = new GameObject(name, typeof(RectTransform), typeof(Image));
        objectToCreate.transform.SetParent(parent, false);
        objectToCreate.GetComponent<Image>().color = color;
        SetRect(objectToCreate.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        return objectToCreate;
    }

    private GameObject CreateShopItemCard(string name, Transform parent, Vector2 anchor, Vector2 size)
    {
        var card = CreatePanel(name, parent, new Color32(0, 0, 0, 0));
        SetRect(card.GetComponent<RectTransform>(), anchor, anchor, Vector2.zero, size);
        card.GetComponent<Image>().raycastTarget = false;
        return card;
    }

    private Image CreateImage(string name, Transform parent, string resourcePath, Vector2 anchor, Vector2 size)
    {
        var objectToCreate = new GameObject(name, typeof(RectTransform), typeof(Image));
        objectToCreate.transform.SetParent(parent, false);
        var image = objectToCreate.GetComponent<Image>();
        var texture = Resources.Load<Texture2D>(resourcePath);
        if (texture != null)
        {
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
            image.preserveAspect = true;
        }
        SetRect(image.rectTransform, anchor, anchor, Vector2.zero, size);
        return image;
    }

    private Text CreateText(string content, Transform parent, int size, Color color, Vector2 anchor, Vector2 pivot)
    {
        var objectToCreate = new GameObject("Text", typeof(RectTransform), typeof(Text));
        objectToCreate.transform.SetParent(parent, false);
        var text = objectToCreate.GetComponent<Text>();
        text.text = content;
        text.font = uiFont;
        text.fontSize = size;
        text.color = color;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        var rect = text.rectTransform;
        SetRect(rect, anchor, anchor, Vector2.zero, new Vector2(700f, size + 24f));
        rect.pivot = pivot;
        return text;
    }

    private Button CreateButton(string label, Transform parent, Vector2 anchor, Vector2 size, Color color)
    {
        var objectToCreate = new GameObject(label + "Button", typeof(RectTransform), typeof(Image), typeof(Button));
        objectToCreate.transform.SetParent(parent, false);
        var image = objectToCreate.GetComponent<Image>();
        var buttonTexture = Resources.Load<Texture2D>("Pixel/wordfall-button");
        if (buttonTexture != null)
        {
            buttonTexture.filterMode = FilterMode.Point;
            image.sprite = Sprite.Create(buttonTexture, new Rect(0f, 0f, buttonTexture.width, buttonTexture.height), new Vector2(0.5f, 0.5f), 100f);
            image.type = Image.Type.Simple;
            image.color = Color.white;
        }
        else image.color = color;

        var outline = objectToCreate.AddComponent<Outline>();
        outline.effectColor = new Color32(3, 8, 18, 255);
        outline.effectDistance = new Vector2(4f, -4f);
        outline.useGraphicAlpha = false;

        var button = objectToCreate.GetComponent<Button>();
        button.transition = Selectable.Transition.ColorTint;
        var navigation = button.navigation;
        navigation.mode = Navigation.Mode.None;
        button.navigation = navigation;
        var colors = button.colors;
        colors.normalColor = color;
        colors.highlightedColor = new Color(Mathf.Min(color.r + 0.08f, 1f), Mathf.Min(color.g + 0.08f, 1f), Mathf.Min(color.b + 0.08f, 1f), 1f);
        colors.pressedColor = new Color(color.r * 0.8f, color.g * 0.8f, color.b * 0.8f, 1f);
        colors.selectedColor = color;
        colors.disabledColor = new Color(color.r * 0.45f, color.g * 0.45f, color.b * 0.45f, 0.75f);
        button.colors = colors;
        objectToCreate.AddComponent<ButtonPressFeedback>();
        SetRect(objectToCreate.GetComponent<RectTransform>(), anchor, anchor, Vector2.zero, size);
        var text = CreateText(label, objectToCreate.transform, 22, Color.white, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        text.rectTransform.sizeDelta = size;
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = Mathf.Clamp(Mathf.RoundToInt(size.y * 0.42f), 12, 20);
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = 10;
        text.resizeTextMaxSize = text.fontSize;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        return button;
    }

    private Button CreateIconButton(string name, Transform parent, string resourcePath, Vector2 anchor, Vector2 size)
    {
        var objectToCreate = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        objectToCreate.transform.SetParent(parent, false);
        var image = objectToCreate.GetComponent<Image>();
        SetIconButtonSprite(objectToCreate.GetComponent<Button>(), resourcePath);
        image.raycastTarget = true;

        var button = objectToCreate.GetComponent<Button>();
        button.transition = Selectable.Transition.ColorTint;
        var colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color32(190, 240, 255, 255);
        colors.pressedColor = new Color32(130, 190, 235, 255);
        colors.selectedColor = Color.white;
        colors.disabledColor = new Color32(100, 120, 150, 150);
        button.colors = colors;
        button.navigation = new Navigation { mode = Navigation.Mode.None };
        SetRect(objectToCreate.GetComponent<RectTransform>(), anchor, anchor, Vector2.zero, size);
        return button;
    }

    private void SetIconButtonSprite(Button button, string resourcePath)
    {
        if (button == null) return;
        var image = button.GetComponent<Image>();
        if (image == null) return;
        var texture = Resources.Load<Texture2D>(resourcePath);
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        image.preserveAspect = true;
        image.color = Color.white;
    }

    private static void SetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size)
    {
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = position;
        if (anchorMin == anchorMax) rect.sizeDelta = size;
        else { rect.offsetMin = position; rect.offsetMax = position + size; }
    }
}
