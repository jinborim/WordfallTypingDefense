using System;
using UnityEngine;

public sealed partial class WordfallPrototype
{
    [Serializable]
    private sealed class AnnouncementPayload
    {
        public bool ok;
        public string error;
        public AnnouncementItem[] items;
    }

    [Serializable]
    private sealed class AnnouncementItem
    {
        public string id;
        public string title;
        public string body;
        public string language;
        public bool isActive;
        public long publishedAt;
    }

    private void LoadAnnouncements()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        FirebaseWebAuthBridge.LoadAnnouncements(gameObject.name);
#else
        ApplyFallbackAnnouncement();
#endif
    }

    public void OnFirebaseAnnouncementsResult(string result)
    {
        if (string.IsNullOrEmpty(result))
        {
            ApplyFallbackAnnouncement();
            return;
        }

        try
        {
            var payload = JsonUtility.FromJson<AnnouncementPayload>(result);
            if (payload == null || !payload.ok || payload.items == null)
            {
                ApplyFallbackAnnouncement();
                return;
            }

            var wantedLanguage = language == Language.Korean ? "ko" : "en";
            AnnouncementItem selected = null;
            for (var i = 0; i < payload.items.Length; i++)
            {
                var item = payload.items[i];
                if (item == null || !item.isActive || string.IsNullOrWhiteSpace(item.body)) continue;
                var itemLanguage = (item.language ?? "all").Trim().ToLowerInvariant();
                if (itemLanguage == wantedLanguage)
                {
                    selected = item;
                    break;
                }
                if (selected == null && (itemLanguage == "all" || itemLanguage.Length == 0))
                    selected = item;
            }

            if (selected == null) ApplyFallbackAnnouncement();
            else ApplyAnnouncement(selected.title, selected.body);
        }
        catch (Exception)
        {
            ApplyFallbackAnnouncement();
        }
    }

    private void ApplyAnnouncement(string title, string body)
    {
        if (announcementTitleText != null)
            announcementTitleText.text = string.IsNullOrWhiteSpace(title)
                ? (language == Language.Korean ? "공지사항" : "ANNOUNCEMENTS")
                : title;
        if (announcementMessageText != null)
            announcementMessageText.text = body;
    }

    private void ApplyFallbackAnnouncement()
    {
        ApplyAnnouncement(
            language == Language.Korean ? "공지사항" : "ANNOUNCEMENTS",
            language == Language.Korean
                ? "새로운 소식이 없습니다."
                : "There are no announcements at this time.");
    }
}
