public sealed partial class WordfallPrototype
{
    private void CloseAttendancePanel()
    {
        attendancePanel.SetActive(false);
    }

    private void ShowInsufficientCoinsPanel()
    {
        insufficientCoinsTitleText.text = language == Language.Korean ? "NOT ENOUGH COINS" : "NOT ENOUGH COINS";
        insufficientCoinsMessageText.text = language == Language.Korean ? "?????諛몃마??????????堉온?????⑥ル??????븐뼐????????????곸죩." : "You do not have enough coins.";
        SetButtonText(insufficientCoinsCloseButton, language == Language.Korean ? "OK" : "OK");
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
        announcementTitleText.text = language == Language.Korean ? "ANNOUNCEMENTS" : "ANNOUNCEMENTS";
        announcementMessageText.text = language == Language.Korean
            ? "??????????源낆쭍?????????댄뱼????汝뷴젆?琉?????????곌떽釉붾???援온???轅붽틓??????\n????????????? ??????⑤９????????????饔낅떽???????????轅붽틓??影?놁쟼???"
            : "Welcome to Wordfall!\nCheck this panel for the latest news.";
        SetButtonText(announcementCloseButton, language == Language.Korean ? "???뗢뵛" : "CLOSE");
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

