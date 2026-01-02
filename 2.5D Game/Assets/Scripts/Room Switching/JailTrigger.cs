using UnityEngine;
using Fusion;
public class JailTrigger : NetworkBehaviour
{
    public InfirmaryTrigger infirmaryTrigger;
    public SwitchCameraPosition switchCameraPosition;
    public Transform CamPos1;
    public Transform CamPos2;

    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority)
        {
            //destroy barrirer
        }

        if (other.gameObject.CompareTag("Player"))
        {
            if (infirmaryTrigger.inInfirmary == true)
            {
                switchCameraPosition.MoveCamera(CamPos1); //move cam to jail
                infirmaryTrigger.inInfirmary = false;
            }
            else
            {
                switchCameraPosition.MoveCamera(CamPos2); //move cam back to infirmary
                infirmaryTrigger.inInfirmary = true;
            }
        }
    }

}

