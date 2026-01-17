using Fusion;
using System.Collections;
using UnityEditor.Analytics;
using UnityEngine;


public class booksSpawner : MonoBehaviour
{
    private NetworkRunner runner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject books;

    private IEnumerator Start()
    {
        NetworkRunner runner;

        // Wait until this client has a runner
        while ((runner = FindFirstObjectByType<NetworkRunner>()) == null)
            yield return null;

        // Wait until this client has joined
        while (!runner.IsRunning)
            yield return null;

        // Now this client knows if it's the server
        books.SetActive(!runner.IsServer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnBooks()
    {
        if (!runner.IsServer)
        {
            books.SetActive(true);
        }
    }
}
