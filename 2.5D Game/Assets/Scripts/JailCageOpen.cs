using UnityEngine;
using Fusion;

public class JailCageOpen : NetworkBehaviour
{
    public Animator animator;
    private bool opened;
    [Networked]
    public bool Open { get; set; }
    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        opened = false;
    }

    public override void Render()
    {
        if (opened == true)
            return;

        animator.SetBool("open", Open);
        opened = Open;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]   
    public void RPC_Opencage()
    {
        if (Open)
            return;
        Open = true;
    }
}
