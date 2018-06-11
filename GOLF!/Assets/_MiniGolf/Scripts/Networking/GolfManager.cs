using UnityEngine;
using System.Collections;

public class GolfManager 
{
	/// <summary>
	/// Called when we want to start the hole -- lets get out of the init state.
	/// </summary>
	public delegate void OnStartHole();
	public static event  OnStartHole onStartHole;
	public static void startHole()
	{
		if(onStartHole!=null)
		{
			onStartHole();
		}
	}
	/// <summary>
	/// Called when the player finishes the hole.
	/// </summary>
	public delegate void OnFinishHole();
	public static event  OnFinishHole onFinishHole;
	public static void finishHole()
	{
		if(onFinishHole!=null)
		{
			onFinishHole();
		}
	}
	/// <summary>
	/// Called when the player finishes the hole.
	/// </summary>
	public delegate void OnEveryoneFinishedHole();
	public static event  OnEveryoneFinishedHole onEveryoneFinishedHole;
	public static void everyoneFinishedHole()
	{
		if(onEveryoneFinishedHole!=null)
		{
			onEveryoneFinishedHole();
		}
	}


	/// <summary>
	/// Called when the player finishes the hole.
	/// </summary>
	public delegate void OnLoadLevel(int levelToLoad, int playerID);
	public static event  OnLoadLevel onLevelToLoad;
	public static void loadLevel(int levelToLoad, int playerID)
	{
		if(onLevelToLoad!=null)
		{
			onLevelToLoad(levelToLoad,playerID);
		}
	}
	/// <summary>
	/// Called when we want to set the other players score.
	/// </summary>
	public delegate void OnSetPlayersScore(int playerIndex,int score);
	public static event  OnSetPlayersScore onSetPlayersScore;
	public static void setPlayersScore(int playerIndex,int score)
	{
		if(onSetPlayersScore!=null)
		{
			onSetPlayersScore(playerIndex, score);
		}
	}
	/// <summary>
	/// Called when we want to get the other players score.
	/// </summary>
	public delegate int OnGetOtherScore(int playerIndex);
	public static event  OnGetOtherScore onGetOtherScore;
	public static int getOtherScore(int playerIndex)
	{
		int rc = -1;
		if(onGetOtherScore!=null)
		{
			rc = onGetOtherScore(playerIndex);
		}
		return rc;
	}
	/// <summary>
	/// Called when we run out of time
	/// </summary>
	public delegate void OnTimesUp();
	public static event  OnTimesUp onTimesUp;
	public static void timesUp()
	{
		int rc = -1;
		if(onTimesUp!=null)
		{
			onTimesUp();
		}
	}
}
