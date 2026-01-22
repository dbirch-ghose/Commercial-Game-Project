using UnityEngine;
using Fusion;
using System;

public class HealthDisplay : MonoBehaviour
{
    public PlayerHealth playerHealth;

    // Assign prefab references in the player prefab
    public GameObject fullHeartPrefab;
    public GameObject twoHeartPrefab;
    public GameObject oneHeartPrefab;
    public GameObject deadHeartPrefab;

    private GameObject fullHeart;
    private GameObject twoHeart;
    private GameObject oneHeart;
    private GameObject deadHeart;

    private int lastHealth = -1;

    // Performance: Use serialized field instead of FindWithTag
    [SerializeField] private Transform canvasTransform;

    private void Start()
    {
        // Performance: Only use FindWithTag as fallback if not assigned in inspector
        if (canvasTransform == null)
            canvasTransform = GameObject.FindWithTag("player1Canvas")?.transform;
            
        if (canvasTransform == null)
        {
            Debug.LogError("No Canvas found with tag 'player1Canvas'");
            return;
        }


        // Instantiate heart prefabs under the Canvas
        fullHeart = Instantiate(fullHeartPrefab, canvasTransform);
        twoHeart = Instantiate(twoHeartPrefab, canvasTransform);
        oneHeart = Instantiate(oneHeartPrefab, canvasTransform);
        deadHeart = Instantiate(deadHeartPrefab, canvasTransform);

        // Disable all hearts at start
        fullHeart.SetActive(false);
        twoHeart.SetActive(false);
        oneHeart.SetActive(false);
        deadHeart.SetActive(false);

        // Show initial health
        if (playerHealth != null)
        {
            UpdateHearts(playerHealth.health);
            lastHealth = playerHealth.health;
        }
    }

    private void Update()
    {
        if (playerHealth == null || playerHealth.Object == null) return;

        int currentHealth = playerHealth.health;

        if (currentHealth != lastHealth)
        {
            UpdateHearts(currentHealth);
            lastHealth = currentHealth;
        }
    }

    private void UpdateHearts(int health)
    {
        fullHeart.SetActive(health == 3);
        twoHeart.SetActive(health == 2);
        oneHeart.SetActive(health == 1);
        deadHeart.SetActive(health <= 0);
    }

    // Performance: Cleanup to prevent memory leaks
    private void OnDestroy()
    {
        if (fullHeart != null) Destroy(fullHeart);
        if (twoHeart != null) Destroy(twoHeart);
        if (oneHeart != null) Destroy(oneHeart);
        if (deadHeart != null) Destroy(deadHeart);
    }
}
