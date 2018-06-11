using UnityEngine;
using System.Collections;

/// <summary>
/// Roll camera.
/// </summary>
public class RollCamera : MonoBehaviour 
{
	/// <summary>
	/// is this camera on.
	/// </summary>
	public bool m_on=false;
	
	/// <summary>
	/// The look slerp.
	/// </summary>
	public float lookSlerp=5;
	
	/// <summary>
	/// The z offset.
	/// </summary>
	public float zOffset=5;
	
	/// <summary>
	/// The y offset.
	/// </summary>
	public float yOffset = 2;
	

	private BallScript m_ballScript;

	public void OnEnable()
	{
		GameManager.onEnterState += onEnterState;
	}
	public void OnDisable()
	{
		GameManager.onEnterState -= onEnterState;
	}
	public void onEnterState(string stateID)
	{
		if(stateID.Equals(GameScript.State.ROLL.ToString()))
		{
			m_on = true;
		}else{
			m_on = false;
		}
	}

	public void updateLookAt()
	{
		Vector3 targetPoint = m_ballScript.getPos() + m_ballScript.GetComponent<Rigidbody>().velocity.normalized*5f;
		targetPoint.y = transform.position.y;
		Vector3 dir = targetPoint - transform.position;
		if(dir!=Vector3.zero)
		{
			Quaternion targetRotation = Quaternion.LookRotation (dir,  Vector3.up);
 			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,Time.deltaTime *lookSlerp);	
			transform.position = m_ballScript.getPos() + new Vector3(0,yOffset,0) + transform.forward*-zOffset;
		}
	}
	
	public void Update()
	{
		if(m_on)
		{
			m_ballScript = (BallScript)GameObject.FindObjectOfType( typeof(BallScript));
		//	Camera camera0 = Camera.mainCamera;
			if(m_ballScript)
			{


				updateLookAt();

			}
		}
	}
	
}
