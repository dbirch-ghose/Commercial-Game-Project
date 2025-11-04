using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{

    public GameObject player;

    private Scene currentScene;
    private Scene nextScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();

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
            ChangeScene();
        }
    }

    public bool IsCurrentScene(string sceneName) //stores the scene so it can be used in the next function
    {
        return SceneManager.GetActiveScene().name == sceneName; 
    }

  

    public void ChangeScene() //to be tweaked in the future
    {
        if (IsCurrentScene("Scene 2"))
        {
            SceneManager.LoadScene("Scene 3");
        }
    }

}


    





