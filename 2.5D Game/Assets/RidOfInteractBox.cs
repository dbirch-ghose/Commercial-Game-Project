using PixelCrushers.DialogueSystem;
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
    }

    // Update is called once per frame
    void Update()
    {
        var usable = selector.CurrentUsable;

        if (usable == null)
        {
            prompt.SetActive(false);
            return;
        }

        float distance = Vector3.Distance(
            selector.transform.position,
            usable.transform.position
        );

        prompt.SetActive(distance <= usable.maxUseDistance);
    }
}
