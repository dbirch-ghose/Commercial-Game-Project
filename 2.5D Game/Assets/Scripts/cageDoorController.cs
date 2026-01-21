using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class cageDoorController : NetworkBehaviour
{
    [SerializeField] private Animator animator;

    [Networked]
    public bool DoorOpen { get; set; }
    private bool _lastDoorState;


    private void Start()
    {
        animator=GetComponent<Animator>();
    }
    public override void Render()
    {
        if (_lastDoorState == DoorOpen)
            return;

        animator.SetBool("isOpen", DoorOpen);
        _lastDoorState = DoorOpen;
    }

    // Called by player interaction
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestOpenDoor()
    {
        if (DoorOpen)
            return;

        DoorOpen = true;
    }
}