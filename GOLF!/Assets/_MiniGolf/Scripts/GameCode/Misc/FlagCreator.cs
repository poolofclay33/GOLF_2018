using UnityEngine;
using System.Collections;

/// <summary>
/// Flag creator - spawns the flag
/// </summary>
public class FlagCreator : MonoBehaviour {
	
	/// <summary>
	/// The flag prefab to create
	/// </summary>
	public GameObject flagPrefab;
	// Use this for initialization
	void Start () {
		GameObject go = GameObject.FindWithTag("Hole");
		if(go && flagPrefab)
		{
			Instantiate(flagPrefab,go.transform.position,Quaternion.identity);
		}	
	}
	
}
