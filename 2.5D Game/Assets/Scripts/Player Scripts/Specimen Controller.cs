using UnityEngine;
using Fusion;

public class SpecimenController : NetworkBehaviour
{
    public bool pickup = false;


    public Animator animator;
    public SpriteRenderer sr;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();


    }


    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            PickUpBoulder();
        }
        
    }

    private void PickUpBoulder()
    {
        if (HasStateAuthority && Input.GetKeyDown(KeyCode.Space))
        {
            pickup = true;
            animator.SetBool("isPickingUp", true);
        } 
    }

}
