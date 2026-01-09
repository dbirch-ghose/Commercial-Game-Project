using Fusion;
using UnityEngine;

public class AkTakeDamage : NetworkBehaviour
{
    public AkagaulBehaviour akBehaviour;
    [Networked] public int Health { get; set; }
    [Networked] private bool IsDead { get; set; }

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

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(int damage)
    {
        if (IsDead)
            return;

        Health -= damage;
        Debug.Log($"Enemy took {damage} damage. Health now {Health}");
    }
}
