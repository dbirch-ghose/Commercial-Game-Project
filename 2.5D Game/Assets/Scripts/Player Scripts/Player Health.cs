using UnityEngine;
using Fusion;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
            health = maxHealth;

        pp = GameObject.FindWithTag("pp").GetComponent<Volume>();
        pp.profile = Instantiate(pp.profile);

        if (pp.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority)
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

        if (health < 0)
            health = 0;
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
}
