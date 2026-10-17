using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public sealed class SceneButtonBridge : MonoBehaviour
{
    [SerializeField] private string actionName;

    private void Start()
    {
        var button = GetComponent<Button>();
        if (button == null) return;
        button.onClick.AddListener(InvokeAction);
    }

    private void InvokeAction()
    {
        var runtimeObject = GameObject.Find("WordfallPrototypeRuntime");
        if (runtimeObject == null) return;
        var prototype = runtimeObject.GetComponent<WordfallPrototype>();
        if (prototype == null) return;
        var method = typeof(WordfallPrototype).GetMethod(actionName, BindingFlags.Instance | BindingFlags.NonPublic);
        if (method != null) method.Invoke(prototype, null);
    }
}
