using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public int health;
    public int maxHealth;

    public Image fullHeart;
    public Image twoHeart;
    public Image oneHeart;
    public Image deadHeart;
    public Image hearts;


    private void Start()
    {
        twoHeart.enabled = false;
        oneHeart.enabled = false;
        deadHeart.enabled = false;

    }

    void Update()
    {

        health = playerHealth.health;
        maxHealth = playerHealth.maxHealth;

        fullHeart.enabled = (playerHealth.health == 3);
        twoHeart.enabled = (playerHealth.health == 2);
        oneHeart.enabled = (playerHealth.health == 1);
        deadHeart.enabled = (playerHealth.health <= 0);


        

        //if (playerHealth.health == 3)
        //{
        //    hearts = fullHeart;
        //}
        //else if (playerHealth.health == 2)
        //{
        //    hearts = twoHeart;
        //}
        //else if (playerHealth.health == 1)
        //{
        //    hearts = oneHeart;
        //}
        //else if (playerHealth.health <= 0)
        //{
        //    hearts = deadHeart;
        //}
    }
}
