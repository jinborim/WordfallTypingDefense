using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void ResetHomeHubMenuState()
    {
        homeHubMenuOpen = false;
        if (homeHubMenuCoroutine != null)
        {
            StopCoroutine(homeHubMenuCoroutine);
            homeHubMenuCoroutine = null;
        }
        if (homeHubMenuPanel != null)
        {
            homeHubMenuPanel.SetActive(false);
            homeHubMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-560f, 0f);
        }
        if (homeHubMenuToggleButton != null)
        {
            homeHubMenuToggleButton.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        if (homeHubHamburgerIcon != null) homeHubHamburgerIcon.gameObject.SetActive(true);
        if (homeHubCloseIcon != null) homeHubCloseIcon.SetActive(false);
    }
}
