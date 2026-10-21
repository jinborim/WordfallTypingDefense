using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private void EnsureLobbyAudioButtons()
    {
        if (homeHubPanel == null) return;

        if (homeHubBgmButton == null)
            homeHubBgmButton = CreateIconButton("HomeHubBgmButton_Runtime", homeHubPanel.transform, "Pixel/audio-on-icon", new Vector2(0.90f, 0.88f), new Vector2(58f, 58f));
        if (homeHubSfxButton == null)
            homeHubSfxButton = CreateIconButton("HomeHubSfxButton_Runtime", homeHubPanel.transform, "Pixel/audio-on-icon", new Vector2(0.96f, 0.88f), new Vector2(58f, 58f));
        if (homeHubBgmLabelText == null)
            homeHubBgmLabelText = CreateText("\uBC30\uACBD\uC74C", homeHubPanel.transform, 14, new Color32(220, 230, 248, 255), new Vector2(0.90f, 0.79f), new Vector2(0.90f, 0.79f));
        if (homeHubSfxLabelText == null)
            homeHubSfxLabelText = CreateText("\uD6A8\uACFC\uC74C", homeHubPanel.transform, 14, new Color32(220, 230, 248, 255), new Vector2(0.96f, 0.79f), new Vector2(0.96f, 0.79f));
        ApplyLobbyAudioLayout();
    }

    private void ApplyLobbyAudioLayout()
    {
        if (homeHubBgmButton != null)
            SetRect(homeHubBgmButton.GetComponent<RectTransform>(), new Vector2(0.90f, 0.88f), new Vector2(0.90f, 0.88f), Vector2.zero, new Vector2(58f, 58f));
        if (homeHubSfxButton != null)
            SetRect(homeHubSfxButton.GetComponent<RectTransform>(), new Vector2(0.96f, 0.88f), new Vector2(0.96f, 0.88f), Vector2.zero, new Vector2(58f, 58f));
        if (homeHubBgmLabelText != null)
        {
            SetRect(homeHubBgmLabelText.rectTransform, new Vector2(0.90f, 0.88f), new Vector2(0.90f, 0.88f), new Vector2(0f, -48f), new Vector2(78f, 24f));
            homeHubBgmLabelText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            homeHubBgmLabelText.alignment = TextAnchor.MiddleCenter;
        }
        if (homeHubSfxLabelText != null)
        {
            SetRect(homeHubSfxLabelText.rectTransform, new Vector2(0.96f, 0.88f), new Vector2(0.96f, 0.88f), new Vector2(0f, -48f), new Vector2(78f, 24f));
            homeHubSfxLabelText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            homeHubSfxLabelText.alignment = TextAnchor.MiddleCenter;
        }
    }
}
