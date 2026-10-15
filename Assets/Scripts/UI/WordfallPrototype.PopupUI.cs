public sealed partial class WordfallPrototype
{
    private void CloseAttendancePanel()
    {
        attendancePanel.SetActive(false);
    }

    private void ShowInsufficientCoinsPanel()
    {
        insufficientCoinsTitleText.text = language == Language.Korean ? "코인 부족" : "NOT ENOUGH COINS";
        insufficientCoinsMessageText.text = language == Language.Korean ? "코인이 부족합니다." : "You do not have enough coins.";
        SetButtonText(insufficientCoinsCloseButton, language == Language.Korean ? "확인" : "OK");
        insufficientCoinsPanel.SetActive(true);
    }

    private void CloseInsufficientCoinsPanel()
    {
        insufficientCoinsPanel.SetActive(false);
    }

    private void ShowAnnouncementPanel()
    {
        if (playing) return;
        homeHubPanel.SetActive(false);
        announcementTitleText.text = language == Language.Korean ? "공지사항" : "ANNOUNCEMENTS";
        announcementMessageText.text = language == Language.Korean
            ? "워드폴에 오신 것을 환영합니다!\n새로운 소식은 공지사항에서 확인하세요."
            : "Welcome to Wordfall!\nCheck this panel for the latest news.";
        SetButtonText(announcementCloseButton, language == Language.Korean ? "닫기" : "CLOSE");
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
