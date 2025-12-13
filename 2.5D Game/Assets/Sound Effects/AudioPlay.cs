using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    // 挂载音频源组件
    private AudioSource audioSource;

    void Awake()
    {
        // 自动获取当前物体的AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    // 动画事件调用的播放方法（直接传音频片段）
    public void PlaySoundEffect(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // 单次播放，不打断其他音频
        }
    }
}