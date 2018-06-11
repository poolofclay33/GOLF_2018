using UnityEngine;
using System.Collections;
/// <summary>
/// Base game manager.
/// </summary>
public class BaseGameManager  {
	

	public delegate void OnGameStart();
	public static event OnGameStart onGameStart;
	public static void startGame()
	{
		if(onGameStart!=null)
		{
			onGameStart();	
		}
	}
	
	public delegate void OnGamePause(bool pause);
	public static event OnGamePause onGamePause;
	public static void pauseGame(bool pause)
	{
		if(onGamePause!=null)
		{
			onGamePause(pause);	
		}
	}
	public delegate void OnSubmitState();
	public static event OnSubmitState onSubmitState;
	public static void submitSate()
	{
		if(onSubmitState!=null)
		{
			onSubmitState();	
		}
	}
	public delegate void OnGameOver(bool victory);
	public static event OnGameOver onGameOver;
	public static void gameover(bool victory)
	{
		if(onGameOver!=null)
		{
			onGameOver(victory);	
		}
	}
	public delegate void OnPlayerHit(float normalizedHealth);
	public static event OnPlayerHit onPlayerHit;
	public static void playerHit(float normalizedHealth)
	{
		if(onPlayerHit!=null)
		{
			onPlayerHit(normalizedHealth);	
		}
	}
	public delegate void OnAddPoints(int points);
	public static event OnAddPoints onAddPoints;
	public static void addPoints(int points)
	{
		if(onAddPoints!=null)
		{
			onAddPoints(points);	
		}
	}

	
	public delegate void OnNextRound(int round);
	public static event OnNextRound onNextRound;
	public static void nextRound(int round)
	{
		if(onNextRound!=null)
		{
			onNextRound(round);	
		}
	}
	public delegate void OnObjectEnterBounds(GameObject bp, string id);
	public static event OnObjectEnterBounds onObjectEntersBounds;
	public static void objectEntersBounds(GameObject bp, string id)
	{
		if(onObjectEntersBounds!=null)
		{
			onObjectEntersBounds(bp,id);	
		}
	}
	public delegate void OnButtonPress(string id);
	public static event OnButtonPress onButtonPress;
	public static void buttonPress(string id)
	{
		if(onButtonPress!=null)
		{
			onButtonPress(id);	
		}
	}
	public delegate void OnButtonTogglePress(string id, bool status);
	public static event OnButtonTogglePress onButtonTogglePress;
	public static void buttonTogglePress(string id, bool status)
	{
		if(onButtonTogglePress!=null)
		{
			onButtonTogglePress(id,status);	
		}
	}


	public delegate void OnPushString(string str);
	public static event OnPushString onPushString;
	public static void pushString(string str)
	{
		if(onPushString!=null)
		{
			onPushString(str);	
		}
	}
	public delegate void OnObjectExitsBounds(GameObject bp, string id);
	public static event OnObjectExitsBounds onObjectExitsBounds;
	public static void objectExitsBounds(GameObject bp, string id)
	{
		if(onObjectExitsBounds!=null)
		{
			onObjectExitsBounds(bp,id);	
		}
	}
	

	public delegate void OnGemCollect();
	public static event OnGemCollect onGemCollect;
	public static void gemCollect()
	{
		if(onGemCollect!=null)
		{
			onGemCollect();	
		}
	}
	public delegate void OnTimeOut();
	public static event OnTimeOut onTimeout;
	public static void timeout()
	{
		if(onTimeout!=null)
		{
			onTimeout();	
		}
	}
	public delegate void OnAddEnemy();
	public static event OnAddEnemy onAddEnemy;

	public delegate void OnRemoveEnemy();
	public static event OnRemoveEnemy onRemoveEnemy;

	private static int K_ENEMIES = 0;
	public static void setNomEnemies(int nomEnemies)	
	{
		K_ENEMIES = nomEnemies;

	}
	public static int addEnemy()
	{
		K_ENEMIES++;
		if(onAddEnemy!=null)
		{
			onAddEnemy();
		}
		return K_ENEMIES;
	}
	public static void removeEnemy()
	{
		K_ENEMIES--;
		if(onRemoveEnemy!=null)
		{
			onRemoveEnemy();
		}
	}
	public static int getNomEnemies()
	{
		return K_ENEMIES;
	}

	/*
	 * Called when there is a victory
	 */	
	public delegate void OnVictory();
	public static event OnVictory onVictory;
	public static void victory()
	{
		if(onVictory!=null)
		{
			onVictory();
		}
	}	


}
