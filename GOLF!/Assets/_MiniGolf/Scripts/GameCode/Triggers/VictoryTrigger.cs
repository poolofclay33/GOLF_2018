using UnityEngine;
using System.Collections;


/// <summary>
/// The victory trigger is placed on the hole. 
///	When the ball falls in the hole, it will call the gamescript victory function.
/// </summary>
public class VictoryTrigger : MonoBehaviour 
{
	#region variables
	//a ref player object
	private GameObject m_playerObj;
	
	//a ref to the ballscript
	private BallScript m_ballScript;

	#endregion
	
	public void Start()
	{
		//gets the player object
		m_playerObj = GameObject.FindWithTag("Player");
		
		//gets teh ball script
		if(m_playerObj)
		{
			m_ballScript = m_playerObj.GetComponent<BallScript>();// typeof(BallScript));
		}

	}
	public void OnTriggerEnter (Collider col)
	{
		//the player object has entered into the hole
	//	Debug.Log ("Victory"+col.gameObject);
		if(col.gameObject == m_playerObj)
		{
//			Debug.Log ("Victory2");
			//call the victory trigger.
			if(m_ballScript)
			{
				m_ballScript.victory();

				BaseGameManager.gameover(true);
				GameManager.enterState( GameScript.State.SHOWSCORE.ToString());
			}
		}
	}
	
}
