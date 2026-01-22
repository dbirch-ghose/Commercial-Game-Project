 using UnityEngine;
using Fusion;
using System.Collections;

public class JailTrigger : NetworkBehaviour
{
    public InfirmaryTrigger infirmaryTrigger  ;
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

        if (onCooldown)
            return;

        StartCoroutine(Cooldown());
        
        if (infirmaryTrigger.inInfirmary == true)
        {
            switchCameraPosition.MoveCamera(CamPos1); //move cam
            infirmaryTrigger.inInfirmary = false;
            inInfirmary = true;
            string[] rooms = { "Jail", "Specimen", "Enemy" };
            switchCameraPosition.ShowRoom(rooms);

        }
        else
        {
            switchCameraPosition.MoveCamera(CamPos2); //move cam
            infirmaryTrigger.inInfirmary = true;
            string[] rooms = { "Infirmary" };
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

