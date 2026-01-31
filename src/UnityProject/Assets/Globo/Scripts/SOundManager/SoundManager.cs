using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Setup")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSourcePrefab;

    [Header("The Library")]
    public List<SoundData> soundLibrary;
    private Dictionary<string, SoundData> libraryDict = new Dictionary<string, SoundData>();

    [Header("Pooling")]
    private List<AudioSource> sfxPool = new List<AudioSource>();
    [SerializeField] private int initialPoolSize = 20;

    private float originalMusicVol;
    private float originalMusicPitch;

    // Track the fade coroutine so we can stop it if a new level loads fast
    private Coroutine musicFadeRoutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeLibrary();
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (musicSource != null)
        {
            musicSource.pitch = originalMusicPitch;
            musicSource.volume = originalMusicVol;
        }
    }

    private void InitializeLibrary()
    {
        libraryDict.Clear();
        foreach (var s in soundLibrary)
        {
            if (!libraryDict.ContainsKey(s.soundName))
            {
                libraryDict.Add(s.soundName, s);
            }
        }
    }

    private void InitializePool()
    {
        if (musicSource != null)
        {
            musicSource.gameObject.SetActive(true);
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewSource();
        }
    }

    private AudioSource CreateNewSource()
    {
        AudioSource newSource = Instantiate(sfxSourcePrefab, transform);
        newSource.gameObject.SetActive(false);
        sfxPool.Add(newSource);
        return newSource;
    }


    public void PlaySound(string name)
    {
        if (!libraryDict.TryGetValue(name, out SoundData data))
        {
            Debug.LogWarning($"Sound {name} not found in library!");
            return;
        }

        if (Time.realtimeSinceStartup - data.lastPlayedTime < data.spamThreshold) return;
        data.lastPlayedTime = Time.realtimeSinceStartup;

        AudioSource source = GetAvailableSource();
        if (source == null) return; // Extra safety

        source.clip = data.clip;
        source.outputAudioMixerGroup = data.mixerGroup;
        source.loop = false;

        float finalPitch = data.pitch;
        float finalVol = data.volume;

        if (data.useRandomVariance)
        {
            finalPitch += Random.Range(-data.randomPitchRange, data.randomPitchRange);
            finalVol += Random.Range(-data.randomVolumeRange, data.randomVolumeRange);
        }

        source.pitch = finalPitch;
        source.volume = finalVol;

        source.gameObject.SetActive(true);
        source.Play();

        // Pass the clip length now so the coroutine doesn't have to access source.clip later
        StartCoroutine(DisableSourceAfterPlay(source, source.clip != null ? source.clip.length : 0f));
    }

    private IEnumerator DisableSourceAfterPlay(AudioSource source, float clipLength)
    {
        // 1. Calculate delay immediately while we know the source/pitch is valid
        // Avoid division by zero if pitch is somehow 0
        float pitchAbs = Mathf.Abs(source.pitch);
        float delay = (pitchAbs > 0.001f) ? (clipLength / pitchAbs) : clipLength;

        // 2. Wait using Realtime so it works during pause/slow-mo
        yield return new WaitForSecondsRealtime(delay + 0.1f);

        // 3. Robust Null Check before disabling
        // If the scene changed or the object was destroyed, this prevents the NullRef
        if (source != null && source.gameObject != null)
        {
            source.Stop();
            source.gameObject.SetActive(false);
        }
    }

    // --- NEW: Play Looping Sound (Required for PlayerScript) ---
    public AudioSource PlayLoopingSound(string name)
    {
        if (!libraryDict.TryGetValue(name, out SoundData data))
        {
            Debug.LogWarning($"Looping Sound {name} not found!");
            return null;
        }

        AudioSource source = GetAvailableSource();

        source.clip = data.clip;
        source.outputAudioMixerGroup = data.mixerGroup;
        source.loop = true; // IMPORTANT
        source.pitch = data.pitch;
        source.volume = data.volume;

        source.gameObject.SetActive(true);
        source.Play();

 
        return source;
    }

    public void PlayMusicWithFade(string name, float transitionDuration = 1.0f)
    {
        if (!libraryDict.TryGetValue(name, out SoundData data))
        {
            Debug.LogWarning($"Music '{name}' not found!");
            return;
        }

        // If the same song is already playing, do nothing
        if (musicSource.isPlaying && musicSource.clip == data.clip)
        {
            if (musicFadeRoutine == null) musicSource.volume = data.volume;
            return;
        }

        if (musicFadeRoutine != null) StopCoroutine(musicFadeRoutine);

        musicFadeRoutine = StartCoroutine(FadeMusicRoutine(data, transitionDuration));
    }

    private IEnumerator FadeMusicRoutine(SoundData nextTrack, float duration)
    {
        float halfDuration = duration / 2f;

        // FADE OUT
        if (musicSource.isPlaying)
        {
            float startVol = musicSource.volume;
            float t = 0;
            while (t < halfDuration)
            {
                t += Time.unscaledDeltaTime;
                musicSource.volume = Mathf.Lerp(startVol, 0f, t / halfDuration);
                yield return null;
            }
        }

        musicSource.Stop();
        musicSource.volume = 0f;

        // SWAP
        musicSource.clip = nextTrack.clip;
        musicSource.outputAudioMixerGroup = nextTrack.mixerGroup;
        musicSource.pitch = nextTrack.pitch;

        originalMusicVol = nextTrack.volume;
        originalMusicPitch = nextTrack.pitch;

        musicSource.Play();

        // FADE IN
        float fadeInTime = 0;
        while (fadeInTime < halfDuration)
        {
            fadeInTime += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(0f, originalMusicVol, fadeInTime / halfDuration);
            yield return null;
        }

        musicSource.volume = originalMusicVol;
        musicFadeRoutine = null;
    }

    // --- Helpers ---

    private AudioSource GetAvailableSource()
    {
        foreach (var source in sfxPool)
        {
            if (!source.gameObject.activeInHierarchy) return source;
        }
        return CreateNewSource();
    }


    public void ResetMusicToOriginal()
    {
        musicSource.UnPause();
        musicSource.volume = originalMusicVol > 0 ? originalMusicVol : 1f;
        musicSource.pitch = originalMusicPitch > 0 ? originalMusicPitch : 1f;
    }


    public List<SoundData> GetLibrary() => soundLibrary;
}