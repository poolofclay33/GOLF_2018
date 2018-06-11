//#define PHOTON_MULTIPLAYER
using UnityEngine;
using System.Collections;

/*
 * Network events.
 */

public class NetworkEvents : MonoBehaviour
{

	
	public void OnEnable()
	{
		BaseGameManager.onButtonPress += onButtonPress;
	}
	public void OnDisable()
	{
		BaseGameManager.onButtonPress -= onButtonPress;
	}
	public void onButtonPress(string str)
	{
#if PHOTON_MULTIPLAYER
		if(ConnectManager.Instance==null)
		{
			return;
		}
		if(str.Equals("start"))
		{
			ConnectManager.Instance.handleSinglePlayer();
		}
		if(str.Equals("Multiplayer"))
		{	
			ConnectManager.Instance.handleMultiplayer();
		}
#else
		if(str.Equals("start"))
		{
			Application.LoadLevel(1);
		}
#endif
	}



}
