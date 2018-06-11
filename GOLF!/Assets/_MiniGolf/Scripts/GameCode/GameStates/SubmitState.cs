//#define PHOTON_MULTIPLAYER

//#define GOT_PRIME31_GAMECENTER


using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FMG;
///this menu is displayed when the gamescript is in show score state
public class SubmitState : MonoBehaviour {
	
	#region variables
	///have we already subbmited the score
//	private bool m_submittedScore=false;
	///a ref to the gamescript
	private GameScript m_gameScript;
	
	///the name of the player!
	public string playerName = "InaneGamer";

	public string submitID = "Submit";
	public string returnToMainID = "MainMenu";

	public GameObject submitState;
	public GameObject scoreState;
	/// <summary>
	/// The results gui text.
	/// </summary>
	public Text resultsGT;
	public GameObject resultGO;
	/// <summary>
	/// The achivement ids
	/// </summary>
	public AchivementEx[] achivements;
	#endregion
	
	public void Start()
	{
		#if PHOTON_MULTIPLAYER

		//if we are offline or have no opponet then lets hide the results.
		if(PhotonNetwork.offlineMode || Misc.haveOpponent()==false)
		{
			resultGO.SetActive(false);
		}
#endif
		//get out gamescript component
		GameObject go = GameObject.FindWithTag("Game");
		if(go)
		{
			m_gameScript = go.GetComponent<GameScript>();
		}
	}
	
	public void onSubmitState()
	{
		FMG.Constants.fadeInFadeOut(submitState,scoreState);

	}

	public  void OnEnable()
	{
		BaseGameManager.onSubmitState += onSubmitState;
	}
	public  void OnDisable()
	{
		BaseGameManager.onSubmitState -= onSubmitState;
	}
	void submitScore(int playerScore)
	{
#if GOT_PRIME31_GAMECENTER
		AchivementEx[] achievementResults = AchivementEx.getAchivements(achivements,playerScore,Constants.getCourseIndex());
		if(achievementResults!=null)
		{
			for(int i=0; i<achievementResults.Length; i++)
			{
				GameCenterBinding.reportAchievement(achievementResults[i].achivementID,100f);
			}
		}
#endif
		
#if GOT_PRIME31_GAMECENTER

		GameCenterBinding.reportScore( playerScore, 
				leaderBoardIDs[Constants.getCourseIndex()] );
#endif
		
	}

	public void submitScore()
	{
		int totalScore = m_gameScript.getTotalScore();
	
	
		submitScore(totalScore);

	}

	public void return2Main()
	{
		m_gameScript.returnToMain();
	}

	public Text usernameGT;
	public Text totalScoreGT;
	public Text totalParGT;



	public  void Update()
	{

		int courseIndex = m_gameScript.getCourseIndex();
		string userName = playerName;
		int totalScore = m_gameScript.getTotalScore();
		int totalPar = m_gameScript.getTotalPar();
		#if PHOTON_MULTIPLAYER

		//if we are online, and we have an opponent lets show the results
		if(PhotonNetwork.offlineMode==false && Misc.haveOpponent())
		{
			if(m_oneTime==false)
			{
				resultsGT.text = LevelingManager.setResults(totalScore);
				m_oneTime=true;
			}
		}

#endif
		if(RuntimePlatform.IPhonePlayer == RuntimePlatform.IPhonePlayer)
		{
#if GOT_PRIME31_GAMECENTER
			userName = GameCenterBinding.playerAlias();
#endif
		}

		if(usernameGT)
			usernameGT.text = userName;
		if(totalScoreGT)
			totalScoreGT.text = totalScore.ToString();

		if(totalParGT)
			totalParGT.text = totalPar.ToString();

	}
	private bool m_oneTime =false;



}