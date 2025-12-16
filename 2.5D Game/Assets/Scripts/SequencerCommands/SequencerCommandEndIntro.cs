using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using Fusion;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    public class SequencerCommandEndIntro : SequencerCommand
    {
        private referencer referencer;
        private NetworkObject cutscene;
        private NetworkObject starter;
        
        
        public void Awake()
        {
            referencer = FindFirstObjectByType<referencer>();
            cutscene = referencer.cutscene;
            starter = referencer.starter;
            if (cutscene.HasStateAuthority)
            {
                cutscene.gameObject.SetActive(false);
            }
            if (starter.HasStateAuthority)
            {
                starter.gameObject.SetActive(true);
            }
            Stop();
            
        }

        public void Update()
        {
            
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
