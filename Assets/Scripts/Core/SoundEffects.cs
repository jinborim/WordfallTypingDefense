using UnityEngine;

public sealed partial class WordfallPrototype
{
    private enum SoundEffect
    {
        UiClick,
        Correct,
        Wrong,
        Success,
        GameOver
    }

    private void InitializeSoundEffects()
    {
        sfxSource = gameObject.GetComponent<AudioSource>();
        if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.volume = 0.55f;

        uiClickSound = CreateTone("UI Click", 660f, 0.055f, 0.18f, false);
        correctSound = CreateTone("Correct", 880f, 0.10f, 0.22f, false);
        wrongSound = CreateTone("Wrong", 150f, 0.16f, 0.25f, true);
        successSound = CreateTone("Success", 1046f, 0.24f, 0.22f, false);
        gameOverSound = CreateTone("Game Over", 220f, 0.42f, 0.24f, true);
    }

    private AudioClip CreateTone(string name, float frequency, float duration, float volume, bool descending)
    {
        const int sampleRate = 44100;
        int sampleCount = Mathf.Max(1, Mathf.RoundToInt(sampleRate * duration));
        var clip = AudioClip.Create(name, sampleCount, 1, sampleRate, false);
        var samples = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            float progress = i / (float)sampleCount;
            float currentFrequency = descending ? frequency * (1f - progress * 0.55f) : frequency + progress * frequency * 0.12f;
            float envelope = Mathf.Clamp01(Mathf.Min(progress * 18f, (1f - progress) * 12f));
            samples[i] = Mathf.Sin(2f * Mathf.PI * currentFrequency * i / sampleRate) * volume * envelope;
        }
        clip.SetData(samples, 0);
        return clip;
    }

    private void PlaySoundEffect(SoundEffect effect)
    {
        if (sfxSource == null) return;
        AudioClip clip = effect == SoundEffect.UiClick ? uiClickSound
            : effect == SoundEffect.Correct ? correctSound
            : effect == SoundEffect.Wrong ? wrongSound
            : effect == SoundEffect.Success ? successSound
            : gameOverSound;
        if (clip != null) sfxSource.PlayOneShot(clip);
    }
}
