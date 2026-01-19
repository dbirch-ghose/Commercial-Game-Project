using UnityEngine;
using Fusion;
using System.Collections;

public class OTTrigger : NetworkBehaviour
{
    public StorageTrigger storageTrigger;
    public SwitchCameraPosition switchCameraPosition;
    public Transform CamPos1; 
    public Transform CamPos2; 
    public bool inOT = false;

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

            if (storageTrigger.inStorage == true)
            {
                switchCameraPosition.MoveCamera(CamPos1);
                storageTrigger.inStorage= false;
                inOT = true;
                switchCameraPosition.ShowRoom("Operating Theatre"); 
            }
            else
            {
                switchCameraPosition.MoveCamera(CamPos2);
                storageTrigger.inStorage = true;
                switchCameraPosition.ShowRoom("Storage"); 

            }
        
    }
    private IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}

