using UnityEngine;
using Fusion;

public class SpecimenController : NetworkBehaviour
{
    public BoulderTrigger boulderTrigger;

    public bool pickup = false;


    public Animator animator;
    public SpriteRenderer sr;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        GameObject boulder = GameObject.FindGameObjectWithTag("Boulder");
        boulderTrigger = boulder.GetComponent<BoulderTrigger>();
    }


    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority && boulderTrigger != null && boulderTrigger.inRange)
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
