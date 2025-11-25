using UnityEngine;
using Fusion;

public class corpseActivation : NetworkBehaviour
{
    public NetworkObject Hanger;
    public bool activate = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (activate == true)
        {

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
