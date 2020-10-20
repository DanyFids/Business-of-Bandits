using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
	public float CAM_MOV_SPEED;
	public float CAM_BOOST_MULT;
	public float CAM_ROT_SPEED;

	private float x_rot = 0.0f;
	private float y_rot = 0.0f;
	private bool cursor_locked = false;

    // Update is called once per frame
    void Update()
    {
		Transform transform = GetComponent<Transform>();
		float dt = Time.deltaTime;

		if (Input.GetMouseButton(1))
		{
			if (!cursor_locked)
			{
				Cursor.lockState = CursorLockMode.Locked;
				cursor_locked = true;
			}

			float frame_mov_speed = CAM_MOV_SPEED * dt;

			if (Input.GetKey(KeyCode.LeftShift))
			{
				frame_mov_speed *= CAM_BOOST_MULT;
			}

			float mouse_x = Input.GetAxis("Mouse X");
			float mouse_y = Input.GetAxis("Mouse Y");

			y_rot += mouse_x;
			x_rot -= mouse_y;

			x_rot = Mathf.Clamp(x_rot, -90.0f, 90.0f);

			if (y_rot < -360.0f)
				y_rot += 360.0f;
			else if (y_rot > 360.0f)
				y_rot -= 360.0f;

			transform.localRotation = Quaternion.Euler(x_rot, y_rot, 0.0f);

			//Vector3 mov_direction = (Vector3.right * mov_horizontal + Vector3.forward * mov_vertical).normalized * frame_mov_speed;
			//transform.Translate(mov_direction);

			if (Input.GetKey(KeyCode.W))
			{
				transform.Translate(Vector3.forward * frame_mov_speed);
			}
			if (Input.GetKey(KeyCode.A))
			{
				transform.Translate(Vector3.right * -frame_mov_speed);
			}
			if (Input.GetKey(KeyCode.S))
			{
				transform.Translate(Vector3.forward * -frame_mov_speed);
			}
			if (Input.GetKey(KeyCode.D))
			{
				transform.Translate(Vector3.right * frame_mov_speed);
			}
			if (Input.GetKey(KeyCode.Space))
			{
				transform.Translate(Vector3.up * frame_mov_speed);
			}
			if (Input.GetKey(KeyCode.LeftControl))
			{
				transform.Translate(Vector3.up * -frame_mov_speed);
			}
		}
		else
		{
			if (cursor_locked)
			{
				Cursor.lockState = CursorLockMode.None;
				cursor_locked = false;
			}
		}
	}
}
