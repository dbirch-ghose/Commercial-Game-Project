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
        var playerNO = other.GetComponentInParent<NetworkObject>();
        if (playerNO == null)
            return;

        if (!playerNO.HasInputAuthority)
            return;
        if (onCooldown) return;

        StartCoroutine(Cooldown());

        if (other.gameObject.CompareTag("Player"))
        {
            if (receptionTrigger.inReception == true)
            {
                switchCameraPosition.MoveCamera(CamPos1); //move cam
                receptionTrigger.inReception = false;
                inInfirmary = true;
                switchCameraPosition.ShowRoom("Infirmary"); //show infirmary and hide reception
            }
            else
            {
                switchCameraPosition.MoveCamera(CamPos2); //move cam
                receptionTrigger.inReception = true;
                switchCameraPosition.ShowRoom("Reception"); //show reception again

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    private IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}

