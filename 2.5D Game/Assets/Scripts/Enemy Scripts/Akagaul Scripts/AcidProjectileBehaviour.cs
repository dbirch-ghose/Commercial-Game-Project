using UnityEngine;

public class AcidProjectileBehaviour : MonoBehaviour
{

    public PlayerHealth playerHealth;

    public int damage = 1;
    
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        //instantiate particle effect
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            playerHealth.TakeDamage(damage);
        }
    }
}
