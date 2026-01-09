using UnityEngine;
using Fusion;

public class BoarCharge : NetworkBehaviour
{
    [Header("Charge")]
    public float chargeDuration = 0.25f;
    public float chargeCooldown = 2f;

    [Networked] public bool IsCharging { get; private set; }
    [Networked] private TickTimer chargeTimer { get; set; }
    [Networked] private TickTimer cooldownTimer { get; set; }

    [Header("Contact Damage")]
    public int damage = 1;
    public float hitCooldown = 0.5f;
    [Networked] private TickTimer hitTimer { get; set; }

    public Animator animator;

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
    }

    public bool TryStartCharge()
    {
        if (!Object.HasStateAuthority) return false;
        if (IsCharging) return false;
        if (cooldownTimer.IsRunning) return false;

        IsCharging = true;
        chargeTimer = TickTimer.CreateFromSeconds(Runner, chargeDuration);
        cooldownTimer = TickTimer.CreateFromSeconds(Runner, chargeCooldown);

        animator.SetBool("isCharging", true);
        return true;
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;

        if (IsCharging && chargeTimer.Expired(Runner))
        {
            IsCharging = false;
            animator.SetBool("isCharging", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Object.HasStateAuthority) return;
        if (!IsCharging) return;
        if (!collision.gameObject.CompareTag("Enemy")) return;
        if (hitTimer.IsRunning) return;

        var etd = collision.collider.GetComponentInParent<EnemyTakeDamage>();
        if (etd == null) return;

        etd.RPC_TakeDamage(damage);
        hitTimer = TickTimer.CreateFromSeconds(Runner, hitCooldown);
    }
}
