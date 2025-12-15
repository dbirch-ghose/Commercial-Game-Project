using UnityEngine;

public class MeleeDamage : MonoBehaviour
{

    public PlayerHealth playerHealth;
    private int damage = 1 ;

    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player has been hit by cane");
            playerHealth.RPC_TakeDamage(1);
        }

    }
    public void MeleeAnim()
    {
        animator.SetBool("isHitting", true);
    }
}

