using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void ApplyPixelInputFrame(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/wordfall-input");
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<UnityEngine.UI.Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, texture.height * 0.16f, texture.width, texture.height * 0.70f), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, new Vector4(110f, 110f, 110f, 110f));
        image.type = UnityEngine.UI.Image.Type.Sliced;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void ApplyPixelRectBackground(GameObject panel, Color fillColor, Color borderColor)
    {
        var image = panel.GetComponent<UnityEngine.UI.Image>();
        image.sprite = null;
        image.type = UnityEngine.UI.Image.Type.Simple;
        image.color = fillColor;
        image.raycastTarget = false;

        var outline = panel.GetComponent<UnityEngine.UI.Outline>();
        if (outline == null) outline = panel.AddComponent<UnityEngine.UI.Outline>();
        outline.effectColor = borderColor;
        outline.effectDistance = new Vector2(4f, 4f);
        outline.useGraphicAlpha = true;
    }

    private void ApplyCompactPixelRectImage(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/compact-pixel-rect");
        if (texture == null)
        {
            ApplyPixelRectBackground(panel, new Color32(5, 18, 48, 235), new Color32(41, 162, 235, 255));
            return;
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<UnityEngine.UI.Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, new Vector4(120f, 120f, 120f, 120f));
        image.type = UnityEngine.UI.Image.Type.Sliced;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void ApplyPixelPlayerInfoCard(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/lobby-player-info-card");
        if (texture == null)
        {
            ApplyCompactPixelRectImage(panel);
            return;
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<UnityEngine.UI.Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        image.type = UnityEngine.UI.Image.Type.Simple;
        image.preserveAspect = true;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void ApplyPixelHudStrip(GameObject panel, string resourcePath)
    {
        var texture = Resources.Load<Texture2D>(resourcePath);
        if (texture == null)
        {
            ApplyCompactPixelRectImage(panel);
            return;
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<UnityEngine.UI.Image>();
        var cropHeight = texture.height * 0.20f;
        var cropY = texture.height * 0.40f;
        image.sprite = Sprite.Create(texture, new Rect(0f, cropY, texture.width, cropHeight), new Vector2(0.5f, 0.5f), 100f);
        image.type = UnityEngine.UI.Image.Type.Simple;
        image.preserveAspect = false;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private void ApplyPixelScoreHud(GameObject panel)
    {
        var texture = Resources.Load<Texture2D>("Pixel/score-hud-frame");
        if (texture == null) return;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        var image = panel.GetComponent<UnityEngine.UI.Image>();
        image.sprite = Sprite.Create(texture, new Rect(0f, texture.height * 0.14f, texture.width, texture.height * 0.72f), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, new Vector4(130f, 130f, 130f, 130f));
        image.type = UnityEngine.UI.Image.Type.Sliced;
        image.color = Color.white;
        image.raycastTarget = false;
    }
}
