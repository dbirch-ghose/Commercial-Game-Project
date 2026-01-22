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

    public Transform canvasTransform;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        canvasTransform = GameObject.FindWithTag("canvas")?.transform;
        if (canvasTransform == null)
        {
            Debug.LogError("No Canvas found with tag 'canvas'");
            return;
        }
        var no = GetComponentInParent<NetworkObject>();
        if (no == null || !no.HasInputAuthority)
        {
            enabled = false;
            return;
        }

        // Instantiate heart prefabs under the Canvas
        fullHeart = Instantiate(fullHeartPrefab, canvasTransform);
        SetHeartPosition(fullHeart, new Vector2(-500, 70));

        twoHeart = Instantiate(twoHeartPrefab, canvasTransform);
        SetHeartPosition(twoHeart, new Vector2(-500, 70));

        oneHeart = Instantiate(oneHeartPrefab, canvasTransform);
        SetHeartPosition(oneHeart, new Vector2(-500, 70));

        deadHeart = Instantiate(deadHeartPrefab, canvasTransform);
        SetHeartPosition(deadHeart, new Vector2(-500, 70));


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

    private void SetHeartPosition(GameObject heart, Vector2 anchoredPos)
    {
        RectTransform rt = heart.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;
    }


}
