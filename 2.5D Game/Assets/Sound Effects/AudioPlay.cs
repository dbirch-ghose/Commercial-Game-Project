using UnityEngine;
using Fusion;

public class AudioPlay : NetworkBehaviour
{
   
    private AudioSource audioSource;

    void Awake()
    {

        audioSource = GetComponent<AudioSource>();
    }

   
    public void PlaySoundEffect(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); 
        }
    }
}