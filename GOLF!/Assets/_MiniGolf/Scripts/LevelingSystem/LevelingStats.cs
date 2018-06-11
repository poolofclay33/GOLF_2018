using UnityEngine;
using System.Collections;

public class LevelingStats : MonoBehaviour {
	public enum Stats
	{
		TOTAL_WINS,
		TOTAL_TIES,
		TOTAL_LOSES,
		TOTAL_GAMES
	};
	public Stats stats;
	public string prefix = "Total Wins: ";
	void Awake () {
		if(GetComponent<GUIText>())
		{
			GetComponent<GUIText>().text =prefix+ PlayerPrefs.GetInt(stats.ToString(),0).ToString();
		}
	}
	

}
