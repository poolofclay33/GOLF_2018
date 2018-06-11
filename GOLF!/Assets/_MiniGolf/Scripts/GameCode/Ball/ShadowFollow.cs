using UnityEngine;
using System.Collections;
/// <summary>
/// Shadow follow.
/// </summary>
public class ShadowFollow : MonoBehaviour {

	private Transform m_playerTransform;
	
	/// <summary>
	/// The height of the shadow.
	/// </summary>
	public float shadowHeight = 10;
	
	/// <summary>
	/// The inital rotation of the shadow
	/// </summary>
	public float m_initalRotX = 88f;

	void findPlayer()
	{
		GameObject go = GameObject.FindWithTag("Player");
		if(go)
		{
			m_playerTransform = go.transform;
		}
	}
	// Update is called once per frame
	void Update () {
		findPlayer();

		if(m_playerTransform)
		{
			Transform t = m_playerTransform;
			Vector3 pos = t.position;
			Quaternion rot = t.rotation;
			Vector3 e  =rot.eulerAngles;
			e.x=m_initalRotX;
			e.z=0f;
			e.y=0;
			rot.eulerAngles = e;
			
			pos.y += shadowHeight;
			transform.position = pos;
			transform.rotation = rot;
		}
		
		
	}
}
