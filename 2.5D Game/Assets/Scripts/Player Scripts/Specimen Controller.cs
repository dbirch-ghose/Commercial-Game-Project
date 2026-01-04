using UnityEngine;
using Fusion;

public class SpecimenController : NetworkBehaviour
{
    public GameObject Boulder;

    public Animator animator;
    public SpriteRenderer sr;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

}
