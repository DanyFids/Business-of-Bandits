using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLogHandler : MonoBehaviour
{
    enum Actions { Undo,Redo,Create,Destroy}

    Stack<GameObject> objectsUNDO;
    Stack<Actions> ActionsUNDO;

    public void Log()
    {

    }


    public void Undo()
    {

    }

    public void Redo()
    {

    }
}
