//#define PHOTON_MULTIPLAYER
using UnityEngine;
using System.Collections;
/// <summary>
/// The mini golf gameScript.
/// </summary>
public class GameScript : MonoBehaviour {
	#region variables
	///the currentNumber of shots 
	public int currentNumberOfStrokes = 0;
	
	///the maximum number of strokes
	public int maxNomStrokes = 8;
	/// <summary>
	/// The number of holes per course
	/// </summary>
	public static int HOLES_PER_COURSE = 18;

	/// the audio clip to play when the player has not gotten it in the hole but taken the maxmium number of strokes!	
	public AudioClip big10AC;
	
	///the audio clip to play when the ball has fallen in the water
	public AudioClip falloutAC;
	
	///the sound to play when the ball has gotten it in the hole
	public AudioClip winAC;

	
	///did we get in the hole before we ran out of strokes.
	private bool m_failedToGetInHole = false;
	
	///a ref to the ballscript
	private BallScript m_ballScript;

	
	public enum State
	{
		INIT=0,
		PLAY=1,
		PAUSE=2,
		SHOWSCORE=3,
		SUBMTSCORE=4,
		WATER_HAZARD=8,
		ROLL=16
	};
	
	///our current state
	public State m_state;
	
	///the gui skin to use
	public GUISkin guiSkin0;
	
	///the list of states -- should correspond with the gamescript states.
	public GameObject[] states;
	
	private int m_par;
	
	///the default ambient color.
	static float color = 51f / 255f;
	public Color ambientColor =  new Color(color,color,color,1f);
	
	/// <summary>
	/// The course names.
	/// </summary>
	public string[] courseNames = {"Course A", "Course B", "Course C", "Course D"};
	private static int m_totalPar = 0;
//	private int m_totalScore;
	#endregion
	

	void OnEnable()	
	{
		GameManager.onEnterState += onEnterState;
		GolfManager.onTimesUp += onTimesUp;

		BaseGameManager.onGameStart += startGame;
	}
	void OnDisable()
	{
		GameManager.onEnterState -= onEnterState;
		GolfManager.onTimesUp -= onTimesUp;
		BaseGameManager.onGameStart -= startGame;
	}
	
	void onEnterState(string state)
	{
		if(state.Equals(State.PLAY.ToString() ))
		{
			m_ballScript.setMode( BallScript.BallMode.AIM);
			m_state = GameScript.State.PLAY;
		}
	}
	
	public int getParAtHole(int par)
	{
		return m_par;
	}
	private GameObject m_scoreStateGO;
	public void Awake()
	{
		m_par = 3;
		RenderSettings.ambientLight = ambientColor;


		//unpause the game
		GameConfig.setPaused(false);
		ScoreState ss = (ScoreState)GameObject.FindObjectOfType(typeof(ScoreState));
		if(ss)
		{
			m_scoreStateGO = ss.gameObject;
		}
		//if its levle 1 reset the scores.
		int holeIndex = getHoleNomUsingCourse();
//		Debug.Log("holeIndex"+holeIndex);
		if(holeIndex==1)
		{
			setScoreForHole(getHoleNomUsingCourse(),currentNumberOfStrokes);
			setTotalScore(0);
//			m_totalScore=0;
			m_totalPar = 0;
		}else{

//			m_totalScore = getTotalScore();
		}
		ParScript ps = (ParScript)GameObject.FindObjectOfType(typeof(ParScript));
		if(ps)
		{
			m_par = ps.par;
			setParForHole(getHoleNomUsingCourse(),m_par);
			m_totalPar+=m_par;
		}else{
			setParForHole(getHoleNomUsingCourse(),m_par);
		}
		

		//get a ref to the gamescript!
		GameObject go = GameObject.FindWithTag("Player");
		if(go)
		{
			m_ballScript = go.GetComponent<BallScript>();
		}	
		
		GameManager.enterState( GameScript.State.INIT.ToString() );
		
	}


	public void Update()
	{
		bool paused = GameConfig.getPaused();


		if(m_state==State.PLAY || m_state==State.PAUSE)
		{
			if(paused==true)
			{
				
				m_state = State.PAUSE; 
				Screen.lockCursor =false;
			}else{
				m_state = State.PLAY; 
			}
		}
	}
	
