//#define PHOTON_MULTIPLAYER


using UnityEngine;
using System.Collections;


public class NetworkCreate : MonoBehaviour {
	public string gameVersion = "2.5";
	public GameObject mainMenu;
	public GameObject connectManager;
	public static GameObject K_CONNECTMANAGER;

	
	public void Awake()
	{
#if PHOTON_MULTIPLAYER
		init();
#else
		GameObject go = GameObject.Find("TheMainMenu");
		if(go)
		{
			go.transform.localPosition = Vector3.zero;
		}
		OnFailedToConnectToPhoton();
#endif
	}
	public void OnFailedToConnectToPhoton()
	{
		if(connectManager)
		{
			K_CONNECTMANAGER = connectManager;
			connectManager.SetActive(true);
		}
		Destroy(gameObject);
	}
	#if PHOTON_MULTIPLAYER


	public void init()
	{


		//if were failed to connect to photon we used our scene created object, so destroy it normally.
		if(K_CONNECTMANAGER)
		{
			Destroy(K_CONNECTMANAGER);
		}

		//if we are in a room lets leave the room
		if(PhotonNetwork.inRoom)
		{
			PhotonNetwork.LeaveRoom();
		}

		//if we are connected lets diconnect
		if(PhotonNetwork.connected)
		{
			PhotonNetwork.Disconnect();
		}

		//lets not clean up our objects, because we are switching scenes we want to keep them.
		PhotonNetwork.autoCleanUpPlayerObjects=false;
		if(PhotonNetwork.connected==false)
		{
			PhotonNetwork.ConnectUsingSettings(gameVersion);
		}
	}

	//we failed to connect to photon
	//so lets use our connectmanager -- photonview that was created in the scene.


	/// <summary>
	/// We joined the lobby, lets make a room for only us.
	/// </summary>
	void OnJoinedLobby()
	{
		PhotonNetwork.CreateRoom(null,true,true,1);

	}

	/// <summary>
	/// We joined a room, now lets simply create our connectmanager script using photon network. 
	/// </summary>
	void OnJoinedRoom() {

		if(connectManager)
		{
			K_CONNECTMANAGER = connectManager;
			connectManager.SetActive(true);
		}
		Destroy(GameObject.Find ("FailedToConnect"));

		Destroy(gameObject);
	}
#endif

}
