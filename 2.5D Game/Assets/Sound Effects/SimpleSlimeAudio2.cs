using UnityEngine;
using UnityEngine.AI;

public class SimpleSlimeAudio2 : MonoBehaviour
{
    [Header("音效剪辑")]
    public AudioClip footstepSound;
    public AudioClip hitSound;
    public AudioClip deathSound;
    public AudioClip alertSound;
    public AudioClip idleSound;
    public AudioClip chaseSound;

    [Header("音频参数")]
    public float footstepInterval = 0.8f;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    public float voiceIntervalMin = 3f;
    public float voiceIntervalMax = 10f;
    public float chaseVoiceChance = 0.02f; // 追逐时播放声音的几率

    // 组件引用
    private AudioSource audioSource;
    private NavMeshAgent agent;
    private Transform player;

    // 状态跟踪
    private float footstepTimer;
    private float voiceTimer;
    private float nextVoiceTime;
    private Vector3 lastPosition;
    private bool isMoving = false;
    private bool isAlive = true;
    private bool wasChasing = false;
    private float distanceToPlayer;

    void Start()
    {
        // 获取必要的组件
        agent = GetComponent<NavMeshAgent>();

        // 查找玩家
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // 初始化音频源
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 15f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;

        // 初始化状态跟踪
        lastPosition = transform.position;

        // 设置随机语音时间
        nextVoiceTime = Random.Range(voiceIntervalMin, voiceIntervalMax);

        // 播放生成音效
        PlaySpawnSound();
    }

    void Update()
    {
        if (!isAlive) return;

        // 更新玩家距离
        if (player != null)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.position);
        }

        // 检测移动状态
        Vector3 currentPosition = transform.position;
        float distanceMoved = Vector3.Distance(currentPosition, lastPosition);
        isMoving = distanceMoved > 0.1f && agent.velocity.magnitude > 0.1f;
        lastPosition = currentPosition;

        // 更新脚步声
        UpdateFootsteps();

        // 更新状态音效
        UpdateStateAudio();

        // 更新随机语音
        UpdateRandomVoices();

        // 检测是否死亡（通过检查是否被禁用或销毁中）
        if (gameObject.activeInHierarchy == false && isAlive)
        {
            PlayDeathSound();
            isAlive = false;
        }
    }

    void UpdateFootsteps()
    {
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
    }

    void UpdateStateAudio()
    {
        // 检测是否在追逐玩家
        bool isChasing = player != null && distanceToPlayer <= 5f;

        // 状态变化时播放相应音效
        if (isChasing && !wasChasing)
        {
            // 开始追逐
            PlayAlertSound();
        }
        else if (!isChasing && wasChasing)
        {
            // 停止追逐
            PlayIdleSound();
        }

        wasChasing = isChasing;

        // 追逐时随机播放追逐音效
        if (isChasing && Random.Range(0f, 1f) < chaseVoiceChance)
        {
            PlayChaseSound();
        }
    }

    void UpdateRandomVoices()
    {
        // 不在追逐状态时播放随机语音
        if (player != null && distanceToPlayer > 5f)
        {
            voiceTimer += Time.deltaTime;
            if (voiceTimer >= nextVoiceTime)
            {
                if (Random.Range(0f, 1f) < 0.4f) // 40%几率播放空闲声音
                {
                    PlayIdleSound();
                }
                voiceTimer = 0f;
                nextVoiceTime = Random.Range(voiceIntervalMin, voiceIntervalMax);
            }
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
            tempAudio.transform.position = transform.position;
            AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
            tempSource.spatialBlend = 1f;
            tempSource.minDistance = 1f;
            tempSource.maxDistance = 15f;
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

    void PlayIdleSound()
    {
        if (idleSound != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(idleSound, 0.5f);
        }
    }

    void PlayChaseSound()
    {
        if (chaseSound != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(chaseSound, 0.8f);
        }
    }

    void PlaySpawnSound()
    {
        // 使用空闲音效作为生成音效
        if (idleSound != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(idleSound, 0.4f);
        }
    }
}