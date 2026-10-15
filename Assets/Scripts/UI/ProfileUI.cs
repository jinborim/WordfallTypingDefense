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
        profileTitleText.text = language == Language.Korean ? "프로필 수정" : "EDIT PROFILE";
        profilePromptText.text = language == Language.Korean ? "닉네임과 캐릭터를 수정하세요" : "Update your nickname and character";
        nicknameInput.text = playerDataManager.Data.nickname;
        profileErrorText.text = string.Empty;
        SelectCharacter(Mathf.Clamp(playerDataManager.Data.selectedCharacter, 0, 2));
        SetButtonText(createProfileButton, language == Language.Korean ? "저장하기" : "SAVE PROFILE");
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
            profileErrorText.text = language == Language.Korean ? "닉네임을 2글자 이상 입력하세요." : "Enter at least 2 characters.";
            nicknameInput.ActivateInputField();
            return;
        }

        bool nicknameChanged = !profileCreated || nickname != playerDataManager.Data.nickname;
        if (editingProfile && nicknameChanged && nicknameTickets <= 0)
        {
            profileErrorText.text = language == Language.Korean ? "닉네임 변경권이 필요합니다. 상점에서 구매하세요." : "You need a nickname ticket. Buy one in the shop.";
            profilePanel.SetActive(false);
            myPagePanel.SetActive(true);
            ShowShopPanel();
            return;
        }
        if (!OwnsCharacter(selectedCharacter))
        {
            profileErrorText.text = language == Language.Korean ? "구매한 캐릭터 프로필만 선택할 수 있습니다." : "You can only select profiles you own.";
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
            SetButtonText(createProfileButton, language == Language.Korean ? "프로필 만들기" : "CREATE PROFILE");
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
        if (selectedCharacter == 1) return language == Language.Korean ? "마법사" : "MAGE";
        if (selectedCharacter == 2) return language == Language.Korean ? "해커" : "HACKER";
        return language == Language.Korean ? "기사" : "KNIGHT";
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
            ? "플레이어  " + nickname + "  /  " + GetCharacterName()
            : "PLAYER  " + nickname + "  /  " + GetCharacterName();
        if (profileSummaryText != null) profileSummaryText.text = summary;
        if (homeHubPlayerText != null) homeHubPlayerText.text = summary;
    }
}
