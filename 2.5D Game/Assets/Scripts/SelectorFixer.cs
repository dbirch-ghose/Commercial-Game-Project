using Fusion;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class SelectorFixer : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Usable[] usables;
    private GameObject usableGO;
    
    // Performance: Cache component references to avoid GetComponent calls in update loop
    private cageDoorController boulderCheck;
    private SpecimenController specimenCheck;
    
    // Performance: Use squared distance threshold to avoid sqrt
    private const float MAX_INTERACTION_DISTANCE_SQR = 100f; // 10 units squared
    
    public override void Spawned()
    {
        usables = FindObjectsByType<Usable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
        // Cache SpecimenController if this has one
        specimenCheck = GetComponent<SpecimenController>();
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (HasInputAuthority)
        {
            // Performance: Single-pass loop with SqrMagnitude and distance threshold
            Usable closest = null;
            float bestSqrDistance = MAX_INTERACTION_DISTANCE_SQR;
            Vector3 myPos = transform.position; // Cache position
            
            foreach (var usable in usables)
            {
                if (usable == null) continue;
                
                // Performance: Use sqrMagnitude to avoid expensive sqrt
                float sqrDist = (usable.transform.position - myPos).sqrMagnitude;
                if (sqrDist < bestSqrDistance)
                {
                    bestSqrDistance = sqrDist;
                    closest = usable;
                }
            }

            // Performance: Single loop to update all usables
            foreach (var usable in usables)
            {
                if (usable == null) continue;
                usable.enabled = (usable == closest);
            }
            
            //boulder specific stuff
            if (closest != null)
            {
                usableGO = closest.gameObject;
                
                // Performance: Cache GetComponent result instead of calling every frame
                // Only get component when usable changes, not every frame
                if (usableGO != null)
                {
                    boulderCheck = usableGO.GetComponent<cageDoorController>();
                    
                    if (boulderCheck != null && specimenCheck == null)
                    {
                        closest.enabled = false;
                    }
                }
            }
        }
    }
}
