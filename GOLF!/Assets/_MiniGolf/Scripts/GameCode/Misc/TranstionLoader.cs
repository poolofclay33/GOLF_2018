using UnityEngine;
using System.Collections;
/// <summary>
/// Transtion loader.
/// </summary>
public class TranstionLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Application.LoadLevel( Constants.getLastSceneIndex());
	}
	

}
