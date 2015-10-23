using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FuelSDKSimpleJSON;
using FUEL.Utils;
using FUEL.UserProfile;
//using FUEL.Scenery.Application;
//using FUEL.Scenery.Presentation;

namespace FUEL.SDK {
	
	public class FuelHandler : Singleton<FuelHandler> {
		
		#region ===================================== structures =====================================
		
		public enum State {
			OnCompeteUI = 0,
			InMatch = 1,
			Disable = 2,
			Failed = 3
		}
		
		#endregion
		
		#region ===================================== variable =====================================
		
		public bool inMultiplayer = false;
		
		public State currentState = State.Disable;
		public int challengeCount = 0;
		
		private FuelListener m_listener = null;
		private GameMatchData m_matchData;
		private TournamentData m_tournament;
		
		public string failedReason = "";
		
		#endregion
		
		#region ===================================== MonoBehaviour ====================================
		
		void Awake() {
			//#if !UNITY_EDITOR
			m_listener = new FuelListener();
			
			FuelSDK.setListener(m_listener);
			
			m_matchData = new GameMatchData ();
			m_matchData.ValidMatchData = false;
			m_matchData.MatchComplete = false;
			//#endif
			
			m_tournament = new TournamentData ();
			m_tournament.Enable = false;
		}
		
		void Start() {
			#if !UNITY_EDITOR
			// enable push and local notifications
			EnableNotifications (UserProfileManager.Instance.currentUser.notificationsEnabled);
			
			
			// validate enabled notifications - result is false since local notifications are disabled
			bool fuelNotifEnable = FuelSDK.IsNotificationEnabled( FuelSDK.NotificationType.all );	
			Debug.Log ("FuelSDKManager - Start. Notifications Enable: "+fuelNotifEnable);
			#endif
		}
		
		void Update() {
		}
		
		void OnApplicationPause(bool paused)
		{
			// application entering background
			if (paused) 
			{
				#if UNITY_IPHONE
				UnityEngine.iOS.NotificationServices.ClearLocalNotifications ();
				UnityEngine.iOS.NotificationServices.ClearRemoteNotifications ();
				#endif
			}
		}
		
		#endregion
		
		#region ===================================== Init ====================================
		
		public IEnumerator InitCoroutine() {
			while( !FuelSDK.Initialized ) {
				yield return 0;
			}
			yield return new WaitForSeconds( 2.0f );
			UpdateData();
			yield return 1;
		}
		
		#endregion
		
		#region ===================================== Update Data ====================================
		
		public void UpdateData() {
			#if !UNITY_EDITOR
			SyncChallengeCounts();
			SyncTournamentInfo();
			SyncVirtualGoods();
			GetEvents();
			#endif
		}
		
		#endregion
		
		#region ===================================== Launch Routines ====================================
		
		public void LaunchCompeteDashBoard()
		{
			Debug.Log ("FuelSDKManager - LaunchCompeteDashBoard. Called method");
			inMultiplayer = true;
			currentState = State.OnCompeteUI;
			FuelSDK.Launch ();	
		}
		
		public void ExitCompeteDashBoard() {
			UserProfileManager.Instance.currentUser.lastMatchID = m_matchData.MatchID;
			UserProfileManager.Instance.currentUser.lastTournamentID = m_matchData.TournamentID;
			currentState = State.Disable;
			UpdateData();
			inMultiplayer = false;
		}
		
		#endregion
		
		#region ===================================== Notification Function ====================================
		
		public void EnableNotifications ( bool active ) {
			if (active) {
				FuelSDK.EnableNotification ( FuelSDK.NotificationType.all );
			} 
			else {
				FuelSDK.DisableNotification( FuelSDK.NotificationType.all );
			}
		}
		
		#endregion
		
		#region ===================================== Compete Match Function ====================================
		
