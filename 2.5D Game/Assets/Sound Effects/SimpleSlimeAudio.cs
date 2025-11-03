using UnityEngine;
using UnityEngine.AI;

public class SimpleSlimeAudio : MonoBehaviour
{
    [Header("音效剪辑")]
    public AudioClip footstepSound;
    public AudioClip hitSound;
    public AudioClip deathSound;
    public AudioClip alertSound;

    [Header("音频参数")]
    public float footstepInterval = 0.8f;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    // 组件引用
    private AudioSource audioSource;
    private NavMeshAgent agent;

    // 状态跟踪
    private float footstepTimer;
    private Vector3 lastPosition;
    private bool isMoving = false;
    private bool isAlive = true;

    void Start()
    {
        // 获取必要的组件
        agent = GetComponent<NavMeshAgent>();

        // 初始化音频源
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 15f;

        // 初始化状态跟踪
        lastPosition = transform.position;
    }

    void Update()
    {
        if (!isAlive) return;

        // 检测移动状态
        Vector3 currentPosition = transform.position;
        float distanceMoved = Vector3.Distance(currentPosition, lastPosition);
        isMoving = distanceMoved > 0.1f && agent.velocity.magnitude > 0.1f;
        lastPosition = currentPosition;

        // 更新脚步声
        if (isMoving)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                PlayFootstep();
                footstepTimer = 0f;
            }
        }
        else
        {
            footstepTimer = 0f;
        }

        // 检测是否死亡（通过检查是否被禁用或销毁中）
        if (gameObject.activeInHierarchy == false)
        {
            PlayDeathSound();
            isAlive = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 简单检测：任何碰撞都可能是受伤
        if (isAlive && collision.relativeVelocity.magnitude > 2f)
        {
            PlayHitSound();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 检测常见伤害触发器
        if (other.CompareTag("DamageArea") ||
            other.CompareTag("Trap") ||
            other.CompareTag("Bullet") ||
            other.CompareTag("Weapon"))
        {
            PlayHitSound();
        }
    }

    void OnDestroy()
    {
        // 对象被销毁时播放死亡音效
        if (isAlive)
        {
            PlayDeathSound();
        }
    }

    // 音效播放方法
    void PlayFootstep()
    {
        if (footstepSound != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(footstepSound, 0.6f);
        }
    }

    void PlayHitSound()
    {
        if (hitSound != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(hitSound, 0.8f);
        }
    }

    void PlayDeathSound()
    {
        if (deathSound != null && isAlive)
        {
            // 创建一个临时的音频源来播放死亡音效，因为原对象可能即将被销毁
            GameObject tempAudio = new GameObject("TempAudio");
            AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
            tempSource.clip = deathSound;
            tempSource.volume = 1f;
            tempSource.pitch = Random.Range(minPitch, maxPitch);
            tempSource.Play();

            // 销毁临时对象
            Destroy(tempAudio, deathSound.length + 0.5f);

            isAlive = false;
        }
    }

    void PlayAlertSound()
    {
        if (alertSound != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(alertSound, 0.7f);
        }
    }
}