using Fusion;
using UnityEngine;

public class EnemyTakeDamage : NetworkBehaviour
{
    [Networked] public int Health { get; set; }
    [Networked] private bool IsDead { get; set; }
    [Networked] private TickTimer damageFlashTimer { get; set; }

    private SpriteRenderer spriteRenderer;

    public referencer referenceBlock;

    private bool _spawned;

    public override void Spawned()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        referenceBlock = FindFirstObjectByType<referencer>();

        _spawned = true;
    }

    public override void FixedUpdateNetwork()
    {
        if (!_spawned)
            return;

        // Death logic (state authority only)
        if (Object.HasStateAuthority && !IsDead && Health <= 0)
        {
            IsDead = true;

            if (referenceBlock != null && referenceBlock.heartItem != null)
                Runner.Spawn(referenceBlock.heartItem, transform.position, Quaternion.identity);

            Runner.Despawn(Object);
        }
    }

    public override void Render()
    {
        if (!_spawned || spriteRenderer == null)
            return;

        // Visual flash logic (runs on all clients)
        bool flashing = damageFlashTimer.IsRunning && !damageFlashTimer.Expired(Runner);
        spriteRenderer.color = flashing ? Color.red : Color.white;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(int damage)
    {
        // Prevent early access before Spawned()
        if (!_spawned)
            return;

        if (IsDead)
            return;

        Health -= damage;
        damageFlashTimer = TickTimer.CreateFromSeconds(Runner, 0.1f);
    }
}
