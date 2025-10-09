using UnityEngine;

public class PlayerMelee : MonoBehaviour
{

    public Transform attackPoint;
    public float attackRange = 0.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        //play animation
        //detect enemies
        //apply damage
    }
}
