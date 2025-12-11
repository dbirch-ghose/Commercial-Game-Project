using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class HealthDisplay : MonoBehaviour
{
    
    public PlayerHealth playerHealth;

    public Image fullHeart;
    public Image twoHeart;
    public Image oneHeart;
    public Image deadHeart;


    private int lastHealth = 3;

    private void Start()
    {
        // Disable all hearts at the start
        fullHeart.enabled = false;
        twoHeart.enabled = false;
        oneHeart.enabled = false;
        deadHeart.enabled = false;

        // Show the correct heart based on current health
        UpdateHearts(playerHealth.health);
        lastHealth = playerHealth.health;
    }

    private void Update()
    {
        if (!playerHealth.Object.HasInputAuthority) return; 


        // Only update when health changes
        if (playerHealth.health != lastHealth)
        {
            UpdateHearts(playerHealth.health);
            lastHealth = playerHealth.health;
        }
    }

    private void UpdateHearts(int health)
    {
        fullHeart.enabled = (health == 3);
        twoHeart.enabled = (health == 2);
        oneHeart.enabled = (health == 1);
        deadHeart.enabled = (health <= 0);
    }
}
