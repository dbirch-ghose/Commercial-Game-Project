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
        Debug.Log(opened);
        if (opened == true)
            return;

        Debug.Log("rendering cage anim");
        animator.SetBool("open", Open);
        opened = Open;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]   
    public void RPC_Opencage()
    {
        Debug.Log("open cage RPC called");
        if (Open)
            return;
        Debug.Log("changing open variable for all");
        Open = true;
    }
}
