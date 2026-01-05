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
        animator.SetBool("open", true);
    }
}
