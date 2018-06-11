using UnityEngine;
using System.Collections;
/// <summary>
///Ballscript
///	Our ballscript has a number of modes.

///	INIT:	
///		occurs when the camera is spinning around the ball.
	
///	ON_TEE:
///		occurs when our ball is on top of the golf-mat aka on the "tee"
		
///	AIM:
///		This occurs when we are able to control the ball settings its rotation and power
		
		
///	ACTION:
///		This occurs when the ball is rolling.
		
///	FALL:
///		The ball has fallen in the water
		
///	WIN:
///		This occurs whenever the ball has either reached the hole or has run out of strokes.
/// </summary>
public class BallScript : MonoBehaviour 
{
	#region varaibles
	public enum BallMode
	{
		//Our inital position
		INIT,
		//the ball is on the tee
		ON_TEE,		
		//we are aiming
		AIM,
		//we are in motion
		ACTION,
		//falling state
		FALL,
		NULL,
		WIN,
		BIRDS_EYE
	};
	public Texture fireButtonTex;
	//the inital mode should mostly be AIM
	public BallMode initalMode = BallMode.AIM;
	
	//the balls current mode
	public BallMode m_ballMode;
	
	//the rotation scalar for the camera/ball
	public float rotateSpeed = 10f;
	
	
	//the minimum ammount of power
	public float minPower 	    = 0.0125f;
	//the maximum ammount of power -- I cant imagine why you would need to have this anything but 1.0 but who knows.
	public float maxPower 		= 1.0f;
	
	//the power scale.
	public float powerScale = 0.1f;
	//the currentpower
	public float m_currentPower = 0.5f;
	
	//the maximum force needed.
	public float ballForce = 2200f;
	
	
	//the delay time
	public float fallTime = 2.0f;
	private float m_fallTime;
	
	// a referance to the gamescript	
	private GameScript m_gameScript;
	
	//the max roll time
	public float minRollTime = 1.2f;
	public float maxRollTime = 5f;
	private float m_rollTime;
	
	//the velocity we use to determine its stopped
	public float minVelocity = 1.5f;
	//the angular velocity we use to help determine if the ball is stopped.
	public float minAngularVelocity = 1.0f;
	
	
	//the time the ball has to be "stopped before it registers"
	public float requiredStoppedTime = 0.15f;
	private float m_stoppedTime = 0f;
	
	public Texture greenTex;
	public Texture redTex;
	
	private Quaternion m_ballRotation;
	
	
	
	//its the default position of the object
	private  Vector3 m_deafultPos;
	//its the default orientation of hte object
	private  Quaternion m_deafultRot;
	
	
	//the scale of the trale
	//private Vector3 m_traleScale = new Vector3(1f,1f,1f);
	
	
	//the audio clip that will be played when the ball hits a wall
	public AudioClip wallHitAC;
	public AudioClip strokeAC;
	
	//should our ball be inverted
	private int m_invertedX = 0;
	
	public float slowDownDrag = 1f;
	public float slowDownAngularDrag = 0.5f;
	
	//the balls initla drag
	private float m_initalLinearDrag;
	//the balls inital angular drag
	private float m_initalAngularDrag;
	
	private float m_currentMaxRollTime;
	//should we use super wall bounce
	public bool useSuperBounce;	
	//the wall time before we can wall boost again
	public float wallBoostTime = 1.0f;
	//a private variable for wall boosting
	private float m_wallBoostTime;
	
	//the wall boost scalar (it will multiply the balls velocity)
	public float wallBoostScalar = 4f;
	
	public PhysicMaterial ballPhysicsMat;
	
	//the ball movement when we are on the tee
	public float onTeeSpeed = 4f;
	
	//the bounding box for our tee
	public Vector4 teeBounds = new Vector4(-2,2,-1,1);
	
	private Transform m_trail;
	private Transform m_trailNode;
	//the balls tee offset (from the centre).
	private Vector3 m_teeOffset=Vector3.zero;
	
	//a ref to the golf-mats transform
	//	private Transform m_teeTransform;
	
	private Transform m_holeTransform;
	
	//	private Joysticks m_joysticks;
	
	public GameObject onWallHitGO;
	public GameObject onWaterHitGO;
	private PhysicMaterial m_initalMaterial;
	public GameObject birdsEye;
	
	private int m_count = 0;
	private Vector3 m_dir;
	private Vector3 m_prevDir;
	
