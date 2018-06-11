using UnityEngine;
using System.Collections;
/// <summary>
/// Set gravity.
/// </summary>
public class SetGravity : MonoBehaviour {
	
	/// <summary>
	/// The gravity.
	/// </summary>
	public Vector3 gravity = new Vector3(0,-79f,0);

	void Start () {
		Physics.gravity = gravity;

		
	}
}
