using UnityEngine;
using System.Collections;

/// <summary>
/// Init camera.
/// </summary>
public class InitCamera : MonoBehaviour {
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

	
	public void OnEnable()
	{
		GameManager.onEnterState += onEnterState;
		BaseGameManager.onGameStart += onGameStart;
	}
	public void OnDisable()
	{
		GameManager.onEnterState -= onEnterState;
		BaseGameManager.onGameStart -= onGameStart;

	}
	public void onGameStart()
	{m_on=false;
	}
	public void onEnterState(string stateID)
	{

			
		if(stateID.Equals( GameScript.State.INIT.ToString()) || 
			stateID.Equals( GameScript.State.SUBMTSCORE.ToString()) ||
			stateID.Equals( GameScript.State.SHOWSCORE.ToString()))
		{
			m_on = true;
		}else{
			m_on = false;
		}
	}
	
	public void Update()
	{
		if(m_on)
		{
			BallScript ballScript = (BallScript)GameObject.FindObjectOfType( typeof(BallScript));
			Camera camera0 = Camera.main;
			if(ballScript && camera0)
			{
				Vector3 pos = ballScript.getPos();

				camera0.transform.position = pos + Quaternion.AngleAxis(m_angle,Vector3.up) * cameraOffset;	
				camera0.transform.LookAt( pos );
			}
			m_angle += Time.deltaTime * spinSpeed;
		}
	}
	
}
