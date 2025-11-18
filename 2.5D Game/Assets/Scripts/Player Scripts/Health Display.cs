using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public int health;
    public int maxHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts; //unity UI


    void Start()
    {

    }

    // Update is called once per frame
    void Update() { }
    //{
    //    health = playerHealth.health;
    //    maxHealth = playerHealth.maxHealth;

    //    for (int i = 0; i < hearts.Length; i++)
    //    {
    //        if (i < health)
    //        {
    //            hearts[i].sprite = fullHeart;
    //        }
    //        else 
    //        {
    //            hearts[i].sprite = emptyHeart;
    //        }

    //        if (i < maxHealth)
    //        {
    //            hearts[i].enabled = true; //turns on each heart in the UI
    //        }
    //        else
    //        {
    //            hearts[i].enabled = false; //turns off hearts that shouldn't be active
    //        }
    //    }
    //}
}
