using DG.Tweening;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;


public class corpseActivation : NetworkBehaviour
{
    public NetworkObject Hanger;
    public bool activate = false;
    private float counter = 0;
    private Vector3 leftMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (activate == true)
        {
            if (counter < 100 && activate)
            {
                leftMove = Hanger.GetComponent<Rigidbody>().position;
                Debug.Log("Running update");
                leftMove = new Vector3(leftMove.x-0.25f, leftMove.y, leftMove.z);
                Hanger.GetComponent<Rigidbody>().MovePosition(leftMove);

                counter += 1;
            }

        }
    }

    public void activated()
    {
        activate = true;
    }
}
