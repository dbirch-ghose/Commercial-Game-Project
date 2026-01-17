using UnityEngine;
using Fusion;

public class JailCageOpen : NetworkBehaviour
{
    public Animator animator;

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
    }


   

    public void Opencage()
    {
        if (!HasStateAuthority)
        {
            return;
        }

        animator.SetBool("open", true);
    }
}
