using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private void CloseAttendancePanel()
    {
        attendancePanel.SetActive(false);
    }

    private void ShowInsufficientCoinsPanel()
    {
        PlaySoundEffect(SoundEffect.Wrong);
        insufficientCoinsTitleText.text = language == Language.Korean ? "\uCF54\uC778 \uBD80\uC871" : "NOT ENOUGH COINS";
        insufficientCoinsMessageText.text = language == Language.Korean ? "\uCF54\uC778\uC774 \uBD80\uC871\uD569\uB2C8\uB2E4." : "You do not have enough coins.";
        SetButtonText(insufficientCoinsCloseButton, language == Language.Korean ? "\uD655\uC778" : "OK");
        insufficientCoinsPanel.SetActive(true);
    }

    private void CloseInsufficientCoinsPanel()
    {
        insufficientCoinsPanel.SetActive(false);
    }

    private void ShowPurchaseCompletePanel(string itemName)
    {
        PlaySoundEffect(SoundEffect.Success);
        purchaseCompleteTitleText.text = language == Language.Korean ? "구매 완료" : "PURCHASE COMPLETE";
        purchaseCompleteMessageText.text = language == Language.Korean
            ? $"{itemName} 상품을 구매했습니다."
            : $"Purchased {itemName}.";
        SetButtonText(purchaseCompleteCloseButton, language == Language.Korean ? "확인" : "OK");
        purchaseCompletePanel.SetActive(true);
    }

    private void ClosePurchaseCompletePanel()
    {
        purchaseCompletePanel.SetActive(false);
    }

    private void ShowNicknameTicketPanel()
    {
        bool korean = language == Language.Korean;
        if (nicknameTicketTitleText != null) nicknameTicketTitleText.text = korean ? "\uB2C9\uB124\uC784 \uBCC0\uACBD\uAD8C \uBD80\uC871" : "NICKNAME TICKET REQUIRED";
        if (nicknameTicketMessageText != null) nicknameTicketMessageText.text = korean ? "\uB2C9\uB124\uC784\uC744 \uBCC0\uACBD\uD558\uB824\uBA74 \uBCC0\uACBD\uAD8C\uC774 \uD544\uC694\uD569\uB2C8\uB2E4." : "You need a nickname ticket to change your nickname.";
        SetButtonText(nicknameTicketCloseButton, korean ? "\uD655\uC778" : "OK");
        SetButtonText(nicknameTicketShopButton, korean ? "\uC0C1\uC810 \uAC00\uAE30" : "GO TO SHOP");
        if (nicknameTicketPanel != null) nicknameTicketPanel.SetActive(true);
    }

    private void CloseNicknameTicketPanel()
    {
        if (nicknameTicketPanel != null) nicknameTicketPanel.SetActive(false);
        if (profilePanel != null) profilePanel.SetActive(true);
    }

    private void GoToShopFromNicknameTicketPanel()
    {
        if (nicknameTicketPanel != null) nicknameTicketPanel.SetActive(false);
        if (profilePanel != null) profilePanel.SetActive(false);
        ShowShopPanel();
    }

    private void ShowAnnouncementPanel()
    {
        if (playing) return;
        homeHubPanel.SetActive(false);
        announcementTitleText.text = language == Language.Korean ? "\uACF5\uC9C0\uC0AC\uD56D" : "ANNOUNCEMENTS";
        announcementMessageText.text = language == Language.Korean ? "\uACF5\uC9C0\uC0AC\uD56D\uC744 \uBD88\uB7EC\uC624\uB294 \uC911..." : "Loading announcements...";
        SetButtonText(announcementCloseButton, language == Language.Korean ? "\uB2EB\uAE30" : "CLOSE");
        announcementPanel.SetActive(true);
        LoadAnnouncements();
    }

    private void CloseAnnouncementPanel()
    {
        announcementPanel.SetActive(false);
        ResetHomeHubMenuState();
        homeHubPanel.SetActive(true);
        ShowLobbyDialogue();
    }

    private void ShowPrivacyPanel()
    {
        if (playing) return;
        EnsurePrivacyPolicySceneLayout();
        privacyOpenedFromMainMenu = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenuScene";
        ApplyPrivacyPanelLayout();
        homeHubMenuPanel.SetActive(false);
        homeHubPanel.SetActive(false);
        SetMainMenuOverlayContentVisible(false);
        if (mainStartButton != null) mainStartButton.gameObject.SetActive(false);
        if (languageOpenButton != null) languageOpenButton.gameObject.SetActive(false);
        privacyPanel.transform.SetAsLastSibling();
        privacyPanel.SetActive(true);
    }

    private void SetMainMenuOverlayContentVisible(bool visible)
    {
        if (titleText != null) titleText.gameObject.SetActive(visible);
        if (subtitleText != null) subtitleText.gameObject.SetActive(visible);
        if (descriptionText != null) descriptionText.gameObject.SetActive(visible);
        if (profileSummaryText != null) profileSummaryText.gameObject.SetActive(visible);
        if (mainPanel == null) return;
        var logo = mainPanel.transform.Find("WordfallEnglishLogo");
        if (logo != null) logo.gameObject.SetActive(visible);
    }

    private void ApplyPrivacyPanelLayout()
    {
        if (privacyTitleText != null)
        {
            privacyTitleText.rectTransform.anchorMin = new Vector2(0.5f, 0.88f);
            privacyTitleText.rectTransform.anchorMax = new Vector2(0.5f, 0.88f);
            privacyTitleText.rectTransform.anchoredPosition = Vector2.zero;
            privacyTitleText.rectTransform.sizeDelta = new Vector2(900f, 48f);
            privacyTitleText.alignment = TextAnchor.MiddleCenter;
        }

        if (privacyCloseButton != null)
        {
            var rect = privacyCloseButton.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.12f);
            rect.anchorMax = new Vector2(0.5f, 0.12f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(220f, 58f);
        }
        if (privacyScrollRect != null)
        {
            if (privacyMessageText != null && privacyMessageText.transform.parent != null)
            {
                var content = privacyMessageText.transform.parent.GetComponent<RectTransform>();
                if (content != null)
                {
                    privacyScrollRect.content = content;
                    privacyScrollRect.vertical = true;
                    privacyScrollRect.horizontal = false;
                }
            }
            privacyScrollRect.verticalNormalizedPosition = 1f;
        }
        if (privacyMessageText != null)
        {
            ApplyExpandedPrivacyPolicy(language == Language.Korean);
            privacyMessageText.gameObject.SetActive(true);
            privacyMessageText.color = new Color32(214, 222, 238, 255);
            privacyMessageText.rectTransform.anchorMin = new Vector2(0.5f, 1f);
            privacyMessageText.rectTransform.anchorMax = new Vector2(0.5f, 1f);
            privacyMessageText.rectTransform.pivot = new Vector2(0.5f, 1f);
            privacyMessageText.rectTransform.anchoredPosition = Vector2.zero;
            privacyMessageText.rectTransform.sizeDelta = new Vector2(730f, 1540f);
            privacyMessageText.verticalOverflow = VerticalWrapMode.Overflow;
            privacyMessageText.horizontalOverflow = HorizontalWrapMode.Wrap;
            privacyMessageText.resizeTextForBestFit = false;
        }
    }

    private void ClosePrivacyPanel()
    {
        privacyPanel.SetActive(false);
        if (privacyOpenedFromMainMenu)
        {
            if (mainStartButton != null) mainStartButton.gameObject.SetActive(true);
            if (languageOpenButton != null) languageOpenButton.gameObject.SetActive(true);
        }
        SetMainMenuOverlayContentVisible(true);
        privacyOpenedFromMainMenu = false;
        ResetHomeHubMenuState();
        homeHubPanel.SetActive(true);
        ShowLobbyDialogue();
    }

    private void ShowWithdrawConfirm()
    {
        if (playing) return;
        homeHubMenuPanel.SetActive(false);
        homeHubPanel.SetActive(false);
        myPagePanel.SetActive(false);
        privacyPanel.SetActive(false);
        withdrawConfirmPanel.SetActive(true);
    }

    private void CloseWithdrawConfirm()
    {
        withdrawConfirmPanel.SetActive(false);
        if (profileCreated)
        {
            ResetHomeHubMenuState();
            homeHubPanel.SetActive(true);
            ShowLobbyDialogue();
        }
        else
        {
            ShowMainMenu();
        }
    }

    private void ConfirmDeleteAccount()
    {
        withdrawConfirmPanel.SetActive(false);
        DeleteAccount();
    }

    private void ShowLogoutConfirm()
    {
        if (playing) return;
        ResetHomeHubMenuState();
        if (homeHubPanel != null) homeHubPanel.SetActive(false);
        if (logoutConfirmPanel != null) logoutConfirmPanel.SetActive(true);
    }

    private void CloseLogoutConfirm()
    {
        if (logoutConfirmPanel != null) logoutConfirmPanel.SetActive(false);
        if (profileCreated)
        {
            ResetHomeHubMenuState();
            if (homeHubPanel != null) homeHubPanel.SetActive(true);
            ShowLobbyDialogue();
        }
        else
        {
            ShowMainMenu();
        }
    }

    private void ConfirmLogout()
    {
        if (logoutConfirmPanel != null) logoutConfirmPanel.SetActive(false);
        ExecuteLogout();
    }
}

