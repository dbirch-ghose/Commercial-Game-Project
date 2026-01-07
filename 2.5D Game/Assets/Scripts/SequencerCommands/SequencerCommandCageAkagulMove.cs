using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using Fusion;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    public class SequencerCommandAkagulMove : SequencerCommand
    {
        
        private MiniAk ak;
        private int num;
        public void Awake()
        {
            ak = FindFirstObjectByType<MiniAk>();
            num = GetParameterAsInt(0);
            switch (num)
            {
                case 0:
                    ak.EnterRoom();
                    break;
                case 1:
                    ak.LeaveRoom();
                    break;
            }
            Stop();
        }

        public void Update()
        {
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
