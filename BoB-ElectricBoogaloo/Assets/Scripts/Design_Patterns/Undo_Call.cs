using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undo_Call : Callback_Interface
{
    private ActionLogHandler Handler;
    public string TargetMethod;

    public Undo_Call(ActionLogHandler recieverObj, string methodPtr)
    {
        Handler = recieverObj;
        TargetMethod = methodPtr;
    }

    public void Execute()
    {
        if(TargetMethod == "Undo")
        {
            Handler.Undo();
        }
    }
}
