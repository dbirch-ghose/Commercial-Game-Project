using UnityEngine;
using Fusion;
using UnityEngine.Rendering;

public class PickupHeart : NetworkBehaviour
{
    public BasicSpawner basicSpawner;
    public PlayerHealth playerHealth;
    public override void Spawned()
    {
        basicSpawner = FindFirstObjectByType<BasicSpawner>();

        if (basicSpawner == null)
        {
            Debug.LogError("BasicSpawner not found in scene");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority)
            return;

        basicSpawner.RPC_RequestDestroy(GetComponent<NetworkObject>());

        if (!Object.HasInputAuthority)
            return;

        playerHealth.health = +1;
    }


}
