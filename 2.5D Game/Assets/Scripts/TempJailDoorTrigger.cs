using UnityEngine;
using Fusion;

public class TempJailDoorTrigger : NetworkBehaviour
{
    public JailCageOpen jailCage;

    private void OnTriggerEnter(Collider other)
    {
        jailCage.RPC_Opencage();
    }
}
