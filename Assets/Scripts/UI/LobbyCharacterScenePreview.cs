using UnityEngine;

// Scene-editing previews are visible in the editor, but never overlap the
// runtime lobby character when the game is running.
public sealed class LobbyCharacterScenePreview : MonoBehaviour
{
    private void OnEnable()
    {
        if (Application.isPlaying)
            gameObject.SetActive(false);
    }
}
