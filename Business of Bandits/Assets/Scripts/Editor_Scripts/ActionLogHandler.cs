using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLogHandler : MonoBehaviour
{
    public enum Actions {_Create,_Destroy}

    public static Stack<GameObject> ObjectsUNDO = new Stack<GameObject>(); //The past
    public static Stack<Actions> ActionsUNDO = new Stack<Actions>();

    public static Stack<GameObject> CurrentObject = new Stack<GameObject>(); //The most recent Thing
    public static Stack<Actions> CurrentAction = new Stack<Actions>();

    public static Stack<GameObject> ObjectsREDO = new Stack<GameObject>(); //The 'Future' (if undone)
    public static Stack<Actions> ActionsREDO = new Stack<Actions>();

    private const int MAX_ACTIONS_HELD = 5; //Maximum number of Undo's
   


    //private GameObject spawnedObject = null;

    public void HoldObject(GameObject obj)
    {
        obj.tag = "stack_obj";
        obj.layer = 13;

        MeshRenderer m_r;

        if (m_r = obj.GetComponent<MeshRenderer>())
        {
            m_r.enabled = false;
        }

        Rigidbody r;
        if (r = obj.GetComponent<Rigidbody>())
        {
            r.useGravity = false;
            r.detectCollisions = false;
        }
    }

    public void FreeObject(GameObject obj)
    {
        obj.tag = "editor_obj";
        obj.layer = 10;

        MeshRenderer m_r;

        if (m_r = obj.GetComponent<MeshRenderer>())
        {
            m_r.enabled = true;
        }

        Rigidbody r;
        if (r = obj.GetComponent<Rigidbody>())
        {
            r.useGravity = true;
            r.detectCollisions = true;
        }
    }

    private void MaintainStack()
    {
        //Stack size cannot be greater than 15
        if(ObjectsUNDO.Count > MAX_ACTIONS_HELD)
        {
            print("Maintaining stacks..");

            Stack<GameObject> tempObjs = new Stack<GameObject>();
            Stack<Actions> tempActs = new Stack<Actions>();

            for(int i = 0; i < ObjectsUNDO.Count; i++)
            {
                if (i < MAX_ACTIONS_HELD) // Destroy anything past the 15 most recent things
                {
                    tempObjs.Push(ObjectsUNDO.Pop());
                    tempActs.Push(ActionsUNDO.Pop());
                }
                else
                {
                    GameObject tmp = ObjectsUNDO.Pop();
                    ActionsUNDO.Pop();

                    if(tmp.tag == "stack_obj")
                    {
                        Destroy(tmp);
                    }
                }
            }

            for(int i = 0; i < tempObjs.Count; i++) // Re-ordering the stack.
            {
                ObjectsUNDO.Push(tempObjs.Pop());
                ActionsUNDO.Push(tempActs.Pop());
            }

        }
    }

    private void ClearStack(Stack<GameObject> stack)
    {
        //Custom Stack clearing to destroy instantiated objects that are currently 'deleted'
        if (stack.Count > 0)
        {
            for (int i = 0; i < stack.Count; i++)
            {
                GameObject temp = stack.Pop();

                if (temp.tag == "stack_obj")
                {
                    Destroy(temp);
                }
            }
        }
    }

    public void Log(GameObject obj,string action)
    {
        ActionsREDO.Clear();        // Upon logging a new action, removes any stored redo's
        ClearStack(ObjectsREDO);

        if (CurrentObject.Count == 0)
        {
            CurrentObject.Push(obj);

            if (action == "Create")
            {
                CurrentAction.Push(Actions._Create);
            }

            if (action == "Destroy")
            {
                CurrentAction.Push(Actions._Destroy);
            }
        }
        else
        {
            ObjectsUNDO.Push(CurrentObject.Pop()); // Push Object onto stack (Pointer to original object)
            ActionsUNDO.Push(CurrentAction.Pop()); // 

            //Convert string to Enum then push appropriate Action onto the stack
            CurrentObject.Push(obj);

            if (action == "Create")
            {
                CurrentAction.Push(Actions._Create);
            }

            if (action == "Destroy")
            {
                CurrentAction.Push(Actions._Destroy);
            }
        }

        MaintainStack(); //Keeps the number of 

    }


    public void Undo()
    {
       

        //Debug.Log("Reached Handler");
        if (CurrentObject.Count > 0 && ObjectsUNDO.Count == 0)
        {
            GameObject tempObj = CurrentObject.Pop();
            Actions tempAct = CurrentAction.Pop();

            ObjectsUNDO.Push(tempObj);
            ActionsUNDO.Push(tempAct);

            switch (tempAct)
            {
                case Actions._Create:
                    //print("I should destroy here");
                    HoldObject(tempObj);

                    break;
                case Actions._Destroy:
                    //print("I Should create here");
                    FreeObject(tempObj);

                    break;
                default:
                    print("something went wrong");
                    break;

            }
        }
        else if (ObjectsUNDO.Count > 0  && ActionsUNDO.Count > 0) 
        {
            GameObject tempObj = ObjectsUNDO.Pop();     
            Actions tempAct = ActionsUNDO.Pop();

            ObjectsREDO.Push(tempObj);
            ActionsREDO.Push(tempAct);

            switch (tempAct)
            {
                case Actions._Create:
                    //print("I should destroy here");
                    HoldObject(tempObj);

                    break;
                case Actions._Destroy:
                    //print("I Should create here");
                    FreeObject(tempObj);
                    
                    break;
                default:
                    print("something went wrong");
                   break;

            }
        }

        
    }

    public void Redo()
    {
        //print("Reached Handler (Redo)");

        if (CurrentObject.Count == 0 && ObjectsUNDO.Count > 0)
        {
            GameObject tempObj = ObjectsUNDO.Pop();
            Actions tempAct = ActionsUNDO.Pop();

            CurrentObject.Push(tempObj);
            CurrentAction.Push(tempAct);

            switch (tempAct)
            {
                case Actions._Create:
                    //print("I should destroy here");
                    HoldObject(tempObj);

                    break;
                case Actions._Destroy:
                    //print("I Should create here");
                    FreeObject(tempObj);

                    break;
                default:
                    print("something went wrong");
                    break;

            }
        }
        else if (ObjectsREDO.Count > 0 && ActionsREDO.Count > 0)
        {

            GameObject tempObj = ObjectsREDO.Pop();
            Actions tempAct = ActionsREDO.Pop();

            ObjectsUNDO.Push(tempObj);
            ActionsUNDO.Push(tempAct);

            switch (tempAct)
            {
                case Actions._Create:
                    //print("I should destroy here");
                    FreeObject(tempObj);

                    break;
                case Actions._Destroy:
                    //print("I Should create here");
                    HoldObject(tempObj);

                    break;
                default:
                    print("something went wrong");
                    break;

            }
        }

        
    }


    void Start()
    {   //Deletes All Objects that were 'Deleted' from all Stacks Upon game start.

    }
}
