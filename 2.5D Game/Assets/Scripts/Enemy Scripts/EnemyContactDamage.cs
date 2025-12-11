//using Fusion;
//using UnityEngine;

//public class EnemyContactDamage : NetworkBehaviour
//{
//    public PlayerHealth playerHealth;
//    public int damage = 1;


//    void OnCollisionEnter(Collision collision)
//    {

//        if (collision.gameObject.tag == "Player")
//        {
//            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>(); //reference to the players health

//            playerHealth.TakeDamage(damage);
//        } 
//    }
//}
using UnityEngine;
using Fusion;

public class EnemyContactDamage : NetworkBehaviour
{
    public int damage = 1;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerHealth component
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Directly apply damage like the acid vial does
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
