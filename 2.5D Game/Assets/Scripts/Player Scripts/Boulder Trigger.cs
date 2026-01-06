using UnityEngine;
using Fusion;
public class BoulderTrigger : NetworkBehaviour
{
    public SpecimenController specController;
    public GameObject boulder;

    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HasStateAuthority && other.gameObject.CompareTag("Player"))
        {
            if (specController.pickup)
            {

            }
        }
    }

}
