
using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    public class SequencerCommandEndScene : SequencerCommand
    { 

        public void Awake()
        {
            SceneManager.LoadScene("Menu Possibility");
            Stop();
        }


    }

}

