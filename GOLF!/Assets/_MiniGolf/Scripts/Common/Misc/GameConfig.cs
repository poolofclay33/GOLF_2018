using UnityEngine;
using System.Collections; 
/// <summary>
/// Game config.
/// </summary>
public class GameConfig : MonoBehaviour
{

	
	static int m_paused = 0;
	///is the game paused
	public static bool getPaused()
	{
		return m_paused==1;
	}
	
	///set the game to paused or unpaused
	public static void setPaused(bool paused)
	{
		m_paused = (paused) ? 1 : 0;
		
		if(m_paused==1)
		{
			Time.timeScale = 0.0f;
			
		}else{
			Time.timeScale = 1f;
		}
	}

	///toggle the game pause state	
	public static void togglePause()
	{
		m_paused ^= 1;
		setPaused(m_paused==1);
	}
		
};