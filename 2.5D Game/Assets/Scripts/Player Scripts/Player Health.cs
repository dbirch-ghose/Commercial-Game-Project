using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : NetworkBehaviour
{
    [Networked] public int health { get; set; }
    public int maxHealth = 3;
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
            Debug.Log("First frame is true");
            vignetteProgress = 0;
            firstFrame = false;
        }
        if (vignetteActivate && vignetteProgress < 1)
        {
            Debug.Log("running mathness");
            mathness = 0.35f * (-(2f * vignetteProgress - 1f) * (2f * vignetteProgress - 1f) + 1f);
            vignette.intensity.value = mathness;
            vignetteProgress += 0.02f;
        }

        if (health < 0)
            health = 0;
    }

    // Enemy / server requests damage
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(int damage)
    {
        if (health <= 0)
            return;
        Debug.Log("take damage");
        vignetteActivate = true;
        firstFrame = true;
        health -= damage;
        Debug.Log($"Player took {damage} damage. Health now {health}");
    }

}
