using UnityEngine;
using System.Collections;

/// <summary>
/// Water trigger.
/// </summary>
public class WaterTrigger : MonoBehaviour 
{
	#region variables
	//a ref to the player object
	private GameObject m_playerObj;
	//a ref to the ballscript
	private BallScript m_ballScript;
	#endregion
	
	public void Start()
	{		
		
		//get the player object
		GameObject go = GameObject.FindWithTag("Player");
		if(go)
		{
			
			//save a ref of the players gameobject
			m_playerObj=go;
			
			//get the players ballscript
			m_ballScript = m_playerObj.GetComponent<BallScript>();
		}
		
	}

	public void OnTriggerEnter (Collider col)
	{
		if(col.gameObject == m_playerObj)
		{
			//ensure there is a ballscript object and then call its fallout funciton.
			if(m_ballScript)
			{
				
				m_ballScript.fallout();
			}
		}
	}
	
}
