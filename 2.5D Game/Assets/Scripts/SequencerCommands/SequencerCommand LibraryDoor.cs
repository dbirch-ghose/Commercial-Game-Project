using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using Fusion;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    public class SequencerCommandLibraryDoor : SequencerCommand
    {
        private NetworkObject libraryDoor;
        private referencer referenceBlock;
        private cageDoorController controller;
        
        public void Awake()
        {
            Debug.Log("Open Cage Sequence Run");
            referenceBlock = FindFirstObjectByType<referencer>();
            libraryDoor = referenceBlock.libraryDoor;
            controller = libraryDoor.GetComponent<cageDoorController>();
            controller.RPC_RequestOpenDoor();
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
