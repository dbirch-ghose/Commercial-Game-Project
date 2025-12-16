using UnityEngine;
using Fusion;

public class EnemyContactDamage : NetworkBehaviour
{
    public int damage = 1;
    public float hitCooldown = 2f;

    private Collider hitCollider;

    // Local lock to stop same-tick multiple triggers
    private bool localHitLock;

    // Networked cooldown
    [Networked] private TickTimer hitCooldownTimer { get; set; }

    public override void Spawned()
    {
        hitCollider = GetComponent<Collider>();
        hitCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority)
            return;

        // Already hit this tick? skip
        if (localHitLock)
            return;

        // Only trigger on collider with "Player" tag
        if (!other.CompareTag("Player"))
            return;

        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null)
            return;

        // Lock immediately to prevent multiple triggers in same tick
        localHitLock = true;

        // Deal damage
        playerHealth.RPC_TakeDamage(damage);

        // Start cooldown
        hitCollider.enabled = false;
        hitCooldownTimer = TickTimer.CreateFromSeconds(Runner, hitCooldown);
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority)
            return;

        // Cooldown finished
        if (hitCooldownTimer.IsRunning && hitCooldownTimer.Expired(Runner))
        {
            localHitLock = false;
            hitCollider.enabled = true;
            hitCooldownTimer = default;
        }
    }
}
