using UnityEngine;
using UnityEngine.EventSystems;

public sealed partial class WordfallPrototype
{
    private void ShowProfilePanel()
    {
        editingProfile = false;
        profileEditReturnToMyPage = false;
        setupPanel.SetActive(false);
        languagePanel.SetActive(false);
        profilePanel.SetActive(true);
        nicknameInput.text = string.Empty;
        profileErrorText.text = string.Empty;
        SelectCharacter(selectedCharacter);
        EventSystem.current.SetSelectedGameObject(nicknameInput.gameObject);
        nicknameInput.ActivateInputField();
    }

    private void ShowProfileEditFromHome()
    {
        if (!profileCreated) return;
        homeHubMenuPanel.SetActive(false);
        homeHubMenuOpen = false;
        profileEditReturnToMyPage = false;
        profileEditReturnToHomeHub = true;
        ShowProfileEdit();
    }

    private void ShowProfileEdit()
    {
        if (!profileCreated) return;
        editingProfile = true;
        if (!profileEditReturnToHomeHub) profileEditReturnToMyPage = true;
        myPagePanel.SetActive(false);
        setupPanel.SetActive(false);
        languagePanel.SetActive(false);
        profilePanel.SetActive(true);
        profilePromptText.text = language == Language.Korean ? "?꾨줈???섏젙" : "EDIT PROFILE";
        profilePromptText.text = language == Language.Korean ? "TEXT" : "Update your nickname and character";
        nicknameInput.text = playerDataManager.Data.nickname;
        profileErrorText.text = string.Empty;
        SelectCharacter(Mathf.Clamp(playerDataManager.Data.selectedCharacter, 0, 2));
        SetButtonText(createProfileButton, language == Language.Korean ? "SAVE PROFILE" : "SAVE PROFILE");
        EventSystem.current.SetSelectedGameObject(nicknameInput.gameObject);
        nicknameInput.ActivateInputField();
    }

    private void SelectCharacter(int characterIndex)
    {
        selectedCharacter = Mathf.Clamp(characterIndex, 0, characterButtons.Length - 1);
        UpdateCharacterButtonStates();
    }

    private void UpdateCharacterButtonStates()
    {
        if (characterButtons == null) return;
        for (int i = 0; i < characterButtons.Length; i++)
        {
            var colors = characterButtons[i].colors;
            colors.normalColor = i == selectedCharacter ? new Color32(255, 211, 94, 255) : Color.white;
            colors.highlightedColor = new Color32(255, 232, 150, 255);
            colors.pressedColor = new Color32(216, 173, 68, 255);
            characterButtons[i].colors = colors;
            characterButtons[i].interactable = OwnsCharacter(i);
        }
    }

    private void CreateProfile()
    {
        var nickname = nicknameInput.text.Trim();
        if (nickname.Length < 2)
        {
            profileErrorText.text = language == Language.Korean ? "??????怨???????獄쏅챶留?????2?????筌?????????????????쇨덫櫻???轅붽틓??影?놁쟼???" : "Enter at least 2 characters.";
            nicknameInput.ActivateInputField();
            return;
        }

        bool nicknameChanged = !profileCreated || nickname != playerDataManager.Data.nickname;
        if (editingProfile && nicknameChanged && nicknameTickets <= 0)
        {
            profileErrorText.text = language == Language.Korean ? "??????怨?????????곕츥????汝뷴젆?琉??????????獄쏅챶留????轅붽틓?????? ???????살몝??????????傭????轅붽틓??影?놁쟼???" : "You need a nickname ticket. Buy one in the shop.";
            profilePanel.SetActive(false);
            myPagePanel.SetActive(true);
            ShowShopPanel();
            return;
        }
        if (!OwnsCharacter(selectedCharacter))
        {
            profileErrorText.text = language == Language.Korean ? "????傭??????????????獄쏅챶留??貫???????獄쏅챶留????????壤굿??Β??????????????????곸죩." : "You can only select profiles you own.";
            return;
        }

        playerDataManager.SaveProfile(nickname, selectedCharacter);
        profileCreated = true;
        ownedCharacterMask |= 1 << selectedCharacter;
        SaveShopData();
        UpdateProfileSummary();
        profilePanel.SetActive(false);
        if (editingProfile)
        {
            if (nicknameChanged) nicknameTickets--;
            ownedCharacterMask |= 1 << selectedCharacter;
            SaveShopData();
            editingProfile = false;
            SetButtonText(createProfileButton, language == Language.Korean ? "CREATE PROFILE" : "CREATE PROFILE");
            UpdateMyPage();
            if (profileEditReturnToMyPage)
            {
                myPagePanel.SetActive(true);
                profileEditReturnToMyPage = false;
            }
            else if (profileEditReturnToHomeHub)
            {
                profileEditReturnToHomeHub = false;
                UpdateHomeHubCharacter();
                homeHubPanel.SetActive(true);
                ShowLobbyDialogue();
            }
            return;
        }
        ShowSetupPanel();
    }

    private string GetCharacterName()
    {
        if (selectedCharacter == 1) return language == Language.Korean ? "MAGE" : "MAGE";
        if (selectedCharacter == 2) return language == Language.Korean ? "??怨룸Ы" : "HACKER";
        return language == Language.Korean ? "KNIGHT" : "KNIGHT";
    }

    private void UpdateProfileSummary()
    {
        if (!profileCreated)
        {
            if (profileSummaryText != null) profileSummaryText.text = string.Empty;
            if (homeHubPlayerText != null) homeHubPlayerText.text = string.Empty;
            return;
        }

        string nickname = string.IsNullOrEmpty(playerDataManager.Data.nickname) ? "PLAYER" : playerDataManager.Data.nickname;
        string summary = language == Language.Korean
            ? "??????????源낆┸?? " + nickname + "  /  " + GetCharacterName()
            : "PLAYER  " + nickname + "  /  " + GetCharacterName();
        if (profileSummaryText != null) profileSummaryText.text = summary;
        if (homeHubPlayerText != null) homeHubPlayerText.text = summary;
    }
}

