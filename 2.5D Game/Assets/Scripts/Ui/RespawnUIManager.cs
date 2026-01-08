using UnityEngine;
using TMPro;

public class RespawnUIManager : MonoBehaviour
{
    public static RespawnUIManager Instance;
    public TextMeshProUGUI respawnText;

    private void Awake()
    {
        Instance = this;
        respawnText.gameObject.SetActive(false);
    }
}
