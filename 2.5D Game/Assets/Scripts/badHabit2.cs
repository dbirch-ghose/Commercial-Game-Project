using UnityEngine;
using Fusion;

public class badHabit2 : NetworkBehaviour
{
    [Networked]
    public bool down { get; set; }
    public GameObject container;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Spawned()
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

