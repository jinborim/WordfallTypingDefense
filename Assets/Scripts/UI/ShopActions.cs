using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void UpdateShopText()
    {
        if (shopPanel == null) return;
        shopCoinText.text = language == Language.Korean
            ? $"코인 {coins} · 변경권 {nicknameTickets}"
            : $"COINS {coins} · TICKETS {nicknameTickets}";
        shopStatusText.text = language == Language.Korean
            ? "구매한 상품은 프로필 수정에서 사용할 수 있습니다."
            : "Purchased items can be used in Edit Profile.";
        SetButtonText(nicknameTicketButton, nicknameTickets > 0
            ? (language == Language.Korean ? $"닉네임 변경권 보유 {nicknameTickets}" : $"NICKNAME TICKETS {nicknameTickets}")
            : (language == Language.Korean ? "닉네임 변경권 30코인" : "NICKNAME TICKET 30 COINS"));
        SetButtonText(knightShopButton, OwnsCharacter(0)
            ? (language == Language.Korean ? "기사 프로필 보유" : "KNIGHT PROFILE OWNED")
            : (language == Language.Korean ? "기사 프로필 100코인" : "KNIGHT PROFILE 100 COINS"));
        SetButtonText(mageShopButton, OwnsCharacter(1)
            ? (language == Language.Korean ? "마법사 프로필 보유" : "MAGE PROFILE OWNED")
            : (language == Language.Korean ? "마법사 프로필 100코인" : "MAGE PROFILE 100 COINS"));
        SetButtonText(hackerShopButton, OwnsCharacter(2)
            ? (language == Language.Korean ? "해커 프로필 보유" : "HACKER PROFILE OWNED")
            : (language == Language.Korean ? "해커 프로필 100코인" : "HACKER PROFILE 100 COINS"));
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
    }

    private void BuyCharacter(int characterIndex)
    {
        if (characterIndex < 0 || characterIndex > 2) return;
        if (OwnsCharacter(characterIndex))
        {
            shopStatusText.text = language == Language.Korean ? "이미 보유한 프로필입니다." : "You already own this profile.";
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
    }
}
