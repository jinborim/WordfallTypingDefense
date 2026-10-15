using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private PixelAmbientMotion CreateMainAmbientMotion(GameObject panel)
    {
        var motionObject = new GameObject("MainAmbientMotion", typeof(RectTransform));
        motionObject.transform.SetParent(panel.transform, false);
        SetRect(motionObject.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        motionObject.transform.SetAsFirstSibling();
        return motionObject.AddComponent<PixelAmbientMotion>();
    }

    private sealed class PixelAmbientMotion : MonoBehaviour
    {
        private Image backgroundLayer;
        private readonly List<RectTransform> motes = new List<RectTransform>();
        private readonly List<Vector2> offsets = new List<Vector2>();
        private readonly List<float> speeds = new List<float>();
        private readonly List<float> phases = new List<float>();
        private readonly List<float> twinkleSpeeds = new List<float>();
        private readonly List<float> baseAlphas = new List<float>();

        private static readonly Vector2[] Positions =
        {
            new Vector2(0.08f, 0.82f), new Vector2(0.18f, 0.63f), new Vector2(0.29f, 0.88f),
            new Vector2(0.41f, 0.73f), new Vector2(0.57f, 0.84f), new Vector2(0.68f, 0.62f),
            new Vector2(0.79f, 0.79f), new Vector2(0.91f, 0.57f), new Vector2(0.13f, 0.34f),
            new Vector2(0.33f, 0.29f), new Vector2(0.72f, 0.35f), new Vector2(0.87f, 0.24f)
        };

        private void Awake()
        {
            CreateDriftingBackgroundLayer();
            for (int i = 0; i < Positions.Length; i++)
            {
                var moteObject = new GameObject("Mote" + i, typeof(RectTransform), typeof(Image));
                moteObject.transform.SetParent(transform, false);
                var rect = moteObject.GetComponent<RectTransform>();
                rect.anchorMin = Positions[i];
                rect.anchorMax = Positions[i];
                rect.pivot = new Vector2(0.5f, 0.5f);
                float size = i % 3 == 0 ? 7f : 4f;
                rect.sizeDelta = new Vector2(size, size);
                rect.anchoredPosition = Vector2.zero;

                var image = moteObject.GetComponent<Image>();
                image.color = i % 3 == 0 ? new Color32(255, 220, 130, 125) : new Color32(162, 215, 255, 100);
                image.raycastTarget = false;

                motes.Add(rect);
                offsets.Add(new Vector2(18f + (i % 3) * 8f, 8f + (i % 2) * 5f));
                speeds.Add(0.12f + (i % 4) * 0.035f);
                phases.Add(i * 0.83f);
                twinkleSpeeds.Add(0.8f + (i % 3) * 0.25f);
                baseAlphas.Add(image.color.a);
            }
        }

        private void CreateDriftingBackgroundLayer()
        {
            var texture = Resources.Load<Texture2D>("Pixel/wordfall-background");
            if (texture == null) return;

            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            var viewportObject = new GameObject("SkyMotionViewport", typeof(RectTransform), typeof(RectMask2D));
            viewportObject.transform.SetParent(transform, false);
            viewportObject.transform.SetAsFirstSibling();
            var viewport = viewportObject.GetComponent<RectTransform>();
            viewport.anchorMin = new Vector2(0f, 0.24f);
            viewport.anchorMax = Vector2.one;
            viewport.offsetMin = Vector2.zero;
            viewport.offsetMax = Vector2.zero;

            var layerObject = new GameObject("DriftingSkyLayer", typeof(RectTransform), typeof(Image));
            layerObject.transform.SetParent(viewportObject.transform, false);
            var rect = layerObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(1440f, 820f);

            backgroundLayer = layerObject.GetComponent<Image>();
            backgroundLayer.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
            backgroundLayer.type = Image.Type.Simple;
            backgroundLayer.preserveAspect = false;
            backgroundLayer.color = new Color(0.42f, 0.62f, 1f, 0.20f);
            backgroundLayer.raycastTarget = false;
        }

        private void Update()
        {
            float time = Time.unscaledTime;
            if (backgroundLayer != null)
            {
                backgroundLayer.rectTransform.anchoredPosition = new Vector2(Mathf.Sin(time * 0.10f) * 30f, Mathf.Cos(time * 0.075f) * 12f);
            }

            for (int i = 0; i < motes.Count; i++)
            {
                float phase = phases[i] + time * speeds[i];
                motes[i].anchoredPosition = new Vector2(Mathf.Sin(phase) * offsets[i].x, Mathf.Cos(phase * 0.83f) * offsets[i].y);
                var image = motes[i].GetComponent<Image>();
                Color color = image.color;
                color.a = baseAlphas[i] * (0.72f + Mathf.Sin(time * twinkleSpeeds[i] + phases[i]) * 0.28f);
                image.color = color;
            }
        }
    }
}
