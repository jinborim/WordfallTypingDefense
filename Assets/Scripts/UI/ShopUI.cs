using UnityEngine;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private void ShowShopPanel()
    {
        if (!profileCreated) return;
        shopOpenedFromMyPage = true;
        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        setupPanel.SetActive(false);
        languagePanel.SetActive(false);
        profilePanel.SetActive(false);
        homeHubMenuPanel.SetActive(false);
        homeHubPanel.SetActive(false);
        myPagePanel.SetActive(false);
        SetMainMenuOverlayContentVisible(false);
        if (mainStartButton != null) mainStartButton.gameObject.SetActive(false);
        if (languageOpenButton != null) languageOpenButton.gameObject.SetActive(false);
        ApplyShopLayout();
        UpdateShopText();
        PreparePopupBackgroundOnly(shopPanel);
    }

    private void ShowShopFromHome()
    {
        if (!profileCreated) return;
        shopOpenedFromMyPage = false;
        homeHubPanel.SetActive(false);
        SetMainMenuOverlayContentVisible(false);
        if (mainStartButton != null) mainStartButton.gameObject.SetActive(false);
        if (languageOpenButton != null) languageOpenButton.gameObject.SetActive(false);
        ApplyShopLayout();
        UpdateShopText();
        PreparePopupBackgroundOnly(shopPanel);
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

    private void ApplyShopLayout()
    {
        if (shopTitleText != null)
        {
            SetRect(shopTitleText.rectTransform, new Vector2(0.5f, 0.90f), new Vector2(0.5f, 0.90f), Vector2.zero, new Vector2(900f, 54f));
            shopTitleText.alignment = TextAnchor.MiddleCenter;
        }
        if (shopCoinText != null)
        {
            SetRect(shopCoinText.rectTransform, new Vector2(0.5f, 0.81f), new Vector2(0.5f, 0.81f), Vector2.zero, new Vector2(700f, 34f));
            shopCoinText.alignment = TextAnchor.MiddleCenter;
        }
        if (shopAllCategoryButton != null)
            SetRect(shopAllCategoryButton.GetComponent<RectTransform>(), new Vector2(0.39f, 0.68f), new Vector2(0.39f, 0.68f), Vector2.zero, new Vector2(170f, 46f));
        if (shopProfileCategoryButton != null)
            SetRect(shopProfileCategoryButton.GetComponent<RectTransform>(), new Vector2(0.50f, 0.68f), new Vector2(0.50f, 0.68f), Vector2.zero, new Vector2(170f, 46f));
        if (shopItemCategoryButton != null)
            SetRect(shopItemCategoryButton.GetComponent<RectTransform>(), new Vector2(0.61f, 0.68f), new Vector2(0.61f, 0.68f), Vector2.zero, new Vector2(170f, 46f));
        if (shopStatusText != null)
        {
            SetRect(shopStatusText.rectTransform, new Vector2(0.5f, 0.60f), new Vector2(0.5f, 0.60f), Vector2.zero, new Vector2(900f, 30f));
            shopStatusText.alignment = TextAnchor.MiddleCenter;
            shopStatusText.horizontalOverflow = HorizontalWrapMode.Wrap;
            shopStatusText.verticalOverflow = VerticalWrapMode.Truncate;
            shopStatusText.resizeTextForBestFit = true;
            shopStatusText.resizeTextMinSize = 10;
            shopStatusText.resizeTextMaxSize = 16;
        }
    }

    private void SetShopCategory(int category)
    {
        shopCategory = Mathf.Clamp(category, 0, 2);
        if (shopTicketCard == null) return;

        ApplyShopProductPositions();

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
        if (shopScrollRect != null)
        {
            shopScrollRect.horizontalNormalizedPosition = 0f;
            shopScrollRect.verticalNormalizedPosition = 1f;
        }
    }

    private void ApplyShopProductPositions()
    {
        bool profilesOnly = shopCategory == 1;
        bool itemsOnly = shopCategory == 2;

        SetShopProductPosition(shopTicketCard, itemsOnly ? 0.50f : 0.125f, 0.50f);
        SetShopProductPosition(shopTicketImage, itemsOnly ? 0.50f : 0.125f, 0.72f);
        SetShopProductPosition(nicknameTicketButton != null ? nicknameTicketButton.gameObject : null, itemsOnly ? 0.50f : 0.125f, 0.25f);

        var firstProfileX = profilesOnly ? 0.25f : 0.375f;
        var secondProfileX = profilesOnly ? 0.50f : 0.625f;
        var thirdProfileX = profilesOnly ? 0.75f : 0.875f;
        SetShopProductPosition(shopKnightCard, firstProfileX, 0.50f);
        SetShopProductPosition(shopKnightImage, firstProfileX, 0.72f);
        SetShopProductPosition(knightShopButton != null ? knightShopButton.gameObject : null, firstProfileX, 0.25f);
        SetShopProductPosition(shopMageCard, secondProfileX, 0.50f);
        SetShopProductPosition(shopMageImage, secondProfileX, 0.72f);
        SetShopProductPosition(mageShopButton != null ? mageShopButton.gameObject : null, secondProfileX, 0.25f);
        SetShopProductPosition(shopHackerCard, thirdProfileX, 0.50f);
        SetShopProductPosition(shopHackerImage, thirdProfileX, 0.72f);
        SetShopProductPosition(hackerShopButton != null ? hackerShopButton.gameObject : null, thirdProfileX, 0.25f);
    }

    private void SetShopProductPosition(Graphic graphic, float x, float y)
    {
        if (graphic == null) return;
        SetShopProductPosition(graphic.rectTransform, x, y);
    }

    private void SetShopProductPosition(GameObject target, float x, float y)
    {
        if (target == null) return;
        SetShopProductPosition(target.GetComponent<RectTransform>(), x, y);
    }

    private void SetShopProductPosition(RectTransform rect, float x, float y)
    {
        if (rect == null) return;
        rect.anchorMin = new Vector2(x, y);
        rect.anchorMax = new Vector2(x, y);
        rect.anchoredPosition = Vector2.zero;
        rect.pivot = new Vector2(0.5f, 0.5f);
    }

    private void SetCategoryButtonColor(Button button, bool selected)
    {
        if (button == null) return;
        button.interactable = true;
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
