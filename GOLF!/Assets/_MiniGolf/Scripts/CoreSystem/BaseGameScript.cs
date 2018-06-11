using UnityEngine;
using System.Collections;
/// <summary>
/// Base game script.
/// </summary>
public class BaseGameScript : MonoBehaviour 
{
	/// <summary>
	/// The current number of points
	/// </summary>
	protected int m_points = 0;
	
	/// <summary>
	/// The current round.
	/// </summary>
	protected int m_round = 1;
	
	/// <summary>
	/// The deafeat light intensity.
	/// </summary>
	public float deafeatLightIntensity = 1f;
	
	/// <summary>
	/// The deafeat light intensity.
	/// </summary>
	public float victoryLightIntensity = 1f;
	
	/// <summary>
	/// The next round Audio clip.
	/// </summary>
	public AudioClip nextRoundAC;
	
	/// <summary>
	/// The next round floating text time to live.
	/// </summary>
	public float nextRoundTTL = 1;
	
	/// <summary>
	/// The floating text gameObject.
	/// </summary>
	public GameObject floatingTextGO;
	
	/// <summary>
	/// The round prefix.
	/// </summary>
	public string roundPrefix = "Round:";
	
	
	/// <summary>
	/// The audio to play when an enemy dies.
	/// </summary>
	public AudioClip onEnemyDeathAC;
	
	/// <summary>
	/// The sound to play when the player loses
	/// </summary>
	public AudioClip onDefeatAC;
	
	/// <summary>
	/// The sound to play when the player wins
	/// </summary>
	public AudioClip onVictoryAC;
	

	/// <summary>
	/// The ammomount of kills before saying a 1 liner.
	/// </summary>
	public int oneLinerKillLimit = 3;
	
	
	public int m_oneLinerKillLimit = 0;
	
	public string victorySTR = "Victory!";
	public string defeatSTR = "Defeat!";
	
	/// <summary>
	/// The ref to the one liners.
	/// </summary>
	protected bool m_gameover=false;
	private bool m_victory = false;
	
	/// <summary>
	/// The target framerate.
	/// </summary>
	public int framerate = 60;
	
	public int initalGold = 100;
	protected float m_gold;

	private AudioClip m_onHealthPickUpAC;
	private AudioClip m_onManaPickUpAC;
	private AudioClip m_onGemCollectAC;
	
	/// <summary>
	/// The score G.
	/// </summary>
	public GUIText scoreGT;
	/// <summary>
	/// The score prefix.
	/// </summary>
	public string scorePrefix = "Score: ";
	/// <summary>
	/// The score leading zeroes.
	/// </summary>
	public string scoreLeadingZeroes = "0000";
	
	/// <summary>
	/// The on gem collect A.
	/// </summary>
	public AudioClip onGemCollectAC;
	/// <summary>
	/// The on gem collect message.
	/// </summary>
	public string onGemCollectMessage = "Gem Collected!";
	
	public int gemBonus = 100;
	

	protected bool m_started = false;
	public void Awake()
	{

		Application.targetFrameRate = framerate;
		m_gold = initalGold;
		BaseGameManager.setNomEnemies(0);
		Time.timeScale=0;
	}
	//private NoiseEffect m_noiseEffect;
	public bool wantToDisableGrayScaleOnStart = true;
//	protected GrayscaleEffect m_grayScale;
	public virtual void Start()
	{
		//m_grayScale = (GrayscaleEffect)Object.FindObjectOfType(typeof(GrayscaleEffect));

		//if(m_grayScale==null){
		//	m_grayScale = Camera.main.gameObject.AddComponent<GrayscaleEffect>();
		//	m_grayScale.shader = Shader.Find("Hidden/Grayscale Effect");			
		//}

		
		myStart();
		
	}

	public virtual void myStart(){
	}
	
	public virtual void OnEnable()
	{
		BaseGameManager.onPlayerHit += onPlayerHit;
		BaseGameManager.onNextRound += onNextRound;
		BaseGameManager.onPushString += onPushString;
		BaseGameManager.onGameOver += onGameOver;
		BaseGameManager.onGameStart += onGameStart;
		BaseGameManager.onAddPoints+=onAddPoints;

		
	}
	public virtual void OnDisable()
	{
		BaseGameManager.onPlayerHit -= onPlayerHit;
		BaseGameManager.onNextRound -= onNextRound;
		BaseGameManager.onGameOver -= onGameOver;
		BaseGameManager.onGameStart -= onGameStart;
		BaseGameManager.onAddPoints-=onAddPoints;

		BaseGameManager.onPushString -= onPushString;
	}	
	public void setPointsGT(int points)
	{
		if(scoreGT)
		{
			scoreGT.text = scorePrefix + " " + m_points.ToString(scoreLeadingZeroes);
		}
	}	
	public void onPushString(string str)
	{
		pushText(str);
	}
	public void onGemCollect()
	{
		playAudioClip(m_onGemCollectAC);
		pushText( onGemCollectMessage );
		m_points += gemBonus;
		setPointsGT( m_points );
	}		
	public virtual void onAddPoints(int points)
	{
		m_points += points;
	}

	public virtual void onGameStart()
	{
		if(wantToDisableGrayScaleOnStart)
		{
			//if(m_grayScale)
			{
			//	m_grayScale.enabled = false;
			}

		}
		Time.timeScale=1;
		m_started=true;
	}
	
	

	public void pushText(string str)
	{

	}
	public virtual void onNextRound(int round)
	{
		m_round++;
		pushText(roundPrefix + round);
		if(GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().PlayOneShot( nextRoundAC );
		}
	}
	public void hideGUITexts()
	{
		GUIText[] texts = (GUIText[])GameObject.FindObjectsOfType(typeof(GUIText));
		for(int i=0; i<texts.Length; i++)
		{
			texts[i].gameObject.SetActive(false);
		}
	}
	public virtual void onGameOver(bool vic)
	{
		hideGUITexts();
		m_gameover = true;
		m_victory = vic;
		//if(m_grayScale)
		{
			//m_grayScale.enabled = true;
		}

			
		if(vic)
		{
			playAudioClip(onVictoryAC);
			setLightIntensity(victoryLightIntensity);
		}else
		{
			playAudioClip(onDefeatAC);
			setLightIntensity(deafeatLightIntensity);
		}
	}
	public virtual void setLightIntensity(float intensity)
	{
		Light l0 = (Light)GameObject.FindObjectOfType(typeof(Light));
		if(l0)
		{
			l0.intensity = intensity;
		}
	}
	public virtual void onPlayerHit(float playerHealthAsScalar)
	{
		
	}
	
	public void playAudioClip(AudioClip ac)
	{
		if(GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().PlayOneShot( ac );
		}
	}
	
	public string getResultGameString()
	{
		string str = defeatSTR;
		if(m_victory)
		{
			str = victorySTR;
		}
		
		str += "\n\n";
		str += getResultValues();
		return str;
	}
	public virtual string getResultValues()
	{
		return "Score: " + m_points.ToString("0000");
	}
	public virtual bool isPlayState()
	{
		return true;
	}
	public int getGold()
	{
		return (int)m_gold;
	}
	public virtual void addGold(int gold)
	{
		m_gold += gold;
	
	}
}
