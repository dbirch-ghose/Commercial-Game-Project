using Fusion;
using UnityEngine;

public class FightTrigger : NetworkBehaviour
{
    public AkagaulBehaviour akagaulBehaviour;


    private void OnTriggerEnter(Collider other)
    {
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
}