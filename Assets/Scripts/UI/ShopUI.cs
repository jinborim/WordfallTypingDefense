using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private void ShowShopPanel()
    {
        if (!profileCreated) return;
        shopOpenedFromMyPage = true;
        myPagePanel.SetActive(false);
        UpdateShopText();
        shopPanel.SetActive(true);
    }

    private void ShowShopFromHome()
    {
        if (!profileCreated) return;
        shopOpenedFromMyPage = false;
        homeHubPanel.SetActive(false);
        UpdateShopText();
        shopPanel.SetActive(true);
    }

    private void CloseShopPanel()
    {
        shopPanel.SetActive(false);
        if (shopOpenedFromMyPage)
        {
            myPagePanel.SetActive(true);
        }
        else
        {
            ResetHomeHubMenuState();
            homeHubPanel.SetActive(true);
            ShowLobbyDialogue();
        }
        shopOpenedFromMyPage = false;
    }

    private void SetShopCategory(int category)
    {
        shopCategory = Mathf.Clamp(category, 0, 2);
        if (shopTicketCard == null) return;

        bool showProfiles = shopCategory == 0 || shopCategory == 1;
        bool showItems = shopCategory == 0 || shopCategory == 2;
        shopTicketCard.SetActive(showItems);
        shopTicketImage.gameObject.SetActive(showItems);
        nicknameTicketButton.gameObject.SetActive(showItems);
        shopKnightCard.SetActive(showProfiles);
        shopMageCard.SetActive(showProfiles);
        shopHackerCard.SetActive(showProfiles);
        shopKnightImage.gameObject.SetActive(showProfiles);
        shopMageImage.gameObject.SetActive(showProfiles);
        shopHackerImage.gameObject.SetActive(showProfiles);
        knightShopButton.gameObject.SetActive(showProfiles);
        mageShopButton.gameObject.SetActive(showProfiles);
        hackerShopButton.gameObject.SetActive(showProfiles);

        SetCategoryButtonColor(shopAllCategoryButton, shopCategory == 0);
        SetCategoryButtonColor(shopProfileCategoryButton, shopCategory == 1);
        SetCategoryButtonColor(shopItemCategoryButton, shopCategory == 2);
        if (shopScrollRect != null) shopScrollRect.horizontalNormalizedPosition = 0f;
    }

    private void SetCategoryButtonColor(Button button, bool selected)
    {
        if (button == null) return;
        var colors = button.colors;
        colors.normalColor = selected ? new Color32(67, 149, 255, 255) : new Color32(58, 80, 122, 255);
        colors.highlightedColor = new Color32(116, 190, 255, 255);
        colors.pressedColor = new Color32(216, 173, 68, 255);
        button.colors = colors;
    }

    private bool OwnsCharacter(int characterIndex)
    {
        return (ownedCharacterMask & (1 << characterIndex)) != 0;
    }

    private void SaveShopData()
    {
        playerDataManager.SaveShopData(coins, nicknameTickets, ownedCharacterMask);
    }
}
