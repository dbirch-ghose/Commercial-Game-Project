using UnityEngine;

public class testingOutStaticVariables : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(UIController.roomCode);
        Debug.Log(UIController.hosting);
    }
}
