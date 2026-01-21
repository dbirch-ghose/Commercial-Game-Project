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
        var playerNO = other.GetComponentInParent<NetworkObject>();
        if (playerNO == null)
            return;

        if (!playerNO.HasInputAuthority)
            return;

        if (onCooldown)
            return;

        StartCoroutine(Cooldown());

        
            if (receptionTrigger.inReception == true)
            {
                switchCameraPosition.MoveCamera(CamPos1); //move cam
                receptionTrigger.inReception = false;
                inLibrary = true;
            string[] rooms = { "Library" };
            switchCameraPosition.ShowRoom(rooms);
        }
            else
            {
                switchCameraPosition.MoveCamera(CamPos2); //move cam
                receptionTrigger.inReception = true;
            string[] rooms = { "Reception" };
            switchCameraPosition.ShowRoom(rooms);
        }
        
    }
    private IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}

