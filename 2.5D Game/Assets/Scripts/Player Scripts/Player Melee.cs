using System.Collections;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{

    public Transform attackPoint;
    //public GameObject attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    //public float attackDuration = 0.2f;
    //private bool isAttacking = false;

    private void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
            //StartCoroutine(Attack());
        }
    }


    //private IEnumerator Attack()
    //{
    //    isAttacking = true;
    //    GetComponent<Collider>().enabled = true; //enables trigger collider
    //    yield return new WaitForSeconds(attackDuration);    
    //    GetComponent<Collider>().enabled = false; //disables collider   
    //    isAttacking =false;
    //}

    //private void OnTriggerEnter(Collider enemy)
    //{
    //    if (!isAttacking) return;
    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //    SlimeBehaviour slimeBehaviour = enemy.GetComponent<SlimeBehaviour>();
    //    if (slimeBehaviour != null)
    //    {
    //        slimeBehaviour.TakeDamage();
    //    }
    //}



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
