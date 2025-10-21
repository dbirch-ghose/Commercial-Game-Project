using UnityEngine;

[System.Serializable]
public class FootstepAudioSettings
{
    [Header("��������")]
    public AudioClip[] footstepClips;
    public AudioClip jumpSound;
    public AudioClip landSound;

    [Header("ʱ�����")]
    public float walkStepInterval = 0.5f;
    public float runStepInterval = 0.3f;
    public float minLandingForce = 2f;

    [Header("��������")]
    [Range(0f, 1f)] public float walkVolume = 0.7f;
    [Range(0f, 1f)] public float runVolume = 0.9f;
    [Range(0f, 1f)] public float minPitch = 0.9f;
    [Range(0f, 1f)] public float maxPitch = 1.1f;
}

public class FootstepAudioManager : MonoBehaviour
{
    public FootstepAudioSettings settings;

    private AudioSource audioSource;
    private Rigidbody rb;
    private bool isGrounded;
    private bool isMoving;
    private bool isRunning;
    private float stepTimer;
    private bool justLanded;
    private float lastLandingForce;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        // ���û��AudioSource���Զ�����
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f; // 3D��Ч
        }
    }

    public void UpdateFootstepState(bool grounded, bool moving, bool running)
    {
        bool wasGrounded = isGrounded;
        isGrounded = grounded;
        isMoving = moving;
        isRunning = running;

        // ������
        if (isGrounded && !wasGrounded && rb != null && rb.linearVelocity.y < 0)
        {
            lastLandingForce = Mathf.Abs(rb.linearVelocity.y);
            justLanded = true;
        }
    }

    public void UpdateFootstepTimer()
    {
        if (!isGrounded)
        {
            stepTimer = 0f;
            return;
        }

        if (isMoving)
        {
            float stepInterval = isRunning ? settings.runStepInterval : settings.walkStepInterval;
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    public void CheckLanding()
    {
        if (justLanded && lastLandingForce >= settings.minLandingForce)
        {
            PlayLandSound();
            justLanded = false;
        }
    }

    public void PlayFootstep()
    {
        if (audioSource == null || settings.footstepClips == null || settings.footstepClips.Length == 0)
            return;

        AudioClip clip = settings.footstepClips[Random.Range(0, settings.footstepClips.Length)];
        float volume = isRunning ? settings.runVolume : settings.walkVolume;
        float pitch = Random.Range(settings.minPitch, settings.maxPitch);

        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip, volume);
    }

    public void PlayJumpSound()
    {
        if (audioSource != null && settings.jumpSound != null)
        {
            audioSource.PlayOneShot(settings.jumpSound);
        }
    }

    public void PlayLandSound()
    {
        if (audioSource != null && settings.landSound != null)
        {
            float volume = Mathf.Clamp(lastLandingForce / 10f, 0.3f, 1f);
            audioSource.PlayOneShot(settings.landSound, volume);
        }
    }
}