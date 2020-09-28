using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolControllerBehavior : MonoBehaviour
{
	//Public objects
	public GameObject Transform_Widget;
	public Camera Main_Camera;

	//Widget Axes
	private GameObject Translate_X_Axis;
	private GameObject Translate_Y_Axis;
	private GameObject Translate_Z_Axis;

	private GameObject selectedObj = null;

	private void Start()
	{
		if (Transform_Widget != null)
		{
			Translate_X_Axis = Transform_Widget.transform.Find("x-axis").gameObject;
			Translate_Y_Axis = Transform_Widget.transform.Find("y-axis").gameObject;
			Translate_Z_Axis = Transform_Widget.transform.Find("z-axis").gameObject;

			ToggleWidgetRender(false);
		}
	}

	// Update is called once per frame
	void Update()
    {
		RaycastHit hit;
		Ray ray = Main_Camera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit))
		{
			Transform objectHit = hit.transform;

			if (Input.GetMouseButton(0))
			{
				ToggleWidgetRender(true);
				Transform_Widget.transform.position = objectHit.position;
			}
		}
    }

	void ToggleWidgetRender(bool enabled)
	{
		Transform_Widget.GetComponent<Renderer>().enabled = enabled;

		foreach (Renderer r in Transform_Widget.transform.GetComponentsInChildren<Renderer>())
		{
			r.enabled = enabled;
		}
	}
}
