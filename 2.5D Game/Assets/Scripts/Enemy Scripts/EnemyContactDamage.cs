using UnityEngine;
using Fusion;

public class EnemyContactDamage : NetworkBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority)
            return;

        if (!other.CompareTag("Player"))
            return;

        // Get the PlayerHealth component on the root player object
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogWarning("PlayerHealth not found on " + other.name);
            return;
        }

        // Ask the player to take damage via RPC
        playerHealth.RPC_TakeDamage(damage);
    }
}
