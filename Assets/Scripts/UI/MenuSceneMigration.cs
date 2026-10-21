using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private void EnsureHomeHubLogoutButton()
    {
        if (homeHubMenuPanel == null || homeHubLogoutButton != null) return;
        homeHubLogoutButton = CreateButton("LOGOUT_Runtime", homeHubMenuPanel.transform, new Vector2(0.50f, 0.18f), new Vector2(210f, 56f), new Color32(149, 71, 92, 255));
    }
}
