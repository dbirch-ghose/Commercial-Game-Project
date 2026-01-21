

    using Fusion;
using UnityEngine;

public class OT2Trigger : NetworkBehaviour
{
    public SwitchCameraPosition switchCameraPosition;
    public Transform CamPos;

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority) return;
        if (!other.CompareTag("Player")) return;


        switchCameraPosition.MoveCamera(CamPos);


    }
}

