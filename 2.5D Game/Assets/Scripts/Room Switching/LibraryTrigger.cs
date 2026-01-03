using UnityEngine;
using Fusion;
using System.Collections;

public class LibraryTrigger : NetworkBehaviour
{
    public ReceptionTrigger receptionTrigger;
    public SwitchCameraPosition switchCameraPosition;
    public Transform CamPos1; //infirm
    public Transform CamPos2; //recepetion
    public bool inLibrary = false;

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
                inLibrary = true;
                switchCameraPosition.ShowRoom("Library"); //show infirmary and hide reception
            }
            else
            {
                switchCameraPosition.MoveCamera(CamPos2); //move cam
                receptionTrigger.inReception = true;
                switchCameraPosition.ShowRoom("Reception"); //show reception again

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

