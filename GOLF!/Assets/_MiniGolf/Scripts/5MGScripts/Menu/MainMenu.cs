using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace FMG
{
	public class MainMenu : MonoBehaviour {
		public GameObject mainMenu;
		public GameObject lobbyState;
		public GameObject optionsMenu;
		public GameObject creditsMenu;

		public bool useExitButton = true;

		public GameObject exitButton;


		public void Awake()
		{
			if(useExitButton==false)
			{
				exitButton.SetActive(false);
			}
		}

		public void onCommand(string str)
		{
			Debug.Log ("onCommand"+str);
			if(str.Equals("Start"))
			{

			}

			if(str.Equals("Lobby"))
			{
				Constants.fadeInFadeOut(lobbyState,mainMenu);
				
			}
			if(str.Equals("LobbyBack"))
			{
				Constants.fadeInFadeOut(mainMenu,lobbyState);
				
			}

			if(str.Equals("Exit"))
			{
				Application.Quit();
			}
			if(str.Equals("Credits"))
			{
				Constants.fadeInFadeOut(creditsMenu,mainMenu);

			}
			if(str.Equals("CreditsBack"))
			{
				Constants.fadeInFadeOut(mainMenu,creditsMenu);
			}

			
			if(str.Equals("OptionsBack"))
			{
				Constants.fadeInFadeOut(mainMenu,optionsMenu);

			}
			if(str.Equals("Options"))
			{
				Constants.fadeInFadeOut(optionsMenu,mainMenu);
			}


		}
	}
}
