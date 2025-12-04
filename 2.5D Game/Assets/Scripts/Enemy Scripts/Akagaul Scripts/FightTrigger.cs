using UnityEngine;

public class FightTrigger : MonoBehaviour
{
    public AkagaulBehaviour akagaulBehaviour;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision with trigger wall");
        if (other.gameObject.CompareTag("Player"))
        {
            akagaulBehaviour.StartCoroutine(akagaulBehaviour.AttackLoop());
            Destroy(gameObject);
        }
    }
}
