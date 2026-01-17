using JetBrains.Annotations;
using UnityEngine;
using Fusion;

public class BadHabit3 : NetworkBehaviour
{
    public bool down;
    public GameObject container;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        down = false;
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (down)
        {
            container.transform.eulerAngles = new Vector3(180f, 180f, 0f);
        }
        else
        {
            container.transform.eulerAngles = new Vector3(0f, 180f, 0f);

        }
    }
}
