using UnityEngine;

public class LevelMusicStarter : MonoBehaviour
{
    [Header("Select Music from SoundManager Library")]
    [Tooltip("Type the EXACT name of the sound as it appears in your SoundManager Library.")]
    public string musicName;

    [Tooltip("Time in seconds to crossfade into this track.")]
    public float fadeDuration = 2.0f;

    private void Start()
    {
        // Delay slightly to allow SoundManager to initialize
        Invoke(nameof(ApplyMusic), 0.2f);
    }

    private void ApplyMusic()
    {
        if (SoundManager.Instance != null && !string.IsNullOrEmpty(musicName))
        {
            SoundManager.Instance.PlayMusicWithFade(musicName, fadeDuration);
        }
        else
        {
            Debug.LogWarning("LevelMusicStarter: SoundManager missing or MusicName is empty.");
        }
    }
}