using System.Collections;
using UnityEngine;
using Fusion;
public class AcidProjectileBehaviour : NetworkBehaviour
{
    public GameObject particlePrefab;
    private GameObject particles;
    public Vector3 contactPosition;

    public PlayerHealth playerHealth;

    public int damage = 1;
    
    void OnCollisionEnter(Collision collision)
    {
        //instantiate particle effect
        SpawnParticles();
        Destroy(gameObject); //destroys vial
        //glass breaking sound effect

        //damage to player
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            playerHealth.RPC_TakeDamage(1);
        }
    }

    private void SpawnParticles()
    {
        contactPosition = gameObject.transform.position;
        particles = Instantiate(particlePrefab, contactPosition, Quaternion.Euler(70, 0, Random.Range(0f, 360f))); //spawns at contact position with a random Z rotation
        Destroy(particles, 3f);
    }
}
