using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    // Constant key for storing music volume in player preferences
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    // Singleton instance of MusicManager
    public static MusicManager Instance { get; private set; }

    // Reference to the AudioSource component
    private AudioSource audioSource;
    // Variable to store the current volume level
    private float volume = .3f;

    private void Awake()
    {
        // Set the singleton instance to this instance
        Instance = this;
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Load the saved volume from player preferences, defaulting to 0.3f if not set
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
        // Set the AudioSource volume to the loaded volume
        audioSource.volume = volume;
    }

    public void ChangeVolume()
    {
        // Increase the volume by 0.1f
        volume += 0.1f;
        // If the volume exceeds 1f, reset it to 0f
        if (volume > 1f)
        {
            volume = 0f;
        }
        // Update the AudioSource volume
        audioSource.volume = volume;

        // Save the new volume to player preferences
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        // Return the current volume level
        return volume;
    }
}
