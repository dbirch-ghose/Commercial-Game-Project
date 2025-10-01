using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
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
        if (health < minHealth)
        {
            health = minHealth;
        }
    }

    public void TakeDamage(int damage) //to be referenced in enemy damage scripts
    {
        health -= damage; //decreases health based on damage
    }

}
