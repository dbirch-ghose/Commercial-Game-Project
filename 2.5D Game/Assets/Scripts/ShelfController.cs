using UnityEngine;
using Fusion;

public class ShelfController : NetworkBehaviour
{
    public NetworkObject shelf1;
    public NetworkObject shelf2;
    private int shelf1Pos;
    private int shelf2Pos;
    private int currentFormation;
    private int newFormation;
    public bool lever1Down;
    public bool lever2Down;
    public bool lever3Down;
    private bool moving;
    private bool shelf1left;
    private bool shelf2left;
    private bool shelf1right;
    private bool shelf2right;
    private int i;
    
    void Start()
    {
        moving = false;
        currentFormation = 1;
        lever1Down = false;
        lever2Down = false;
        lever3Down = false;
        shelf1left = false;
        shelf2left = false;
        shelf1right = false;
        shelf2right = false;
        i = 0;
    }

    public override void FixedUpdateNetwork()
    {
        if(moving)
        {

        }
        if (!moving)
        {
            //chooses new formation
            if (lever1Down && lever2Down && lever3Down)
            {
                newFormation = 1;
            }
            if (!lever1Down && lever2Down && !lever3Down)
            {
                newFormation = 2;
            }
            if (lever1Down && !lever2Down && !lever3Down)
            {
                newFormation = 3;
            }

            //decides moves
            if (currentFormation == 1)
            {
                switch (newFormation)
                {
                    case 1:
                        break;
                    case 2:
                        currentFormation = 2;
                        moving=true;
                        i = 0;
                        shelf1left = true;
                        break;
                    case 3:
                        currentFormation = 3;
                        moving=true;
                        i = 0;
                        shelf1left = true;
                        shelf2left = true;
                        break;
                }
            }
            if (currentFormation == 2)
            {
                switch (newFormation)
                {
                    case 1:
                        currentFormation = 1;
                        moving=true;
                        i = 0;
                        shelf1right = true;
                        break;
                    case 2:
                        break;
                    case 3:
                        currentFormation = 3;
                        moving=true;
                        i = 0;
                        shelf2left = true;
                        break;
                }
            }
            if (currentFormation == 3)
            {
                switch (newFormation)
                {
                    case 1:
                        currentFormation = 1;
                        moving=true;
                        i = 0;
                        shelf1right = true;
                        shelf2right = true;
                        break;
                    case 2:
                        currentFormation = 2;
                        moving=true;
                        i = 0;
                        shelf2right = true;
                        break;
                    case 3:
                        break;
                }
            }
        }
    }
}
