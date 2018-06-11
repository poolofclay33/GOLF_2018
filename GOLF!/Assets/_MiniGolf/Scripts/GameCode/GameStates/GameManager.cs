using UnityEngine;
using System.Collections;
/// <summary>
/// Game manager for mini golf
/// </summary>
public static class GameManager  {

	public delegate void EnterState(string stateID);
	public static event EnterState onEnterState;
	
	public static void enterState(string stateID)
	{
		if(onEnterState!=null)
		{
			onEnterState(stateID);
		}
	}
	
	public delegate void OnNextScene();
	public static event OnNextScene onNextScene;
	
	public static void nextScene()
	{
		if(onNextScene!=null)
		{
			onNextScene();
		}
	}
}