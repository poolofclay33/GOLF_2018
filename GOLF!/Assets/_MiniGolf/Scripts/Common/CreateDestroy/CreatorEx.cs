using UnityEngine;
using System.Collections;
/// <summary>
/// Creator ex will spawn a different object on the various platforms.
/// </summary>
public class CreatorEx : MonoBehaviour {
	/// <summary>
	/// The gameObject to create on android.
	/// </summary>
	public GameObject prefabDroid;
	
	/// <summary>
	/// The gameObject to create on IOS
	/// </summary>	
	public GameObject prefabIOS;
	
	/// <summary>
	/// The gameObject to create if not android or IOS
	/// </summary>	
	public GameObject prefab;
	public void Awake()
	{
		if(Application.platform==RuntimePlatform.Android)
		{
			if(prefabDroid)
				Instantiate(prefabDroid,transform.position,Quaternion.identity);
		}
		else if(Application.platform==RuntimePlatform.IPhonePlayer)
		{
			if(prefabIOS)
				Instantiate(prefabIOS,transform.position,Quaternion.identity);
		}else{
			if(prefab)
				Instantiate(prefab,transform.position,Quaternion.identity);
		}
	}
}
