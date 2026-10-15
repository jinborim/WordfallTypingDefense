using System.Collections.Generic;
using UnityEngine;

public sealed class WordDatabase
{
    private static readonly string[] DefaultKoreanWords = { "사과", "하늘", "컴퓨터", "게임", "방어", "키보드", "단어", "우주" };
    private static readonly string[] DefaultEnglishWords = { "apple", "sky", "computer", "game", "defense", "keyboard", "word", "space" };

    public string[] KoreanWords { get; }
    public string[] EnglishWords { get; }

    public WordDatabase()
    {
        KoreanWords = LoadWords("Words/korean", DefaultKoreanWords);
        EnglishWords = LoadWords("Words/english", DefaultEnglishWords);
    }

    private static string[] LoadWords(string resourcePath, string[] fallback)
    {
        var wordAsset = Resources.Load<TextAsset>(resourcePath);
        if (wordAsset == null) return fallback;

        var lines = wordAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        var words = new List<string>();
        for (int i = 0; i < lines.Length; i++)
        {
            string word = lines[i].Trim();
            if (string.IsNullOrEmpty(word) || word.StartsWith("#")) continue;
            words.Add(word);
        }

        return words.Count > 0 ? words.ToArray() : fallback;
    }
}
