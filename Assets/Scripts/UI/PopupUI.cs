public sealed partial class WordfallPrototype
{
    private void CloseAttendancePanel()
    {
        attendancePanel.SetActive(false);
    }

    private void ShowInsufficientCoinsPanel()
    {
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

    private void ShowAnnouncementPanel()
    {
        if (playing) return;
        homeHubPanel.SetActive(false);
        announcementTitleText.text = language == Language.Korean ? "\uACF5\uC9C0\uC0AC\uD56D" : "ANNOUNCEMENTS";
        announcementMessageText.text = language == Language.Korean
            ? "\uC0C8\uB85C\uC6B4 \uC18C\uC2DD\uC774 \uC788\uC2B5\uB2C8\uB2E4.\n\uACF5\uC9C0\uC0AC\uD56D\uC744 \uD655\uC778\uD574\uC8FC\uC138\uC694."
            : "Welcome to Wordfall!\nCheck this panel for the latest news.";
        SetButtonText(announcementCloseButton, language == Language.Korean ? "\uB2EB\uAE30" : "CLOSE");
        announcementPanel.SetActive(true);
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
        homeHubMenuPanel.SetActive(false);
        homeHubPanel.SetActive(false);
        privacyPanel.SetActive(true);
    }

    private void ClosePrivacyPanel()
    {
        privacyPanel.SetActive(false);
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
}

