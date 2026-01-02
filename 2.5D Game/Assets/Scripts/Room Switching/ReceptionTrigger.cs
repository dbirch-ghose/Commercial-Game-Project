using UnityEngine;
using Fusion;
public class ReceptionTrigger : NetworkBehaviour
{
    public SwitchCameraPosition switchCameraPosition;
    public GameObject FrontWall;
    public NetworkPrefabRef barrier1;
    public NetworkPrefabRef barrier2;

    public Transform CamPos;

    public bool inReception = false;


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
          
            //Transform barrier1pos = barrier1.transform.position;
            //Runner.Spawn(barrier1, barrier1.position);
        }
    }
    
}

