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

    private void EnterRoom()
    {

    }


}
