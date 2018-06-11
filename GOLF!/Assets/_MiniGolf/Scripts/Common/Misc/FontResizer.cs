#define MY_IPHONE_BUILD  
using UnityEngine;
using System.Collections;

public class FontResizer : MonoBehaviour {
	
	private static Vector2 SCALAR = Vector2.zero;
	public void Start()
	{

			SCALAR = Constants.getScreenScalar();

			resizeGUITexts( Constants.getScreenScalar() );	

	}
	
	void resizeGUITexts(Vector2 vec)
	{
		GUIText[] guiTexts = ( GUIText[])GameObject.FindObjectsOfType(typeof(GUIText));	
		for(int i=0; i<guiTexts.Length; i++)
		{
			Misc.resizeGUIText(guiTexts[i],vec);
			Misc.changeFont(guiTexts[i],vec);

		}
		
	}
	
}