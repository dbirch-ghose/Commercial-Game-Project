using UnityEngine;

public class HorseBehaviour : MonoBehaviour
{
    public float speed = 15f;
    private Vector3 targetPos;

    public void SetTarget(Vector3 target)
    {
        targetPos = target;
        transform.LookAt(target); // face the right way
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // destroy when it reaches the opposite side
        if (Vector3.Distance(transform.position, targetPos) < 0.5f)
        {
            Destroy(gameObject);
        }
    }
}
