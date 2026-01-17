using UnityEngine;
using Fusion;
using System.Collections;

public class StorageTrigger : NetworkBehaviour
{
    public StudyTrigger studyTrigger;
    public SwitchCameraPosition switchCameraPosition;
    public Transform CamPos1; 
    public Transform CamPos2; 
    public bool inStorage = false;

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
            if (studyTrigger.inStudy == true)
            {
                switchCameraPosition.MoveCamera(CamPos1);
                studyTrigger.inStudy = false;
                inStorage = true;
                switchCameraPosition.ShowRoom("Storage"); 
            }
            else
            {
                switchCameraPosition.MoveCamera(CamPos2);
                studyTrigger.inStudy = true;
                switchCameraPosition.ShowRoom("Study"); 

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

