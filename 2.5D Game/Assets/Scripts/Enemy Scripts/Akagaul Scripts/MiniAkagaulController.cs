using UnityEngine;
using Fusion;    

public class MiniAk : NetworkBehaviour
{
    public Animator animator;
    public SpriteRenderer sr;

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void EnterRoom()
    {
        Debug.Log("Aki enters");
        animator.SetBool("isEntering", true);
    }
    public void LeaveRoom()
    {
        Debug.Log("Aki leaves");
        animator.SetBool("isLeaving", true);
    }
}
