using UnityEngine;
using System.Collections;
/// <summary>
/// A simple Timer.
/// </summary>
public class Timer : MonoBehaviour{
	
	//the ammount of time remaining.
	private float m_timeRemaining;
	
	private float m_initalTime = 0;
	//has the timer expired
	private bool m_expired=false;
	
	//is the timer on
	private bool m_on = true;	
	
	public void init(float time)
	{
		m_timeRemaining = time;
		m_initalTime = time;
	}
	//reset the timer
	public float getTimeAsScalar()
	{
		return m_timeRemaining / m_initalTime;
	}
	
	//set wether the timer is on
	public void setOn(bool on)
	{
		m_on = on;
	}
	
	//is the timer on
	public bool isOn()
	{
		return m_on;
	}
	
	
	public void update(float dt)
	{
		
		if(isOn())
		{
			//decase teh time
			m_timeRemaining -= dt;
			
			//if there is time remaining turn off the timer
			if(m_timeRemaining<0)
			{
				m_timeRemaining=0;
				m_expired=true;
				setOn(false);
			}
		}
	}
	//add some time to the timer if the timer is on and the timer has not already expired
	public void addTime(float dt)
	{
		if(isOn() && m_expired==false){
			m_timeRemaining += dt;
		}
	}
	
	//return the ammount of time remaining.
	public float getTimeRemaining()
	{
		return m_timeRemaining;
	}
	
	//has the timer expired.
	public bool hasExpired()
	{
		return m_expired;
	}
	//return the timer as a string formated in the fashion 00:00
	public string getAsString()
	{
		float etime = getTimeRemaining();

		
		int rawMinutes = (int)etime / 60;
		
		int minutes = rawMinutes % 60;		
		int seconds = (int)etime % 60;
		
		string strMinutes = minutes.ToString();
		if(strMinutes.Length<2)
		{
			strMinutes = "0" + strMinutes;
		}
		string strSeconds = seconds.ToString();
		if(strSeconds.Length<2)
		{
			strSeconds = "0" + strSeconds;
		}
		return strMinutes + ":" + strSeconds;
	}

}
