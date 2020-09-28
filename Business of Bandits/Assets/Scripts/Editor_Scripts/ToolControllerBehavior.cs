using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolControllerBehavior : MonoBehaviour
{
	//Widget drop off
	public GameObject Transform_Widget;

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
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
