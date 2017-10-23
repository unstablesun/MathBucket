using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour 
{
	public static bool sComingFromGame = false;


	public enum eGameType 
	{
		practice,
		fuelEasy,
		fuelHard,
		fuelGenius,
	};
	private static eGameType _gameType = eGameType.practice;

	void Start () 
	{
		//PropellerSDK.SyncChallengeCounts ();
		//PropellerSDK.SyncTournamentInfo ();

		/*
		FuelHandler.Instance.GetEvents ();
		
		if (sComingFromGame == true && _gameType != eGameType.practice) 
		{
			FuelHandler.Instance.SendProgress (2, 1);
			FuelHandler.Instance.LaunchCompeteDashBoard ();
			sComingFromGame = false;
		}
		*/
	}
	
	void Update () 
	{
	
	}


	public void LaunchPractice () 
	{
		_gameType = eGameType.practice;
		Application.LoadLevel("PlayField");
	}
	
	public void LaunchEasy () 
	{
		_gameType = eGameType.fuelEasy;
		//FuelHandler.Instance.LaunchCompeteDashBoard ();

		LoadGame ();
	}

	public void LoadGame () 
	{
		Application.LoadLevel("PlayField");
	}

	public void LaunchMissionPopup () 
	{
		Application.LoadLevelAdditive("missionPopup");
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
