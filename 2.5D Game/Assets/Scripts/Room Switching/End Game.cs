using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
public class EndGame : MonoBehaviour
{
    public AkagaulBehaviour AB;

    public void Update()
    {
        if (AB.IsDead)
        {
            EndScene();
        }
    }

    private void EndScene()
    {
        SceneManager.LoadScene("menu possibility");
    }
}
