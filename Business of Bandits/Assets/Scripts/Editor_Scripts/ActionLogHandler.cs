using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLogHandler : MonoBehaviour
{
    public enum Actions {_Create,_Destroy}

    public static Stack<GameObject> ObjectsUNDO = new Stack<GameObject>();
    public static Stack<Actions> ActionsUNDO = new Stack<Actions>();


    public static Stack<GameObject> ObjectsREDO = new Stack<GameObject>();
    public static Stack<Actions> ActionsREDO = new Stack<Actions>();

    public void Log(GameObject obj,string action)
    {
        ObjectsUNDO.Push(obj); // Push Object onto stack


        //Convert string to Enum

        if (action == "Create")
        {
            ActionsUNDO.Push(Actions._Create);
        }

        if (action == "Destroy")
        {
            ActionsUNDO.Push(Actions._Destroy);
        }

        //Debugging
        Debug.Log(ObjectsUNDO.Peek());
        Debug.Log(ActionsUNDO.Peek());
    }


    public void Undo()
    {
        //Debug.Log("Reached Handler");

        if (ObjectsUNDO.Count > 0  && ActionsUNDO.Count > 0) {

            GameObject tempObj = ObjectsUNDO.Pop();
            Actions tempAct = ActionsUNDO.Pop();

            ObjectsREDO.Push(tempObj);
            ActionsREDO.Push(tempAct);

            switch (tempAct)
            {
                case Actions._Create:
                    //print("I should destroy here");
                    Destroy(tempObj);

                    Debug.Log(ObjectsREDO.Peek());
                    break;
                case Actions._Destroy:
                    //print("I Should create here");

                    break;
                default:
                    print("something went wrong");
                   break;

            }
        }
    }

    public void Redo()
    {
        print("Reached Handler (Redo)");
    }
}
