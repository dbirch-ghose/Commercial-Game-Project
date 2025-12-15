using Fusion;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [Networked] public int health { get; set; }
    public int maxHealth = 3;

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
            health = maxHealth;
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority)
            return;

        if (health < 0)
            health = 0;
    }

    // Enemy / server requests damage
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(int damage)
    {
        if (health <= 0)
            return;

        health -= damage;
        Debug.Log($"Player took {damage} damage. Health now {health}");
    }
}
