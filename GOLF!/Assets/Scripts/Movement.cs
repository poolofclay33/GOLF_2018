using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float moveSpeed;
    private Rigidbody rb;
    private Light myLight;
    bool isGrounded = true;
    public float jumpForce = 2.0f;
    public Vector3 jump;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.5f, 0.0f);
        myLight = GetComponent<Light>(); 
	}

    // Update is called once per frame
    void Update()
    {

        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * moveSpeed);

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded == true) {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        }
        if(Input.GetKeyDown(KeyCode.L)) {
            myLight.enabled = !myLight.enabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            isGrounded = false;
        }
    }
}