using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
		//PropellerSDK.Launch (m_listener);
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
		
		//PropellerSDK.SubmitMatchResult (matchResult);

		// Re-launch the Propeller SDK online experience
		// to see who won or to play another match
		//PropellerSDK.Launch (m_listener);

	}


	public void Launch ()
	{
		//LaunchPropeller ();
	}


	/*
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

		// Must parse long values manually since SimpleJSON
		// doesn't yet provide this function automatically.
		long seed = 0;
		if (!long.TryParse(json ["seed"], out seed))
		{
			// invalid string encoded long value, defaults to 0
		}

		
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


		Debug.Log ("LaunchMultiplayerGame with seed : " + (int)seed);


		UnityEngine.Random.seed = (int)seed;

		Application.LoadLevel("PlayField");

		//NotificationCenter.DefaultCenter.PostNotification (getMainMenuClass(), "LaunchGamePlay");
	}
	*/


	public void SetMatchScore(int scoreValue)
	{
	}

	private void sendMatchResult (long score)
	{
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
