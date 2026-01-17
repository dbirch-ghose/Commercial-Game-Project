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
    private bool finishedMove;
    
    void Start()
    {
        shelf1Pos = 0;
        shelf2Pos = 0;
        currentFormation = 1;
        lever1Down = false;
        lever2Down = false;
        lever3Down = false;
    }

    public override void FixedUpdateNetwork()
    {
        //chooses new formation
        if (lever1Down && lever2Down && lever3Down)
        {
            newFormation = 1;
        }
        if (!lever1Down&& lever2Down && !lever3Down)
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
                    finishedMove = false;
                    //move shelf 1 left
                    break;
                case 3:
                    currentFormation = 3;
                    finishedMove = false;
                    //move shelf 1 left
                    //move shelf 2 left
                    break;
            }
        }
        if (currentFormation == 2)
        {
            switch (newFormation)
            {
                case 1:
                    currentFormation = 1;
                    finishedMove = false;
                    //move shelf 1 right
                    break;
                case 2:
                    break;
                case 3:
                    currentFormation = 3;
                    finishedMove=false;
                    //move shelf 2 left
                    break;
            }
        }
        if (currentFormation == 3)
        {
            switch (newFormation)
            {
                case 1:
                    currentFormation= 1;
                    finishedMove = false;
                    //move shelf 1 right
                    //move shelf 2 right
                    break;
                case 2:
                    currentFormation = 2;
                    finishedMove = false;
                    //move shelf 2 right
                    break;
                case 3:
                    break;
            }
        }
    }
}
