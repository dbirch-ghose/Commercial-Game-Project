using UnityEngine;
using Fusion;
using System.Collections;

public class JailTrigger : NetworkBehaviour
{
    public InfirmaryTrigger infirmaryTrigger  ;
    public CameraBehaviour cameraBehaviour;
    public Transform CamPos1; //infirm
    public Transform CamPos2; //recepetion
    public bool inInfirmary = false;

    public float cooldown = 0.5f;
    private bool onCooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority)
        {
            //destroy barrirer
        }

        if (onCooldown)
            return;

        StartCoroutine(Cooldown());

        if (other.gameObject.CompareTag("Player"))
        {
            if (infirmaryTrigger.inInfirmary == true)
            {
                cameraBehaviour.MoveCamera(CamPos1); //move cam
                infirmaryTrigger.inInfirmary = false;
                inInfirmary = true;
            }
            else
            {
                cameraBehaviour.MoveCamera(CamPos2); //move cam
                infirmaryTrigger.inInfirmary = true;
            }
        }
    }
    private IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}

