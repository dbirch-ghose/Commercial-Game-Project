using UnityEngine;
using Fusion;
public class HorseBehaviour : NetworkBehaviour
{
    [SerializeField] float moveSpeed = 30f;
    [SerializeField] float leftBound = -20f;
    [SerializeField] float rightBound = 20f;

    public SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    private int moveDir = -1; //-1 for left, 1 for right

    public void SetDirection(int direction)
    {
        moveDir = direction;
    }

    public override void FixedUpdateNetwork()
    {

        if (!Object.HasStateAuthority) return;

        //sets move direction based on whether it's positive or negative, which is generated in the coroutine in akagaulBehaviour
        transform.position += Vector3.right * moveDir * moveSpeed * Time.deltaTime; 

        if (moveDir > 0)
        {
            sr.flipX = true;
        }

        //if (transform.position.x <= leftBound)
        if (moveDir == -1 & transform.position.x <= leftBound) //destroys horse if its moving left and past the boundary
        {
            Destroy(gameObject);
        }
        if (moveDir == 1 & transform.position.x >= rightBound) 
        {
            Destroy(gameObject);
        }

    }

}
