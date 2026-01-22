using Fusion;
using UnityEngine;

public class AkTakeDamage : NetworkBehaviour
{
    public AkagaulBehaviour akBehaviour;

    [Networked] public int Health { get; set; }
    [Networked] private bool IsDead { get; set; }

    private SpriteRenderer spriteRenderer;

    [Networked] private TickTimer damageFlashTimer { get; set; }

    // Tune this
    private const float FLASH_SECONDS = 0.12f;

    public override void Spawned()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // or GetComponent<SpriteRenderer>()
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority || IsDead)
            return;

        if (Health <= 0)
        {
            IsDead = true;
            akBehaviour.Die();
        }
    }

    public override void Render()
    {
        if (spriteRenderer == null)
            return;

        bool flashing = damageFlashTimer.IsRunning && !damageFlashTimer.Expired(Runner);
        spriteRenderer.color = flashing ? Color.red : Color.white;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(int damage)
    {
        if (IsDead)
            return;

        Health -= damage;

        // Start/refresh flash timer on StateAuthority so it replicates
        damageFlashTimer = TickTimer.CreateFromSeconds(Runner, FLASH_SECONDS);

        Debug.Log($"Enemy took {damage} damage. Health now {Health}");
    }
}
