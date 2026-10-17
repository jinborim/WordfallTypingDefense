using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public sealed class SceneUiReferenceEntry
{
    public string key;
    public UnityEngine.Object value;
}

public sealed class SceneUiReferences : MonoBehaviour
{
    [SerializeField] private List<SceneUiReferenceEntry> references = new List<SceneUiReferenceEntry>();

    public void CaptureFrom(WordfallPrototype source)
    {
        references.Clear();
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        foreach (var field in typeof(WordfallPrototype).GetFields(flags))
        {
            if (field.IsStatic) continue;
            var value = field.GetValue(source);
            if (value is UnityEngine.Object objectValue)
            {
                Add(field.Name, objectValue);
                continue;
            }
            if (!(value is Array array)) continue;
            for (var i = 0; i < array.Length; i++)
                if (array.GetValue(i) is UnityEngine.Object arrayValue) Add(field.Name + "[" + i + "]", arrayValue);
        }
    }

    public bool ApplyTo(WordfallPrototype target)
    {
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var applied = 0;
        foreach (var entry in references)
        {
            if (entry == null || string.IsNullOrEmpty(entry.key)) continue;
            var bracket = entry.key.IndexOf('[', StringComparison.Ordinal);
            var fieldName = bracket >= 0 ? entry.key.Substring(0, bracket) : entry.key;
            var field = typeof(WordfallPrototype).GetField(fieldName, flags);
            if (field == null) continue;
            if (bracket < 0)
            {
                field.SetValue(target, entry.value);
                applied++;
                continue;
            }
            var closeBracket = entry.key.IndexOf(']', bracket);
            if (closeBracket < 0 || !int.TryParse(entry.key.Substring(bracket + 1, closeBracket - bracket - 1), out var index)) continue;
            var array = field.GetValue(target) as Array;
            if (array == null || array.Length <= index)
            {
                var expanded = Array.CreateInstance(field.FieldType.GetElementType(), index + 1);
                if (array != null) Array.Copy(array, expanded, array.Length);
                array = expanded;
                field.SetValue(target, array);
            }
            array.SetValue(entry.value, index);
            applied++;
        }
        return applied > 0;
    }

    private void Add(string key, UnityEngine.Object value)
    {
        if (value != null) references.Add(new SceneUiReferenceEntry { key = key, value = value });
    }
}
