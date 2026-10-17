using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void UpdateShopText()
    {
        if (shopPanel == null) return;
        shopCoinText.text = language == Language.Korean
            ? $"\uCF54\uC778 {coins} \u00B7 \uBCC0\uACBD\uAD8C {nicknameTickets}"
            : $"COINS {coins} \u00B7 TICKETS {nicknameTickets}";
        shopStatusText.text = language == Language.Korean
            ? "\uAD6C\uB9E4\uD55C \uC0C1\uD488\uC740 \uD504\uB85C\uD544 \uC218\uC815\uC5D0\uC11C \uC0AC\uC6A9\uD560 \uC218 \uC788\uC2B5\uB2C8\uB2E4."
            : "Purchased items can be used in Edit Profile.";
        SetButtonText(nicknameTicketButton, nicknameTickets > 0
            ? (language == Language.Korean ? $"\uB2C9\uB124\uC784 \uBCC0\uACBD\uAD8C \uBCF4\uC720 {nicknameTickets}" : $"NICKNAME TICKETS {nicknameTickets}")
            : (language == Language.Korean ? "\uB2C9\uB124\uC784 \uBCC0\uACBD\uAD8C 30\uCF54\uC778" : "NICKNAME TICKET 30 COINS"));
        SetButtonText(knightShopButton, OwnsCharacter(0)
            ? (language == Language.Korean ? "\uAE30\uC0AC \uD504\uB85C\uD544 \uBCF4\uC720" : "KNIGHT PROFILE OWNED")
            : (language == Language.Korean ? "\uAE30\uC0AC \uD504\uB85C\uD544 100\uCF54\uC778" : "KNIGHT PROFILE 100 COINS"));
        SetButtonText(mageShopButton, OwnsCharacter(1)
            ? (language == Language.Korean ? "\uB9C8\uBC95\uC0AC \uD504\uB85C\uD544 \uBCF4\uC720" : "MAGE PROFILE OWNED")
            : (language == Language.Korean ? "\uB9C8\uBC95\uC0AC \uD504\uB85C\uD544 100\uCF54\uC778" : "MAGE PROFILE 100 COINS"));
        SetButtonText(hackerShopButton, OwnsCharacter(2)
            ? (language == Language.Korean ? "\uD574\uCEE4 \uD504\uB85C\uD544 \uBCF4\uC720" : "HACKER PROFILE OWNED")
            : (language == Language.Korean ? "\uD574\uCEE4 \uD504\uB85C\uD544 100\uCF54\uC778" : "HACKER PROFILE 100 COINS"));
        SetShopCategory(shopCategory);
    }

    private void BuyNicknameTicket()
    {
        if (nicknameTickets > 0)
        {
            UpdateShopText();
            return;
        }
        if (coins < 30)
        {
            ShowInsufficientCoinsPanel();
            return;
        }
        coins -= 30;
        nicknameTickets++;
        SaveShopData();
        UpdateCoinText();
        UpdateCharacterButtonStates();
        UpdateShopText();
        ShowPurchaseCompletePanel(language == Language.Korean ? "닉네임 변경권" : "nickname ticket");
    }

    private void BuyCharacter(int characterIndex)
    {
        if (characterIndex < 0 || characterIndex > 2) return;
        if (OwnsCharacter(characterIndex))
        {
            shopStatusText.text = language == Language.Korean ? "\uC774\uBBF8 \uBCF4\uC720\uD55C \uD504\uB85C\uD544\uC785\uB2C8\uB2E4." : "You already own this profile.";
            return;
        }
        if (coins < 100)
        {
            ShowInsufficientCoinsPanel();
            return;
        }
        coins -= 100;
        ownedCharacterMask |= 1 << characterIndex;
        SaveShopData();
        UpdateCoinText();
        UpdateCharacterButtonStates();
        UpdateShopText();
        string characterName = characterIndex == 0
            ? (language == Language.Korean ? "기사 프로필" : "knight profile")
            : characterIndex == 1
                ? (language == Language.Korean ? "마법사 프로필" : "mage profile")
                : (language == Language.Korean ? "해커 프로필" : "hacker profile");
        ShowPurchaseCompletePanel(characterName);
    }
}
