using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private Image CreateCircularProfileImage(Transform parent, string resourcePath, Vector2 anchor, float diameter)
    {
        var maskObject = new GameObject("MyPageProfileMask", typeof(RectTransform), typeof(Image), typeof(Mask));
        maskObject.transform.SetParent(parent, false);

        var maskRect = maskObject.GetComponent<RectTransform>();
        SetRect(maskRect, anchor, anchor, Vector2.zero, new Vector2(diameter, diameter));

        var maskImage = maskObject.GetComponent<Image>();
        maskImage.sprite = CreateCircularMaskSprite(256);
        maskImage.color = Color.white;
        maskImage.raycastTarget = false;
        maskObject.GetComponent<Mask>().showMaskGraphic = false;

        var portrait = CreateImage("MyPageCharacter", maskObject.transform, resourcePath, new Vector2(0.5f, 0.5f), new Vector2(diameter + 12f, diameter + 12f));
        portrait.rectTransform.anchoredPosition = new Vector2(0f, 6f);
        portrait.raycastTarget = false;
        portrait.preserveAspect = true;
        return portrait;
    }

    private Sprite CreateCircularMaskSprite(int size)
    {
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        var pixels = new Color32[size * size];
        var center = (size - 1) * 0.5f;
        var radius = size * 0.5f;
        for (var y = 0; y < size; y++)
        {
            for (var x = 0; x < size; x++)
            {
                var dx = x - center;
                var dy = y - center;
                pixels[y * size + x] = dx * dx + dy * dy <= radius * radius ? Color.white : Color.clear;
            }
        }
        texture.SetPixels32(pixels);
        texture.Apply();
        return Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), size);
    }
}
