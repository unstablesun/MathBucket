using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class FuelExample : MonoBehaviour
{
	private enum EventType
	{
		leaderboard = 0,
		mission = 1,
		quest = 2
	};

	private bool m_bInitialized;

	private FuelSDKListener m_listener;

	private string m_transactionID;
	private List<string> m_consumedVirtualGoodIDs;

	private string m_tournamentID;
	private string m_matchID;

	private List<string> m_optionalEventIDs;
	private List<string> m_leaderBoardIDs;
	private List<string> m_missionIDs;
	private List<string> m_questIDs;

	void Awake ()
	{
		if (!m_bInitialized) {
			GameObject.DontDestroyOnLoad (gameObject);
			
			if (!Application.isEditor) {
				Initialize ();
			}
			
			m_bInitialized = true;

			m_transactionID = "";
			m_consumedVirtualGoodIDs = new List<string> ();

			m_optionalEventIDs = new List<string> ();
			m_leaderBoardIDs = new List<string> ();
			m_missionIDs = new List<string> ();
			m_questIDs = new List<string> ();
		} else {
			GameObject.Destroy (gameObject);
		}
	}

	private void Initialize()
	{
		m_listener = new FuelListenerExample (this);
		FuelSDK.setListener (m_listener);
	}

	public void LaunchMultiplayer ()
	{
		FuelSDK.Launch ();
	}

	public void GetChallengeCount ()
	{
		FuelSDK.SyncChallengeCounts ();
	}

	public void GetTournamentInfo ()
	{
		FuelSDK.SyncTournamentInfo ();
	}

	public void GetVirtualGoods ()
	{
		FuelSDK.SyncVirtualGoods ();
	}

	public void GetUserValues ()
	{
		FuelSDK.SyncUserValues ();
	}
	
	public void GetEventsViaExecMethod ()
	{
		List<object> tags = new List<object> ();
		tags.Add ("allowedMissions");
		tags.Add ("allowedQuests");

		// invoke FuelSDK.GetEvents () via FuelSDK.ExecMethod ()
		FuelSDK.ExecMethod ("GetEvents", tags);
	}

	public void GetEvents ()
	{
		List<object> tags = new List<object> ();
		tags.Add ("allowedMissions");
		tags.Add ("allowedQuests");

		// retrieve events for the given fake tag set
		FuelSDK.GetEvents (tags);
	}

	public void GetLeaderBoards ()
	{
		// request information on the leaderboards that
		// the player is eligible for

		if (m_leaderBoardIDs.Count == 0) {
			Debug.Log ("GetLeaderBoards - no leaderboard events to retrieve");
			return;
		}

		foreach (string leaderBoardID in m_leaderBoardIDs) {
			FuelSDK.GetLeaderBoard (leaderBoardID);
		}
	}

	public void GetMissions ()
	{
		// request information on the missions that
		// the player is eligible for

		if (m_missionIDs.Count == 0) {
			Debug.Log ("GetMissions - no mission events to retrieve");
			return;
		}

		foreach (string missionID in m_missionIDs) {
			FuelSDK.GetMission (missionID);
		}
	}

	public void GetQuests ()
	{
		// request information on the quests that
		// the player is eligible for

		if (m_questIDs.Count == 0) {
			Debug.Log ("GetQuests - no quest events to retrieve");
			return;
		}

		foreach (string questID in m_questIDs) {
			FuelSDK.GetQuest (questID);
		}
	}

	public void JoinOptionalEvents ()
	{
		// request joining optional events that
		// the player is eligible for

		if (m_optionalEventIDs.Count == 0) {
			Debug.Log ("JoinOptionalEvents - no optional events to join");
			return;
		}

		foreach (string optionalEventID in m_optionalEventIDs) {
			FuelSDK.JoinEvent (optionalEventID);
		}
	}

	public void SendProgress ()
	{
		Dictionary<string, object> progress = new Dictionary<string, object> ();
		progress.Add ("score", "499");

		List<object> tags = new List<object> ();
		tags.Add ("wizard");

		// send fake player progress
		FuelSDK.SendProgress (progress, tags);
	}

	#region ======================================= helper methods =======================================

	private IEnumerator PerformSocialLogin (bool allowCache)
	{
		// fake retrieval of social login info
		yield return new WaitForSeconds (0.5f);
		
		Dictionary<string, string> loginInfo = new Dictionary<string, string> ();
		loginInfo.Add ("provider", "facebook");
		loginInfo.Add ("email", "testguy@fuelpowered.com");
		loginInfo.Add ("id", "testguyid");
		loginInfo.Add ("nickname", "testguy445");
		loginInfo.Add ("token", "testguy445");
		
		FuelSDK.SdkSocialLoginCompleted (loginInfo);
		
		// return null in the case of a failure to retrieve
		// social login info
		//FuelSDK.SdkSocialLoginCompleted(null);
	}

	private IEnumerator PerformSocialInvite (Dictionary<string, string> inviteDetail)
	{
		// fake sending a social invite
		yield return new WaitForSeconds (0.5f);

		FuelSDK.SdkSocialInviteCompleted ();
	}
	
	private IEnumerator PerformSocialShare (Dictionary<string, string> shareDetail)
	{
		// fake sending a social share
		yield return new WaitForSeconds (0.5f);
		
		FuelSDK.SdkSocialShareCompleted ();
	}

	private IEnumerator PerformMatch (Dictionary<string, object> matchInfo)
	{
		// fake obtaining match results
		yield return new WaitForSeconds (0.5f);

		Dictionary<string, object> matchResult = new Dictionary<string, object> ();
		matchResult.Add ("matchID", m_matchID);
		matchResult.Add ("tournamentID", m_tournamentID);
		matchResult.Add ("score", "55");
		
		FuelSDK.SubmitMatchResult (matchResult);

		// return back to multiplayer
		FuelSDK.Launch ();
	}

	#endregion

	#region ===================================== listener callbacks =====================================

	public void OnVirtualGoodList (string transactionID, List<object> virtualGoods)
	{
		if (transactionID == null) {
			Debug.Log ("OnVirtualGoodList - transaction ID: <undefined>");
			return;
		}

		Debug.Log ("OnVirtualGoodList - transaction ID: " + transactionID);

		m_transactionID = transactionID;

		if (virtualGoods == null) {
			Debug.Log ("OnVirtualGoodList - virtual good: <undefined_list>");
			return;
		}

		m_consumedVirtualGoodIDs.Clear ();

		if (virtualGoods.Count == 0) {
			Debug.Log ("OnVirtualGoodList - virtual good: <empty_list>");
			return;
		}

		string virtualGoodsString = FuelSDKCommon.Serialize (virtualGoods);
		
		if (virtualGoodsString == null) {
			Debug.Log ("OnVirtualGoodList - unable to serialize the virtual good list");
			return;
		}
		
		Debug.Log ("OnVirtualGoodList - virtual goods: " + virtualGoodsString);
		
		bool consumed = true;
		
		foreach (object virtualGoodObject in virtualGoods) {
			Dictionary<string, object> virtualGood = virtualGoodObject as Dictionary<string, object>;
			
			if (virtualGood == null) {
				Debug.Log ("OnVirtualGoodList - invalid virtual good data type: " + virtualGoodObject.GetType ().Name);
				consumed = false;
				break;
			}
			
			object goodIDObject = virtualGood["goodId"];
			
			if (goodIDObject == null) {
				Debug.Log ("OnVirtualGoodList - missing expected virtual good ID");
				consumed = false;
				break;
			}
			
			if (!(goodIDObject is string)) {
				Debug.Log ("OnVirtualGoodList - invalid virtual good ID data type: " + goodIDObject.GetType ().Name);
				consumed = false;
				break;
			}
			
			string goodID = (string)goodIDObject;
			
			// based on the virtual good ID, update the player
			// inventory with the received virtual good
			
			m_consumedVirtualGoodIDs.Add (goodID);
		}
		
		if (!consumed) {
			foreach (string goodID in m_consumedVirtualGoodIDs) {
				// revert the currently awarded virtual goods for
				// this transaction from the player inventory
			}

			m_consumedVirtualGoodIDs.Clear ();
		}
		
		FuelSDK.AcknowledgeVirtualGoods(transactionID, consumed);
	}
	
	public void OnVirtualGoodRollback (string transactionID)
	{
		if (transactionID == null) {
			Debug.Log ("OnVirtualGoodRollback - transaction ID: <undefined>");
			return;
		}

		Debug.Log ("OnVirtualGoodRollback - transaction ID: " + transactionID);
		
		if (!transactionID.Equals (m_transactionID)) {
			Debug.Log ("OnVirtualGoodRollback - transaction ID (" + transactionID + ") does not match the last cached trasaction (" + m_transactionID + ")");
			return;
		}

		foreach (string goodID in m_consumedVirtualGoodIDs) {
			// revert the currently awarded virtual goods for
			// this transaction from the player inventory
		}
	}
	
	public void OnNotificationEnabled(FuelSDK.NotificationType type) 
	{
		Debug.Log ("OnNotificationEnabled - enabled: " + type.ToString ());

		// update game notification UI widget for the given notification type
	}
	
	public void OnNotificationDisabled(FuelSDK.NotificationType type) 
	{
		Debug.Log ("OnNotificationDisabled - disabled: " + type.ToString ());
		
		// update game notification UI widget for the given notification type
	}

	public void OnSocialLogin (bool allowCache)
	{
		Debug.Log ("OnSocialLogin - allowCache: " + allowCache.ToString ());
		Debug.Log ("OnSocialLogin - invoking fake social login");

		// fake social login
		StartCoroutine (PerformSocialLogin (allowCache));
	}
	
	public void OnSocialInvite (Dictionary<string, string> inviteDetail)
	{
		if (inviteDetail == null) {
			Debug.Log ("OnSocialInvite - undefined invite details");
			return;
		}

		if (inviteDetail.Count == 0) {
			Debug.Log ("OnSocialInvite - empty invite details");
			return;
		}

		string inviteDetailString = FuelSDKCommon.Serialize (inviteDetail);
		
		if (inviteDetailString == null) {
			Debug.Log ("OnSocialInvite - unable to serialize the invite details");
			return;
		}
		
		Debug.Log ("OnSocialInvite - invite details: " + inviteDetailString);
		
		Debug.Log ("OnSocialInvite - invoking fake social invite");
		
		// fake social invite
		StartCoroutine (PerformSocialInvite (inviteDetail));
	}
	
	public void OnSocialShare (Dictionary<string, string> shareDetail)
	{
		if (shareDetail == null) {
			Debug.Log ("OnSocialShare - undefined share details");
			return;
		}

		if (shareDetail.Count == 0) {
			Debug.Log ("OnSocialShare - empty share details");
			return;
		}

		string shareDetailString = FuelSDKCommon.Serialize (shareDetail);
		
		if (shareDetailString == null) {
			Debug.Log ("OnSocialShare - unable to serialize the share details");
			return;
		}
		
		Debug.Log ("OnSocialShare - share details: " + shareDetailString);
		
		Debug.Log ("OnSocialShare - invoking fake social share");
		
		// fake social share
		StartCoroutine (PerformSocialShare (shareDetail));
	}
	
	public void OnImplicitLaunch(FuelSDK.ApplicationState applicationState)
	{
		Debug.Log ("OnImplicitLaunch - application state: " + applicationState.ToString ());

		// apply implicit launch policy based on the given application state
	}
	
	public void OnUserValues (Dictionary<string, string> conditions, Dictionary<string, string> variables)
	{
		if (conditions == null) {
			Debug.Log ("OnUserValues - undefined conditions");
			return;
		}

		if (conditions.Count == 0) {
			Debug.Log ("OnUserValues - empty conditions");
		}

		string conditionsString = FuelSDKCommon.Serialize (conditions);
		
		if (conditionsString == null) {
			Debug.Log ("OnUserValues - unable to serialize conditions");
			return;
		}
		
		Debug.Log ("OnUserValues - conditions: " + conditionsString);

		if (variables == null) {
			Debug.Log ("OnUserValues - undefined variables");
			return;
		}

		if (variables.Count == 0) {
			Debug.Log ("OnUserValues - empty variables");
		}

		string variablesString = FuelSDKCommon.Serialize (variables);
		
		if (variablesString == null) {
			Debug.Log ("OnUserValues - unable to serialize variables");
			return;
		}
		
		Debug.Log ("OnUserValues - variables: " + variablesString);

		// update game experience depending on the user values (variables) received
	}
	
	public virtual void OnCompeteChallengeCount (int count)
	{
		Debug.Log ("OnCompeteChallengeCount - count: " + count.ToString ());

		// update game UI with updated count of outstanding challenges
	}
	
	public void OnCompeteTournamentInfo (Dictionary<string, string> tournamentInfo)
	{
		if (tournamentInfo == null) {
			Debug.Log ("OnCompeteTournamentInfo - undefined tournamentInfo");
			return;
		}

		if (tournamentInfo.Count == 0) {
			Debug.Log ("OnCompeteTournamentInfo - empty tournamentInfo");
			// can have no tournament info in the case where
			// no current or future tournament has been
			// scheduled
			return;
		}

		string tournamentInfoString = FuelSDKCommon.Serialize (tournamentInfo);
		
		if (tournamentInfoString == null) {
			Debug.Log ("OnCompeteTournamentInfo - unable to serialize tournament info");
			return;
		}
		
		Debug.Log ("OnCompeteTournamentInfo - tournament info: " + tournamentInfoString);

		// compare the tournament start date against the current date to
		// determine if the tournament is currently running or is scheduled
		// to run in the future

		// update game UI with scheduled/running tournament information
	}

	public void OnCompeteUICompletedWithExit ()
	{
		Debug.Log ("OnCompeteUICompletedWithExit - clean exit from compete UI");

		// clean exit from the compete UI back to the game main menu
	}

	public void OnCompeteUICompletedWithMatch (Dictionary<string, object> matchInfo)
	{
		if (matchInfo == null) {
			Debug.Log ("OnCompeteUICompletedWithMatch - undefined match info");
			return;
		}

		if (matchInfo.Count == 0) {
			Debug.Log ("OnCompeteUICompletedWithMatch - empty match info");
			return;
		}

		string matchInfoString = FuelSDKCommon.Serialize (matchInfo);
		
		if (matchInfoString == null) {
			Debug.Log ("OnCompeteUICompletedWithMatch - unable to serialize match info");
			return;
		}
		
		Debug.Log ("OnCompeteUICompletedWithMatch - match info: " + matchInfoString);

		object tournamentIDObject = matchInfo["tournamentID"];

		if (tournamentIDObject == null) {
			Debug.Log ("OnCompeteUICompletedWithMatch - missing expected tournament ID");
			return;
		}

		if (!(tournamentIDObject is string)) {
			Debug.Log ("OnCompeteUICompletedWithMatch - invalid tournament ID data type: " + tournamentIDObject.GetType ().Name);
			return;
		}

		string tournamentID = (string)tournamentIDObject;

		object matchIDObject = matchInfo["matchID"];
		
		if (matchIDObject == null) {
			Debug.Log ("OnCompeteUICompletedWithMatch - missing expected match ID");
			return;
		}
		
		if (!(matchIDObject is string)) {
			Debug.Log ("OnCompeteUICompletedWithMatch - invalid match ID data type: " + matchIDObject.GetType ().Name);
			return;
		}
		
		string matchID = (string)matchIDObject;

		// Caching match information for later
		m_tournamentID = tournamentID;
		m_matchID = matchID;

		// fake playing a match
		StartCoroutine (PerformMatch (matchInfo));
	}

	public void OnCompeteUIFailed (string reason)
	{
		if (reason == null) {
			Debug.Log ("OnCompeteUIFailed - undefined failure reason");
			return;
		}

		Debug.Log ("OnCompeteUIFailed - reason: " + reason);

		// failure exit from the compete UI back to the game main menu
	}

	public void OnIgniteEvents (List<object> events)
	{
		// based on the events data the game can retrieve leaderboard
		// information, mission information, quest information, or
		// event joined status information

		m_leaderBoardIDs.Clear ();
		m_missionIDs.Clear ();
		m_questIDs.Clear ();
		m_optionalEventIDs.Clear ();

		if (events == null) {
			Debug.Log ("OnIgniteEvents - undefined list of events");
			return;
		}

		if (events.Count == 0) {
			Debug.Log ("OnIgniteEvents - empty list of events");
			return;
		}

		foreach (object eventObject in events) {
			Dictionary<string, object> eventInfo = eventObject as Dictionary<string, object>;
			
			if (eventInfo == null) {
				Debug.Log ("OnIgniteEvents - invalid event data type: " + eventObject.GetType ().Name);
				continue;
			}
			
			object eventIdObject = eventInfo["id"];
			
			if (eventIdObject == null) {
				Debug.Log ("OnIgniteEvents - missing expected event ID");
				continue;
			}
			
			if (!(eventIdObject is string)) {
				Debug.Log ("OnIgniteEvents - invalid event ID data type: " + eventIdObject.GetType ().Name);
				continue;
			}
			
			string eventId = (string)eventIdObject;
			
			object eventTypeObject = eventInfo["type"];
			
			if (eventTypeObject == null) {
				Debug.Log ("OnIgniteEvents - missing expected event type");
				continue;
			}
			
			if (!(eventTypeObject is long)) {
				Debug.Log ("OnIgniteEvents - invalid event type data type: " + eventTypeObject.GetType ().Name);
				continue;
			}
			
			long eventTypeLong = (long)eventTypeObject;
			
			int eventTypeValue = (int)eventTypeLong;
			
			if (!Enum.IsDefined (typeof (EventType), eventTypeValue)) {
				Debug.Log ("OnIgniteEvents - unsupported event type value: " + eventTypeValue.ToString ());
				continue;
			}
			
			EventType eventType = (EventType)eventTypeValue;
			
			object eventJoinedObject = eventInfo["joined"];
			
			if (eventJoinedObject == null) {
				Debug.Log ("OnIgniteEvents - missing expected event joined status");
				continue;
			}
			
			if (!(eventJoinedObject is bool)) {
				Debug.Log ("OnIgniteEvents - invalid event joined data type: " + eventJoinedObject.GetType ().Name);
				continue;
			}
			
			bool eventJoined = (bool)eventJoinedObject;
			
			string eventTypeString = eventType.ToString ();
			
			if (eventJoined) {
				Debug.Log ("OnIgniteEvents - player is joined in event of type '" + eventTypeString + "' with event ID: " + eventId);
				
				switch (eventType) {
				case EventType.leaderboard:
					m_leaderBoardIDs.Add (eventId);
					break;
				case EventType.mission:
					m_missionIDs.Add (eventId);
					break;
				case EventType.quest:
					m_questIDs.Add (eventId);
					break;
				default:
					Debug.Log ("OnIgniteEvents - unsupported event type: " + eventTypeString);
					continue;
				}
			} else {
				Debug.Log ("OnIgniteEvents - player can opt-in to join event of type '" + eventTypeString + "' with event ID: " + eventId);
				m_optionalEventIDs.Add (eventId);
			}
		}

		// cache event information for later or process immediately
	}
	
	public void OnIgniteLeaderBoard (Dictionary<string, object> leaderBoard)
	{
		if (leaderBoard == null) {
			Debug.Log ("OnIgniteLeaderBoard - undefined leaderboard");
			return;
		}

		if (leaderBoard.Count == 0) {
			Debug.Log ("OnIgniteLeaderBoard - empty leaderboard");
			return;
		}

		string leaderBoardString = FuelSDKCommon.Serialize (leaderBoard);
		
		if (leaderBoardString == null) {
			Debug.Log ("OnIgniteLeaderBoard - unable to serialize the leaderboard");
			return;
		}
		
		Debug.Log ("OnIgniteLeaderBoard - leaderboard: " + leaderBoardString);

		// process the leaderboard information
	}
	
	public void OnIgniteMission (Dictionary<string, object> mission)
	{
		if (mission == null) {
			Debug.Log ("OnIgniteMission - undefined mission");
			return;
		}

		if (mission.Count == 0) {
			Debug.Log ("OnIgniteMission - empty mission");
			return;
		}

		string missionString = FuelSDKCommon.Serialize (mission);
		
		if (missionString == null) {
			Debug.Log ("OnIgniteMission - unable to serialize the mission");
			return;
		}
		
		Debug.Log ("OnIgniteMission - mission: " + missionString);

		// process the mission information
	}
	
	public void OnIgniteQuest (Dictionary<string, object> quest)
	{
		if (quest == null) {
			Debug.Log ("OnIgniteQuest - undefined quest");
			return;
		}

		if (quest.Count == 0) {
			Debug.Log ("OnIgniteQuest - empty quest");
			return;
		}

		string questString = FuelSDKCommon.Serialize (quest);
		
		if (questString == null) {
			Debug.Log ("OnIgniteQuest - unable to serialize the quest");
			return;
		}
		
		Debug.Log ("OnIgniteQuest - quest: " + questString);

		// process the quest information
	}
	
	public void OnIgniteJoinEvent (string eventID, bool joinStatus)
	{
		if (eventID == null) {
			Debug.Log ("OnIgniteJoinEvent - undefined event ID");
			return;
		}

		if (joinStatus) {
			Debug.Log ("OnIgniteJoinEvent - player has joined event: " + eventID);
		} else {
			Debug.Log ("OnIgniteJoinEvent - player has not joined event: " + eventID);
		}

		// process the join event
	}

	#endregion
}
