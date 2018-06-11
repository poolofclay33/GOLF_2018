using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RendererEx 
{
	public Renderer renderer;
	public Shader shader;
}

/// <summary>
/// Aim camera.
/// </summary>
public class AimCamera : MonoBehaviour 
{
	/// <summary>
	/// The camera offset.
	/// </summary>
	public Vector3 cameraOffset = new Vector3(4,4,0);
	
	/// <summary>
	/// The target offset.
	/// </summary>
	public Vector3 targetOffset = new Vector3(-2,2,0);
	
	/// <summary>
	/// is this camera on.
	/// </summary>
	public bool m_on=false;
	
	/// <summary>
	/// The rotate towards time.
	/// </summary>
	public float rotateTowardsTime = 1f;
	
	/// <summary>
	/// The color of the alpha.
	/// </summary>
	public float alphaColor = 0.3f;
	private float m_time = 0;
	
	private Vector3 m_cameraPos;
	private Quaternion m_cameraRot;
	
	private bool m_cleared=false;
	private Dictionary<string,RendererEx> m_renderers = new Dictionary<string,RendererEx>();
	
	private Dictionary<string,RendererEx> m_renderers2 = new Dictionary<string,RendererEx>();
	
	public void OnEnable()
	{
		BallManager.onEnterState += onEnterState;
		BaseGameManager.onGameStart += turnOn;

	}
	public void OnDisable()
	{
		BallManager.onEnterState -= onEnterState;
		BaseGameManager.onGameStart -= turnOn;

	}
	public void onEnterState(string stateID)
	{
		if(stateID.Equals( BallScript.BallMode.AIM.ToString()) )
		{
			turnOn();
		}else{
			m_on = false;
		}
	}
	void turnOn()
	{
		Debug.Log ("AIMCAMERA turn On");
		BallScript ballScript = (BallScript)
			GameObject.FindObjectOfType( typeof(BallScript));
		if(ballScript)
		{
			ballScript.lookAtHole();	
		}
		m_cameraPos= Camera.main.transform.position;
		m_cameraRot = Camera.main.transform.rotation;
		m_time = 0f;
		m_on = true;
	}
	public void Update()
	{
		float dt = Time.deltaTime;
		if(m_on)
		{
			if(m_time<rotateTowardsTime)
			{
				rotateTransCam(dt);
			}else{
				rotateCam(dt);
				raycastCamera();

			}
		}else{
			raycastClearCamera();
		}
		
		
	}
	public bool isNotAiming()
	{
		return m_time >= rotateTowardsTime;
	}
	
	void raycastCamera()
	{
		Vector3 ballPos = Vector3.zero;
		BallScript ballScript = (BallScript)GameObject.FindObjectOfType( typeof(BallScript));
		if(ballScript)
		{
			ballPos = ballScript.transform.position;
		}
		Vector3 dir =  GetComponent<Camera>().transform.position - ballPos;
		
		RaycastHit[] hits = Physics.RaycastAll (ballPos,
												dir.normalized,
												2000.0f);
		
		m_renderers2.Clear();
		for (var i = 0;i < hits.Length; i++) 
		{
			RaycastHit hit  = hits[i];
			Renderer renderer =  hit.collider.GetComponent<Renderer>();
			string nom = renderer.gameObject.name;
			if(m_renderers.ContainsKey(nom)==false){
				m_renderers[nom] = new RendererEx();
				m_renderers[nom].renderer = renderer;
				m_renderers[nom].shader = renderer.material.shader;
				
				m_renderers2[nom] = m_renderers[nom];
				
				renderer.material.shader = Shader.Find("Transparent/Diffuse");
				Color col = renderer.material.color;
				col.a = alphaColor;
				renderer.material.color = col;
				
			}

		}
		m_cleared=false;
	}
	void raycastClearCamera()
	{
		if(m_cleared==false)
		{
			Dictionary<string,RendererEx> dic = new Dictionary<string, RendererEx>();
			foreach(KeyValuePair<string,RendererEx> entry in m_renderers)
			{
				if(m_renderers2.ContainsKey(entry.Key)==false)
				{
					m_renderers[entry.Key].renderer.material.shader = 
						m_renderers[entry.Key].shader;
					dic[entry.Key] = m_renderers[entry.Key];
				}
			}
			foreach(KeyValuePair<string,RendererEx> entry in dic)
			{
				if(m_renderers.ContainsKey(entry.Key))
				{
					m_renderers.Remove( entry.Key);
				}
			}
			m_cleared=true;
		}
		
	}
	
	void rotateCam(float dt)
	{
		BallScript ballScript = (BallScript)GameObject.FindObjectOfType( typeof(BallScript));
		Camera camera0 = Camera.main;
		
		
		if(ballScript && camera0)
		{
			Quaternion quat = ballScript.transform.rotation;
			Vector3 pos = ballScript.getPos();
				
			Vector3 cameraPos = pos + quat* cameraOffset;
			Vector3 targetPos = pos + quat* targetOffset;
		
			camera0.transform.position = cameraPos;
			camera0.transform.LookAt( targetPos );	
		}
	}
	void rotateTransCam(float dt)
	{
		BallScript ballScript = (BallScript)GameObject.FindObjectOfType( typeof(BallScript));
		Camera camera0 = Camera.main;
		if(ballScript && camera0)
		{
			Quaternion quat = ballScript.transform.rotation;
			Vector3 pos = ballScript.getPos();
				
			Vector3 cameraPos = pos + quat* cameraOffset;
			Vector3 targetPos = pos + quat* targetOffset;
		
			float clampTime = m_time / rotateTowardsTime;
			m_time+=dt;
			if(clampTime>1)
			{
				clampTime = 1;
			}
			if(clampTime>=1)
			{
				BirdsEyeCamera bec = (BirdsEyeCamera)GameObject.FindObjectOfType(typeof(BirdsEyeCamera));
				if(bec)
				{
					bec.clearTime();
				}
			}
			Vector3 vec = Vector3.Lerp(m_cameraPos,cameraPos,clampTime);
			Vector3 dir = (targetPos - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(dir);

			camera0.transform.position = vec;
			transform.rotation = Quaternion.Slerp(m_cameraRot, lookRotation, clampTime);

			//camera0.transform.LookAt( targetPos );	
		}
	}
	
}
