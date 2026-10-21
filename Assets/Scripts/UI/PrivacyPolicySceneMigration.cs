using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private void EnsurePrivacyPolicySceneLayout()
    {
        if (privacyPanel == null || privacyScrollRect != null) return;

        var panelImage = privacyPanel.GetComponent<Image>();
        if (panelImage != null)
        {
            panelImage.sprite = null;
            panelImage.color = Color.clear;
        }

        var window = CreatePanel("PrivacyPolicyWindow_Runtime", privacyPanel.transform, new Color32(9, 18, 38, 252));
        SetRect(window.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(1000f, 600f));
        ApplyPixelShopWindow(window);

        privacyTitleText.transform.SetParent(window.transform, false);
        SetRect(privacyTitleText.rectTransform, new Vector2(0.5f, 0.88f), new Vector2(0.5f, 0.88f), Vector2.zero, new Vector2(900f, 48f));
        privacyTitleText.alignment = TextAnchor.MiddleCenter;

        var viewportObject = CreatePanel("PrivacyPolicyViewport_Runtime", window.transform, new Color32(4, 12, 30, 245));
        SetRect(viewportObject.GetComponent<RectTransform>(), new Vector2(0.47f, 0.50f), new Vector2(0.47f, 0.50f), Vector2.zero, new Vector2(780f, 350f));
        var viewport = viewportObject.GetComponent<RectTransform>();
        viewportObject.AddComponent<RectMask2D>();
        var privacyMask = viewportObject.AddComponent<Mask>();
        privacyMask.showMaskGraphic = true;

        var contentObject = new GameObject("PrivacyPolicyContent_Runtime", typeof(RectTransform));
        contentObject.transform.SetParent(viewport, false);
        var content = contentObject.GetComponent<RectTransform>();
        SetRect(content, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), Vector2.zero, new Vector2(760f, 1600f));
        content.pivot = new Vector2(0.5f, 1f);

        privacyMessageText.transform.SetParent(content, false);
        SetRect(privacyMessageText.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), Vector2.zero, new Vector2(730f, 1540f));
        privacyMessageText.rectTransform.pivot = new Vector2(0.5f, 1f);
        privacyMessageText.alignment = TextAnchor.UpperCenter;
        privacyMessageText.horizontalOverflow = HorizontalWrapMode.Wrap;
        privacyMessageText.verticalOverflow = VerticalWrapMode.Overflow;
        privacyMessageText.resizeTextForBestFit = false;

        privacyScrollRect = viewportObject.AddComponent<ScrollRect>();
        privacyScrollRect.viewport = viewport;
        privacyScrollRect.content = content;
        privacyScrollRect.horizontal = false;
        privacyScrollRect.vertical = true;
        privacyScrollRect.movementType = ScrollRect.MovementType.Clamped;
        privacyScrollRect.scrollSensitivity = 55f;
        privacyScrollRect.verticalNormalizedPosition = 1f;

        var scrollbarObject = new GameObject("PrivacyVerticalScrollbar_Runtime", typeof(RectTransform), typeof(Image), typeof(Scrollbar));
        scrollbarObject.transform.SetParent(window.transform, false);
        SetRect(scrollbarObject.GetComponent<RectTransform>(), new Vector2(0.91f, 0.50f), new Vector2(0.91f, 0.50f), Vector2.zero, new Vector2(18f, 350f));
        scrollbarObject.GetComponent<Image>().color = new Color32(7, 24, 58, 255);
        var handleObject = new GameObject("Handle", typeof(RectTransform), typeof(Image));
        handleObject.transform.SetParent(scrollbarObject.transform, false);
        var handleRect = handleObject.GetComponent<RectTransform>();
        handleRect.anchorMin = new Vector2(0f, 0.78f);
        handleRect.anchorMax = new Vector2(1f, 1f);
        handleRect.offsetMin = new Vector2(1f, 1f);
        handleRect.offsetMax = new Vector2(-1f, -1f);
        var handleImage = handleObject.GetComponent<Image>();
        handleImage.color = new Color32(67, 199, 255, 255);
        privacyVerticalScrollbar = scrollbarObject.GetComponent<Scrollbar>();
        privacyVerticalScrollbar.direction = Scrollbar.Direction.BottomToTop;
        privacyVerticalScrollbar.handleRect = handleRect;
        privacyVerticalScrollbar.targetGraphic = handleImage;
        privacyScrollRect.verticalScrollbar = privacyVerticalScrollbar;
        privacyScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
        privacyScrollRect.verticalScrollbarSpacing = 10f;

        privacyCloseButton.transform.SetParent(window.transform, false);
        SetRect(privacyCloseButton.GetComponent<RectTransform>(), new Vector2(0.5f, 0.10f), new Vector2(0.5f, 0.10f), Vector2.zero, new Vector2(220f, 58f));
    }
}
