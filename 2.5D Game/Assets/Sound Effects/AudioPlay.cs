using UnityEngine;

public class AudioPlay : MonoBehaviour
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