using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

public class cageOpen : NetworkBehaviour
{
    public GameObject cage;
    private bool pressable = false;

    InputAction fireAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireAction = InputSystem.actions.FindAction("Fire1");
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Button Pressed");
            }
            if (pressable)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Button Pressed and activated");
                }
            }
        }

        if (pressable == true && fireAction.IsPressed())
        {
            Debug.Log("button pressed");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Player"))
        {
            pressable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (CompareTag("Player"))
        {
            pressable = false;
        }
    }
}
