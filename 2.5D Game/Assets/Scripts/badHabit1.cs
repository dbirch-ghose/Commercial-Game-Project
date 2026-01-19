using UnityEngine;
using Fusion;

public class badHabit1 : NetworkBehaviour
{
    [Networked]
    public bool down { get; set; }
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

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_FlipSwitch()
    {
        down = !down;
    }
}