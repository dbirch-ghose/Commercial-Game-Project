using UnityEngine;
using Fusion;

public class TempJailDoorTrigger : NetworkBehaviour
{
    public JailCageOpen jailCage;

    private void OnTriggerEnter(Collider other)
    {
        if (HasStateAuthority)
        {
            jailCage.Opencage();
        }
    }
}
