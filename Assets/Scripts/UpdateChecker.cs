using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public sealed class UpdateChecker : MonoBehaviour
{
    public const string DefaultManifestUrl = "https://raw.githubusercontent.com/jinborim/WordfallTypingDefense/main/version.json";

    [Serializable]
    public sealed class UpdateInfo
    {
        public bool isActive;
        public string version;
        public string message;
        public string messageKo;
        public string messageEn;
        public string downloadUrl;
        public bool forceUpdate;
    }

    public event Action<UpdateInfo> UpdateAvailable;
    public string manifestUrl = DefaultManifestUrl;

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        FirebaseWebAuthBridge.LoadUpdateInfo(gameObject.name);
#else
        StartCoroutine(CheckForUpdate());
#endif
    }

    [Serializable]
    private sealed class FirebaseUpdatePayload
    {
        public bool ok;
        public UpdateInfo update;
    }

    public void OnFirebaseUpdateInfoResult(string result)
    {
        if (string.IsNullOrWhiteSpace(result)) return;

        try
        {
            var payload = JsonUtility.FromJson<FirebaseUpdatePayload>(result);
            if (payload == null || !payload.ok || payload.update == null) return;
            if (!payload.update.isActive) return;
            if (string.IsNullOrWhiteSpace(payload.update.version)) return;
            if (IsNewerVersion(payload.update.version, Application.version))
                UpdateAvailable?.Invoke(payload.update);
        }
        catch (Exception)
        {
            // A malformed remote document must not prevent the game from starting.
        }
    }

    private IEnumerator CheckForUpdate()
    {
        using (var request = UnityWebRequest.Get(manifestUrl))
        {
            request.timeout = 8;
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                yield break;
            }

            UpdateInfo updateInfo;
            try
            {
                updateInfo = JsonUtility.FromJson<UpdateInfo>(request.downloadHandler.text);
            }
            catch (Exception)
            {
                yield break;
            }

            if (updateInfo == null || string.IsNullOrWhiteSpace(updateInfo.version)) yield break;
            if (!IsNewerVersion(updateInfo.version, Application.version)) yield break;
            UpdateAvailable?.Invoke(updateInfo);
        }
    }

    private bool IsNewerVersion(string remoteVersion, string localVersion)
    {
        if (!Version.TryParse(remoteVersion, out var remote)) return false;
        if (!Version.TryParse(localVersion, out var local)) return remoteVersion != localVersion;
        return remote > local;
    }
}
