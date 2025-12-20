using PixelCrushers.DialogueSystem;
using System.Runtime.CompilerServices;
using UnityEngine;

public class selectorCustomPosition : MonoBehaviour
{
    private Selector selector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selector = GetComponent<Selector>();
    }

    // Update is called once per frame
    void Update()
    {
        selector.CustomPosition = transform.position;
        Debug.Log(selector.CustomPosition);
    }
}
