using UnityEngine;

public static class LegacyRuntimeCleanup
{
    private const string LegacyRuntimeName = "WordfallPrototypeRuntime";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RemoveLegacyRuntime()
    {
        var legacyRuntime = GameObject.Find(LegacyRuntimeName);
        if (legacyRuntime != null)
        {
            Object.Destroy(legacyRuntime);
            Debug.Log("Removed legacy DontDestroyOnLoad runtime.");
        }
    }
}
