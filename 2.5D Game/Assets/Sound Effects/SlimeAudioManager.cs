using UnityEngine;
using UnityEngine.AI;

public class SlimeAudioManager : MonoBehaviour
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
    public float spatialBlend = 1f;
    public float minDistance = 1f;
    public float maxDistance = 15f;
    public float voiceChance = 0.3f;

    // 组件引用
    private AudioSource audioSource;
    private SlimeBehaviour slimeBehaviour;
    private NavMeshAgent agent;

    // 状态跟踪
    private float footstepTimer;
    private float voiceTimer;
    private float nextVoiceTime;
    private bool wasPatrolling = true;
    private Vector3 lastPosition;
    private bool isMoving = false;
    private bool isAlive = true;
    private float lastHealth;

    void Start()
    {
        // 获取必要的组件
        slimeBehaviour = GetComponent<SlimeBehaviour>();
        agent = GetComponent<NavMeshAgent>();

        // 初始化音频源
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = spatialBlend;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;

        // 初始化状态跟踪
        lastPosition = transform.position;
        lastHealth = GetCurrentHealth();

        // 设置随机语音时间
        nextVoiceTime = Random.Range(3f, 8f);

        // 播放生成音效
        PlaySpawnSound();
    }

    void Update()
    {
        if (!isAlive) return;

        // 更新状态检测
        UpdateStateDetection();

        // 更新音效
        UpdateAudio();
    }

    void UpdateStateDetection()
    {
        // 检测移动状态
        Vector3 currentPosition = transform.position;
        float distanceMoved = Vector3.Distance(currentPosition, lastPosition);
        isMoving = distanceMoved > 0.1f && agent.velocity.magnitude > 0.1f;
        lastPosition = currentPosition;

        // 检测死亡状态
        float currentHealth = GetCurrentHealth();
        if (currentHealth <= 0 && lastHealth > 0)
        {
            Die();
        }
        lastHealth = currentHealth;
    }

    void UpdateAudio()
    {
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

        // 更新随机语音
        voiceTimer += Time.deltaTime;
        if (voiceTimer >= nextVoiceTime)
        {
            if (Random.Range(0f, 1f) < voiceChance)
            {
                PlayRandomVoice();
            }
            voiceTimer = 0f;
            nextVoiceTime = Random.Range(5f, 15f);
        }

        // 检测巡逻状态变化
        bool isPatrolling = IsPatrolling();
        if (isPatrolling != wasPatrolling)
        {
            if (isPatrolling)
            {
                PlayIdleSound();
            }
            else
            {
                PlayAlertSound();
            }
            wasPatrolling = isPatrolling;
        }

        // 检测追逐状态下的随机声音
        if (!isPatrolling && Random.Range(0f, 1f) < 0.02f)
        {
            PlayChaseSound();
        }
    }

    // 通过反射或其他方式获取怪物状态的辅助方法
    float GetCurrentHealth()
    {
        // 方法1: 通过反射获取私有health变量（不推荐，但可行）
        // 方法2: 通过公共方法获取（如果存在）
        // 方法3: 通过组件通信

        // 这里使用方法1，通过反射
        System.Type type = slimeBehaviour.GetType();
        System.Reflection.FieldInfo healthField = type.GetField("health",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (healthField != null)
        {
            return (float)healthField.GetValue(slimeBehaviour);
        }

        // 如果反射失败，默认返回1
        return 1f;
    }

    bool IsPatrolling()
    {
        // 通过反射获取isPatrolling状态
        System.Type type = slimeBehaviour.GetType();
        System.Reflection.FieldInfo patrolField = type.GetField("isPatrolling",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (patrolField != null)
        {
            return (bool)patrolField.GetValue(slimeBehaviour);
        }

        // 如果反射失败，默认返回true
        return true;
    }

    // 通过碰撞检测来检测受伤
    void OnCollisionEnter(Collision collision)
    {
        // 检测是否被武器击中（根据你的游戏设置调整标签）
        if (collision.gameObject.CompareTag("Weapon") ||
            collision.gameObject.CompareTag("Bullet"))
        {
            PlayHitSound();

            // 检查血量是否很低
            float currentHealth = GetCurrentHealth();
            if (currentHealth > 0 && currentHealth <= 0.3f)
            {
                PlayHurtSound();
            }
        }
    }

    // 通过触发器检测伤害区域
    void OnTriggerEnter(Collider other)
    {
        // 检测是否进入伤害区域
        if (other.CompareTag("DamageArea") || other.CompareTag("Trap"))
        {
            PlayHitSound();
        }
    }

    void Die()
    {
        isAlive = false;
        PlayDeathSound();

        // 可选：禁用音频源以避免重复播放
        StartCoroutine(DisableAfterDeath());
    }

    System.Collections.IEnumerator DisableAfterDeath()
    {
        yield return new WaitForSeconds(2f);
        audioSource.enabled = false;
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
        if (deathSound != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(deathSound, 1f);
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

    void PlayHurtSound()
    {
        // 如果没有专门的受伤音效，使用受伤音效
        PlayHitSound();
    }

    void PlaySpawnSound()
    {
        // 如果有生成音效，可以在这里播放
        // 或者使用空闲音效作为替代
        if (idleSound != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(idleSound, 0.4f);
        }
    }

    void PlayRandomVoice()
    {
        // 随机播放空闲或追逐音效
        if (IsPatrolling())
        {
            PlayIdleSound();
        }
        else
        {
            PlayChaseSound();
        }
    }

    // 公共方法，可供其他脚本调用
    public void PlayCustomSound(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(clip, volume);
        }
    }

    public void StopAllSounds()
    {
        audioSource.Stop();
    }
}