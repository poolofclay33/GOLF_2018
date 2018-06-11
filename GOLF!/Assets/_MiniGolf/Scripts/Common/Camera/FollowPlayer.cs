using UnityEngine;
using System.Collections;
/// <summary>
/// Follow player.
/// </summary>
public class FollowPlayer : MonoBehaviour {
	private Transform m_targetTransform;
	/// <summary>
	/// Should the camera use the x-offset
	/// /// </summary>
	public bool useXOffset = true;
	
	/// <summary>
	/// Should the camera use the y-offset
	/// /// </summary>
	public bool useYOffset = true;
	
	/// <summary>
	/// Should the camera use the z-offset
	/// </summary>
	public bool useZOffset = true;
	
	/// <summary>
	/// The player tag.
	/// </summary>
	public string playerTag = "Player";
	
	public bool useYaw = false;
	void onLandInWater(Vector3 vec)
	{
		setTarget(null);
	}
	
	public void setTarget(Transform target)
	{
		m_targetTransform = target;
	}
	void Start () {
//		Debug.Log ("Start");
		GameObject go = GameObject.Find(playerTag);
		if(go)
		{		Debug.Log ("Startfound");

			
			m_targetTransform = go.transform;
		}	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		float dt = Time.deltaTime;
		if(m_targetTransform)
		{
			handleFollowPlayer(dt);
		}
	}
	
	void handleFollowPlayer(float dt)
	{
		Vector3 offset = Vector3.zero;
		Vector3 targetPos = m_targetTransform.position;
		
		if(useYaw)
		{
			Quaternion rot = transform.rotation;
			Vector3 eulers = rot.eulerAngles;
			eulers.y = m_targetTransform.rotation.eulerAngles.y;
			transform.rotation = Quaternion.Euler(eulers);
		}
		if(useXOffset)
		{
			offset.x = targetPos.x;
		}
		if(useYOffset)
		{
			offset.y = targetPos.y;
		}
		if(useZOffset)
		{
			offset.z = targetPos.z;
		}
		
		transform.position = offset;		
		
		
		
	}
}
