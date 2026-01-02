using UnityEngine;
using Fusion;
using TMPro.Examples;
public class ReceptionTrigger : NetworkBehaviour
{
    public SwitchCameraPosition switchCameraPosition;
    public GameObject FrontWall;
    public Transform CamPos;
    public bool inReception = false;

    public Camera cam;

    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority)
        {
            Runner.Despawn(FrontWall.GetComponent<NetworkObject>());
            inReception = true;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            switchCameraPosition.MoveCamera(CamPos); //move cam
            //switchCameraPosition.ShowRoom(LayerMask.NameToLayer("Reception"));

            //Transform barrier1pos = barrier1.transform.position;
            //Runner.Spawn(barrier1, barrier1.position);
        }
    }
    
}

