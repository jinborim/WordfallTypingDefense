using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private void EnsureHomeHubLanguageButton()
    {
        if (homeHubMenuPanel == null || homeHubLanguageButton != null) return;
        homeHubLanguageButton = CreateImageAction(
            "HomeHubLanguageAction_Runtime",
            homeHubMenuPanel.transform,
            "Pixel/menu-language-icon",
            language == Language.Korean ? "언어 설정" : "LANGUAGE",
            new Vector2(0.50f, 0.20f),
            new Vector2(170f, 130f),
            new Vector2(78f, 78f),
            out homeHubLanguageLabelText);
        homeHubLanguageButton.onClick.AddListener(ShowLanguagePanel);
    }

    private void EnsureHomeHubLogoutButton()
    {
        if (homeHubMenuPanel == null || homeHubLogoutButton != null) return;
        homeHubLogoutButton = CreateButton("LOGOUT_Runtime", homeHubMenuPanel.transform, new Vector2(0.50f, 0.07f), new Vector2(210f, 46f), new Color32(149, 71, 92, 255));
        StyleLogoutLink(homeHubLogoutButton);
    }

    private void EnsureLogoutConfirmPanel()
    {
        if (mainPanel == null || logoutConfirmPanel != null) return;

        logoutConfirmPanel = CreatePanel("LogoutConfirmPanel_Runtime", mainPanel.transform, new Color32(9, 18, 38, 250));
        SetRect(logoutConfirmPanel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        ApplyPixelWindow(logoutConfirmPanel);
        ApplyModalDim(logoutConfirmPanel);
        logoutConfirmTitleText = CreateText("\uB85C\uADF8\uC544\uC6C3", logoutConfirmPanel.transform, 38, new Color32(255, 211, 94, 255), new Vector2(0.5f, 0.67f), new Vector2(0.5f, 0.67f));
        logoutConfirmMessageText = CreateText("\uB85C\uADF8\uC544\uC6C3 \uD558\uC2DC\uACA0\uC2B5\uB2C8\uAE4C?", logoutConfirmPanel.transform, 19, new Color32(214, 222, 238, 255), new Vector2(0.5f, 0.49f), new Vector2(0.5f, 0.49f));
        logoutConfirmMessageText.rectTransform.sizeDelta = new Vector2(760f, 60f);
        logoutConfirmButton = CreateButton("\uD655\uC778", logoutConfirmPanel.transform, new Vector2(0.38f, 0.23f), new Vector2(190f, 58f), new Color32(149, 71, 92, 255));
        logoutCancelButton = CreateButton("\uCDE8\uC18C", logoutConfirmPanel.transform, new Vector2(0.62f, 0.23f), new Vector2(190f, 58f), new Color32(67, 149, 255, 255));
        logoutConfirmPanel.SetActive(false);
    }

    private void EnsureLobbyDialogueTypingControl()
    {
        if (lobbyChatPanel == null) return;
        var dialogueButton = lobbyChatPanel.GetComponent<Button>();
        if (dialogueButton == null)
        {
            dialogueButton = lobbyChatPanel.AddComponent<Button>();
            dialogueButton.transition = Selectable.Transition.None;
            dialogueButton.targetGraphic = lobbyChatPanel.GetComponent<Image>();
        }

        var panelImage = lobbyChatPanel.GetComponent<Image>();
        if (panelImage != null) panelImage.raycastTarget = true;
        if (lobbyChatMessageText != null) lobbyChatMessageText.raycastTarget = false;
    }

    private void StyleLogoutLink(Button button)
    {
        if (button == null) return;
        var image = button.GetComponent<Image>();
        if (image != null)
        {
            image.sprite = null;
            image.color = Color.clear;
        }

        var outline = button.GetComponent<Outline>();
        if (outline != null) Destroy(outline);

        var colors = button.colors;
        colors.normalColor = Color.clear;
        colors.highlightedColor = new Color32(255, 255, 255, 24);
        colors.pressedColor = new Color32(255, 211, 94, 40);
        colors.selectedColor = Color.clear;
        colors.disabledColor = new Color32(160, 170, 190, 90);
        button.colors = colors;

        var text = button.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.fontStyle = FontStyle.Normal;
            text.color = new Color32(214, 222, 238, 255);
        }

        var underlineObject = new GameObject("LogoutUnderline", typeof(RectTransform), typeof(Image));
        underlineObject.transform.SetParent(button.transform, false);
        var underlineRect = underlineObject.GetComponent<RectTransform>();
        SetRect(underlineRect, new Vector2(0.5f, 0.20f), new Vector2(0.5f, 0.20f), Vector2.zero, new Vector2(120f, 2f));
        underlineObject.GetComponent<Image>().color = new Color32(214, 222, 238, 255);
        underlineObject.GetComponent<Image>().raycastTarget = false;
    }
}
