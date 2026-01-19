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
        playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (basicSpawner == null)
        {
            Debug.LogError("BasicSpawner not found in scene");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only react when a player hits the trigger
        if (!other.CompareTag("Player"))
            return;

        // Get the player's NetworkObject
        var playerNO = other.GetComponentInParent<NetworkObject>();
        if (playerNO == null)
            return;

        // If we're the state authority, we can despawn directly
        if (Object.HasStateAuthority)
        {
            Runner.Despawn(Object); // despawn THIS heart pickup
        }
        else
        {
            // Otherwise ask the server/host to despawn it (if you prefer to centralize despawn logic)
            if (basicSpawner != null)
                basicSpawner.RPC_RequestDestroy(Object);
        }

        // Heal ONLY the player who touched it (input authority)
        if (!playerNO.HasInputAuthority)
            return;

        var health = playerNO.GetComponentInParent<PlayerHealth>();
        if (health == null)
            return;

        health.health += 1; // increment
    }


}
