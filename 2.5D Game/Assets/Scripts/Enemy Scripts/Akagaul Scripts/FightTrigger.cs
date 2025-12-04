using UnityEngine;

public class FightTrigger : MonoBehaviour
{
    public AkagaulBehaviour akagaulBehaviour;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            akagaulBehaviour.StartCoroutine(akagaulBehaviour.AttackLoop());
            Destroy(gameObject);
        }
    }
}
