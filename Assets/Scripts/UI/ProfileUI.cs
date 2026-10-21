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
        ResetHomeHubMenuState();
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
        profilePromptText.text = language == Language.Korean ? "\uD504\uB85C\uD544 \uC218\uC815" : "EDIT PROFILE";
        profilePromptText.text = language == Language.Korean ? "\uB2C9\uB124\uC784\uACFC \uCE90\uB9AD\uD130\uB97C \uC218\uC815\uD558\uC138\uC694" : "Update your nickname and character";
        nicknameInput.text = playerDataManager.Data.nickname;
        profileErrorText.text = string.Empty;
        SelectCharacter(Mathf.Clamp(playerDataManager.Data.selectedCharacter, 0, 2));
        SetButtonText(createProfileButton, language == Language.Korean ? "\uD504\uB85C\uD544 \uC800\uC7A5" : "SAVE PROFILE");
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
            characterButtons[i].interactable = !profileCreated || OwnsCharacter(i);
        }
    }

    private void CreateProfile()
    {
        var nickname = nicknameInput.text.Trim();
        if (nickname.Length < 2)
        {
            profileErrorText.text = language == Language.Korean ? "\uB2C9\uB124\uC784\uC740 2\uAE00\uC790 \uC774\uC0C1 \uC785\uB825\uD558\uC138\uC694." : "Enter at least 2 characters.";
            nicknameInput.ActivateInputField();
            return;
        }

        bool nicknameChanged = !profileCreated || nickname != playerDataManager.Data.nickname;
        if (editingProfile && nicknameChanged && nicknameTickets <= 0)
        {
            profileErrorText.text = string.Empty;
            ShowNicknameTicketPanel();
            return;
        }
        if (profileCreated && !OwnsCharacter(selectedCharacter))
        {
            profileErrorText.text = language == Language.Korean ? "\uBCF4\uC720\uD558\uC9C0 \uC54A\uC740 \uCE90\uB9AD\uD130\uB294 \uC120\uD0DD\uD560 \uC218 \uC5C6\uC2B5\uB2C8\uB2E4." : "You can only select profiles you own.";
            return;
        }

        if (nicknameChanged && nicknameCheckApprovedFor != nickname)
        {
            RequestNicknameCheck(nickname);
            return;
        }

        playerDataManager.SaveProfile(nickname, selectedCharacter);
        RegisterLocalNickname(nickname);
        nicknameCheckApprovedFor = string.Empty;
        profileCreated = true;
        ownedCharacterMask |= 1 << selectedCharacter;
        SaveShopData();
        UpdateCharacterButtonStates();
        UpdateProfileSummary();
        profilePanel.SetActive(false);
        if (editingProfile)
        {
            if (nicknameChanged) nicknameTickets--;
            ownedCharacterMask |= 1 << selectedCharacter;
            SaveShopData();
            editingProfile = false;
            SetButtonText(createProfileButton, language == Language.Korean ? "\uD504\uB85C\uD544 \uC0DD\uC131" : "CREATE PROFILE");
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
        ShowHomeHub();
    }

    private void RequestNicknameCheck(string nickname)
    {
        if (nicknameCheckInFlight) return;
        nicknameCheckInFlight = true;
        pendingNickname = nickname;
        profileErrorText.text = language == Language.Korean ? "닉네임 중복을 확인하는 중..." : "Checking nickname availability...";
#if UNITY_WEBGL && !UNITY_EDITOR
        var uid = PlayerPrefs.GetString("Wordfall.AuthUid", string.Empty);
        FirebaseWebAuthBridge.CheckNickname(gameObject.name, nickname, playerDataManager.Data.nickname, uid);
#else
        OnNicknameAvailabilityResult(IsLocalNicknameAvailable(nickname) ? "available|" + nickname : "taken|" + nickname);
#endif
    }

    private void OnNicknameAvailabilityResult(string result)
    {
        nicknameCheckInFlight = false;
        var parts = string.IsNullOrEmpty(result) ? new string[0] : result.Split('|');
        var checkedNickname = parts.Length > 1 ? parts[1] : pendingNickname;
        if (parts.Length == 0 || parts[0] == "error")
        {
            profileErrorText.text = language == Language.Korean ? "닉네임 확인에 실패했습니다. 잠시 후 다시 시도해주세요." : "Could not check the nickname. Please try again.";
            return;
        }
        if (parts[0] != "available")
        {
            profileErrorText.text = language == Language.Korean ? "이미 사용 중인 닉네임입니다." : "That nickname is already in use.";
            nicknameCheckApprovedFor = string.Empty;
            return;
        }
        nicknameCheckApprovedFor = checkedNickname;
        CreateProfile();
    }

    private static string LocalNicknameKey(string nickname)
    {
        return "Wordfall.NicknameRegistry." + nickname.Trim().ToLowerInvariant();
    }

    private bool IsLocalNicknameAvailable(string nickname)
    {
        var owner = PlayerPrefs.GetString(LocalNicknameKey(nickname), string.Empty);
        var currentUid = PlayerPrefs.GetString("Wordfall.AuthUid", "local");
        return string.IsNullOrEmpty(owner) || owner == currentUid;
    }

    private void RegisterLocalNickname(string nickname)
    {
        var currentUid = PlayerPrefs.GetString("Wordfall.AuthUid", "local");
        PlayerPrefs.SetString(LocalNicknameKey(nickname), currentUid);
        PlayerPrefs.Save();
    }

    private string GetCharacterName()
    {
        if (selectedCharacter == 1) return language == Language.Korean ? "MAGE" : "MAGE";
        if (selectedCharacter == 2) return language == Language.Korean ? "\uD574\uCEE4" : "HACKER";
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
        string summary = nickname + "  /  " + GetCharacterName();
        if (profileSummaryText != null) profileSummaryText.text = summary;
        if (homeHubPlayerText != null) homeHubPlayerText.text = summary;
    }
}

