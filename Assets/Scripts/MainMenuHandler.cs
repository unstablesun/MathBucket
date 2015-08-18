using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour 
{
	public static bool sComingFromGame = false;

	void Start () 
	{
		PropellerSDK.SyncChallengeCounts ();
		PropellerSDK.SyncTournamentInfo ();
		
		if (sComingFromGame == true) 
		{
			
			GameCommon.getFuelHandlerClass().LaunchPropeller();
			sComingFromGame = false;
		}

	}
	
	void Update () {
	
	}



	public void Launch () 
	{
		GameCommon.getFuelHandlerClass().Launch ();
	}

	public void RefreshChallengeCount(int challengeCount)
	{
		bool enabled = false;
		if (challengeCount > 0)
			enabled = true;
		
		GameObject go = GameObject.Find("Canvas");
		if (!go)
			return;
		
		Image[] images = go.GetComponentsInChildren<Image>();
		foreach (Image image in images)
		{
			Debug.Log ("image found = " + image.name);
			if(image.name == "AlertIcon")
			{
				image.enabled = enabled;
			}
			
		}
		
		Text[] texts = go.GetComponentsInChildren<Text>();
		foreach (Text text in texts)
		{
			Debug.Log ("image found = " + text.name);
			if(text.name == "AlertText")
			{
				text.enabled = enabled;
				text.text = challengeCount.ToString();
			}
			
		}
		
	}

	public void RefreshTournament(int running, string name, string startDate, string endDate)
	{
		bool enabled = false;
		if (running > 0)
			enabled = true;
		
		GameObject go = GameObject.Find("Canvas");
		if (!go)
			return;
		
		Image[] images = go.GetComponentsInChildren<Image>();
		foreach (Image image in images)
		{
			Debug.Log ("image found = " + image.name);
			if(image.name == "TrophyIcon")
			{
				image.enabled = enabled;
			}
			
		}
	}



	public void AddNotificationObservers()
	{
		//NotificationCenter.DefaultCenter.AddObserver(this, "RefreshChallengeCount");
	}
	
	public void RemoveNotificationObservers()
	{
		//NotificationCenter.DefaultCenter.RemoveObserver(this, "RefreshChallengeCount");
	}
	
	


}
