using PixelCrushers.DialogueSystem;
using UnityEngine;

public class LuaChanger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void luaChange(int option)
    {
        switch (option)
        { 
            case 1:
                DialogueLua.SetVariable("num1", true);
                break;
            case 2:
                DialogueLua.SetVariable("num2", true);
                break;
            case 3:
                DialogueLua.SetVariable("num3", true);
                break;
            case 4:
                DialogueLua.SetVariable("num4", true);
                break;
            case 5:
                DialogueLua.SetVariable("num5", true);
                break;
            case 6:
                DialogueLua.SetVariable("num6", true);
                break;
            case 7:
                DialogueLua.SetVariable("num7", true);
                break;
            case 8:
                DialogueLua.SetVariable("num8", true);
                break;
            case 9:
                DialogueLua.SetVariable("num9", true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
