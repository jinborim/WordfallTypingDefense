using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private PixelAmbientMotion CreateMainAmbientMotion(GameObject panel)
    {
        return CreateAmbientMotion(panel, "Pixel/wordfall-background", "MainAmbientMotion");
    }

    private PixelAmbientMotion CreateLobbyAmbientMotion(GameObject panel)
    {
        return CreateAmbientMotion(panel, "Pixel/lobby-background", "LobbyAmbientMotion");
    }

    private void EnsureLobbyAmbientMotion()
    {
        if (homeHubPanel == null || homeHubPanel.GetComponentInChildren<PixelAmbientMotion>(true) != null) return;
        CreateLobbyAmbientMotion(homeHubPanel);
    }

    private void EnsureMainAmbientMotion()
    {
        if (mainPanel == null || mainPanel.GetComponentInChildren<PixelAmbientMotion>(true) != null) return;
        CreateMainAmbientMotion(mainPanel);
    }

    private PixelAmbientMotion CreateAmbientMotion(GameObject panel, string texturePath, string objectName)
    {
        var motionObject = new GameObject(objectName, typeof(RectTransform));
        motionObject.transform.SetParent(panel.transform, false);
        SetRect(motionObject.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        motionObject.transform.SetAsFirstSibling();
        var motion = motionObject.AddComponent<PixelAmbientMotion>();
        motion.TextureResourcePath = texturePath;
        return motion;
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
        private readonly List<RectTransform> buildingLights = new List<RectTransform>();
        private readonly List<float> buildingPhases = new List<float>();
        private readonly List<float> buildingSpeeds = new List<float>();
        public string TextureResourcePath = "Pixel/wordfall-background";

        private static readonly Vector2[] Positions =
        {
            new Vector2(0.08f, 0.82f), new Vector2(0.18f, 0.63f), new Vector2(0.29f, 0.88f),
            new Vector2(0.41f, 0.73f), new Vector2(0.57f, 0.84f), new Vector2(0.68f, 0.62f),
            new Vector2(0.79f, 0.79f), new Vector2(0.91f, 0.57f), new Vector2(0.13f, 0.34f),
            new Vector2(0.33f, 0.29f), new Vector2(0.72f, 0.35f), new Vector2(0.87f, 0.24f)
        };

        private void Start()
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
                speeds.Add(0.18f + (i % 4) * 0.05f);
                phases.Add(i * 0.83f);
                twinkleSpeeds.Add(1.2f + (i % 3) * 0.35f);
                baseAlphas.Add(image.color.a);
            }
            CreateBuildingLights();
        }

        private void CreateBuildingLights()
        {
            var positions = new[]
            {
                new Vector2(0.08f, 0.16f), new Vector2(0.13f, 0.12f), new Vector2(0.22f, 0.10f),
                new Vector2(0.31f, 0.13f), new Vector2(0.40f, 0.09f), new Vector2(0.59f, 0.12f),
                new Vector2(0.68f, 0.10f), new Vector2(0.77f, 0.14f), new Vector2(0.87f, 0.11f),
                new Vector2(0.94f, 0.16f)
            };
            for (int i = 0; i < positions.Length; i++)
            {
                var lightObject = new GameObject("BuildingLight" + i, typeof(RectTransform), typeof(Image));
                lightObject.transform.SetParent(transform, false);
                var rect = lightObject.GetComponent<RectTransform>();
                rect.anchorMin = positions[i];
                rect.anchorMax = positions[i];
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.sizeDelta = new Vector2(i % 3 == 0 ? 12f : 7f, 3f);
                var image = lightObject.GetComponent<Image>();
                image.color = i % 2 == 0 ? new Color32(255, 211, 94, 170) : new Color32(91, 224, 255, 150);
                image.raycastTarget = false;
                buildingLights.Add(rect);
                buildingPhases.Add(i * 1.37f);
                buildingSpeeds.Add(0.85f + (i % 4) * 0.32f);
            }
        }

        private void CreateDriftingBackgroundLayer()
        {
            var texture = Resources.Load<Texture2D>(TextureResourcePath);
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
                backgroundLayer.rectTransform.anchoredPosition = new Vector2(Mathf.Sin(time * 0.16f) * 30f, Mathf.Cos(time * 0.12f) * 12f);
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

            for (int i = 0; i < buildingLights.Count; i++)
            {
                var image = buildingLights[i].GetComponent<Image>();
                var color = image.color;
                float pulse = 0.45f + 0.55f * Mathf.Clamp01((Mathf.Sin(time * buildingSpeeds[i] + buildingPhases[i]) + 1f) * 0.5f);
                color.a = (i % 2 == 0 ? 0.68f : 0.58f) * pulse;
                image.color = color;
            }
        }
    }
}
