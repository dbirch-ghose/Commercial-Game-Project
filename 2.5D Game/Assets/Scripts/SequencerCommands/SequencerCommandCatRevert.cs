using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using Fusion;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    public class SequencerCommandCatRevert : SequencerCommand
    {
        private SpecimenBehaviour speccy;
        private SisterBehaviour fly;
        private int num;
        
        public void Awake()
        {
            num = GetParameterAsInt(0);
            speccy = FindFirstObjectByType<SpecimenBehaviour>();
            fly = FindFirstObjectByType<SisterBehaviour>();
            if (speccy != null && num ==1)
            {
                speccy.RPC_catReversion();
            }
            if (fly != null)
            {
                fly.RPC_catReversion();
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
