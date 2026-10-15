using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private void UpdateHud()
    {
        scoreText.text = string.Format(language == Language.Korean ? "점수 {0:0000}" : "SCORE {0:0000}", score);
        if (lifeHeartImages != null)
        {
            for (int i = 0; i < lifeHeartImages.Length; i++)
            {
                lifeHeartImages[i].gameObject.SetActive(i < lives);
            }
        }
        comboText.text = (language == Language.Korean ? "콤보 x" : "COMBO x") + combo;
        waveText.text = (language == Language.Korean ? "웨이브 " : "WAVE ") + wave;
        timerText.text = gameMode == GameMode.TimeAttack
            ? (language == Language.Korean ? "시간 " : "TIME ") + Mathf.CeilToInt(timeRemaining)
            : string.Empty;
    }

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
        return Resources.Load<Texture2D>(fullBodyPaths[index]) != null
            ? fullBodyPaths[index]
            : GetCharacterResourcePath(false);
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
        var rootRect = characterStatusPanel.GetComponent<RectTransform>();
        SetRect(rootRect, new Vector2(0.88f, 0.22f), new Vector2(0.88f, 0.22f), Vector2.zero, new Vector2(170f, 170f));

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
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x - center;
                float dy = y - center;
                pixels[y * size + x] = dx * dx + dy * dy <= radius * radius
                    ? new Color32(255, 255, 255, 255)
                    : new Color32(255, 255, 255, 0);
            }
        }
        texture.SetPixels32(pixels);
        texture.Apply();
        return Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), 100f);
    }

    private void UpdateHighScoreText()
    {
        if (highScoreText != null) highScoreText.text = string.Format(language == Language.Korean ? "최고 점수 {0:0000}" : "BEST SCORE {0:0000}", highScore);
    }
}
