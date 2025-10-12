using UnityEngine;

public class SlimeBehaviour : MonoBehaviour
{
    public float health = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage()
    {
        health -= 1;
    }
}
