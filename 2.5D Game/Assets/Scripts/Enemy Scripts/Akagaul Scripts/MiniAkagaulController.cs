using UnityEngine;
using Fusion;    

public class MiniAk : NetworkBehaviour
{
    public Animator animator;
    public SpriteRenderer sr;
    private bool isHere = false;

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void EnterRoom()
    {
        if (!isHere)
        {
            Debug.Log("Aki enters");
            animator.SetBool("isEntering", true);
            isHere = true;
        }
        else
        {
            animator.SetBool("isIdle", true);
        }
    }
   
    public void LeaveRoom()
    {
        Debug.Log("Aki leaves");
        animator.SetBool("isLeaving", true);
    }
}
