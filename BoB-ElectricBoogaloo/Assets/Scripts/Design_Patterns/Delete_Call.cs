using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete_Call : Callback_Interface
{
    private ActionLogHandler Handler;
    public string TargetMethod;
    GameObject TargetObj;

    public Delete_Call(ActionLogHandler recieverObj, string methodPtr, GameObject targObj)
    {
        Handler = recieverObj;
        TargetMethod = methodPtr;
        TargetObj = targObj;
    }

    public void Execute()
    {
        if (TargetMethod == "Delete")
        {
            Handler.Delete(TargetObj);
        }
    }
}
