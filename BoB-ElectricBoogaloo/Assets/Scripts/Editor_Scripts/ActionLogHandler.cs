using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLogHandler : MonoBehaviour
{
    

    public enum Actions {_Create, _Destroy, _NULL} 

    public static Stack<GameObject> ObjectsUNDO = new Stack<GameObject>(); //The past
    public static Stack<Actions> ActionsUNDO = new Stack<Actions>();

    public static Stack<GameObject> ObjectsREDO = new Stack<GameObject>(); //The 'Future' (if undone)
    public static Stack<Actions> ActionsREDO = new Stack<Actions>();

    public File_Plugin_Behavior file_handler;

    private const int MAX_ACTIONS_HELD = 5; //Maximum number of Undo's
   


    //private GameObject spawnedObject = null;

    public void HoldObject(GameObject obj)
    {
        obj.tag = "stack_obj";
        obj.layer = 12;

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

        file_handler.RemoveObject(obj);
        
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

        file_handler.AddObject(obj);
    }

    private void MaintainStack()
    {
        
        //Stack size cannot be greater than 15
        if(ObjectsUNDO.Count > MAX_ACTIONS_HELD && ObjectsUNDO.Count > 0)
        {
            print("Maintaining stacks..");

            Stack<GameObject> tempObjs = new Stack<GameObject>();
            Stack<Actions> tempActs = new Stack<Actions>();

            int num = ObjectsUNDO.Count;

           // print(ObjectsUNDO.Count);

            for (int i = 1; i <= num; i++) //6
            {

                if (i <= MAX_ACTIONS_HELD) // Destroy anything past the 5 most recent things
                {
                    tempObjs.Push(ObjectsUNDO.Pop());


                    tempActs.Push(ActionsUNDO.Pop());


                    //print(i);
                }
                else
                {
                    GameObject tmp = ObjectsUNDO.Pop();
                    Actions tmpAct = ActionsUNDO.Pop();

                    if(tmp.tag == "stack_obj" && tmp != null)
                    {
                        Destroy(tmp);
                    }
                }
            }

            print(tempObjs.Count);
            for (int i = 1; i <= MAX_ACTIONS_HELD; i++) // Re-ordering the stack.
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

                if (temp.tag == "stack_obj" && temp != null)
                {
                    Destroy(temp);
                }
            }
        }
    }

    public void Clean_Slate()
    {   
        ObjectsUNDO.Clear();
        ActionsUNDO.Clear();

        ObjectsREDO.Clear();
        ActionsREDO.Clear();
    }

    public void Log(GameObject obj,string action)
    {
        ActionsREDO.Clear();        // Upon logging a new action, removes any stored redo's
        ClearStack(ObjectsREDO);

      
        ObjectsUNDO.Push(obj); // Push Object onto stack (Pointer to original object)
        
        //Convert string to Enum then push appropriate Action onto the stack
        if (action == "Create")
        {
             ActionsUNDO.Push(Actions._Create);
        }
        
        if (action == "Destroy")
        {
            print("Destroyed.");
            ActionsUNDO.Push(Actions._Destroy);
        }
        
        MaintainStack(); //Keeps the number of Undo's in check.

    }

    /// CALLS ////

    public void Undo()
    {

        // UNDO >>> Current >>> REDO

        //Debug.Log("Reached Handler");
        if ( ObjectsUNDO.Count > 0)
        {

            GameObject tempObj = ObjectsUNDO.Pop();
            Actions tempAct = ActionsUNDO.Pop();

            ObjectsREDO.Push(tempObj);
            ActionsREDO.Push(tempAct);

            //Debug.Log(ObjectsREDO.Peek());

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



            /**if (ObjectsUNDO.Count > 0 && ActionsUNDO.Count > 0)
            {

                CurrentObject.Push(ObjectsUNDO.Pop());
                CurrentAction.Push(ActionsUNDO.Pop());

                //switch (tempAct)
                //{
                //    case Actions._Create:
                //        //print("I should destroy here");
                //        HoldObject(tempObj);
                //
                //        break;
                //    case Actions._Destroy:
                //        //print("I Should create here");
                //        FreeObject(tempObj);
                //        
                //        break;
                //    default:
                //        print("something went wrong");
                //       break;
                //
                //}
            } **/
        }
        
    }

    public void Redo()
    {

        // UNDO <<< CURRENT <<< REDO
        print(ObjectsREDO.Count);

        if (ObjectsREDO.Count > 0)
        {
           
           
                GameObject tempObj = ObjectsREDO.Pop();
                Actions tempAct = ActionsREDO.Pop();

                ObjectsUNDO.Push(tempObj);
                ActionsUNDO.Push(tempAct);

                //Debug.Log(tempObj);

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

    public void Delete(GameObject obj)
    {
        Log(obj, "Destroy");
        HoldObject(obj);
    }


    void Start()
    {   //Deletes All Objects that were 'Deleted' from all Stacks Upon game start.

    }
}
