using PixelCrushers.DialogueSystem;
using UnityEngine;
using Fusion;

public class SelectorFixer : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Usable[] usables;
    void Start()
    {
        usables = FindObjectsByType<Usable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
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
        }
    }
}
