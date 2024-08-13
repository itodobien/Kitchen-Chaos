using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        // Play sound
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}
