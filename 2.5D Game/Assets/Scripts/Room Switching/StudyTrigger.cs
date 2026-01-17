using UnityEngine;
using Fusion;
using System.Collections;

public class StudyTrigger : NetworkBehaviour
{
    public StairwayTrigger stairwayTrigger;
    public SwitchCameraPosition switchCameraPosition;
    public Transform CamPos1; 
    public Transform CamPos2; 
    public bool inStudy = false;

    public float cooldown = 0.5f;
    private bool onCooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority)
        {
            return;
        }

        if (onCooldown)
            return;

        StartCoroutine(Cooldown());

        if (other.gameObject.CompareTag("Player"))
        {
            if (stairwayTrigger.inStairway == true)
            {
                switchCameraPosition.MoveCamera(CamPos1);
                stairwayTrigger.inStairway = false;
                inStudy = true;
                switchCameraPosition.ShowRoom("Study"); 
            }
            else
            {
                switchCameraPosition.MoveCamera(CamPos2);
                stairwayTrigger.inStairway = true;
                switchCameraPosition.ShowRoom("Stairway"); 

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

