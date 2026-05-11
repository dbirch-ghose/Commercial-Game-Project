using UnityEngine;
using Fusion;
using System.Collections;

public class OT2Trigger : NetworkBehaviour
{
    public OTTrigger oTTrigger;
    public SwitchCameraPosition switchCameraPosition;
    public Transform CamPos1;
    public Transform CamPos2;
    public bool inOT2 = false;

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

        if (oTTrigger.inOT == true)
        {
            switchCameraPosition.MoveCamera(CamPos1);
            inOT2 = true;
            string[] rooms = { "Operating Theatre", "Enemy" };
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

