using UnityEngine;
using Fusion;
using System.Collections;

public class InfirmaryTrigger: NetworkBehaviour
{
    public ReceptionTrigger receptionTrigger; 
    public SwitchCameraPosition switchCameraPosition; 
    public Transform CamPos1; //infirm
    public Transform CamPos2; //recepetion
    public bool inInfirmary = false;

    public float cooldown = 0.5f;
    private bool onCooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority)
        {
            //destroy barrirer
        }

        if (onCooldown)
            return;

        StartCoroutine(Cooldown());

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
    private IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}

