using MoreMountains.Feedbacks;
using UnityEngine;

public class feedbacksScriptTest : MonoBehaviour
{
    public MMF_Player player;
    public bool go = false;
    public CameraBehaviour CamBeh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CamBeh = Camera.main.GetComponent<CameraBehaviour>();
        player = FindFirstObjectByType<MMF_Player>();
        Debug.Log("Found player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (go == true)
        {
            CamBeh.enabled = false;
            player.Initialization();
            player.PlayFeedbacks();
            Debug.Log("played Shake");
            go=false;
        }
        else
        {
            CamBeh.enabled=true;
        }
    }
}
