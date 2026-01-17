using UnityEngine;

public class ImpactReceiver : MonoBehaviour
{
    public float mass = 3f;

    private Vector3 impact;
    private CharacterController character;

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (impact.magnitude > 0.2f)
        {
            character.Move(impact * Time.deltaTime);
        }

        impact = Vector3.Lerp(impact, Vector3.zero, 5f * Time.deltaTime);

        if (impact.magnitude < 0.01f)
            impact = Vector3.zero;
    }

    public void AddImpact(Vector3 dir, float force)
    {
        dir.y = 0;          // keep knockback horizontal
        dir.Normalize();

        impact += dir * force / mass;
    }
}
