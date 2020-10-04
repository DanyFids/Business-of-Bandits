using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Button_Script : MonoBehaviour
{
	public GameObject Template_Object;
	public Camera Main_Camera;

	private GameObject new_obj = null;

	private void OnMouseDown()
	{
		if (new_obj == null)
		{
			new_obj = Instantiate(Template_Object);
			new_obj.tag = "place_obj";
		}
	}

	private void OnMouseDrag()
	{
		if (new_obj != null) {
			RaycastHit hit;
			Ray ray = Main_Camera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, 10.0f)) {
				new_obj.transform.position = hit.point;
			}
		}
	}

	private void OnMouseUp()
	{
		new_obj.tag = "editor_obj";
		new_obj = null;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
