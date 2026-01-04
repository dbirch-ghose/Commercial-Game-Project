using UnityEngine;
using Fusion;

public class SpecimenController : NetworkBehaviour
{
    //public GameObject boulder;

    public Animator animator;
    public SpriteRenderer sr;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        GameObject boulder = GameObject.Find("Big Boulder");

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
            animator.SetBool("isPickingUp", true);
        } 
    }

}
