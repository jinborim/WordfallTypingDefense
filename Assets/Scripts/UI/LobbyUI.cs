using System.Collections;
using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void ShowHomeHub()
    {
        if (playing) return;
        if (!profileCreated)
        {
            ShowProfilePanel();
            return;
        }

        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        setupPanel.SetActive(false);
        languagePanel.SetActive(false);
        profilePanel.SetActive(false);
        myPagePanel.SetActive(false);
        UpdateProfileSummary();
        UpdateCoinText();
        UpdateHomeHubCharacter();
        ResetHomeHubMenuState();
        homeHubPanel.SetActive(true);
        ShowLobbyDialogue();
        CheckAttendanceReward();
    }

    private void CloseHomeHub()
    {
        if (lobbyChatPanel != null) lobbyChatPanel.SetActive(false);
        homeHubPanel.SetActive(false);
    }

    private void ShowLobbyDialogue()
    {
        if (lobbyChatPanel == null) return;
        string[] dialogues = language == Language.Korean ? koreanLobbyDialogues : englishLobbyDialogues;
        string dialogue = dialogues[lobbyDialogueIndex % dialogues.Length];
        lobbyDialogueIndex++;
        lobbyChatMessageText.text = dialogue;
        lobbyChatPanel.SetActive(true);
    }

    private void ToggleHomeHubMenu()
    {
        homeHubMenuOpen = !homeHubMenuOpen;
        if (homeHubMenuPanel == null) return;
        if (homeHubHamburgerIcon != null) homeHubHamburgerIcon.gameObject.SetActive(!homeHubMenuOpen);
        if (homeHubCloseIcon != null) homeHubCloseIcon.SetActive(homeHubMenuOpen);
        if (homeHubMenuCoroutine != null) StopCoroutine(homeHubMenuCoroutine);
        if (homeHubMenuOpen) homeHubMenuPanel.SetActive(true);
        homeHubMenuCoroutine = StartCoroutine(AnimateHomeHubMenu(homeHubMenuOpen));
    }

    private IEnumerator AnimateHomeHubMenu(bool open)
    {
        var rect = homeHubMenuPanel.GetComponent<RectTransform>();
        float start = rect.anchoredPosition.x;
        float target = open ? 0f : -560f;
        var toggleRect = homeHubMenuToggleButton.GetComponent<RectTransform>();
        float toggleStart = toggleRect.anchoredPosition.x;
        float toggleTarget = open ? 590f : 0f;
        const float duration = 0.22f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            progress = progress * progress * (3f - 2f * progress);
            rect.anchoredPosition = new Vector2(Mathf.Lerp(start, target, progress), 0f);
            toggleRect.anchoredPosition = new Vector2(Mathf.Lerp(toggleStart, toggleTarget, progress), 0f);
            yield return null;
        }
        rect.anchoredPosition = new Vector2(target, 0f);
        toggleRect.anchoredPosition = new Vector2(toggleTarget, 0f);
        if (!open) homeHubMenuPanel.SetActive(false);
        homeHubMenuCoroutine = null;
    }

    private void StartQuickGame()
    {
        if (!profileCreated)
        {
            ShowProfilePanel();
            return;
        }

        StartGame();
    }
}
