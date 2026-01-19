using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;


public class cageOpen : NetworkBehaviour
{
    public NetworkObject cage;
    public Vector3 upMove;
    public float counter = 10;
    public bool running = false;

    InputAction fireAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Spawned()
    {
        cage.GetComponent<Rigidbody>().useGravity = false;
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (counter < 100 && running)
        {
            Debug.Log("Running update");
            upMove = new Vector3(upMove.x, counter, upMove.z);
            cage.GetComponent<Rigidbody>().MovePosition(upMove);

            counter += 0.01f;
        }
    }

    public void openCage()
    {
        running = true;
    }
}