	private Joystick m_joystick;
	
	private bool m_fireStart = false;
	private float m_powerDir = 1;
	public float powerSpeedChange = 10f;
	private Vector3 m_fireDir2;
	private Vector3 m_fireDir;
	
	public GUIStyle repeatButtonGS;
	#endregion
	public void stop()
	{
		if(GetComponent<Rigidbody>() && GetComponent<Rigidbody>().isKinematic==false)
		{
			GetComponent<Rigidbody>().velocity=Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity=Vector3.zero;
		}
		//	rigidbody.isKinematic=true;
	}
	void setBirdsEye(bool state)
	{
	}
	
	private Vector3 aimCameraOffset = new Vector3(0,8,-8);
	private float rollCameraOffset = 8f;
	private float rollCameraYOffset = 2f;
	
	void init()
	{
		
		AimCamera aimCamera = (AimCamera)GameObject.FindObjectOfType(typeof(AimCamera));
		if(aimCamera)
		{
			//aimCamera.cameraOffset = aimCameraOffset;
		}
		RollCamera rollCam = (RollCamera)GameObject.FindObjectOfType(typeof(RollCamera));
		if(rollCam)
		{
			///	rollCam.yOffset = rollCameraYOffset;
			//	rollCam.zOffset = rollCameraOffset;
		}	
		
		m_joystick = (Joystick)GameObject.FindObjectOfType(typeof(Joystick));
		
		if(GetComponent<Rigidbody>() && GetComponent<Rigidbody>().GetComponent<Collider>())
		{
			m_initalMaterial = GetComponent<Rigidbody>().GetComponent<Collider>().material;
		}	
		//a ref to the hole transform		
		m_holeTransform = GameObject.FindWithTag("Hole").transform;
		
		//get a ref to the golf mat transform
		
		stop();
		
		
	}
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
		if(stateID.Equals( GameScript.State.SHOWSCORE.ToString()) )
		{
			setMode(BallMode.WIN);
		}
	}
	
	public void Start()
	{
		Joystick joy =	(Joystick)GameObject.FindObjectOfType(typeof(Joystick));
		if(joy)
		{
			Destroy(joy.gameObject);
		}
		init();
		m_invertedX = PlayerPrefs.GetInt("GolfXAxis",0);
		
		//save the default position and rotation so that we can set it back if we get a "fallout".
		m_deafultPos = transform.position;
		m_deafultRot = transform.rotation;
		
		m_initalLinearDrag = GetComponent<Rigidbody>().drag;
		m_initalAngularDrag = GetComponent<Rigidbody>().angularDrag;
		//set the inital mode
		//	m_ballMode = initalMode;
		//get the game script
		m_gameScript = (GameScript)GameObject.FindObjectOfType(typeof(GameScript));
		//Constants.getGameScript();
		
		//find the trailNode -- this is an empty node but is useful for scaling the object
		m_trailNode = transform.Find ("TrailNode");		
		
		//get the trail renderer.
		m_trail =m_trailNode.transform.Find ("Trail");		
		enableTrail(false);
		
		
		
		transform.position = m_deafultPos;
		//aim our ball at the hole	
		lookAtHole();
		//transform.rotation *= Quaternion.AngleAxis(180f,Vector3.up);
	}
	
	
	void OnCollisionExit (Collision o){
		GameObject obj =  o.gameObject;
		
		if(obj.layer == LayerMask.NameToLayer("triangle") )
		{
			if(GetComponent<Rigidbody>().isKinematic==false)
			{
				GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				
			}
		}
		
	}
	
	public void changeAngle(Vector3 normal)
	{
		float mag = GetComponent<Rigidbody>().velocity.magnitude;
		Vector3 vec = m_fireDir;
		Vector3 newDir = Vector3.Reflect( vec, normal);
		GetComponent<Rigidbody>().velocity  = newDir * mag;
		m_fireDir = newDir;
		//Debug.Log("REFLECT" + normal + " vec " + vec + " newDir" + newDir);			
	}
	
	public void OnCollisionStay(Collision ci)
	{
		GameObject obj =  ci.gameObject;
		if(obj.layer == LayerMask.NameToLayer("triangle"))
		{
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
	}
	
	public void OnCollisionEnter(Collision ci)
	{
		GameObject obj =  ci.gameObject;
		int layer0 = obj.layer;
		bool hitThing = false;
		if(layer0 == LayerMask.NameToLayer("triangle"))
		{
			Vector3 normal = ci.contacts[0].normal;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;	
			changeAngle(normal);		
			hitThing=true;
		}else if(layer0==LayerMask.NameToLayer("Wall") || layer0==LayerMask.NameToLayer("PaperMache"))
		{
			GetComponent<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity * 0.65f;
			
			m_fireDir = GetComponent<Rigidbody>().velocity.normalized;
			hitThing=true;
		}
		//m_fireDir = rigidbody.velocity.normalized;
		if(hitThing)
		{
			if(onWallHitGO)
			{
				GameObject go = (GameObject)
					Instantiate(onWallHitGO,transform.position,Quaternion.identity);
				Destroy(go,2);
			}
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().PlayOneShot(wallHitAC);
			}
		}
		
	}
	
	
	public void setMode(BallMode bmode)
	{
		BallManager.enterState( bmode.ToString() );
		
		
		if(m_ballMode==BallMode.WIN)
		{
			
			if(GetComponent<Rigidbody>())
			{
				GetComponent<Rigidbody>().isKinematic=true;
			}
			m_won=true;
		}
		if(m_won==false)
		{
			m_ballMode = bmode;
			
			
		}
	}
	
	
	public void setVelocity(Vector3 vec)
	{
		m_dir = vec;
	}
	public void LateUpdate()
	{
		if(m_gameScript==null)
		{
			m_gameScript = (GameScript)GameObject.FindObjectOfType(typeof(GameScript));
		}
		float dt = Time.deltaTime;
		m_count++;
		if(m_count==0)
		{
			m_prevDir = GetComponent<Rigidbody>().velocity;
			m_dir = GetComponent<Rigidbody>().velocity;
		}
		
		
		if(m_count>=0)
		{
			m_prevDir = m_dir;
			m_dir = GetComponent<Rigidbody>().velocity;
			
		}
		
		
		
		//decraese the wall boost time
		m_wallBoostTime -= dt;
		
		//if we are not in aim mode do not lock the cursor.
		if(m_ballMode!=BallMode.AIM && m_ballMode!=BallMode.BIRDS_EYE &&
		   m_ballMode!=BallMode.FALL && m_ballMode!=BallMode.ACTION)
		{
			Screen.lockCursor =false;
		}else{
			if(Time.timeScale==1)
			{
				Screen.lockCursor=true;
			}
		}
		
		if(Time.timeScale==0)
		{
			Screen.lockCursor =false;
		}
		
		
		//if we are not init mode update the input for the ball
		if(m_ballMode != BallMode.INIT)
		{
			updateInputForBall();
		}
	}
	
	public void updateInputForBall()
	{
		float dt  = Time.deltaTime;
		
		//if the game is not paused and we have not already won update the ball
		if(GameConfig.getPaused()==false)
		{
			if(m_ballMode!=BallMode.WIN)
			{
				updateBallMode(dt);
			}			
		}
	}
	
	void updateBallMode(float dt)
	{
		
		switch(m_ballMode)
		{		
		case BallMode.ON_TEE:
		{
			handleOnTee( dt);
		}
			break;
		case BallMode.BIRDS_EYE:
		{
			handleBirdsEye( dt);
		}
			break;
		case BallMode.AIM:
		{
			
			handleAim( dt);
			
		}
			break;
			
		case BallMode.ACTION:
		{	
			handleAction(dt);
		}
			break;
		case BallMode.FALL:
		{
			handleFall(dt);
		}
			break;
			
		}		
	}
	void handleBirdsEye(float dt)
	{
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			//if(isNotBirdWatching())
			{
				setMode(BallMode.AIM);	
			}
		}
	}
	void handleOnTee(float dt)
	{
		float mx = Input.GetAxis("Mouse X");
		float my = Input.GetAxis("Mouse Y");
		
		
		
		float sx = dt*-mx* onTeeSpeed;
		float sy = dt*-my * onTeeSpeed;
		
		
		
		
		Screen.lockCursor = true;
		
		//disable gravirty for now
		GetComponent<Rigidbody>().useGravity=false;
		
		//our ball is on the tee (move it around) constraining it to the size of the golf mat
		m_teeOffset.x += sx;
		m_teeOffset.x = Mathf.Clamp(m_teeOffset.x,teeBounds.x,teeBounds.y);
		
		m_teeOffset.y=1f;
		
		m_teeOffset.z += sy;
		m_teeOffset.z = Mathf.Clamp(m_teeOffset.z,teeBounds.z,teeBounds.w);
		
		//move the golf ball to the new location
		//		transform.position = m_teeTransform.position + m_teeOffset;
		
		//If the Left mouse button is pressed call the gamescripts place ball function
		if(Input.GetMouseButton(0))
		{
			//put gravity back
			GetComponent<Rigidbody>().useGravity = true;
			
			//set the default position (IE if the ball goes out of water).
			m_deafultPos = transform.position;
			
			//move it down 1 unit
			m_deafultPos.y -=1f;
			
			//move the ball to the default position
			transform.position = m_deafultPos;
			
			Input.ResetInputAxes();
			m_gameScript.placeBall();
			
			//aim our ball at the hole	
			lookAtHole();
		}
		
	}
	void aimStraight()
	{
		Vector3 holePos = transform.position + transform.rotation * new Vector3(0,0,1);
		
		holePos.y = transform.position.y;		
		
		
		Vector3 dir = transform.position-holePos;
		transform.rotation = Quaternion.LookRotation (dir, Vector3.up);
		
	}
	public Vector3 getHolePos()
	{
		Vector3 holePos = m_holeTransform.position;// + bc.center;
		holePos.y=transform.position.y;
		return holePos;
	}
	
	//aim the ball at the hole!
	public void lookAtHole()
	{
		transform.LookAt( getHolePos() );
	}
	private bool m_onHill = false;
	
	public void onHill(bool onHill,
	                   PhysicMaterial hillMAT)
	{
		m_onHill=onHill;
		
		PhysicMaterial mat = m_initalMaterial;
		if(onHill)
		{
			mat = hillMAT;
		}
		if(GetComponent<Rigidbody>() && GetComponent<Rigidbody>().GetComponent<Collider>())
		{
			GetComponent<Rigidbody>().GetComponent<Collider>().material = mat;
		}
		
	}
	
	
	
	void OnGUI()
	{
		if(m_ballMode == BallMode.AIM && Time.timeScale==1 && m_won==false)
		{
			handleIOSGui();
			GUIHelper.drawHealthbar( GUIHelper.screenRect(0.025f,0.25f,0.045f,0.5f),greenTex,redTex,m_currentPower);
			
		}
		
	}
	
	void handleIOSGui()
	{
		if(Misc.isMobilePlatform())
		{
			if(GUI.RepeatButton(GUIHelper.screenRect(0.8f,0.85f,0.15f,0.15f),fireButtonTex,repeatButtonGS))
			{
				m_fireStart = true;
			}else if(Event.current.type == EventType.Repaint)
			{
				if(m_fireStart)
				{
					m_fireStart = false;
					float currentForce = m_currentPower * ballForce;
					fireBall(currentForce);	
				}
			}
		}
	}
	//our ball is rolling
	private float m_angVelocityTime = 0f;
	
	
	void handleAction(float dt)
	{
		float angVelocity = GetComponent<Rigidbody>().angularVelocity.magnitude;
		
		
		
		//check to see the coniditions are okay
		//both the balls linear velocity and angular velocity have to be below a certain threshold in order for us to consider it to be "stopped".
		//otherwise reset our stop time.
		
		//Debug.Log("angVel"+ angVelocity + " minAngularVelocity " + minAngularVelocity );
		
		if(angVelocity < minAngularVelocity)
		{
			m_angVelocityTime += dt;
		}
		//m_rollTime +=dt;
		
		
		float speed = Mathf.Abs(GetComponent<Rigidbody>().velocity.magnitude);
		if(m_slowDown && speed < minVelocity)
		{
			m_stoppedTime+=dt;
		}
		
		if(m_stoppedTime>requiredStoppedTime)
		{
			stopBall();
		}	
		
		
		
		
		m_rollTime +=dt;			
		//if our stopped time is greater than required stop time OR we have rolled for longer than our maximum rolltime
		//stop the ball
		
		if(m_rollTime > m_currentMaxRollTime)
		{
			if(m_onHill==false)
			{
				GetComponent<Rigidbody>().drag = slowDownDrag;
				GetComponent<Rigidbody>().angularDrag = slowDownAngularDrag;	
			}	
			//rigidbody.angularDrag=1.5f;
			m_slowDown=true;
			//stopBall();
		}
		
		if(m_rollTime > maxRollTime*2f)
		{
			stopBall();
		}
	}
	public Vector3 getVelocity(){
		return GetComponent<Rigidbody>().velocity;
	}
	void stopBall()
	{
		//zero out the velocity and angular velocity
		
		if(GetComponent<Rigidbody>().isKinematic==false)
		{
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
		//	rigidbody.isKinematic=true;
		m_slowDown=false;
		//look at the hole
		//transform.LookAt( m_holeTransform.position);
		
		//set our current power to 0
		m_currentPower=0f;
		
		//call the gameScripts onStroke end function
		if(m_gameScript)
		{
			m_gameScript.onStrokeEnd();
		}	
		
		//if we the ball hasnt already got in the hole	
		if(m_ballMode!=BallMode.WIN)
		{
			//set the balls mode to aim
			setMode( BallMode.AIM);
			
			//turn back on the trail
			//m_trail.renderer.enabled = true;
			//.renderer.enabled = true;
		}				
	}
	void enableTrail(bool enabled)
	{
		if(m_trail && m_trail.GetComponent<Renderer>())
		{
			m_trail.GetComponent<Renderer>().enabled=enabled;
		}				
	}
	
	public bool aimDisabled=false;
	void handleAim(float dt)
	{
		float v =0f;
		if(isNotAiming()==false)
		{
			enableTrail(false);
			return;
		}
		if(m_won)
		{
			return;
		}
		if(aimDisabled)
		{
			return;
		}
		
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			setMode( BallMode.BIRDS_EYE);
		}
		
		Quaternion q = transform.rotation;
		Vector3 vv = q.eulerAngles;
		vv.x = 0;
		vv.z=0f;
		q.eulerAngles = vv;
		
		transform.rotation = q;
		enableTrail(true);
		
		
		
		v = Input.GetAxis("Mouse X"); 
		
		
		if(Misc.isMobilePlatform())
		{
			v=0f;
			if(Input.touchCount>0)
			{
				Touch touch = Input.GetTouch(0);
				
				if(m_fireStart==false && touch.phase==TouchPhase.Moved)				
				{
					v = touch.deltaPosition.x;
				}
			}
		}
		
		
		
		//should our ball rotate the opposition direction.
		if(m_invertedX==1)
		{
			v *= -1f;
		}
		
		
		float scalarSlider = 1f;//Constants.getSliderScalar();
		//PlayerPrefs.GetFloat( "GG_SLIDE_SCALARX3",.5f);
		transform.rotation *= Quaternion.AngleAxis(scalarSlider * v * rotateSpeed,Vector3.up);
		
		
		float currentForce = m_currentPower * ballForce;
		
		GetComponent<Rigidbody>().isKinematic=true;
		//fire teh ball if the left mouse button is pressed
		bool requestsFire = false;
		if(Misc.isMobilePlatform())
		{
		}else{
			if( Input.GetMouseButton(0) )
			{
				m_fireStart = true;
			}else{
				if(m_fireStart)
				{
					m_fireStart = false;
					requestsFire = true;
				}
			}
		}
		if(m_fireStart)
		{
			changePower(dt);
		}
		if(requestsFire)
		{	
			fireBall(currentForce);	
		}		
		
		
	}
	public void changePower(float dt)
	{
		m_currentPower += (m_powerDir * powerSpeedChange * dt);
		if(m_currentPower > 1)
		{
			m_powerDir = -1;
			m_currentPower = 1f;
		}
		if(m_currentPower < 0.001f)
		{
			m_powerDir = 1f;
			m_currentPower = 0.001f;
		}
	}
	
	public Quaternion getBallRotation(){
		return m_ballRotation;
	}
	
	void fireBall(float currentForce)
	{
		m_deafultPos = transform.position;
		m_angVelocityTime=0f;
		if(GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().PlayOneShot( strokeAC );
		}
		if(m_won==false)
		{
			GetComponent<Rigidbody>().isKinematic = false;
		}
		m_ballRotation = transform.rotation;
		
		GameManager.enterState(GameScript.State.ROLL.ToString());
		//m_distanceMoved = 0f;
		m_currentMaxRollTime = m_currentPower * maxRollTime;
		m_currentMaxRollTime = Mathf.Clamp(m_currentMaxRollTime,minRollTime,maxRollTime);
		
		Vector3 vec = transform.rotation * new Vector3(0,0,1);
		//	Debug.Log("rotVec" + vec);
		m_fireDir = vec.normalized;
		m_fireDir2 = m_fireDir;
		
		
		transform.rotation = Quaternion.identity;
		
		//add some force to the ball
		GetComponent<Rigidbody>().AddForce( vec *  currentForce  ) ;
		if(m_gameScript)
		{
			//call game scripts roll mode
			m_gameScript.onRollMode();
		}
		
		
		//disable the trail 
		enableTrail(false);
		
		
		//reset the stopped time
		m_stoppedTime=0f;
		
		//set the balls drag back to the inital linear drag
		GetComponent<Rigidbody>().drag = m_initalLinearDrag;
		//set the balls drag back to the inital angular drag
		GetComponent<Rigidbody>().angularDrag = m_initalAngularDrag;
		m_slowDown=false;
		//reset our roll time
		m_rollTime = 0f;
		
		setMode( BallMode.ACTION);
	}
	
	
	void handleFall(float dt)
	{
		
		m_fallTime -= dt;
		if(m_fallTime < 0f)
		{
			//respawn our ball
			//	respawn();
			
			//call the gamescripts on stroke end function
			m_gameScript.onStrokeEnd();
			
			//	m_gameScript.takeStroke();			
			respawn();
			
		}		
	}
	private bool m_won=false;
	public void victory()
	{
		m_won=true;
		GetComponent<Rigidbody>().isKinematic=true;
		m_gameScript.onStrokeEnd();
		if(GetComponent<Rigidbody>().isKinematic==false)
		{
			GetComponent<Rigidbody>().velocity=Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity=Vector3.zero;
		}
		if(m_gameScript)
		{
			m_gameScript.victory();
		}
		
		m_ballMode = BallMode.WIN;
	}
	
	//the ball has fallen out 
	public void fallout()
	{
		if(BallMode.FALL!=m_ballMode && m_won==false)
		{
			
			Debug.Log("FALLOUT" + m_gameScript.getNomStrokes());
			//call the gamescripts fallout start
			
			
			m_gameScript.onFallOutStart();
			
			if(onWaterHitGO)
			{
				GameObject go = (GameObject)
					Instantiate(onWaterHitGO,transform.position,Quaternion.identity);
				Destroy(go,2);
			}
			
			
			if(m_ballMode!=BallMode.WIN)
			{
				m_fallTime = fallTime;
				m_ballMode = BallMode.FALL;
			}
		}		
	}
	
	
	private bool m_slowDown=false;
	public void respawn()
	{
		//		Debug.Log("respawn");
		m_slowDown=false;
		if(m_gameScript.getNomStrokes() < 10)
		{
			
			//call the gamescripts respawn funciton
			m_gameScript.onAimMode();
		}
		//set the balls position and rotation back to the default
		transform.position = m_deafultPos;// + new Vector3(0,.1f,0);
		transform.rotation = m_deafultRot;
		//		Debug.Log("MOVE TO DEFAULT" + m_ballMode + " pos + " + transform.position);
		
		if(m_won==false)
		{
			GetComponent<Rigidbody>().isKinematic=false;
		}
		if(GetComponent<Rigidbody>().isKinematic==false)
		{
			//set the balls velocity and angular velocity to zero (so it doesnt roll).
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}	
		//set the current power back to 0
		m_currentPower = 0f;
		lookAtHole();
		m_fallTime=0f;
		
		
	}	
	public Vector3 getFireDir()
	{
		return m_fireDir2;
	}
	
	//return the balls position
	public Vector3 getPos()
	{
		return transform.position;
	}
	
	//has hte ball reached the hole
	public bool hasWon()
	{
		return m_won;
	}
	
	//retunr the balls mode
	public BallMode getMode()
	{
		return m_ballMode;
	}
	public Vector3 getDir()
	{
		return m_dir;
	}
	public Vector3 getPrevDir()
	{
		return m_prevDir;
	}
	public bool isNotBirdWatching()
	{
		bool rc = false;
		BirdsEyeCamera bec = (BirdsEyeCamera)GameObject.FindObjectOfType(typeof(BirdsEyeCamera));
		if(bec)
		{
			rc = bec.isNotAiming();
		}
		return rc;
	}
	public bool isNotAiming()
	{
		bool rc = false;
		AimCamera ac = (AimCamera)GameObject.FindObjectOfType(typeof(AimCamera));
		if(ac)
		{
			rc = ac.isNotAiming();
		}
		return rc;
	}
}
