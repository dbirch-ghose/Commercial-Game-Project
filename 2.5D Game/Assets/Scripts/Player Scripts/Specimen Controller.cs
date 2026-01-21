using UnityEngine;
using Fusion;

public class SpecimenController : NetworkBehaviour
{
    public BoulderTrigger boulderTrigger;
    public bool pickup = false;
    public bool specActive;
    public Animator animator;
    public SpriteRenderer sr;

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        //    GameObject boulder = GameObject.FindGameObjectWithTag("Boulder");
        //    boulderTrigger = boulder.GetComponent<BoulderTrigger>();
        //    specActive = true;
    }
    //public override void FixedUpdateNetwork()
    //{
    //    if (HasInputAuthority && boulderTrigger != null && boulderTrigger.inRange)
    //    {
    //        PickUpBoulder();
    //    }        
    //}
    //private void PickUpBoulder()
    //{
    //    if (HasInputAuthority && Input.GetKeyDown(KeyCode.Space))
    //    {
    //        pickup = true;
    //        animator.SetBool("isPickingUp", true);
    //        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 1.5f);
    //    }
    //}
}
