using UnityEngine;
using System.Collections;
/// <summary>
/// Destroy non mobile.
/// </summary>
public class DestroyNonMobile : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(Misc.isMobilePlatform()==false)
		{
			Destroy(gameObject);
		}
	}

}
