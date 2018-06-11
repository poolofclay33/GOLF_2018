//#define PHOTON_MULTIPLAYER
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FMG;
/// <summary>
/// Mini Golf Init state.
/// If its multiplayer its going to load it when all players are finished loading
/// If its singleplayer.
/// </summary>
public class InitState : MonoBehaviour {
	
	
	#region variables
	public Text holeGT;
	
	private GameScript m_gameScript;
	
	public GameObject startButtonGO;
	public GameObject waitGO;

	public GameObject introState;
	public GameObject playState;
	#endregion
	void Start()
	{
		
		m_gameScript = (GameScript)GameObject.FindObjectOfType(typeof(GameScript));		
		
		
		#if PHOTON_MULTIPLAYER
		
			//we are offline lets hide the start button, otherwise lets hide the wait button.
			if(PhotonNetwork.offlineMode==false)
			{
				startButtonGO.SetActive(false);
			}else{
				waitGO.SetActive(false);
			}
			if(ConnectManager.Instance && ConnectManager.Instance.getStartedHole())
			{
				waitGO.SetActive(false);
				
				startButtonGO.SetActive(true);
				
			}
		
		#else
		if(waitGO)
		{
			waitGO.SetActive(false);
		}
		#endif
		int holeIndex = m_gameScript.getHoleNomUsingCourse();
		holeGT.text = holeIndex.ToString();
		
		
	}
	public  void OnEnable()
	{
		GolfManager.onStartHole += onShowStart;
	}
	

	public  void OnDisable()
	{
		GolfManager.onStartHole -= onShowStart;
		
	}
	public void onShowStart()
	{
		if(waitGO)
		{
			waitGO.SetActive(false);
			startButtonGO.SetActive(true);
		}
	}

	/// <summary>
	/// We got a start message lets now show them the start button, hide the wait object
	/// </summary>
	public void onStart()
	{
		if(waitGO)
		{
			waitGO.SetActive(false);
		}

		startButtonGO.SetActive(true);
		FMG.Constants.fadeInFadeOut(playState,introState);
		BaseGameManager.startGame();
			
	}
}
