using UnityEngine;

public class PlayerMelee : MonoBehaviour
{

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;


    //public SlimeBehaviour slimeBehaviour;

    private void Start()
    {
        
    }

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
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers); //creates a hitbox for the weapon

        //apply damage
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log(enemy.name + "has been hit");

            //assigns the script to the hit enemy
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            SlimeBehaviour slimeBehaviour = enemy.GetComponent<SlimeBehaviour>();

            //Damages the enemy
            if (slimeBehaviour != null)
            {
                slimeBehaviour.TakeDamage();
            }
            
        }

        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(attackPoint.position, attackRange);
    }
}
