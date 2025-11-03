using UnityEngine;

public class SimpleMusicPlayer : MonoBehaviour
{
    [Header("“Ù¿÷…Ë÷√")]
    public AudioClip backgroundMusic;
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip victoryMusic;
    public AudioClip gameOverMusic;

    [Header("“Ù∆µ…Ë÷√")]
    [Range(0f, 1f)] public float musicVolume = 0.7f;
    public bool playOnStart = true;
    public bool loop = true;

    private AudioSource audioSource;
    private AudioClip currentMusic;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ≈‰÷√AudioSource
        audioSource.playOnAwake = false;
        audioSource.loop = loop;
        audioSource.volume = musicVolume;
        audioSource.spatialBlend = 0f; // 2D“Ù¿÷

        if (playOnStart && backgroundMusic != null)
        {
            PlayMusic(backgroundMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;

        currentMusic = clip;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PauseMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }

    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (audioSource != null)
        {
            audioSource.volume = musicVolume;
        }
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    public void FadeIn(AudioClip clip, float duration)
    {
        StartCoroutine(FadeInCoroutine(clip, duration));
    }

    private System.Collections.IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        StopMusic();
        audioSource.volume = startVolume;
    }

    private System.Collections.IEnumerator FadeInCoroutine(AudioClip clip, float duration)
    {
        PlayMusic(clip);
        audioSource.volume = 0f;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, musicVolume, timer / duration);
            yield return null;
        }
    }

    // ±„Ω›∑Ω∑®
    public void PlayMenuMusic() => PlayMusic(menuMusic);
    public void PlayGameMusic() => PlayMusic(gameMusic);
    public void PlayVictoryMusic() => PlayMusic(victoryMusic);
    public void PlayGameOverMusic() => PlayMusic(gameOverMusic);
}