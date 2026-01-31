using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundData
{
    public string soundName;
    public AudioClip clip;
    public bool ignoreTimeScale = false;

    [Header("Settings")]
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;

    [Header("Variance (Good for Gameplay SFX)")]
    public bool useRandomVariance = false;
    [Range(0f, 0.5f)] public float randomPitchRange = 0.1f;
    [Range(0f, 0.2f)] public float randomVolumeRange = 0.1f;

    [Header("Routing")]
    public AudioMixerGroup mixerGroup; // Drag SFX or UI group here

    [Header("Saturation Control")]
    [Tooltip("Minimum time between playing this specific sound again.")]
    public float spamThreshold = 0.05f;
    [HideInInspector] public float lastPlayedTime;
}