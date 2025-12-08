using Fusion;
using UnityEngine;
 
public class PlayerHealth : NetworkBehaviour
{
    [Networked] public int health { get; set; } = 0;
    public int maxHealth = 3;
    public int minHealth = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth; //sets health at the start of the game
    }

    // Update is called once per frame
    void Update()
    {
        if (Object.HasStateAuthority)
        {
            if (health < minHealth)
            {
                health = minHealth;
            }
        }
    }

    public void TakeDamage(int damage) //to be referenced in enemy damage scripts
    {
        if (!Object.HasStateAuthority) return; 

        health -= damage; //decreases health based on damage
    }

    public void Downed()
    {
        if (health >= 0)
        {
            //set down state
        }
    }

}
