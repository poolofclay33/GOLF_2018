//#define PHOTON_MULTIPLAYER

using UnityEngine;
using System.Collections;
/// <summary>
///  misc functions
/// </summary>
public class Misc : MonoBehaviour {
	public static string MAX_LEVEL_STR = "XXX_MAXX_LEVEL";

	public static float MOBILE_SPAWN_DELAY_TIME_SCALAR  = 2f;
	public static float MOBILE_ASTEROID_MOVE_SCALAR = 0.5f;
		
	
	public static string START_ROUND_STR = "XX_START_ROUND";
	public static string GOLD_STR = "XX_GOLD";
	public static string START_SPAWNER_STR = "XX_START_SPAWNER";
		public static void setGold(int gold)
	{
		PlayerPrefs.SetInt(GOLD_STR,gold);
	}

	//lets get the other player 
	public static int getOtherPlayer()
	{
		int otherID = 0;
#if PHOTON_MULTIPLAYER
		for(int i=0; i<PhotonNetwork.playerList.Length; i++)
		{
			if(PhotonNetwork.playerList[i].ID != PhotonNetwork.player.ID)
			{
				otherID = PhotonNetwork.playerList[i].ID;
			
			}
		}
#endif
		return otherID;
	}
	//do we have more then 1 player in the room we need an opponet. 
	public static bool haveOpponent()
	{
		bool rc = false;
		#if PHOTON_MULTIPLAYER

		if(PhotonNetwork.room!=null)
		{
			rc = PhotonNetwork.room.playerCount>1;
		}
		#endif
		return rc;
	}

	public static int getGold()
	{
		return PlayerPrefs.GetInt(GOLD_STR,0);
	}
	
		public static void setIntValue(string str,int val)
	{
		PlayerPrefs.SetInt(str,val);
	}
	
	public static int getIntValue(string nom)
	{
		return PlayerPrefs.GetInt(nom,0);
	}
	
	public static void setStartSpawnerIndex(int startRound)
	{
		PlayerPrefs.SetInt(START_SPAWNER_STR,startRound);
	}
	
	public static int getStartSpawnerIndex()
	{
		return PlayerPrefs.GetInt(START_SPAWNER_STR,0);
	}
	
	
	public static void setStartRound(int startRound)
	{
		PlayerPrefs.SetInt(START_ROUND_STR,startRound);
	}
	
	public static int getStartRound()
	{
		return PlayerPrefs.GetInt(START_ROUND_STR,1);
	}
	
	public static void setChildrenActive(GameObject go,
											bool active)
	{
		Transform t0 = go.transform;
		int t= t0.childCount;
		for(int i=0; i<t; i++)
		{
			if(t0.gameObject!=go)
			{
				t0.gameObject.SetActive(active);
			}
		}
	}
	public static void SetActive(GameObject go,
											bool active)
	{
		Transform t0 = go.transform;
		int t= t0.childCount;
		for(int i=0; i<t; i++)
		{
			t0.gameObject.SetActive(active);
		}
	}
	public static string getPostFix(Vector2 vec)
	{
		string postFix="";
		if(vec.x==.5f)
		{
			postFix = "_smaller";
		}
		if(vec.x>=2f)
		{
			postFix = "_bigger";
		}
		return postFix;
		
	}
	public static Font getFont(string fontName,
	                           Vector2 vec)
	{
		Font rc = null;
		string postFix = getPostFix(vec);
		if(postFix!=null)
		{
			rc = Resources.Load("Fonts/" + fontName + postFix) as Font;
		}
		return rc;
	}
	public static void changeFont(GUIText gt,
	                              Vector2 vec)
	{
		Font newFont = getFont(gt.font.name,vec);
		if(newFont)
		{
			gt.font = newFont;
			gt.material = newFont.material;	
		}
	}
	public static void resizeGUIText(GUIText gt,
	                                 Vector2 vec)
	{
		if(gt!=null)
		{
			Vector2 v = gt.pixelOffset;
			v.x *= vec.x;
			v.y *= vec.y;		
			gt.pixelOffset = v;
		}
	}
	
	
	public static bool setMaxLevel(int maxLevel)
	{
		bool newMaxLevel = false;
		int curMax = getMaxLevel();
		if(maxLevel > curMax)
		{
			PlayerPrefs.SetInt(MAX_LEVEL_STR,maxLevel);
			newMaxLevel = true;
		}
		return newMaxLevel;
	}
	
	public static int getMaxLevel()
	{
		return PlayerPrefs.GetInt(MAX_LEVEL_STR,1);
	}
	public static bool isMobilePlatform()
	{
		return RuntimePlatform.IPhonePlayer==Application.platform || Application.platform==RuntimePlatform.Android;
	}
	public static void createAndDestroyGameObject (GameObject effectGO,
													Vector3 pos,
													float effectTTL) 
	{
		if(effectGO)
		{
			GameObject g0 = (GameObject)Instantiate( effectGO,pos,Quaternion.identity);
			if(g0)
			{
				Destroy(g0,effectTTL);
			}
		}
	
	}
	public static Component getComponentInChildrenNotSelf(Transform t1, string scriptName)
	{
		Component rc = null;
		for(int i=0; i<t1.childCount; i++)
		{
			Transform t0 = t1.GetChild(i);
			if(t0!=t1)
			{
				rc = t0.GetComponent(scriptName);
				i = t1.childCount;
			}
		}
		return rc;
	}
	public static void setChildrenActiveRecursively(Transform t1,bool state)
	{
		for(int i=0; i<t1.childCount; i++)
		{
			Transform t0 = t1.GetChild(i);
			if(t0!=t1)
			{
				t0.gameObject.SetActive(state);
			}
		}
	}
}
