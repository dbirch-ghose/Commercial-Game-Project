//using Fusion;
//using UnityEngine;

using UnityEngine;
using Fusion;

public class EnemyContactDamage : NetworkBehaviour
{
    public int damage = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Get the PlayerHealth component
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.RPC_TakeDamage(1);
            }
        }
    }
}
