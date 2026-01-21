using Fusion;
using UnityEngine;

public class FightTrigger : NetworkBehaviour
{
    public AkagaulBehaviour akagaulBehaviour;
    public NetworkObject Ack;

    private void OnTriggerEnter(Collider other)
    {
        //RPC_EnableAck();
        if (!Object.HasStateAuthority) return;
        if (!other.CompareTag("Player")) return;


        Debug.Log("collision with trigger wall");
      
        if (akagaulBehaviour != null)
        {
            NetworkObject bossNetObj = akagaulBehaviour.GetComponent<NetworkObject>();
            if (bossNetObj != null && bossNetObj.HasStateAuthority)
            {
                akagaulBehaviour.StartAttackLoop();
                Debug.Log("starting start attack loop");
            }
        }
    

        //network safe destroyAW
        if (Runner != null)
        {
            Runner.Despawn(Object);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_EnableAck()
    {
        RPC_EnableAck2();
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_EnableAck2()
    {
        Ack.enabled = true;
    }
}