using PixelCrushers.DialogueSystem;
using System;
using UnityEngine;


public class RidOfInteractBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LiterallyJustHoldsAReferenceToThisText interactPrompt;
    private Selector selector;
    private GameObject prompt;
    void Start()
    {
        interactPrompt = FindFirstObjectByType<LiterallyJustHoldsAReferenceToThisText>(FindObjectsInactive.Include);
        prompt = interactPrompt.text;
        selector = GetComponent<Selector>();
        selector.CustomPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        selector.CustomPosition=transform.position;
        //Debug.Log("Selector position: " + selector.CustomPosition);
        var usable = selector.CurrentUsable;
        //Debug.Log(usable);
        

        if (usable == null)
        {
            prompt.SetActive(false);
            //Debug.Log("Running");
            return;
        }

        float distance = Vector3.Distance(
            selector.transform.position,
            usable.transform.position
        );
        //Debug.Log(distance);

        prompt.SetActive(distance <= usable.maxUseDistance);
    }
}
