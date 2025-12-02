using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int damage = 1;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>(); //reference to the players health

            playerHealth.TakeDamage(damage);
        } 
    }
}
