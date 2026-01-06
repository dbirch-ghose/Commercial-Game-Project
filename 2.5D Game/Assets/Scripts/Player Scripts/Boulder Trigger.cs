using UnityEngine;
using Fusion;
public class BoulderTrigger : NetworkBehaviour
{
    public SpecimenController specController;
    public BoulderController   boulderController;

    public bool inRange = false;
   

    private void OnTriggerEnter(Collider other)
    {
        if (HasStateAuthority && other.gameObject.CompareTag("Player"))
        {                   
            SpecimenController specController = other.GetComponentInParent<SpecimenController>();

            inRange = true;

            if (specController.pickup)
            {
                boulderController.BoulderAnim();
            }
        }
    }

}
