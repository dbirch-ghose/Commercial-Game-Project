using UnityEngine;
using Fusion;

public class BoulderController : NetworkBehaviour
{
    public Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void BoulderAnim()
    {
        animator.SetBool("isPickingUp", true);
        GetComponent<Collider>().enabled = false;
    }


}
