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
        public string version;
        public string message;
        public string downloadUrl;
    }

    public event Action<UpdateInfo> UpdateAvailable;
    public string manifestUrl = DefaultManifestUrl;

    private void Start()
    {
        StartCoroutine(CheckForUpdate());
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
