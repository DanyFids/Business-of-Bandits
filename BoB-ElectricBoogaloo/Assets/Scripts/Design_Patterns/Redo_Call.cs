using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redo_Call : Callback_Interface
{
    private ActionLogHandler Handler;
    public string TargetMethod;

    public Redo_Call(ActionLogHandler recieverObj, string methodPtr)
    {
        Handler = recieverObj;
        TargetMethod = methodPtr;
    }

    public void Execute()
    {
        if (TargetMethod == "Redo")
        {
            Handler.Redo();
        }
    }
}
