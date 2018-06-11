using UnityEngine;
using System.Collections;
/// <summary>
/// Birds eye camera.
/// </summary>
public class BirdsEyeCamera : MonoBehaviour {
	/// <summary>
	/// is this camera on.
	/// </summary>
	public bool m_on=false;
	
	
	/// <summary>
	/// The rotate towards time.
	/// </summary>
	public float rotateTowardsTime = 1f;
	private float m_time = 0;
	
	/// <summary>
	/// The camera offset.
	/// </summary>
	public Vector3 cameraOffset = new Vector3(0,45f,0);

	
	
	public void OnEnable()
	{
		BallManager.onEnterState += onEnterState;
	}
	public void OnDisable()
	{
		BallManager.onEnterState -= onEnterState;
	}
	public bool isNotAiming()
	{
		return m_time >= rotateTowardsTime;
	}
	public void onEnterState(string stateID)
	{
		if(stateID.Equals( BallScript.BallMode.BIRDS_EYE.ToString()) )
		{
			m_time = 0;
			m_on = true;
		}else{
			m_on = false;
		}
	}
	public void clearTime()
	{
		m_time=0f;
	}
	void rotateTransCam(float dt)
	{
		BallScript ballScript = (BallScript)GameObject.FindObjectOfType( typeof(BallScript));
		Camera camera0 = Camera.main;
		if(ballScript && camera0)
		{
			Vector3 pos = ballScript.getPos();
			Vector3 cameraPos = pos + cameraOffset;
					float clampTime = m_time / rotateTowardsTime;
			m_time+=dt;
			if(clampTime>1)
			{
				clampTime = 1;
			}
			Vector3 vec = Vector3.Lerp(camera0.transform.position ,cameraPos,clampTime);

			camera0.transform.position = vec;
			camera0.transform.LookAt(pos);
		}
	}
	public void Update()
	{
		float dt = Time.deltaTime;
		if(m_on)
		{
			if(m_time<rotateTowardsTime)
			{
				rotateTransCam(dt);
			}
		}
		
		
	}	
}
