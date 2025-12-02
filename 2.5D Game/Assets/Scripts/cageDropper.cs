using UnityEngine;

public class cageDropper : MonoBehaviour
{
    public GameObject cage;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnExectute()
    {
        cage.GetComponent<Rigidbody>().useGravity = true;
    }
}
