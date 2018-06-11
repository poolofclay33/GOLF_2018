//#define PHOTON_MULTIPLAYER
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ConnectManager : MonoBehaviour 
{
#if PHOTON_MULTIPLAYER
	private PhotonView m_photonView;

	/// <summary>
	/// The maximum number of players we want in the rooms.
	/// </summary>
	private static int MAX_NOM_PLAYERS = 2;


	/// <summary>
	/// The game version.
	/// </summary>
	public string gameVersion = "2.5f";
	/// <summary>
	/// The level to load.
	/// </summary>
	private static int m_levelToLoad = 1;

	/// <summary>
	/// The number of players that are finished.
	/// </summary>
	private int m_playerFinishedCount = 0;


	/// A dictionary to hold the players score.
	private Dictionary<int,int> m_playerScores = new Dictionary<int, int>();

	//an instance of the connectmanager
	private static ConnectManager instance;

	//our we in single player mode
	private bool m_startSinglePlayer=false;

	//are we in multiplayer mode
	private bool m_startMultiPlayer=false;

	//have we started the hole
	private bool m_startedHole = false;

	#region shotclock Variables
	//do want to use the shot clock
	public bool useShotClock = true;
	/// The maximum time you give the players beefore they get a max score.
	private int m_shotClock;
	//the time for the shot clock
	public int shotClock = 60;
	//the gui text for the shot clock
	private GUIText m_shoClockGUIText;
	#endregion


	public static ConnectManager Instance
	{
		get
		{
			if(instance==null){
				instance = (ConnectManager)FindObjectOfType(typeof(ConnectManager));
			}
			return instance;
		}
	}
	public void Awake()
	{
		Debug.Log ("awake in scene " + Application.loadedLevelName);
		PhotonNetwork.isMessageQueueRunning = true;
		GameObject go = GameObject.Find("TheMainMenu");
		if(go)
		{
			go.transform.localPosition = Vector3.zero;
		}
		PhotonNetwork.isMessageQueueRunning = true;
		m_photonView = (PhotonView)gameObject.GetComponent<PhotonView>();
		DontDestroyOnLoad(gameObject);

		///lets get our shotclock gui text.
		m_shoClockGUIText = gameObject.GetComponentInChildren<GUIText>();
	}

	public void OnEnable()
	{
		GolfManager.onFinishHole 	+= onFinishHole;
		GolfManager.onGetOtherScore += onGetOtherScore;
		GolfManager.onLevelToLoad 	+= onLoadLevel;
 		GolfManager.onSetPlayersScore += onSetPlayersScore;
	}

	public void OnDisable()
	{
		GolfManager.onFinishHole 		-= onFinishHole;
		GolfManager.onGetOtherScore 	-= onGetOtherScore;
		GolfManager.onLevelToLoad 		-= onLoadLevel;
		GolfManager.onSetPlayersScore 	-= onSetPlayersScore;
	}


	//someone loaded the level, was it the master client, if so lets move to the next one. 
	void OnLevelWasLoaded(int level){
		if(level>0 )
		{

			if(PhotonNetwork.isMasterClient)
			{
				m_photonView.RPC ("startHoleRPC",PhotonTargets.AllBuffered);
			}
		}
	}
	[RPC]
	public void startHoleRPC()
	{
		Debug.Log ("startHole");
		//lets send our start hole.
		m_startedHole = true;

		GolfManager.startHole();

		//only use the shot clock if we want to use it and the player is online with another player.
		if(useShotClock && PhotonNetwork.offlineMode==false)
		{
			if(PhotonNetwork.isMasterClient)
			{
				m_shotClock = shotClock;
				stopClock();
				StartCoroutine(ShotClockIE());
			}
		}
	}
	public void stopClock()
	{
		//okay we are going to stop all coroutines...
		StopAllCoroutines();
		m_shoClockGUIText.text="";
	}
	public IEnumerator ShotClockIE()
	{
		m_photonView.RPC("setTimeRPC",PhotonTargets.All,m_shotClock);

		while(m_shotClock>0)
		{

			yield return new WaitForSeconds(1);
			m_shotClock--;
			m_photonView.RPC("setTimeRPC",PhotonTargets.All,m_shotClock);
		}
		m_shoClockGUIText.text="";

		m_photonView.RPC("timesUpRPC",PhotonTargets.All);
	}
	[RPC]
	public void timesUpRPC()
	{
		GolfManager.timesUp();
	}
	[RPC]
	public void setTimeRPC(int timeLeft)
	{
		if(m_shoClockGUIText)
		{
			m_shoClockGUIText.text  = timeLeft.ToString();
		}
	}


	[RPC]
	public void increaseFinishCountRPC()
	{
		m_playerFinishedCount++;
		Debug.Log ("m_playerFinishedCount" + m_playerFinishedCount);
		
		//if we are the master client lets increase our playerfinished count.
		if(PhotonNetwork.isMasterClient)
		{
			
			//if our player finished count is to the numbers of players in the room, then we can trigger the event
			if(m_playerFinishedCount == PhotonNetwork.room.playerCount)
			{
				m_photonView.RPC ("allPlayersFinishedRPC",PhotonTargets.All);
			}
		}
	}
	/// <summary>
	/// A player has finished the hole. We set the players score in the score state class.
	/// </summary>
	public void onFinishHole()
	{
		m_startedHole = false;
		m_photonView.RPC ("increaseFinishCountRPC",PhotonTargets.All);
	}

	/// <summary>
	/// Alls the players are finished
	/// </summary>
	[RPC]
	public void allPlayersFinishedRPC()
	{
		stopClock();
		GolfManager.everyoneFinishedHole();
		m_playerFinishedCount=0;

	}






	/// <summary>
	/// Handles the single player.
	/// Disconnect from photon if connected, turn offline mode ot true, leave any room, join a random room.
	/// </summary>
	public  void handleSinglePlayer()
	{
		if(PhotonNetwork.connected)
		{
			PhotonNetwork.Disconnect();
		}
		PhotonNetwork.offlineMode = true;
		PhotonNetwork.automaticallySyncScene=false;
		if(PhotonNetwork.room!=null)
		{
			PhotonNetwork.LeaveRoom();
		}
		PhotonNetwork.JoinRandomRoom();
		m_startSinglePlayer=true;
		startGame();
	}
	/// <summary>
	/// Handles the multiplayer.
	/// </summary>
	public void handleMultiplayer()
	{
		if(PhotonNetwork.connected)
		{
			PhotonNetwork.Disconnect();
		}
		PhotonNetwork.automaticallySyncScene=true;
		if(PhotonNetwork.room!=null)
		{
			PhotonNetwork.LeaveRoom();
		}
		PhotonNetwork.offlineMode = false;
		m_startMultiPlayer=true;
		m_startSinglePlayer=false;

		connect();
	}


	/// <summary>
	/// Attempt to join a random room
	/// </summary>
	void OnJoinedLobby()
	{
		if(m_startMultiPlayer)
		{
			PhotonNetwork.JoinRandomRoom();
		}
	}
	
	/// <summary>
	/// We couldnt join a room, lets try making our own room.
	/// </summary>
	void OnPhotonRandomJoinFailed()
	{
		if(m_startMultiPlayer)
		{
			PhotonNetwork.CreateRoom(null,true,true,MAX_NOM_PLAYERS);
		}
	}

	private bool m_started=false;
	public void Update()
	{

		if(PhotonNetwork.room!=null && m_startMultiPlayer)
		{
			if(PhotonNetwork.room.playerCount==PhotonNetwork.room.maxPlayers && PhotonNetwork.room.playerCount>1)
			{
				PhotonNetwork.room.open=false;
				m_startMultiPlayer=false;
				handleMultiplayerStart();
			}
		}
	}


	public void handleMultiplayerStart()
	{
		//if we have a photonview, and we have are the master client lets tell all players that we want to start -- using an RPC call.
		if(m_photonView)
		{
			m_photonView.RPC ("handleMultiplayerStartRPC",PhotonTargets.All);
		}
	}

	/// <summary>
	/// tells all the players that we want to start.
	/// </summary>
	[RPC]
	public void handleMultiplayerStartRPC()
	{
		if(PhotonNetwork.isMasterClient)
		{
			startGame();
		}
	}

	/// <summary>
	/// Connect to the network, if we are already in a room leave that room.
	/// </summary>
	void connect () {
		if(PhotonNetwork.connected==false)
		{
			if(PhotonNetwork.room!=null)
			{
				PhotonNetwork.LeaveRoom();
			}
			PhotonNetwork.ConnectUsingSettings(gameVersion);
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
	}


	/// We got disconnected from photon, you can do what you want here but we are going to keep it simple we are simply going to reload this level.
	void OnDisconnectedFromPhoton()
	{
		stopClock();
		//you might want to increase the losses when this happens. 
		if(Application.loadedLevel>0)
		{
			//we got disconnected, lets simply kick him back to the main menu.
			loadLevel(0);
		}


	}
	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		stopClock();
		//if our opponent was disconnected lets simply kick us back to the main menu, now you might want to give him a win.
		if(Application.loadedLevel>0)
		{
			if(player.ID != PhotonNetwork.player.ID)
			{
				loadLevel(0);
			}
		}


	}

	public void onLoadLevel(int level, int playerID)
	{
		if(PhotonNetwork.player.ID==playerID)
		{
			loadLevel(level);
		}
	}
	private IEnumerator MoveToGameScene(int index)
	{
		yield return 0;
		//lets load the level.
		if(PhotonNetwork.offlineMode==false)
		{
			PhotonNetwork.isMessageQueueRunning=false;

			PhotonNetwork.LoadLevel(index);
		}else{
			Application.LoadLevel(index);
		}
	}
	public   void loadLevel(int index)
	{
		if(Application.loadedLevel!=index)
		{
			StartCoroutine(MoveToGameScene(index));
		}
	}

	/// <summary>
	/// We want to start our game.
	/// </summary>
	public   void startGame()
	{
		Debug.Log ("startGame");
		if(PhotonNetwork.room!=null)
		{
			PhotonNetwork.room.open = false;
		}
		loadLevel(m_levelToLoad);
	}
	//lets set the players score.
	public void onSetPlayersScore(int playerIndex, int score)
	{
		m_photonView.RPC("setScoreRPC",PhotonTargets.All,playerIndex,score);
	}
	//lets set the players score
	[RPC]
	public void setScoreRPC(int playerIndex, int score)
	{
		Debug.Log ("setScoreRPC PlayerIndex " + playerIndex + " score " + score);
		
		m_playerScores[playerIndex] = score;
	}
	
	public int onGetOtherScore(int playerIndex)
	{
		int rc = 0;
		if(m_playerScores.ContainsKey(playerIndex))
		{
			rc = m_playerScores[playerIndex];
		}
		return rc;
	}


	public bool getStartedHole()
	{
		return m_startedHole;
	}
#endif
}
