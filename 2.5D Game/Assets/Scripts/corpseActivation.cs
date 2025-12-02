using UnityEngine;
using Fusion;
using DG.Tweening;

public class corpseActivation : NetworkBehaviour
{
    public NetworkObject Hanger;
    public NetworkObject mother;
    public bool activate = false;
    public Animator Swinger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (activate == true)
        {
            Swinger.SetBool("activated", true);
            Swinger.GetComponent<Transform>().position = new Vector3(15, 0, 8);
            mother.GetComponent<Transform>().position = new Vector3(15, 0, 8);
            

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
