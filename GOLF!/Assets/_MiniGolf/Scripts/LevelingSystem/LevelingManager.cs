using UnityEngine;
using System.Collections;

public class LevelingManager : MonoBehaviour {

	public static void increasePlayerPrefs(string id)
	{
		int index = PlayerPrefs.GetInt(id,0);
		PlayerPrefs.SetInt(id,index+1);
	}
	public static void increaseGamesPlayed(string prefix)
	{
		increasePlayerPrefs(LevelingStats.Stats.TOTAL_WINS.ToString());
		increasePlayerPrefs(prefix);
		
	}
	/// <summary>
	/// We might do something intersting here for example we could give them some experience everytime they win, less if they tie, and less if they win. Then you could a simple leveling system,
	/// its quite easy to add that to photon networking.
	/// </summary>
	public static string setResults(int totalScore)
	{
		string resultString = "";
		int otherID = Misc.getOtherPlayer();
		
		//lets get the other score.
		int otherScore = GolfManager.getOtherScore(otherID);
		//now you might want to do something here where you are going to save how many wins, how many loses, how many ties -- then have some sort of leveling system.
		if(totalScore < otherScore)
		{
			resultString = "You Won!";
			increaseGamesPlayed(LevelingStats.Stats.TOTAL_WINS.ToString());
		}
		if(totalScore == otherScore)
		{
			resultString = "You Tied!";
			increaseGamesPlayed(LevelingStats.Stats.TOTAL_TIES.ToString());
		}
		if(totalScore > otherScore)
		{
			resultString = "You Lost!";
			increaseGamesPlayed(LevelingStats.Stats.TOTAL_LOSES.ToString());
		}
		return resultString;
	}
}
