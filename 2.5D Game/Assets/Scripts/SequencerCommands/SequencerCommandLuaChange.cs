using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using Fusion;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    public class SequencerCommandLuaChange : SequencerCommand
    {
        private BasicSpawner bs;
        
        
        public void Awake()
        {
            bs = FindFirstObjectByType<BasicSpawner>();
            bs.RequestUnlock(1);
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
