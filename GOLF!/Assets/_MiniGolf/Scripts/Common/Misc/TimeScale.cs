using UnityEngine;
using System.Collections;

public class TimeScale : MonoBehaviour {
	public float timeScale = 1;
	// Use this for initialization
	void Awake () {
		Time.timeScale=timeScale;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
