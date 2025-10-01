using UnityEngine;

public class RoomNavigationBehaviour : MonoBehaviour
{
    public GameObject wall;
    public GameObject player;
    public GameObject newWall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision with player detected!");
            Destroy(wall);
            Instantiate(newWall);
        }
    }
}


