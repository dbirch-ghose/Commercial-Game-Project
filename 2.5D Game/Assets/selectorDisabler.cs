using UnityEngine;
using Fusion;
using PixelCrushers.DialogueSystem;

public class selectorDisabler : NetworkBehaviour
{
    private Selector[] selectors;
    private bool done;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Spawned()
    {
        done = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (done) return;
        if (!Object.HasStateAuthority) return;
        RPC_DisableSelectorsForEveryone();
        done = true;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_DisableSelectorsForEveryone()
    {
        DisableLocalPlayerSelectors();
    }

    void DisableLocalPlayerSelectors()
    {
        // Get all player objects on THIS client
        var players = FindObjectsOfType<NetworkObject>();

        foreach (var netObj in players)
        {
            Selector selector = netObj.GetComponent<Selector>();
            if (selector != null)
            {
                selector.enabled = false;
            }
        }
    }
    
}
