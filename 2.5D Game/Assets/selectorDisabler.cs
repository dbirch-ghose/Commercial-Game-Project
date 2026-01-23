using UnityEngine;
using Fusion;
using PixelCrushers.DialogueSystem;

public class selectorDisabler : NetworkBehaviour
{
    Selector[] selectors;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Spawned()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        RPC_disableSelector();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_disableSelector()
    {
        selectors = FindObjectsByType<Selector>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if (selectors == null) return;
        foreach (Selector selector in selectors)
        {
            selector.enabled = false;
        }
    }
}
