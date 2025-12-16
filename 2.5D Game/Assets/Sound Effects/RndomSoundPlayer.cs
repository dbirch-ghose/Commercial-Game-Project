using UnityEngine;

using System.Collections;



public class RandomSoundPlayer : MonoBehaviour

{

    [SerializeField] private AudioSource audioSource;

    // 存放所有可选的随机音效，在Inspector面板拖入 

    [SerializeField] private AudioClip[] soundClips;

    // 随机播放的最小/最大间隔（单位：秒） 

    [SerializeField] private float minInterval = 2f;

    [SerializeField] private float maxInterval = 8f;



    void Start()

    {

        // 开始协程，执行随机播放逻辑 

        StartCoroutine(PlayRandomSoundCoroutine());

    }



    // 控制随机播放的协程 

    private IEnumerator PlayRandomSoundCoroutine()

    {

        while (true)

        {

            // 随机生成下一次播放的间隔时间 

            float waitTime = Random.Range(minInterval, maxInterval);

            // 等待对应时长 

            yield return new WaitForSeconds(waitTime);



            // 随机选择一段音效播放 

            if (soundClips != null && soundClips.Length > 0 && audioSource != null)

            {

                int randomIndex = Random.Range(0, soundClips.Length);

                AudioClip randomClip = soundClips[randomIndex];

                audioSource.PlayOneShot(randomClip);

            }

        }

    }



    // 可以外部调用这个方法，停止随机播放 

    public void StopRandomSound()

    {

        StopCoroutine(PlayRandomSoundCoroutine());

    }

}