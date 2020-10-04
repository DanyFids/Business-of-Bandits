using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolControllerBehavior : MonoBehaviour
{
	//Public objects
	public GameObject Transform_Widget;
	public Camera Main_Camera;
	public Camera ToolSubCam;
	public float translate_speed = 1/10.0f;

	//Widget Axes
	private GameObject Translate_X_Axis;
	private GameObject Translate_Y_Axis;
	private GameObject Translate_Z_Axis;

	private string axis_lock = "";
	private Vector3 rotation_init;

	private GameObject selectedObj = null;

	private void Start()
	{
		if (Transform_Widget != null)
		{
			Translate_X_Axis = Transform_Widget.transform.Find("translate-x-axis").gameObject;
			Translate_Y_Axis = Transform_Widget.transform.Find("translate-y-axis").gameObject;
			Translate_Z_Axis = Transform_Widget.transform.Find("translate-z-axis").gameObject;

			ToggleAxisPlane("rotate-x-axis", false);
			ToggleAxisPlane("rotate-y-axis", false);
			ToggleAxisPlane("rotate-z-axis", false);

			ToggleWidgetRender(false);
		}
	}

	// Update is called once per frame
	void Update()
    {

		if (selectedObj != null)
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				DeselectObject();
				//return;
			}

			RaycastHit tool_hit;
			Ray tool_ray = ToolSubCam.ScreenPointToRay(Input.mousePosition);

			if (Input.GetMouseButton(0))
			{
				float mouse_x = Input.GetAxis("Mouse X");
				float mouse_y = Input.GetAxis("Mouse Y");

				float tool_move;

				Vector3 cam_forward = Main_Camera.transform.forward;

				float x_adjust_h = cam_forward.z / Mathf.Abs(cam_forward.z);
				float x_adjust_v = cam_forward.x / Mathf.Abs(cam_forward.x);

				switch (axis_lock)
				{
					case "translate-all-axes":
						float horz_mv = mouse_x * translate_speed;
						float vert_mv = mouse_y * translate_speed;

						selectedObj.transform.position += (Main_Camera.transform.right * horz_mv) + (Main_Camera.transform.up * vert_mv);
						break;
					case "translate-x-axis":
						tool_move = (Mathf.Abs(mouse_x) > Mathf.Abs(mouse_y)) ? mouse_x * x_adjust_h: mouse_y * x_adjust_v;
						tool_move *= translate_speed;
						selectedObj.transform.position += Vector3.right * tool_move;
						break;
					case "translate-y-axis":
						tool_move = (Mathf.Abs(mouse_x) > Mathf.Abs(mouse_y)) ? mouse_x : mouse_y;
						tool_move *= translate_speed;
						selectedObj.transform.position += Vector3.up * tool_move;
						break;
					case "translate-z-axis":
						tool_move = (Mathf.Abs(mouse_x) > Mathf.Abs(mouse_y)) ? mouse_x * -x_adjust_v : mouse_y * x_adjust_h;
						tool_move *= translate_speed;
						selectedObj.transform.position += Vector3.forward * tool_move;
						break;
					case "rotate-x-axis":
						if (Physics.Raycast(tool_ray, out tool_hit, Mathf.Infinity, LayerMask.GetMask("Editor Support")))
						{
							Debug.DrawLine(selectedObj.transform.position, tool_hit.point, Color.cyan, 10.0f);

							Vector3 adjust_line = tool_hit.point - selectedObj.transform.position;

							float deg = Vector3.SignedAngle(rotation_init, adjust_line, selectedObj.transform.right);

							rotation_init = adjust_line;

							selectedObj.transform.Rotate(Vector3.right * deg);
						}

						//tool_move = (Mathf.Abs(mouse_x) > Mathf.Abs(mouse_y)) ? mouse_x * -x_adjust_v : mouse_y;
						//selectedObj.transform.Rotate(Vector3.right * tool_move);
						break;
					case "rotate-y-axis":
						if (Physics.Raycast(tool_ray, out tool_hit, Mathf.Infinity, LayerMask.GetMask("Editor Support")))
						{
							Debug.DrawLine(selectedObj.transform.position, tool_hit.point, Color.cyan, 10.0f);

							Vector3 adjust_line = tool_hit.point - selectedObj.transform.position;

							float deg = Vector3.SignedAngle(rotation_init, adjust_line, selectedObj.transform.up);

							rotation_init = adjust_line;

							selectedObj.transform.Rotate(Vector3.up * deg);
						}

						//tool_move = (Mathf.Abs(mouse_x) > Mathf.Abs(mouse_y)) ? mouse_x : mouse_y;
						//selectedObj.transform.Rotate(Vector3.up * tool_move);
						break;
					case "rotate-z-axis":
						if (Physics.Raycast(tool_ray, out tool_hit, Mathf.Infinity, LayerMask.GetMask("Editor Support")))
						{
							Debug.DrawLine(selectedObj.transform.position, tool_hit.point, Color.cyan, 10.0f);

							Vector3 adjust_line = tool_hit.point - selectedObj.transform.position;

							float deg = Vector3.SignedAngle(rotation_init, adjust_line, selectedObj.transform.forward);

							rotation_init = adjust_line;

							selectedObj.transform.Rotate(Vector3.forward * deg);
						}

						//tool_move = (Mathf.Abs(mouse_x) > Mathf.Abs(mouse_y)) ? mouse_x * -x_adjust_h : mouse_y * x_adjust_h;
						//selectedObj.transform.Rotate(Vector3.forward * tool_move);
						break;

					case "":

						if (Physics.Raycast(tool_ray, out tool_hit, Mathf.Infinity, LayerMask.GetMask("Editor Tools")))
						{
							Transform widgetHit_transform = tool_hit.transform;
							GameObject widgetHit = widgetHit_transform.gameObject;

							axis_lock = widgetHit.name;

							if (axis_lock.Contains("rotate-"))
							{
								Debug.Log(axis_lock + " Hit!");
								rotation_init = tool_hit.point - selectedObj.transform.position;
								ToggleAxisPlane(axis_lock, true);
							}
						}
						break;
				}

				CenterToolWidget();
			}
			else if (axis_lock != "")
			{
				if (axis_lock.Contains("rotate-"))
				{
					ToggleAxisPlane(axis_lock, false);
				}

				axis_lock = "";
			}
		}


		RaycastHit hit;
		Ray main_ray = Main_Camera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(main_ray, out hit))
		{
			Transform objectHit_transform = hit.transform;
			GameObject objectHit = objectHit_transform.gameObject;

			if (Input.GetMouseButton(0))
			{
				if (objectHit.CompareTag("editor_obj"))
				{
					SelectObject(objectHit);
				}
			}
		}
	}

	void ToggleWidgetRender(bool enabled)
	{
		//Transform_Widget.GetComponent<Renderer>().enabled = enabled;

		foreach (Renderer r in Transform_Widget.transform.GetComponentsInChildren<Renderer>())
		{
			r.enabled = enabled;
		}

		foreach (BoxCollider c in Transform_Widget.transform.GetComponentsInChildren<BoxCollider>())
		{
			c.enabled = enabled;
		}

		foreach (CapsuleCollider c in Transform_Widget.transform.GetComponentsInChildren<CapsuleCollider>())
		{
			c.enabled = enabled;
		}

		for (int c = 0; c < Transform_Widget.transform.Find("Grimbals").childCount; c++)
		{
			Transform obj = Transform_Widget.transform.Find("Grimbals").GetChild(c);

			obj.GetComponent<MeshCollider>().enabled = enabled;
		}
	}

	void SelectObject(GameObject obj)
	{
		ToggleWidgetRender(true);
		Transform_Widget.transform.position = obj.transform.position;
		selectedObj = obj;
		selectedObj.GetComponent<Rigidbody>().useGravity = false;
		selectedObj.GetComponent<Rigidbody>().detectCollisions = false;
	}

	void DeselectObject() {
		ToggleWidgetRender(false);
		if (axis_lock != "")
		{
			if (axis_lock.Contains("rotate-"))
			{
				ToggleAxisPlane(axis_lock, false);
			}

			axis_lock = "";
		}

		selectedObj.GetComponent<Rigidbody>().useGravity = true;
		selectedObj.GetComponent<Rigidbody>().detectCollisions = true;
		selectedObj = null;
	}

	void CenterToolWidget()
	{
		if (selectedObj != null)
		{
			Transform widgetTransform = Transform_Widget.GetComponent<Transform>();

			widgetTransform.position = selectedObj.transform.position;

			widgetTransform.Find("Grimbals").transform.rotation = Quaternion.Euler(selectedObj.transform.rotation.eulerAngles);
		}
	}

	void ToggleAxisPlane(string axis, bool enabled)
	{
		Transform_Widget.transform.Find("Grimbals").Find(axis).Find("axis-plane-front").gameObject.GetComponent<MeshCollider>().enabled = enabled;
		Transform_Widget.transform.Find("Grimbals").Find(axis).Find("axis-plane-back").gameObject.GetComponent<MeshCollider>().enabled = enabled;
	}
}
