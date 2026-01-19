using UnityEngine;
using Fusion;
using System.Collections;

public class StairwayTrigger : NetworkBehaviour
{
    public LibraryTrigger libraryTrigger;
    public SwitchCameraPosition switchCameraPosition;
    public Transform CamPos1; 
    public Transform CamPos2; 
    public bool inStairway = false;

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

    
        
            if (libraryTrigger.inLibrary == true)
            {
                switchCameraPosition.MoveCamera(CamPos1); 
                libraryTrigger.inLibrary = false;
                inStairway = true;
                switchCameraPosition.ShowRoom("Stairway"); 
            }
            else
            {
                switchCameraPosition.MoveCamera(CamPos2);
                libraryTrigger.inLibrary = true;
                switchCameraPosition.ShowRoom("Library"); 

            
        }
    }
    private IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}

