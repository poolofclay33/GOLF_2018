using UnityEngine;
using System.Collections;
namespace FMG
	{
	public class GameMenu : MonoBehaviour {
		public GameObject pauseMenu;
		public GameObject gameMenu;	
		public GameObject resultMenu;
		private bool m_gameover=false;
		public void Update()
		{
			if(Input.GetButtonDown("PauseGame") && m_gameover==false)
			{
				Time.timeScale = 0;
				Constants.fadeInFadeOut(pauseMenu,gameMenu);

				pauseMenu.SetActive(true);
			}
		}

		public void onCommand(string str)
		{
			if(str.Equals("Restart"))
			{

				useFadeOut(Application.loadedLevel);
			}
			if(str.Equals("Unapuse"))
			{
				Time.timeScale = 1;
				Constants.fadeInFadeOut(gameMenu,pauseMenu);
				
			}			
			if(str.Equals("unlock"))
			{
				m_gameover=true;
				Time.timeScale = 1;
				Constants.fadeInFadeOut(resultMenu,gameMenu);

			}
			if(str.Equals("MainMenu"))
			{
				useFadeOut(1);
			}
			if(str.Equals("Next"))
			{
				int next = Application.loadedLevel+1;
				Debug.Log ("next " + next);
				useFadeOut(next);
			}

		}
		public void useFadeOut(int sceneToLoad)
		{
			Time.timeScale = 1;
			

			Application.LoadLevel(sceneToLoad);
		}

	}
}