//#define PHOTON_MULTIPLAYER


using UnityEngine;
using System.Collections;
/// <summary>
/// Par script.
/// </summary>
public class ParScript : MonoBehaviour {
	/// <summary>
	/// The par for the hole
	/// </summary>
	public int par = 2;

	public void Awake(){
		#if PHOTON_MULTIPLAYER

		PhotonNetwork.isMessageQueueRunning=true;
#endif
	}
}
