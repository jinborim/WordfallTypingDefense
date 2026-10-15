using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public sealed partial class WordfallPrototype
{
    private void EnsureInputSystemEventSystem()
    {
        var eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            var eventSystemObject = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            eventSystemObject.transform.SetParent(transform, false);
            return;
        }

        var legacyModule = eventSystem.GetComponent<StandaloneInputModule>();
        if (legacyModule != null) Destroy(legacyModule);
        if (eventSystem.GetComponent<InputSystemUIInputModule>() == null)
        {
            eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
        }
    }

    private void SetButtonText(Button button, string value)
    {
        if (button == null) return;
        var text = button.GetComponentInChildren<Text>();
        if (text != null) text.text = value;
    }

    private void SetLabelText(Text label, string value)
    {
        if (label != null) label.text = value;
    }

    private void UpdateCoinText()
    {
        string value = language == Language.Korean ? "코인 " + coins : "COINS " + coins;
        if (coinText != null) coinText.text = value;
        if (homeHubCoinText != null) homeHubCoinText.text = value;
    }

    private void CheckAttendanceReward()
    {
        if (!profileCreated) return;
        DateTime koreaNow = DateTime.UtcNow.AddHours(9);
        DateTime attendanceDay = koreaNow.Hour < 6 ? koreaNow.Date.AddDays(-1) : koreaNow.Date;
        string attendanceKey = attendanceDay.ToString("yyyy-MM-dd");
        if (playerDataManager.HasAttendance(attendanceKey)) return;

        playerDataManager.GrantAttendance(attendanceKey, 10);
        coins = playerDataManager.Data.coins;
        UpdateCoinText();
        attendancePanel.SetActive(true);
    }
}
