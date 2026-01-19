using UnityEngine;
using Fusion;

public class ReceptionTrigger : NetworkBehaviour
{
    public SwitchCameraPosition switchCameraPosition;

    public Transform BarrierPos;
    public GameObject Barrier;

    public Transform CamPos;
    public bool inReception = false;

    public Camera cam;

    private void OnTriggerEnter(Collider other)
    {
        var playerNO = other.GetComponentInParent<NetworkObject>();
        if (playerNO == null)
            return;

        if (!playerNO.HasInputAuthority)
            return;
        switchCameraPosition.MoveCamera(CamPos); //move cam
        switchCameraPosition.ShowRoom("Reception");
        

        inReception = true;
        //Barrier.GetComponent<BoxCollider>().enabled = true;
    }
   
}
