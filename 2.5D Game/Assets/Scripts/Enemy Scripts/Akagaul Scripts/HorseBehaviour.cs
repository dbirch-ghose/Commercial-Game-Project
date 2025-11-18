using UnityEngine;

public class HorseBehaviour : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float leftBound;

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x < leftBound)
            Destroy(gameObject);
    }

}
