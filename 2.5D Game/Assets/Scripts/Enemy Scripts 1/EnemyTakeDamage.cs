using Unity.VisualScripting;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    public int health; //health variable to be changed for each enemies in inspector

    private void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage()
    {
        health -= 1;
        Debug.Log( "this enemy has been hit");
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
