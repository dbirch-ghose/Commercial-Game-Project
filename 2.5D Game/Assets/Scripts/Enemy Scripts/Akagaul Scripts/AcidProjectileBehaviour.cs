using System.Collections;
using UnityEngine;
using Fusion;

public class AcidProjectileBehaviour : NetworkBehaviour
{
    public GameObject particlePrefab;
    public int damage = 1;

    void OnCollisionEnter(Collision collision)
    {
        // Only the host/StateAuthority should handle collision logic
        if (!Object.HasStateAuthority) return;

        Vector3 contactPosition = gameObject.transform.position;
        float randomRotation = Random.Range(0f, 360f);

        // Spawn particles networked so all clients see them
        RPC_SpawnParticles(contactPosition, randomRotation);

        // Damage to player
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.RPC_TakeDamage(damage);
        }

        // Despawn this projectile across the network
        Runner.Despawn(Object);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_SpawnParticles(Vector3 contactPosition, float randomRotation)
    {
        GameObject particles = Instantiate(
            particlePrefab,
            contactPosition,
            Quaternion.Euler(70, 0, randomRotation)
        );
        Destroy(particles, 3f);
    }
}