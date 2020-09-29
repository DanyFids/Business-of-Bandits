using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBehaviour : MonoBehaviour
{
    float moveSpeed = 10;
    float turnSpeed = 200;
    //Define the speed at which the object moves.

    // Update is called once per frame
    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        //Get the value of the Horizontal input axis.

        float verticalInput = Input.GetAxis("Vertical");
        //Get the value of the Vertical input axis.

        transform.Translate(new Vector3(0, 0, verticalInput) * moveSpeed * Time.deltaTime);
        //Move the object to XYZ coordinates defined as horizontalInput, 0, and verticalIsnput respectively.

        transform.Rotate(new Vector3(0, horizontalInput, 0) * turnSpeed * Time.deltaTime);

    }
}
