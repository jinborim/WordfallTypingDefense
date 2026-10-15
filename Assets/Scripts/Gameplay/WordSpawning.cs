using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void SpawnWord()
    {
        bool useKorean = gameMode == GameMode.Korean || (gameMode != GameMode.English && Random.value > 0.5f);
        string[] source = useKorean ? koreanWords : englishWords;
        bool isBoss = wave > 1 && wave % 5 == 0 && spawnedThisWave == 0;
        string word = string.Empty;
        for (int attempt = 0; attempt < source.Length * 3; attempt++)
        {
            string candidate = source[Random.Range(0, source.Length)];
            if (isBoss) candidate += useKorean ? source[Random.Range(0, source.Length)] : " " + source[Random.Range(0, source.Length)];
            if (candidate == lastSpawnedWord || IsWordAlreadyFalling(candidate)) continue;
            word = candidate;
            break;
        }

        if (string.IsNullOrEmpty(word)) return;
        lastSpawnedWord = word;
        var type = isBoss ? WordType.Boss : GetRandomWordType();
        var text = CreateText(word, wordLayer, 30, GetWordColor(type, useKorean), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
        var rect = text.rectTransform;
        rect.sizeDelta = new Vector2(190f, 55f);
        rect.anchoredPosition = GetNonOverlappingSpawnPosition();
        fallingWords.Add(new FallingWord(word, type, rect, text.gameObject, (type == WordType.Boss ? GetMinimumWordSpeed() * 0.8f : Random.Range(GetMinimumWordSpeed(), GetMaximumWordSpeed())) + score * 0.03f));
        spawnedThisWave++;
        if (spawnedThisWave >= 10)
        {
            wave++;
            spawnedThisWave = 0;
            statusText.text = language == Language.Korean ? "웨이브 " + wave + " 도착!" : "WAVE " + wave + " INCOMING!";
            UpdateHud();
        }
    }

    private bool IsWordAlreadyFalling(string word)
    {
        for (int i = 0; i < fallingWords.Count; i++)
        {
            if (fallingWords[i].word == word) return true;
        }

        return false;
    }

    private Vector2 GetNonOverlappingSpawnPosition()
    {
        for (int attempt = 0; attempt < 40; attempt++)
        {
            var candidate = new Vector2(Random.Range(-500f, 500f), Random.Range(250f, 330f));
            bool overlaps = false;
            for (int i = 0; i < fallingWords.Count; i++)
            {
                var existing = fallingWords[i].rect.anchoredPosition;
                if (Mathf.Abs(candidate.x - existing.x) < 205f && Mathf.Abs(candidate.y - existing.y) < 72f)
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps) return candidate;
        }

        return new Vector2(Random.Range(-500f, 500f), 330f);
    }
}
