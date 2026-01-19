using Fusion;
using UnityEngine;

public class EnemyTakeDamage : NetworkBehaviour
{
    [Networked] public int Health { get; set; }
    [Networked] private bool IsDead { get; set; }

    // Networked timer for damage flash
    [Networked] private TickTimer damageFlashTimer { get; set; }

    private SpriteRenderer spriteRenderer;

    public override void Spawned()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void FixedUpdateNetwork()
    {
        // Death logic (state authority only)
        if (Object.HasStateAuthority && !IsDead && Health <= 0)
        {
            IsDead = true;
            Runner.Despawn(Object);
        }

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

        damageFlashTimer = TickTimer.CreateFromSeconds(Runner, 0.1f);
    }
}

//using Fusion;
//using UnityEngine;

//public class EnemyTakeDamage : NetworkBehaviour
//{
//    [Networked] public int Health { get; set; }
//    [Networked] private bool IsDead { get; set; }

//    public override void FixedUpdateNetwork()
//    {
//        if (!Object.HasStateAuthority || IsDead)
//            return;

//        if (Health <= 0)
//        {
//            IsDead = true;
//            Runner.Despawn(Object);
//        }
//    }

//    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
//    public void RPC_TakeDamage(int damage)
//    {
//        if (IsDead)
//            return;

//        Health -= damage;
//        Debug.Log($"Enemy took {damage} damage. Health now {Health}");
//    }
//}
