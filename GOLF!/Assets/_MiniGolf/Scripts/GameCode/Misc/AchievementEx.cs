//#define GOT_PRIME31_GAMECENTER


using UnityEngine;
using System.Collections;

/// <summary>
/// Achivement ex used for the golf game-gamecenter.
/// </summary>
[System.Serializable]
public class AchivementEx
{
	/// <summary>
	/// The index of the course.
	/// </summary>
	public int courseIndex = 0;
	/// <summary>
	/// The minimum score to get that achievement
	/// </summary>
	public int minScore = 0;
	/// <summary>
	/// The max score to get that achivement.
	/// </summary>
	public int maxScore = 36;
	
	/// <summary>
	/// The achivement ID
	/// </summary>
	public string achivementID = "";

	
	public static AchivementEx[] getAchivements(AchivementEx[] achivements,
		int score, int courseIndex)
	{
		AchivementEx[] rc = null;
		int achivementCount=0;
		for(int i=0; i<achivements.Length; i++)
		{
			if(achivements[i].courseIndex == courseIndex && 
				score >= achivements[i].minScore && 
				score <=achivements[i].maxScore)
			{
				achivementCount++;
			}
		}
		if(achivementCount>0)
		{

			rc = new AchivementEx[achivementCount];
						achivementCount=0;
			for(int i=0; i<achivements.Length; i++)
			{
			if(achivements[i].courseIndex == courseIndex && 
				score >= achivements[i].minScore && 
				score <=achivements[i].maxScore)
				{
					rc[achivementCount++] = achivements[i];
				}
			}
		}
		return rc;
	}
				
}