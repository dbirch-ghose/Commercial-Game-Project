using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
public class EndGame : NetworkBehaviour
{
    public AkagaulBehaviour AB;

    public override void FixedUpdateNetwork()
    {
        if (AB != null && AB.IsDead)
        {
            EndScene();
        }
    }

    private void EndScene()
    {
        DialogueManager.StartConversation("After Ackergaul Fight");
    }
}
