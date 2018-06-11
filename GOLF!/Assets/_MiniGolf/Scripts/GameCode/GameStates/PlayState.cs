//#define PHOTON_MULTIPLAYER

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FMG;

/// <summary>
/// Mini Golf Play state.
/// </summary>
public class PlayState : MonoBehaviour {

	#region variables
	//a ref of the gamescript
	private GameScript m_gameScript;
	
	public Text holeGT;
	public Text scoreGT;
	
	private bool m_gameStart = false;
	public GameObject pauseState;
	public GameObject playState;
	private bool m_gamestart=false;
	private bool m_gameover=false;
	#endregion
	void Start()
	{
		
		m_gameScript = (GameScript)GameObject.FindObjectOfType(typeof(GameScript));		
		#if PHOTON_MULTIPLAYER
		
		if(PhotonNetwork.offlineMode)
		{
			Destroy(GameObject.Find("CLOCK"));
		}
		#endif
	}
	

	void onPause ()
	{
		GameManager.enterState(GameScript.State.PAUSE.ToString());
		GameConfig.setPaused(true);
		FMG.Constants.fadeInFadeOut(pauseState,playState);
		
	}
	public  void OnEnable()
	{
		BaseGameManager.onGameOver += onGameOver;
		BaseGameManager.onGameStart += onGameStart;
	}
	public  void OnDisable()
	{
		BaseGameManager.onGameOver -= onGameOver;
		BaseGameManager.onGameStart -= onGameStart;
	}
	public void onGameStart()
	{
		m_gamestart=true;
	}
	public void onGameOver(bool vic)
	{
		m_gameover=true;
	}
	public 	 void Update()
	{
		int holeIndex = m_gameScript.getHoleNomUsingCourse();
		if(holeGT)
		{
			holeGT.text = holeIndex.ToString();
		}
		
		
		
		//display the number of strokes.
		int nomStrokes = m_gameScript.getNomStrokes();		
		if(scoreGT)
		{
			scoreGT.text = nomStrokes.ToString();
		}

		if(Input.GetKeyDown(KeyCode.Escape) && m_gameover==false && m_gamestart)
		{
			onPause();
		}
	}
}
