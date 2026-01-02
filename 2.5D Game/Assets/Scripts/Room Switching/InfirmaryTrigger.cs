using UnityEngine;
using Fusion;
public class InfirmaryTrigger: NetworkBehaviour
{
    public ReceptionTrigger receptionTrigger; 
    public SwitchCameraPosition switchCameraPosition; 
    public Transform CamPos1;
    public Transform CamPos2;
    public bool inInfirmary = false;


    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority)
        {
            //destroy barrirer
        }

        if (other.gameObject.CompareTag("Player"))
        {
            if (receptionTrigger.inReception == true)
            {
                switchCameraPosition.MoveCamera(CamPos1); //move cam
                receptionTrigger.inReception = false;
                inInfirmary = true;
            }
            else
            {
                switchCameraPosition.MoveCamera(CamPos2); //move cam
                receptionTrigger.inReception = true;
            }
        }
    }
    
}

