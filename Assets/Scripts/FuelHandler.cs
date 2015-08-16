using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PropellerSDKSimpleJSON;
using System;

public class FuelHandler : MonoBehaviour 
{
	private bool m_initialized;
	private FuelListener m_listener;

	//private string m_tournamentID;
	//private string m_matchID;


	public const int MATCH_TYPE_SINGLE = 0;
	public const int MATCH_TYPE_MULTI = 1;

	private GameMatchData m_matchData;



	public struct GameMatchData 
	{
		public bool ValidMatchData { get; set; }
		public bool MatchComplete { get; set; }
		public int MatchType { get; set; }
		public int MatchRound { get; set; }
		public int MatchScore { get; set; }
		public int MatchMaxSpeed { get; set; }
		public string TournamentID { get; set; }
		public string MatchID { get; set; }
		public string YourNickname { get; set; }
		public string YourAvatarURL { get; set; }
		public string TheirNickname { get; set; }
		public string TheirAvatarURL { get; set; }
	}


	private void Awake ()
	{			
		if (!m_initialized) {
			GameObject.DontDestroyOnLoad (gameObject);
			
			if (!Application.isEditor) {
				// Initialize the Propeller SDK listener
				// reference for later use by the launch
				// methods.
				m_listener = new FuelListener ();
			}


			m_matchData = new GameMatchData ();
			m_matchData.ValidMatchData = false;
			m_matchData.MatchComplete = false;

			m_initialized = true;
		} else {
			GameObject.Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}





	public void LaunchPropeller ()
	{
		// Launches the Propeller SDK online experience.
		PropellerSDK.Launch (m_listener);
	}


	private void LaunchPropellerWithScore (long score)
	{
		Dictionary<string, object> matchResult = new Dictionary<string, object> ();
		//matchResult.Add ("tournamentID", m_tournamentID);
		//matchResult.Add ("matchID", m_matchID);
		
		// The raw score will be used to compare results
		// between match players. This should be a positive
		// integer value.
		matchResult.Add ("score", score);
		
		// Specify a visual score to represent the raw score
		// in a different format in the UI. If no visual score
		// is provided then the raw score will be used.
		matchResult.Add ("visualScore", score.ToString ());
		
		PropellerSDK.SubmitMatchResult (matchResult);

		// Re-launch the Propeller SDK online experience
		// to see who won or to play another match
		PropellerSDK.Launch (m_listener);

	}


	public void Launch ()
	{
		LaunchPropeller ();
	}



	public void LaunchMultiplayerGame(Dictionary<string, string> matchResult)
	{
		m_matchData.MatchType = MATCH_TYPE_MULTI;
		m_matchData.ValidMatchData = true;
		
		m_matchData.TournamentID = matchResult ["tournamentID"];
		m_matchData.MatchID = matchResult ["matchID"];
		
		// extract the params data
		string paramsJSON = matchResult ["paramsJSON"];
		JSONNode json = JSONNode.Parse (paramsJSON);
		
		m_matchData.MatchRound = json ["round"].AsInt;
		
		JSONClass you = json ["you"].AsObject;
		m_matchData.YourNickname = you ["name"];
		m_matchData.YourAvatarURL = you ["avatar"];
		
		JSONClass them = json ["them"].AsObject;
		m_matchData.TheirNickname = them ["name"];
		m_matchData.TheirAvatarURL = them ["avatar"];
		
		Debug.Log (	"__LaunchMultiplayerGame__" + "\n" +
		           "ValidMatchData = " + m_matchData.ValidMatchData + "\n" +
		           "TournamentID = " + m_matchData.TournamentID + "\n" +
		           "MatchID = " + m_matchData.MatchID + "\n" +
		           "MatchRound = " + m_matchData.MatchRound + "\n" +
		           "adsAllowed = " + "\n" +
		           "fairPlay = " + "\n" +
		           "YourNickname = " + m_matchData.YourNickname + "\n" +
		           "YourAvatarURL = " + m_matchData.YourAvatarURL + "\n" +
		           "TheirNickname = " + m_matchData.TheirNickname + "\n" +
		           "TheirAvatarURL = " + m_matchData.TheirAvatarURL + "\n" 
		           );
		
		
		m_matchData.MatchComplete = false;


		Application.LoadLevel("GamePlay");

		//NotificationCenter.DefaultCenter.PostNotification (getMainMenuClass(), "LaunchGamePlay");
	}



	public void SetMatchScore(int scoreValue)
	{
		Debug.Log ("SetMatchScore = " + scoreValue);
		
		m_matchData.MatchScore = scoreValue;

		sendMatchResult (m_matchData.MatchScore);
	}

	private void sendMatchResult (long score)
	{
		Debug.Log ("sendMatchResult");
		
		long visualScore = score;
		
		Dictionary<string, object> matchResult = new Dictionary<string, object> ();
		matchResult.Add ("tournamentID", m_matchData.TournamentID);
		matchResult.Add ("matchID", m_matchData.MatchID);
		matchResult.Add ("score", m_matchData.MatchScore);
		string visualScoreStr = visualScore.ToString();
		matchResult.Add ("visualScore", visualScoreStr);
		
		PropellerSDK.SubmitMatchResult (matchResult);
	}


	public void OnPropellerSDKChallengeCountUpdated (string count)
	{
		Debug.Log ("OnPropellerSDKChallengeCountUpdated = " + count);

		int countValue = 0;
		
		if (!int.TryParse (count, out countValue)) 
		{
			return;
		}

		getMainMenuClass().RefreshChallengeCount (countValue);

	}



	public void OnPropellerSDKTournamentInfo (Dictionary<string, string> tournamentInfo)
	{
		if ((tournamentInfo == null) || (tournamentInfo.Count == 0)) {
			// There is no tournament currently running or scheduled.
			// Display a regular multiplayer button.
			getMainMenuClass().RefreshTournament(0, " ", " ", " ");

		} else {
			// A tournament is currently running or is the
			// information for the next scheduled tournament.
			
			// Extract the tournament data.
			string name = tournamentInfo["name"];
			//string campaignName = tournamentInfo["campaignName"];
			//string sponsorName = tournamentInfo["sponsorName"];
			string startDate = tournamentInfo["startDate"];
			string endDate = tournamentInfo["endDate"];
			//string logo = tournamentInfo["logo"];
			
			// Display a tournament multiplayer button.

			getMainMenuClass().RefreshTournament(1, name, startDate, endDate);
		}
	}



	/*
	 -----------------------------------------------------
			Access to mainmenu this pointer
	 -----------------------------------------------------
	*/
	private MainMenuHandler getMainMenuClass()
	{
		GameObject _mainmenu = GameObject.Find("MainMenuHandler");
		if (_mainmenu != null) {
			MainMenuHandler _mainmenuScript = _mainmenu.GetComponent<MainMenuHandler> ();
			if(_mainmenuScript != null) {
				return _mainmenuScript;
			}
			throw new Exception();
		}
		throw new Exception();
	}


}
