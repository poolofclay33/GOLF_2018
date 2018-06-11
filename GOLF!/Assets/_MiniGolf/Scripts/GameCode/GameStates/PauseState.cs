//#define PHOTON_MULTIPLAYER


using UnityEngine;
using System.Collections;
/// <summary>
/// Mini Golf Pause state.
/// </summary>
public class PauseState : MonoBehaviour {
	#region variables	
	public string resumeID = "Resume";
	public string giveUpID = "Give Up";
	public string returnToMainID = "Return To Main";

	/// a ref to our gamescript
	private GameScript m_gameScript;
	public GameObject mainMenuGO;
	private string m_prevState = "";

	public GameObject playPanel;
	public GameObject resultsPanel;
	public GameObject pausePanel;
	#endregion

	public void Start()
	{
		#if PHOTON_MULTIPLAYER
		if(PhotonNetwork.offlineMode==false)
		{
			Destroy(mainMenuGO);
		}
		#endif
		//get out gamescript component
		GameObject go = GameObject.FindWithTag("Game");
		if(go)
		{
			m_gameScript = go.GetComponent<GameScript>();
		}
	}
	public void resume()
	{
		m_gameScript.resume();
		FMG.Constants.fadeInFadeOut(playPanel,pausePanel);

	}
	public void giveUp()
	{
		//give up!
		m_gameScript.giveUp();
		FMG.Constants.fadeInFadeOut(resultsPanel,pausePanel);

	
	}

	public void returnToMain()
	{
		//return to the main menu!
		m_gameScript.returnToMain();
	}


}
