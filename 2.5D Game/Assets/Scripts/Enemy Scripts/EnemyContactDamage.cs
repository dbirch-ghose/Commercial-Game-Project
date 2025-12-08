using Fusion;
using UnityEngine;

public class EnemyContactDamage : NetworkBehaviour
{
    public PlayerHealth playerHealth;
    public int damage = 1;


    private void OnCollisionEnter(Collision collision)
    {
        if (!Object.HasStateAuthority) return; // Only the host runs damage logic

        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>(); //reference to the players health

            playerHealth.TakeDamage(damage);
        } 
    }
}
