using MoreMountains.Feedbacks;
using UnityEngine;

public class feedbacksScriptTest : MonoBehaviour
{
    public MMF_Player player;
    public bool go = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<MMF_Player>();
        Debug.Log("Found player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (go == true)
        {
            player.Initialization();
            player.PlayFeedbacks();
            Debug.Log("played Shake");
        }
    }
}
