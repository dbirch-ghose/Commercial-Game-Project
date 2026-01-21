using Fusion;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class SelectorFixer : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Usable[] usables;
    private GameObject usableGO;
    private cageDoorController boulderCheck;
    private SpecimenController specimenCheck;
    void Start()
    {
        usables = FindObjectsByType<Usable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (HasInputAuthority)
        {
            Usable closest = null;
            float bestDistance = float.MaxValue;
            foreach (var usable in usables)
            {
                if (usable == null) continue;
                float dist = Vector3.Distance(transform.position, usable.transform.position);
                if (dist < bestDistance)
                {
                    bestDistance = dist;
                    closest = usable;
                }
            }

            foreach (var usable in usables)
            {
                
                if (usable == null) continue;
                usable.enabled = (usable == closest);
                
            }
            //boulder specific stuff
            boulderCheck = null;
            specimenCheck = null;
            usableGO = closest.gameObject;
            boulderCheck = usableGO.GetComponent<cageDoorController>();
            
            if (boulderCheck != null)
            {
                specimenCheck = GetComponent<SpecimenController>();
                if (specimenCheck == null)
                {
                    closest.enabled = false;
                }
            }
        }
    }
}
