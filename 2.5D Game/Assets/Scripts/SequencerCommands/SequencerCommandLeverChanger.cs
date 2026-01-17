
using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    public class SequencerCommandLeverChanger : SequencerCommand
    { // Rename to SequencerCommand<YourCommand>
        private int num;
        private badHabit1 bad1;
        private badHabit2 bad2;
        private BadHabit3 bad3;
        public void Awake()
        {
            num = GetParameterAsInt(0);
            switch (num)
            {
                case 1:
                    bad1 = FindFirstObjectByType<badHabit1>();
                    bad1.down = !bad1.down;
                    break;
                case 2:
                    bad2 = FindFirstObjectByType<badHabit2>();
                    bad2.down = !bad2.down;
                    break;
                case 3:
                    bad3 = FindFirstObjectByType<BadHabit3>();
                    bad3.down = !bad3.down;
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
