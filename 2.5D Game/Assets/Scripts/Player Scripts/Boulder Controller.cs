using UnityEngine;
using Fusion;

public class BoulderController : NetworkBehaviour
{
    //private bool picked = false;
    //public Animator animator;
    //public override void Spawned()
    //{
    //    animator = GetComponent<Animator>();
    //}

    //[Networked] public bool Open { get; set; }
    //public override void Render()
    //{
    //    if (picked)
    //        return;
    //    //animator.SetBool("isPickingUp", Open);
    //    picked = Open;

    //    if (Open) GetComponent<Collider>().enabled = false;

    //}

    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    //public void RPC_BoulderAnim()
    //{
    //    if (Open)
    //        return;
    //    Open = true;
    //}


}