		public void SendFinishedMatchDetails( int score){
			
			Debug.Log ("FuelSDKManager - SendFinishedMatchDetails. Called method");
			
			Dictionary<string, object> matchResult = new Dictionary<string, object> ();
			matchResult.Add ("matchID", m_matchData.MatchID);
			matchResult.Add ("tournamentID", m_matchData.TournamentID);
			matchResult.Add ("score", score.ToString() );
			
			FuelSDK.SubmitMatchResult (matchResult);
		}
		
		public void LaunchMultiplayerGame(Dictionary<string, string> matchDict) {
			
			Debug.Log ("FuelSDKManager - LaunchMultiplayerGame. Called method");
			
			m_matchData = GameMatchData.ParseFromDictionary( matchDict );
			
			currentState = State.InMatch;
			
			Debug.Log (	
			           "FuelSDKManager - LaunchMultiplayerGame. Match Data" + "\n" +
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
			UserProfileManager.Instance.startMultiplayer = true;

			GameCommon.getMainMenuClass().LoadGame();
		}
		
		#endregion
		
		#region ===================================== Compete Challenge Counts ====================================
		
		private void SyncChallengeCounts ()
		{
			Debug.Log ("FuelSDKManager - SyncChallengeCounts. Called method");
			FuelSDK.SyncChallengeCounts ();
		}
		
		public void OnPropellerSDKChallengeCountUpdated (string count)
		{
			int countValue;
			if (!int.TryParse(count, out countValue)) {
				return;
			}
			
			Debug.Log ("FuelSDKManager - OnPropellerSDKChallengeCountUpdated. Called method. countValue = " + countValue);
			challengeCount = countValue;
		}
		
		#endregion
		
		#region ===================================== Compete Tournament Info ====================================
		
		public bool TournamentEnable () {
			if (!m_tournament.Enable) {
				return false;
			}
			if (!TimeUtility.TimeIsInThePast (m_tournament.StartDate) || !TimeUtility.TimeIsInTheFuture (m_tournament.EndDate)) {
				return false;
			}
			return true;
		}
		
		public string TournamentRemainingTime() {
			if (!TournamentEnable ()) {
				return "";
			}
			return TimeUtility.RemainingTimeString ( m_tournament.EndDate );
		}
		
		private void SyncTournamentInfo ()
		{
			Debug.Log ("FuelSDKManager - SyncTournamentInfo. Called method");
			FuelSDK.SyncTournamentInfo ();
		}
		
		public void OnPropellerSDKTournamentInfo (Dictionary<string, string> tournamentDict)
		{
			Debug.Log ("FuelSDKManager - OnPropellerSDKTournamentInfo. Called method");
			
			if ((tournamentDict == null) || (tournamentDict.Count == 0)) 
			{
				m_tournament.Enable = false;
				Debug.Log ("FuelSDKManager - OnPropellerSDKTournamentInfo. No tournaments currently running");
				Debug.Log ("....no tournaments currently running");
			} 
			else 
			{
				m_tournament = TournamentData.ParseFromDictionary( tournamentDict );
				Debug.Log (
					"FuelSDKManager - OnPropellerSDKTournamentInfo. Tournament Data" + "\n" +
					"tournyname = " + m_tournament.Name + "\n" +
					"campaignName = " + m_tournament.CampaignName + "\n" +
					"startDate = " + m_tournament.StartDate.ToLongDateString() + "\n" +
					"endDate = " + m_tournament.EndDate.ToLongDateString() + "\n" +
					"logo = " + m_tournament.Logo + "\n"
					);
			}
		}
		
		#endregion
		
		#region ===================================== Virtual Goods ====================================
		
		private void SyncVirtualGoods ()
		{
			Debug.Log ("FuelSDKManager - SyncVirtualGoods. Called method");
			FuelSDK.SyncVirtualGoods ();
		}
		
		public void OnPropellerSDKVirtualGoodList (List<object> virtualGoodsList)
		{
			Debug.Log ("FuelSDKManager - OnPropellerSDKVirtualGoodList. Called method");
			
			List<VirtualGoodData> virtualGoodsDataList = new List<VirtualGoodData>();
			foreach( object virtualGoodObject in virtualGoodsList ) {
				Dictionary<string, object> virtualGoodDict = virtualGoodObject as Dictionary<string, object>;
				VirtualGoodData virtualGoodsData = VirtualGoodData.ParseFromDictionary( virtualGoodDict );
				virtualGoodsDataList.Add( virtualGoodsData );
			}
			if( virtualGoodsDataList.Count > 0 ) {
				//GameSceneController.ProcessVirtualGoods( virtualGoodsDataList );
			}
		}
		
		public void OnPropellerSDKVirtualGoodRollback (string transactionId)
		{
			Debug.Log ("FuelSDKManager - OnPropellerSDKVirtualGoodRollback. Called method");
			
			// Rollback the virtual good transaction for the given transaction ID
		}
		
		#endregion
		
		#region ===================================== Ignite Send Progress ====================================
		
		public void SendProgress (int score,int coins) {
			
			Dictionary<string,int> scoreDict = new Dictionary<string, int>();
			scoreDict.Add("value",score);
			Dictionary<string,int> coinsDict = new Dictionary<string, int>();
			coinsDict.Add("value",coins);
			
			Dictionary<string,object> progressDict = new Dictionary<string, object>();
			progressDict.Add("score", scoreDict);
			progressDict.Add("coins", coinsDict);
			
			List<object> tags = null;//new List<object>();
			//tags.Add("allowedMissions");
			
			//FuelSDK.SendProgress( progressDict, tags );
			
			List<object> methodParams = new List<object>();
			methodParams.Add( progressDict );
			methodParams.Add( tags );
			FuelSDK.ExecMethod("SendProgress", methodParams);
			
		}
		
		#endregion
		
		#region ===================================== Ignite Events ====================================
		
		public void GetEvents() {
			List<object> tags = new List<object>();
			tags.Add("allowedMissions");
			FuelSDK.GetEvents(tags);
		}
		
		public void OnIgniteEventsReceive( List<object> eventsList ) {
			Debug.Log( "FuelSDKManager - OnIgniteEventsReceive. event count: "+eventsList.Count );
			List<EventData> events = new List<EventData>();
			foreach(object eventObject in eventsList ) {
				Dictionary<string,object> eventDict = eventObject as Dictionary<string,object>;
				EventData eventData = EventData.ParseFromDictionary( eventDict );
				events.Add( eventData );

				//GameSceneController.PopulateIgniteEventsUI( events );
			}
		}
		
		#endregion
		
		#region ===================================== Ignite LeaderBoard ====================================
		
		public void GetLeaderBoard(string boardID) {
			FuelSDK.GetLeaderBoard( boardID );
		}
		
		public void OnIgniteLeaderBoardReceive( Dictionary<string,object> leaderBoardDict ) {
			LeaderBoardData leaderBoardData = LeaderBoardData.ParseFromDictionary( leaderBoardDict );
			//GameSceneController.PopulateIgniteLeaderBoardUI( leaderBoardData );
		}
		
		#endregion
		
		#region ===================================== Ignite Mission ====================================
		
		public void GetMission(string missionID) {
			FuelSDK.GetMission( missionID );
		}
		
		public void OnIgniteMissionReceive(  Dictionary<string,object> missionDict  ) {
			MissionData missionData = MissionData.ParseFromDictionary( missionDict );

			//GameSceneController.PopulateIgniteMisssionUI( missionData );
		}
		
		#endregion
		
		#region ===================================== Ignite Quest ====================================
		
		public void GetQuest(string questID) {
			FuelSDK.GetQuest( null );
		}
		
		public void OnIgniteQuestReceive() {
			
		}
		
		#endregion
		
		public void OnSdkFailed(string reason ) {
			currentState = State.Failed;
			failedReason = reason;
		}
	}
}
