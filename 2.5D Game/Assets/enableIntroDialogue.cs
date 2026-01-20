using UnityEngine;
using Fusion;
public class enableIntroDialogue : NetworkBehaviour
{
    public GameObject introDialogue;
    public GameObject waitText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_EnableIntroDialogue()
    {
        introDialogue.gameObject.SetActive(true);
        waitText.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
