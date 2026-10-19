using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private void ApplyPixelLobbyBackground(GameObject panel)
    {
        ApplyPixelTexture(panel, "Pixel/lobby-background", false, false);
    }

    private void ApplyPixelShopWindow(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/coin-shop-window-clean");
        if (texture == null) texture = Resources.Load<Texture2D>("Pixel/coin-shop-window");
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, new Vector4(180f, 180f, 180f, 180f));
        image.type = Image.Type.Sliced;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void ApplyPixelInsufficientCoinsWindow(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/coin-shop-window");
        if (texture == null)
        {
            ApplyPixelShopWindow(panel);
            return;
        }

        ApplySimplePixelTexture(panel, texture, true, false);
    }

    private void ApplyPixelBackground(GameObject panel)
    {
        ApplyPixelTexture(panel, "Pixel/wordfall-background", false, false);
    }

    private void ApplyPixelWindow(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/wordfall-window");
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, new Vector4(180f, 180f, 180f, 180f));
        image.type = Image.Type.Sliced;
        image.preserveAspect = false;
        image.color = Color.white;
    }

    private void ApplyPixelLobbyDialogueWindow(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/lobby-dialogue-window-clean");
        if (texture == null) texture = Resources.Load<Texture2D>("Pixel/lobby-dialogue-window");
        if (texture == null)
        {
            ApplyPixelWindow(panel);
            return;
        }

        ApplySimplePixelTexture(panel, texture, true, false);
    }

    private void ApplyPixelSettingsWindow(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/settings-window");
        if (texture == null)
        {
            ApplyPixelWindow(panel);
            ApplyModalDim(panel);
            return;
        }

        ApplySimplePixelTexture(panel, texture, false, true);
    }

    private void ApplyPixelGameOverWindow(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/game-over-window");
        if (texture == null)
        {
            ApplyPixelWindow(panel);
            return;
        }

        ApplySimplePixelTexture(panel, texture, true, false);
    }

    private void ApplyPixelAttendanceWindow(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/attendance-reward-window");
        if (texture == null)
        {
            ApplyPixelWindow(panel);
            return;
        }

        ApplySimplePixelTexture(panel, texture, true, false);
    }

    private void ApplyPixelTexture(GameObject panel, string resourcePath, bool preserveAspect, bool raycastTarget)
    {
        var texture = Resources.Load<Texture2D>(resourcePath);
        if (texture == null) return;
        ApplySimplePixelTexture(panel, texture, preserveAspect, raycastTarget);
    }

    private void ApplySimplePixelTexture(GameObject panel, Texture2D texture, bool preserveAspect, bool raycastTarget)
    {
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        image.type = Image.Type.Simple;
        image.preserveAspect = preserveAspect;
        image.color = Color.white;
        image.raycastTarget = raycastTarget;
    }

    private void ApplyModalDim(GameObject panel)
    {
        var dimObject = new GameObject("ModalDim", typeof(RectTransform), typeof(Image));
        dimObject.transform.SetParent(panel.transform, false);
        var dimImage = dimObject.GetComponent<Image>();
        dimImage.color = new Color32(0, 0, 0, 230);
        dimImage.raycastTarget = true;
        SetRect(dimObject.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        dimObject.transform.SetAsFirstSibling();
    }
}
