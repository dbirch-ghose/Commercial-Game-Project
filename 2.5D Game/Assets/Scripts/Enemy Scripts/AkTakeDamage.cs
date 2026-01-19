using Fusion;
using UnityEngine;

public class AkTakeDamage : NetworkBehaviour
{
    public AkagaulBehaviour akBehaviour;
    [Networked] public int Health { get; set; }
    [Networked] private bool IsDead { get; set; }
    private bool _spawned;
    private SpriteRenderer spriteRenderer;
    [Networked] private TickTimer damageFlashTimer { get; set; }

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
        if (!_spawned || spriteRenderer == null)
            return;

        // Visual flash logic (runs on all clients)
        bool flashing = damageFlashTimer.IsRunning && !damageFlashTimer.Expired(Runner);
        spriteRenderer.color = flashing ? Color.red : Color.white;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(int damage)
    {
        if (IsDead)
            return;

        Health -= damage;
        Debug.Log($"Enemy took {damage} damage. Health now {Health}");
    }
}
