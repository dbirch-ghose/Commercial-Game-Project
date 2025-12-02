using UnityEngine;
using Fusion;

public class corpseActivation : NetworkBehaviour
{
    public NetworkObject Hanger;
    public bool activate = false;

    public Transform cubeStart;
    public Transform cubeEnd;
    private float framesElapsed = 0;
    private bool start = false;
    public float totalFrames = 60;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (activate == true)
        {
            if (!start)
            {
                framesElapsed = 0;
                start = true;
            }
            float ratio = framesElapsed / totalFrames;
            if (ratio > 1)
            {
                ratio = 1;
            }
            Hanger.transform.position=Vector3.Lerp(cubeStart.position, cubeEnd.position, ratio);
            framesElapsed += 1;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activate = true;
        }
    }
}
