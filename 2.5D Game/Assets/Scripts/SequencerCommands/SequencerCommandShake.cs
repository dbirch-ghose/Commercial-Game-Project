using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using Fusion;
using MoreMountains.Feedbacks;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    
    public class SequencerCommandShake : SequencerCommand
    {
        public MMF_Player player;
        private float time = 0;
        public void Awake()
        {
            player = FindFirstObjectByType<MMF_Player>();
            Debug.Log("Found player");
            player.PlayFeedbacks();
            Debug.Log("played Shake");
            
        }

        public void Update()
        {
            time += Time.deltaTime;
            if (time > 4)
            {
                Stop();
            }
            // Add any update code here. When the command is done, call Stop().
            // If you've called stop above in Awake(), you can delete this method.
        }

        public void OnDestroy()
        {
            // Add your finalization code here. This is critical. If the sequence is cancelled and this
            // command is marked as "required", then only Awake() and OnDestroy() will be called.
            // Use it to clean up whatever needs cleaning at the end of the sequencer command.
            // If you don't need to do anything at the end, you can delete this method.
        }

    }

}


/**/
