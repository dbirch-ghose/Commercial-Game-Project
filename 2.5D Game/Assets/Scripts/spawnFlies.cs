using System.Runtime.CompilerServices;
using UnityEngine;
using static Unity.Collections.Unicode;
using Fusion;

public class spawnFlies : NetworkBehaviour

{
    private BasicSpawner runner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void spawnFliesPlease()
    {
        if (HasStateAuthority)
        {
            runner = FindFirstObjectByType<BasicSpawner>();
            runner.RPC_spawnFlies();
        }
    }
}
