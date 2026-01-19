using Fusion;
using System;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class ShelfController : NetworkBehaviour
{
    public NetworkObject shelf1;
    public NetworkObject shelf2;
    public badHabit1 Lever1;
    public badHabit2 Lever2;
    public BadHabit3 Lever3;
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
    
    public override void Spawned()
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
        Lever1 = FindFirstObjectByType<badHabit1>();
        Lever2 = FindFirstObjectByType<badHabit2>();
        Lever3 = FindFirstObjectByType<BadHabit3>();
    }

    public override void FixedUpdateNetwork()
    {
        lever1Down=Lever1.down;
        lever2Down=Lever2.down;
        lever3Down=Lever3.down;
        if(moving)
        {
            if (i <= 100)
            {
                if (HasStateAuthority)
                {

                    if (shelf1left)
                    {
                        Vector3 NewPos = shelf1.transform.position;
                        NewPos = new Vector3(NewPos.x - 0.015f, NewPos.y, NewPos.z);
                        shelf1.transform.position = NewPos;
                    }
                    if (shelf2left)
                    {
                        Vector3 NewPos = shelf2.transform.position;
                        NewPos = new Vector3(NewPos.x - 0.015f, NewPos.y, NewPos.z);
                        shelf2.transform.position = NewPos;
                    }
                    if (shelf1right)
                    {
                        Vector3 NewPos = shelf1.transform.position;
                        NewPos = new Vector3(NewPos.x + 0.015f, NewPos.y, NewPos.z);
                        shelf1.transform.position = NewPos;
                    }
                    if (shelf2right)
                    {
                        Vector3 NewPos = shelf2.transform.position;
                        NewPos = new Vector3(NewPos.x + 0.015f, NewPos.y, NewPos.z);
                        shelf2.transform.position = NewPos;
                    }
                }
                i++;
            }
            else
            {
                moving = false;
                shelf1left = false;
                shelf1right = false;
                shelf2left = false;
                shelf2right = false;
            }
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
