using UnityEngine;
using Fusion;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using System.Collections;
public class PlayerHealth : NetworkBehaviour
{
    
    [Networked] public int health { get; set; }
    public int maxHealth = 3;

    public float hitCooldown = 2f; // seconds between hits

    // Track cooldown per player
    [Networked] private TickTimer hitCooldownTimer { get; set; }

    public Volume pp;
    private Vignette vignette;
    private float vignetteProgress;
    private bool vignetteActivate = false;
    private bool firstFrame = false;
    private float mathness;
    private BasicSpawner runner;
    public NetworkPrefabRef prefabRef;
    private PlayerRef player;
    private bool hasDied;

    public TextMeshProUGUI respawnText;
    public int respawnTime = 3;
    [Networked] public bool playerDead { get; set; }

    public override void Spawned()
    {
        hasDied = false;
        runner = FindFirstObjectByType<BasicSpawner>();
        if (!Object.HasStateAuthority)
            return;

            health = maxHealth;

        pp = GameObject.FindWithTag("pp").GetComponent<Volume>();
        pp.profile = Instantiate(pp.profile);
        

        if (pp.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true;
        }

        respawnText = RespawnUIManager.Instance.respawnText; //assigns respawn text
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority)
            return;

        if (firstFrame)
        {
            vignetteProgress = 0;
            firstFrame = false;
        }

        if (vignetteActivate && vignetteProgress < 1)
        {
            mathness = 0.35f * (-(2f * vignetteProgress - 1f) * (2f * vignetteProgress - 1f) + 1f);
            vignette.intensity.value = mathness;
            vignetteProgress += 0.02f;
        }

        if (health <= 0)
        {
            health = 0;
            if (!hasDied)
            {
                Die();
            }
            hasDied = true;
        }
           
    }

    // Enemy requests damage
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(int damage)
    {
        if (health <= 0)
            return;

        // If cooldown is running and NOT expired, ignore damage
        if (hitCooldownTimer.IsRunning && !hitCooldownTimer.Expired(Runner))
            return;

        // Start/restart cooldown
        hitCooldownTimer = TickTimer.CreateFromSeconds(Runner, hitCooldown);

        if (damage > 0)
        {
            vignetteActivate = true;
            firstFrame = true;
        }

        health -= damage;
        Debug.Log($"Player took {damage} damage. Health now {health}");
    }


    public void  Die()
    {
        if (Object.HasStateAuthority)
        {
            playerDead = true;
        }

        if (!Object.HasInputAuthority) return;
        StartCoroutine(RespawnCountdown());

    }

    private IEnumerator RespawnCountdown()
    {
        respawnText.gameObject.SetActive(true);

        float timer = respawnTime;

        while (timer > 0)
        {
            respawnText.text = $"Respawning in {Mathf.Ceil(timer)}...";
            timer -= Time.deltaTime;
            yield return null;
        }
        player = GetComponent<NetworkObject>().InputAuthority;
        RPC_CallRespawn(prefabRef, player);
        yield return null;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_CallRespawn(NetworkPrefabRef prefab, PlayerRef player1)
    {
        runner.Respawn(prefab, player);
    }
}
