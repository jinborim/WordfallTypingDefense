using System.Collections;
using UnityEngine.EventSystems;

public sealed partial class WordfallPrototype
{
    private void TogglePause()
    {
        if (!playing) return;
        paused = !paused;
        pausePanel.SetActive(paused);
        if (paused) inputField.DeactivateInputField();
        else RefocusInputField();
    }

    private void RefocusInputField()
    {
        if (!playing) return;
        StartCoroutine(RefocusInputFieldNextFrame());
    }

    private IEnumerator RefocusInputFieldNextFrame()
    {
        yield return null;
        if (!playing || inputField == null) yield break;
        EventSystem.current.SetSelectedGameObject(inputField.gameObject);
        inputField.ActivateInputField();
        inputField.caretPosition = inputField.text.Length;
    }
}
