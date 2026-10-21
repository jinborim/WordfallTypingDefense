using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private void EnsureHomeHubLogoutButton()
    {
        if (homeHubMenuPanel == null || homeHubLogoutButton != null) return;
        homeHubLogoutButton = CreateButton("LOGOUT_Runtime", homeHubMenuPanel.transform, new Vector2(0.50f, 0.18f), new Vector2(210f, 56f), new Color32(149, 71, 92, 255));
        StyleLogoutLink(homeHubLogoutButton);
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
