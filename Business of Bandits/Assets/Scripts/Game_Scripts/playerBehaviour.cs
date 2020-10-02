using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class playerBehaviour : MonoBehaviour
{
    public GameObject head;

    public CharacterController controller;
    public Rigidbody rb;

    //Define the speed at which the object moves.

    private bool grounded = false;

    private Vector3 jumpDir = new Vector3(0.0f, 1.0f, 0.0f);
    private float jumpHeight = 20.0f;
    private float gravity = 9.81f;

    private float playerSpeed = 10.0f;
    private float turningSpeed = 80.0f;

    //private Vector3 playerVelocity;

    

    void Start()
    {
        controller = gameObject.GetComponent(typeof(CharacterController)) as CharacterController;
        //rigidbody = gameObject.GetComponent(typeof(Rigidbody)) as Rigidbody;
    }

    // Update is called once per frame
    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        //Get the value of the Horizontal input axis.

        float verticalInput = Input.GetAxis("Vertical");
        //Get the value of the Vertical input axis.

        Vector3 movement = new Vector3(0, 0, verticalInput);
        Vector3 turning = new Vector3(0, horizontalInput, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = playerSpeed * verticalInput;

       
       

        if(turning != Vector3.zero)
        {
            transform.transform.Rotate(turning * turningSpeed * Time.deltaTime);
        }

        if (movement != Vector3.zero)
        {
            controller.SimpleMove(forward * curSpeed);
        }

       if(Input.GetKeyDown("space") && grounded)
       {
            print("jumping");
            rb.AddForce(jumpDir * jumpHeight, ForceMode.Impulse);
            grounded = false;
            //print("spacebar pressed");
        }

        if (Input.GetKeyUp("space"))
        {

           print("spacebar released");
        }


        // playerVelocity.y += gravity * Time.deltaTime;

        // controller.SimpleMove(playerVelocity);

       //transform.position = new Vector3(head.transform.position.x - transform.position.x, transform.position.y, transform.position.z);
    }

    void OnCollisionStay()
    {
        grounded = true;
    }
}
