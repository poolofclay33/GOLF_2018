using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 10, 0);
        StartCoroutine(stopSpring());
    }

    IEnumerator stopSpring() 
    {
        yield return new WaitForSeconds(.2f);

        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(.75f);
        transform.position = new Vector3(.09f, -.15f, 3.75f);
    }
}
