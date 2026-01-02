using UnityEngine;
using Fusion;
public class JailTrigger: NetworkBehaviour
{
    //public ReceptionTrigger receptionTrigger;


    public SwitchCameraPosition switchCameraPosition;
    //public GameObject FrontWall;
    //public NetworkPrefabRef barrier1;
    //public NetworkPrefabRef barrier2;

    public Transform CamPos;

    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority)
        {
        }

        if (other.gameObject.CompareTag("Player"))
        {
            //if (receptionTrigger.inReception == true)
            //{
            //    switchCameraPosition.MoveCamera(CamPos); //move cam

            //    receptionTrigger.inReception = false;
            //}
            switchCameraPosition.MoveCamera(CamPos); //move cam
        }
    }
    
}

