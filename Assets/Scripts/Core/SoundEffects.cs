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
        lobbyMusic = CreateMusic("Lobby Music", 112f, new[] { 57, 60, 62, 65, 62, 60, 57, 55, 57, 60, 62, 67, 65, 62, 60, 57 });
        gameMusic = CreateMusic("Game Music", 128f, new[] { 45, 45, 48, 50, 45, 45, 52, 50, 45, 45, 48, 53, 50, 48, 45, 43 });

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        bgmSource.volume = 0.22f;
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

    private AudioClip CreateMusic(string name, float bpm, int[] melodyMidi)
    {
        const int sampleRate = 22050;
        const int beats = 16;
        float beatLength = 60f / bpm;
        int sampleCount = Mathf.RoundToInt(sampleRate * beatLength * beats);
        var clip = AudioClip.Create(name, sampleCount, 1, sampleRate, false);
        var samples = new float[sampleCount];
        int[] chordRoots = { melodyMidi[0] - 12, melodyMidi[4] - 12, melodyMidi[8] - 12, melodyMidi[12] - 12 };

        for (int i = 0; i < sampleCount; i++)
        {
            float time = i / (float)sampleRate;
            int beat = Mathf.FloorToInt(time / beatLength);
            float beatTime = time - beat * beatLength;
            int note = melodyMidi[Mathf.Clamp(beat, 0, melodyMidi.Length - 1)];
            float lead = NoteAt(time, note, beatLength, 0.10f, 0.20f, sampleRate);
            int chordRoot = chordRoots[Mathf.Clamp(beat / 4, 0, chordRoots.Length - 1)];
            float pad = NoteAt(time, chordRoot, beatLength * 4f, 0.035f, 0.35f, sampleRate);
            float bass = NoteAt(time, chordRoot - 12, beatLength, 0.06f, 0.18f, sampleRate);
            float kick = beatTime < 0.08f ? Mathf.Sin(2f * Mathf.PI * 90f * beatTime) * (1f - beatTime / 0.08f) * 0.12f : 0f;
            float snare = beat % 2 == 1 && beatTime > beatLength * 0.48f && beatTime < beatLength * 0.58f
                ? Mathf.Sin(2f * Mathf.PI * 1700f * beatTime) * 0.035f : 0f;
            float loopFade = Mathf.Min(1f, (sampleCount - i) / (float)sampleRate * 0.02f);
            samples[i] = (lead + pad + bass + kick + snare) * loopFade;
        }
        clip.SetData(samples, 0);
        return clip;
    }

    private float NoteAt(float time, int midiNote, float duration, float volume, float release, int sampleRate)
    {
        float localTime = time % duration;
        float progress = localTime / duration;
        float frequency = 440f * Mathf.Pow(2f, (midiNote - 69) / 12f);
        float envelope = Mathf.Clamp01(Mathf.Min(progress * 35f, (1f - progress) / Mathf.Max(0.01f, release)));
        float fundamental = Mathf.Sin(2f * Mathf.PI * frequency * localTime);
        float harmonic = Mathf.Sin(2f * Mathf.PI * frequency * 2f * localTime) * 0.22f;
        return (fundamental + harmonic) * volume * envelope;
    }

    private void PlayLobbyMusic()
    {
        PlayMusic(lobbyMusic);
    }

    private void PlayGameMusic()
    {
        PlayMusic(gameMusic);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (bgmSource == null || clip == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;
        bgmSource.clip = clip;
        bgmSource.Play();
    }
}
