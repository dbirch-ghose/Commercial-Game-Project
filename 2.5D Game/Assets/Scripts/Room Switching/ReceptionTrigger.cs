using UnityEngine;
using Fusion;

public class ReceptionTrigger : NetworkBehaviour
{
    public SwitchCameraPosition switchCameraPosition;

    //public GameObject FrontWall;
    public Transform BarrierPos;
    public GameObject Barrier;

    public Transform CamPos;
    public bool inReception = false;

    public Camera cam;

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<NetworkObject>().HasInputAuthority)
        {
            //Runner.Despawn(FrontWall.GetComponent<NetworkObject>());s
            Runner.Spawn(Barrier, BarrierPos.position);
            inReception = true;
        }
        //Destroy(FrontWall.GetComponent<NetworkObject>());
        //Instantiate(Barrier, BarrierPos.position, Quaternion.Euler(0f, 90f, 0f));
        //inReception = true;

        if (other.gameObject.CompareTag("Player"))
        {
            switchCameraPosition.MoveCamera(CamPos); //move cam
            switchCameraPosition.ShowRoom("Reception");

            //Transform barrier1pos = barrier1.transform.position;
            //Runner.Spawn(barrier1, barrier1.position);
        }
    }
    
}

