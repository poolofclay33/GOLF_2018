using UnityEngine;
using System.Collections;
/// <summary>
/// Water camera.
/// </summary>
public class WaterCamera : MonoBehaviour 
{
	
	/// <summary>
	/// The spin speed.
	/// </summary>
	public float spinSpeed = 10f;
	
	/// <summary>
	/// The camera offset.
	/// </summary>
	public Vector3 cameraOffset = new Vector3(30,30,30);
	
	
	private float m_angle = 0f;
	
	/// <summary>
	/// is this camera on.
	/// </summary>
	public bool m_on=false;
	
	private Vector3 m_waterPos;
	
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
		
		if(stateID.Equals( GameScript.State.WATER_HAZARD.ToString()))
		{
			BallScript ballScript = (BallScript)GameObject.FindObjectOfType( typeof(BallScript));
			if(ballScript)
			{
				m_waterPos =  ballScript.getPos();
				m_waterPos.y = 0;
			}
			m_on = true;
		}else{
			m_on = false;
		}
	}
	
	public void Update()
	{
		if(m_on)
		{
			Camera camera0 = Camera.main;
			if(camera0)
			{
				camera0.transform.position =  m_waterPos + Quaternion.AngleAxis(m_angle,Vector3.up) * cameraOffset;	
				camera0.transform.LookAt( m_waterPos );
			}
			m_angle += Time.deltaTime * spinSpeed;
		}
	}
	
}
