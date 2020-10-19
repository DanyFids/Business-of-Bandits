using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Button_Script : MonoBehaviour
{
	//private static int EDITOR_TOOL_LAYER = 8;
	//private static int EDITOR_SUPPORT_LAYER = 9;
	private static int EDITOR_OBJECT_LAYER = 10;
	private static int PLACE_OBJECT_LAYER = 11;

	public const float MIN_PLACE_RANGE = 1.0f;

	public GameObject Template_Object = null;
	public Camera Main_Camera;
	public ToolControllerBehavior Tool_Controller;
	public ActionLogHandler Action_Handler;

	private GameObject new_obj = null;
	private float place_distance = 10.0f;

	private void OnMouseDown()
	{
		if (new_obj == null && Template_Object != null)
		{
			new_obj = Instantiate(Template_Object);


			new_obj.tag = "place_obj";
			new_obj.layer = PLACE_OBJECT_LAYER;
			Rigidbody r;
			if (r = new_obj.GetComponent<Rigidbody>())
			{
				r.useGravity = false;
				r.detectCollisions = false;
			}

			

			if (Tool_Controller != null)
				Tool_Controller.ToggleDisabled(true);

			
		}

		
	}

	private void OnMouseDrag()
	{
		if (new_obj != null) {
			RaycastHit hit;
			Ray ray = Main_Camera.ScreenPointToRay(Input.mousePosition);
			LayerMask layerMask = ~(LayerMask.GetMask("Place Objects", "UI"));

			place_distance += Input.mouseScrollDelta.y;
			if (place_distance < 1.0f)
				place_distance = 1.0f;

			if (Physics.Raycast(ray, out hit, place_distance, layerMask))
			{
				new_obj.transform.position = hit.point;
			}
			else
			{
				new_obj.transform.position = ray.origin + (ray.direction * place_distance);
			}
		}
	}

	private void OnMouseUp()
	{
		new_obj.tag = "editor_obj";
		new_obj.layer = EDITOR_OBJECT_LAYER;
		Rigidbody r;
		if (r = new_obj.GetComponent<Rigidbody>())
		{
			r.useGravity = true;
			r.detectCollisions = true;
		}

		// Log That something has been created.
		Action_Handler.Log(new_obj, "Create");

		new_obj = null;

		if(Tool_Controller != null)
			Tool_Controller.ToggleDisabled(false);
	}

	// Start is called before the first frame update
	void Start()
    {
		foreach(Rigidbody r in GameObject.Find("Templates").GetComponentsInChildren<Rigidbody>())
		{
			r.detectCollisions = false;
			r.useGravity = false;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
