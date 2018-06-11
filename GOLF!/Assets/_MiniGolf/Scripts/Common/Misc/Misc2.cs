using UnityEngine;
using System.Collections;
/// <summary>
/// More misc functions
/// </summary>
public class Misc2  : MonoBehaviour {
	public static string MAX_LEVEL_STR = "XX_MAXX_LEVEL";	
	public static bool isMobilePlatform()
	{
		return RuntimePlatform.IPhonePlayer==Application.platform || Application.platform==RuntimePlatform.Android;
	}
	
	public static GameObject createAndDestroy(GameObject go,Vector3 pos, float ttl)
	{
		GameObject newgo =null;
		if(go)
		{
			newgo = (GameObject)Instantiate(go,pos,Quaternion.identity);
			if(newgo)
			{
				Destroy(newgo,ttl);
			}
		}	
		return newgo;
	}


	public static void rotateTowardsTarget(Vector3 targetPos,
											Transform t0,
											float rotateScalar)
	{
    	Quaternion targetRotation = 
			Quaternion.LookRotation(targetPos - t0.position, Vector3.up);
        t0.rotation =
			Quaternion.Slerp(t0.rotation, targetRotation,
							Time.deltaTime * rotateScalar);   
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

}
