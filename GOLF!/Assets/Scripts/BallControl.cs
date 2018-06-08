using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallControl : MonoBehaviour {

    //public Transform clubObj;
    public float zForce = 100;
    public Transform arrowObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
    {

        if(Input.GetButtonDown("Fire1")) 
        {
            GameFlow.currentStrokes += 1;
            GameFlow.totalStrokes += 1;
            Debug.Log(GameFlow.currentStrokes + " " + GameFlow.totalStrokes);
            GetComponent<Rigidbody>().AddRelativeForce(0, 0, zForce);
        }

        if(Input.GetKeyDown("space")) {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            //Instantiate(clubObj, transform.position, clubObj.rotation);
            GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 0); //sets ball to original rotation. 
            arrowObj.GetComponent<Transform>().position = transform.position;
        }

        if (Input.GetKey("a"))
        {
            transform.Rotate(0, -1, 0);
        }

        if (Input.GetKey("d"))
        {
            transform.Rotate(0, 1, 0);
        }

        if (Input.GetKey("w"))
        {
            zForce += 5;
        }

        if (Input.GetKey("s"))
        {
            zForce -= 5;
        }

        if(zForce < 20) {
            zForce = 20;
        }

        if(zForce > 1200) {
            zForce = 1200;
        }
	}



    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Cup") {
            Debug.Log("Completed!");
            GameFlow.currentStrokes = 0;
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            StartCoroutine(delayLoad());
        }
    }

    IEnumerator delayLoad() {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Hole2");
    }
}
