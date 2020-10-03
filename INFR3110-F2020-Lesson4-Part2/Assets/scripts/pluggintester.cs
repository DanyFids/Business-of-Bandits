using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class pluggintester : MonoBehaviour
{
	private const string DLL_NAME = "INFR3110-F2020-Lesson4";

	[StructLayout(LayoutKind.Sequential)]
	struct Vector2D
	{
		public float x;
		public float y;
	}

	[DllImport(DLL_NAME)]
	private static extern int GetId();

	[DllImport(DLL_NAME)]
	private static extern void SetId(int id);

	[DllImport(DLL_NAME)]
	private static extern Vector2D GetPosition();

	[DllImport(DLL_NAME)]
	private static extern void SetPosition(float x, float y);

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey(KeyCode.S))
		{
			Debug.Log("ID: " + GetId().ToString());
		}

		if (Input.GetKey(KeyCode.A))
		{
			SetId(1);
		}

		if (Input.GetKey(KeyCode.D))
		{
			SetPosition(100.0f, 300.0f);
		}

		if (Input.GetKey(KeyCode.F))
		{
			Vector2D pos = GetPosition();
			Debug.Log("x: " + pos.x.ToString() + ", y: " + pos.y.ToString());
		}
    }
}