	//load the next scene
	public void nextScene()
	{
		bool nextLevel = goToNextLevel();
		if(nextLevel==false)
		{
			m_state = State.SUBMTSCORE;
		}		
	}

	public void victory()
	{
//			Debug.Log ("victory: m_state: "+m_state);
		if(m_state == State.PAUSE || m_state == State.PLAY)
		{			
//			m_masterCamera.setState( MasterCamera.State.END);
			setScoreForHole(getHoleNomUsingCourse(),currentNumberOfStrokes);
			m_state = State.SHOWSCORE;
			
//			Debug.Log ("enterScoreState" + m_scoreStateGO.name);

//			MenuStateManager.enterStateUsingGameObject(m_scoreStateGO);
			finishHole();


//			Debug.Log ("victory"+currentNumberOfStrokes);
			if(m_failedToGetInHole==false)			
			{
				if(winAC)
				{
					GetComponent<AudioSource>().PlayOneShot (winAC);
				}
			}else{
				if(big10AC)
				{
					GetComponent<AudioSource>().PlayOneShot (big10AC);
				}
			}
		}else{
			setScoreForHole(getHoleNomUsingCourse(),currentNumberOfStrokes);
		}
	}
	public void OnGUI()
	{
		GUI.skin = guiSkin0;
		

	}

	//return to the main menu
	public void returnToMain()
	{
		#if PHOTON_MULTIPLAYER

			GolfManager.loadLevel(0,PhotonNetwork.player.ID);
		#else
			Application.LoadLevel( 0 );
		#endif

	}
	
	//give up!
	public void giveUp()
	{
		takeMaximumNumberOfStrokes();
		GameConfig.setPaused(false);
		BaseGameManager.gameover(true);
		//GolfManager.setPlayersScore(PhotonNetwork.player.ID,getTotalScore());
		//GolfManager.finishHole();
	}
	
	//unpause the game
	public void resume()
	{
		GameConfig.setPaused( false );
	}
		
	//start the game.
	public void startGame()
	{
//		m_masterCamera.setState( MasterCamera.State.ON_TEE);
		m_ballScript.setMode(BallScript.BallMode.AIM);
		m_state = State.PLAY;		
	}	
	
	//the ball has been placed on the tee, now move into aim mode.
	public void placeBall()
	{
//		m_masterCamera.setState( MasterCamera.State.AIM);
		m_ballScript.setMode(BallScript.BallMode.AIM);
		m_state = State.PLAY;	
	}
	
	//the ball has fallen in the water
	public void onFallOutStart()
	{
		//check to ensure that the ball has not already gotten in the hole 
		if(m_ballScript.hasWon()==false)
		{
			//change the master camera to water state
			GameManager.enterState(GameScript.State.WATER_HAZARD.ToString());
			Debug.Log ("EnterWater");
			//if there is a fallout audio clip play it.
			if(GetComponent<AudioSource>() && falloutAC)
			{
				Debug.Log ("EnterWater2");
				GetComponent<AudioSource>().PlayOneShot (falloutAC);
			}
		}
	}
	
	//change to aim mode 
	public void onAimMode()
	{
		m_ballScript.lookAtHole();
		m_ballScript.setMode(BallScript.BallMode.AIM);
		GameManager.enterState(GameScript.State.PLAY.ToString());
	}
	
	
	//the ball has "stopped" add a stroke
	public void onStrokeEnd()
	{

		//if we have a ball and we have exceeded the maximum number of strokes set it a victory but indicate we didnt finish! 
		if(currentNumberOfStrokes>(maxNomStrokes-1))
		{
			m_failedToGetInHole=true;
			GameManager.enterState(GameScript.State.SHOWSCORE.ToString());

		}else{
			GameManager.enterState(GameScript.State.PLAY.ToString());
		}

	}
		
	
	
	//the ball is rolling!
	public void onRollMode()
	{
//		m_masterCamera.setState( MasterCamera.State.ROLL );
		currentNumberOfStrokes++;
		int courseLevel = getHoleNomUsingCourse();
	
		updateScores( courseLevel );	
	}
	private bool m_finishedHole=false;
	public void onTimesUp()
	{
		if(m_finishedHole==false)
		{
			giveUp();
		}
	}
	public void finishHole()
	{
		if(m_finishedHole==false)
		{
			#if PHOTON_MULTIPLAYER
					GolfManager.setPlayersScore(PhotonNetwork.player.ID,getTotalScore());
			#else
				GolfManager.setPlayersScore(0,getTotalScore());
			#endif
			GolfManager.finishHole();
			m_finishedHole=true;
		}
	}
	//take the maximum number of strokes
	public void takeMaximumNumberOfStrokes()
	{
		currentNumberOfStrokes= maxNomStrokes;
		m_failedToGetInHole=true;
		if(m_ballScript)
			m_ballScript.victory();
		finishHole();
	}


