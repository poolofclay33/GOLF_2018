using UnityEngine;
using System.Collections;
/// <summary>
/// Simple camera.
/// </summary>
public class SimpleCamera : MonoBehaviour 
{
	#region variables
	/// <summary>
	/// The player tag.
	/// </summary>
	public string playerTag = "Player";
	
	/// <summary>
	/// do we want the xOffset to move
	/// </summary>
	public bool zeroXOffset = false;
	
	/// <summary>
	/// do we want to the yOffset to move
	/// </summary>
	public bool zeroYOffset = false;

	/// <summary>
	/// do we want to the zOffset to move
	/// /// </summary>
	public bool zeroZOffset = false;
	
	/// <summary>
	/// The lerp speed of the camera
	/// </summary>
	public float lerpSpeed = 5;
	
	
	//our inital position
	private Vector3 m_initalPos;
	
	//our targetPositon
	private Vector3 m_targetPos;
	
	//a refernece to the target.
	private Transform m_target;
	#endregion
	public void Start()
	{
		m_initalPos = transform.position;
		
		GameObject go = GameObject.FindWithTag(playerTag);
		if(go)
		{
			setTransform ( go.transform );
		}
	}
	
	void Update () {
		if(m_target==null)
		{
			return;
		}
		float dt = Time.deltaTime;
		updateOffset();
		Vector3 pos = Vector3.Lerp(transform.position,m_targetPos,dt * lerpSpeed);
		transform.position = pos;
	}


	public void setTransform(Transform t)
	{
		m_target=t;
		updateOffset();
	}
	


	public void updateOffset()
	{
		if(m_target==null)
		{
			return;
		}
		Vector3 offset = m_target.position;
		if(zeroXOffset)
		{
			offset.x=0;
		}	
		if(zeroYOffset)
		{
			offset.y=0;
		}
		if(zeroZOffset)
		{
			offset.z = 0f;
		}
		Vector3 cameraPos = offset + m_initalPos;
		m_targetPos = cameraPos;

	}
}
