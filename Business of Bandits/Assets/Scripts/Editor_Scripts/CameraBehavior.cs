using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
	public float CAM_MOV_SPEED;
	public float CAM_BOOST_MULT;
	public float CAM_ROT_SPEED;

    // Update is called once per frame
    void Update()
    {
		Transform transform = GetComponent<Transform>();
		float dt = Time.deltaTime;

		if (Input.GetMouseButton(1))
		{
			float frame_mov_speed = CAM_MOV_SPEED * dt;

			if (Input.GetKey(KeyCode.LeftShift))
			{
				frame_mov_speed *= CAM_BOOST_MULT;
			}

			float mouse_x = Input.GetAxis("Mouse X");
			float mouse_y = Input.GetAxis("Mouse Y");

			transform.Rotate(new Vector3(-mouse_y, mouse_x, 0.0f));

			if (Input.GetKey(KeyCode.Q)){
				transform.Rotate(new Vector3(0.0f, 0.0f, CAM_ROT_SPEED * dt));
			}
			if (Input.GetKey(KeyCode.E))
			{
				transform.Rotate(new Vector3(0.0f, 0.0f, -CAM_ROT_SPEED * dt));
			}

			if (Input.GetKey(KeyCode.W))
			{
				transform.Translate(transform.forward * frame_mov_speed);
			}
			if (Input.GetKey(KeyCode.A))
			{
				transform.Translate(transform.right * -frame_mov_speed);
			}
			if (Input.GetKey(KeyCode.S))
			{
				transform.Translate(transform.forward * -frame_mov_speed);
			}
			if (Input.GetKey(KeyCode.D))
			{
				transform.Translate(transform.right * frame_mov_speed);
			}
			if (Input.GetKey(KeyCode.Space))
			{
				transform.Translate(transform.up * frame_mov_speed);
			}
			if (Input.GetKey(KeyCode.LeftControl))
			{
				transform.Translate(transform.up * -frame_mov_speed);
			}
		}
	}
}