	//returns the current state
	public State getState()
	{
		return m_state;
	}
	
	//returns the current hole
	public int getHole()
	{
		return Application.loadedLevel;
	}
	
	//returns the current number of strokes!
	public int getNomStrokes()
	{
		return currentNumberOfStrokes;
	}
	public int getCourseIndex()
	{
		int course = 18;
		int cousreIndex = 0;
		while(course < Application.loadedLevel)
		{
			course+=18;
			cousreIndex++;
		}
		return cousreIndex;
	}
	public int getHoleNomUsingCourse()
	{
		return Application.loadedLevel;
	}
	public bool goToNextLevel()
	{
		bool rc = true;
		int currentLevel = Application.loadedLevel;
		
		int courseLevel = getHoleNomUsingCourse();
		Debug.Log ("courseLevel" + courseLevel);
		updateScores( courseLevel );
		
		TransitionScript ts = (TransitionScript)GameObject.FindObjectOfType(typeof(TransitionScript));
		Constants.setLastSceneIndex(currentLevel+1);

		if(courseLevel+1 < HOLES_PER_COURSE+1)
		{
			GameManager.nextScene();
			if(ts)
			{

				loadLevel(ts.sceneIndex);
				//Application.LoadLevel(ts.sceneIndex);
			}else{
				loadLevel(currentLevel+1);
				//Application.LoadLevel( currentLevel+1 );
			}
		}else{
			if(Constants.getPracticeMode()==false)
			{
				BaseGameManager.submitSate();
				GameManager.enterState(GameScript.State.SUBMTSCORE.ToString() );
			}else{		
				loadLevel(0);
			}
			rc = false;
		}
		return rc;
	}
	void loadLevel(int levelToLoad)
	{
		#if PHOTON_MULTIPLAYER
			GolfManager.loadLevel(levelToLoad,PhotonNetwork.player.ID);

		#else
			Application.LoadLevel(levelToLoad);
		#endif
	}

	void updateScores(int curLevel)
	{

		setScoreForHole( curLevel, currentNumberOfStrokes);
	
	}
	
	//sets the hole at the given index
	public void setParForHole(int holeIndex,
											int score)
	{
		string id = "GolfTotalPar" + holeIndex.ToString();
		PlayerPrefs.SetInt( id,score);
	}
	
	//gets teh score at the current hole
	public int getParForHole(int holeIndex)
	{
		string id = "GolfTotalPar" + holeIndex.ToString();
		int rc  = PlayerPrefs.GetInt(id ,0);
		return rc;
	}

	//sets the hole at the given index
	public void setScoreForHole(int holeIndex,
								int score)
	{
		string id = "GolfTotalHole" + holeIndex.ToString();
		PlayerPrefs.SetInt( id,score);
	}
	
	//gets teh score at the current hole
	public int getTotalScoreAtHole(int holeIndex)
	{
		string id = "GolfTotalHole" + holeIndex.ToString();
		int rc  = PlayerPrefs.GetInt(id ,0);
		return rc;
	}
	public int getTotalPar()
	{
		int totalPar = 0;
		int currentHole = getHoleNomUsingCourse();
		for(int i=1; i<19; i++)
		{
			if (i <= currentHole)
			{
				totalPar += getParForHole(i);
			}
		}
		return totalPar;
	}
	public int getTotalScore()
	{
		int totalPar = 0;
		int currentHole = getHoleNomUsingCourse();
		for(int i=1; i<19; i++)
		{
			if (i <= currentHole)
			{
				totalPar += getTotalScoreAtHole(i);
			}
		}
		return totalPar;
	}
	
	//sets the total score
	public void setTotalScore(int score)
	{

		PlayerPrefs.SetInt( "GolfTotalHole",score);
	}
	public int getPar()
	{
		return m_par;
	}
	
}

