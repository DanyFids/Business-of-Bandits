using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undo_Call : MonoBehaviour, Callback_Interface
{
    private ActionLogHandler TargetObj;
    private string TargetMethod;

    public Undo_Call(ActionLogHandler recieverObj, string methodPtr)
    {
        TargetObj = recieverObj;
        TargetMethod = methodPtr;
    }

    public void Execute()
    {
        if(TargetMethod == "Undo")
        {
            TargetObj.Undo();
        }
    }
}
