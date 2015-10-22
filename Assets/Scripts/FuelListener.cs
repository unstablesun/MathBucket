using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FUEL.SDK {
	
	public class FuelListener : FuelSDKListener {
		
		
		public override void OnChallengeCountChanged (Dictionary<string, object> data)
		{
			Debug.Log ("OnChallengeCountChanged");
			
			// message must be defined or else its an error
			if (data == null) {
				Debug.Log ("OnChallengeCountChanged - null message");
				return;
			}
			
			// message must contain the challenge count or else its an error
			if (string.IsNullOrEmpty (data["count"].ToString())) {
				Debug.Log ("OnChallengeCountChanged - null or empty message");
				return;
			}
			
			Debug.Log ("OnChallengeCountChanged - " + data["count"].ToString());
			
			FuelHandler.Instance.OnPropellerSDKChallengeCountUpdated (data ["count"].ToString ());
		}
		
		public override void OnImplicitLaunch (Dictionary<string, object> data)
		{
			Debug.Log ("OnImplicitLaunch");
			
			string message = data ["applicationState"].ToString ();
			
			// message must contain the application state or else its an error
			if (string.IsNullOrEmpty (message)) {
				Debug.Log ("OnImplicitLaunch - null or empty message");
				return;
			}
			
			Debug.Log ("OnImplicitLaunch - " + data["applicationState"]);
			
			//FuelSDKManager.Instance.OnImplicitLaunch (data["applicationState"]); //TODO Implement a feedback for the player
			
		}
		
		public override void OnNotificationDisabled (Dictionary<string, object> data)
		{
			int notificationTypeValue = -1;
			
			if (!int.TryParse (data["notificationType"].ToString(), out notificationTypeValue)) {
				Debug.Log ("OnNotificationDisabled - unparsable notification type value");
				return;
			}
			
			System.Array notificationTypes = System.Enum.GetValues (typeof(FuelSDK.NotificationType));
			FuelSDK.NotificationType notificationType = FuelSDK.NotificationType.none;
			bool foundNotificationType = false;
			
			foreach (FuelSDK.NotificationType notificationTypeItem in notificationTypes) {
				notificationType = notificationTypeItem;
				
				if (((int)notificationType) == notificationTypeValue) {
					foundNotificationType = true;
					break;
				}
			}
			
			if (!foundNotificationType) {
				Debug.Log ("OnNotificationDisabled - unsupported notification type");
				return;
			}
			
			Debug.Log ("OnNotificationDisabled notification type: " + notificationType.ToString());
			//FuelSDKManager.Instance.OnNotificationDisabled (notificationType); //TODO Implement a feedback for the player
		}
		
		public override void OnNotificationEnabled (Dictionary<string, object> data)
		{
			int notificationTypeValue = -1;
			
			if (!int.TryParse (data["notificationType"].ToString(), out notificationTypeValue)) {
				Debug.Log ("onNotificationEnabled - unparsable notification type value");
				return;
			}
			
			System.Array notificationTypes = System.Enum.GetValues (typeof(FuelSDK.NotificationType));
			FuelSDK.NotificationType notificationType = FuelSDK.NotificationType.none;
			bool foundNotificationType = false;
			
			foreach (FuelSDK.NotificationType notificationTypeItem in notificationTypes) {
				notificationType = notificationTypeItem;
				
				if (((int)notificationType) == notificationTypeValue) {
					foundNotificationType = true;
					break;
				}
			}
			
			if (!foundNotificationType) {
				Debug.Log ("OnNotificationEnabled - unsupported notification type");
				return;
			}
			
			Debug.Log ("OnNotificationEnabled notification type: " + notificationType.ToString());
			//FuelSDKManager.Instance.OnNotificationEnabled (notificationType); //TODO Implement a feedback for the player
		}
		
		public override void OnSdkCompletedWithMatch (Dictionary<string, object> data)
		{
			
			Debug.Log ("OnSdkCompletedWithMatch");
			
			// message must be defined or else its an error
			if (data == null) {
				Debug.Log ("OnSdkCompletedWithMatch - null message");
				return;
			}
			
			// can have no tournament info in the case where
			// no current or future tournament has been
			// scheduled
			if (data.Count != 3) {
				Debug.LogError ("OnSdkCompletedWithMatch - Invalid response from FuelUnitySDK");
				return;
			}
			
			string tournamentID = data["tournamentID"].ToString();
			string matchID = data["matchID"].ToString();
			string paramsJSON = WWW.UnEscapeURL (data["params"].ToString()); //TODO pass this to json
			
			Dictionary<string, string> matchInfo = new Dictionary<string, string> ();
			matchInfo.Add ("tournamentID", tournamentID);
			matchInfo.Add ("matchID", matchID);
			matchInfo.Add ("paramsJSON", paramsJSON);
			
			FuelHandler.Instance.LaunchMultiplayerGame (matchInfo);
			
		}
		
		public override void OnSdkFailed (Dictionary<string, object> data)
		{
			Debug.Log ("OnSdkFailed");
			
			// data must be defined or else its an error
			if (data == null) {
				Debug.Log ("OnSdkFailed - null message");
				return;
			}
			
			string failReason = "";
			
			foreach (KeyValuePair<string, object> entry in data) {
				
				failReason += entry.Value.ToString() + " ";
			}
			
			Debug.Log ("OnSdkFailed reason" + failReason );
			FuelHandler.Instance.OnSdkFailed (failReason);
		}
		
		public override void OnSdkSocialInvite (Dictionary<string, object> data)
		{
			Debug.Log ("OnSdkSocialInvite");
			
			// data must be defined or else its an error
			if (data == null) {
				Debug.Log ("OnSdkSocialInvite - null message");
				return;
			}
			
			Debug.Log ("OnSdkSocialInvite - done" );
			//FuelSDKManager.Instance.OnSdkSocialInvite (notificationType); //TODO Implement a feedback for the player
		}
		
		public override void OnSdkSocialLogin (Dictionary<string, object> data)
		{
			Debug.Log ("OnSdkSocialLogin");
			
			// data must be defined or else its an error
			if (data == null) {
				Debug.Log ("OnSdkSocialLogin - null message");
				return;
			}
			
			bool allowCache = System.Convert.ToBoolean (data["allowCache"].ToString());
			
			Debug.Log ("OnSdkSocialLogin - allow Cache" + allowCache);
			//FuelSDKManager.Instance.OnSdkSocialLogin (notificationType); //TODO Implement a feedback for the player
		}
		
		public override void OnSdkCompletedWithExit ()
		{
			Debug.Log ("Sea-Leg SdkCompletedWithExit");
			FuelHandler.Instance.ExitCompeteDashBoard();
		}
		
		public override void OnSdkSocialShare (Dictionary<string, object> data)
		{
			Debug.Log ("OnSdkSocialShare");
			
			// data must be defined or else its an error
			if (data == null) {
				Debug.Log ("OnSdkSocialShare - null message");
				return;
			}
			
			Debug.Log ("OnSdkSocialShare - done" );
			//FuelSDKManager.Instance.OnSdkSocialShare (notificationType); //TODO Implement a feedback for the player
		}
		
		public override void OnTournamentInfo (Dictionary<string, object> data)
		{
			Debug.Log ("OnTournamentInfo");
			
			// message must be defined or there is no tournament or else its an error
			if (data == null) {
				Debug.Log ("OnTournamentInfo - null message");
				return;
			}
			
			// can have no tournament info in the case where
			// no current or future tournament has been
			// scheduled
			
			
			Dictionary<string, string> tournamentInfo = new Dictionary<string, string> ();
			
			
			if (!string.IsNullOrEmpty (data["name"].ToString())) {
				tournamentInfo.Add ("name", WWW.UnEscapeURL (data["name"].ToString()));
			}
			
			if (!string.IsNullOrEmpty (data["campaignName"].ToString())) {
				tournamentInfo.Add ("campaignName", WWW.UnEscapeURL (data["campaignName"].ToString()));
			}
			
			if (!string.IsNullOrEmpty (data["sponsorName"].ToString())) {
				tournamentInfo.Add ("sponsorName", WWW.UnEscapeURL (data["sponsorName"].ToString()));
			}
			
			if (!string.IsNullOrEmpty (data["startDate"].ToString())) {
				tournamentInfo.Add ("startDate", WWW.UnEscapeURL (data["startDate"].ToString()));
			}
			
			if (!string.IsNullOrEmpty (data["endDate"].ToString())) {
				tournamentInfo.Add ("endDate", WWW.UnEscapeURL (data["endDate"].ToString()));
			}
			
			if (!string.IsNullOrEmpty (data["logo"].ToString())) {
				tournamentInfo.Add ("logo", WWW.UnEscapeURL (data["logo"].ToString()));
			}
			
			FuelHandler.Instance.OnPropellerSDKTournamentInfo (tournamentInfo);
		}
		
		public override void OnUserValues (Dictionary<string, object> data)
		{
			// message must be defined or else its an error
			if (data == null) {
				Debug.Log ("OnUserValues - null message");
				return;
			}
			
			
			Dictionary<string, string> userValuesInfo = new Dictionary<string, string> ();
			
			if (data != null) {
				
				Dictionary<string, object> variablesMap = data["variables"] as Dictionary<string, object>;
				
				if(variablesMap != null) {
					
					foreach(KeyValuePair<string, object> variableMap in variablesMap) {
						
						if(string.IsNullOrEmpty(variableMap.Key)) {
							continue;
						}
						
						string valueObject = (string)variableMap.Value;
						
						if (string.IsNullOrEmpty(valueObject)) {
							continue;
						}
						
						userValuesInfo.Add(variableMap.Key, valueObject);
					}
					
				}
				
				Dictionary<string, object> dynamicConditions = data["dynamicConditions"] as Dictionary<string, object>;
				
				if(dynamicConditions != null) {
					
					foreach(KeyValuePair<string, object> dynamicCondition in dynamicConditions) {
						
						if(string.IsNullOrEmpty(dynamicCondition.Key)) {
							continue;
						}
						
						string valueObject = (string)dynamicCondition.Value;
						
						if (string.IsNullOrEmpty(valueObject)) {
							continue;
						}
						
						userValuesInfo.Add(dynamicCondition.Key, valueObject);
					}
					
				}
			}
			
			Debug.Log ("OnUserValues - userValuesInfo count :" + userValuesInfo.Count.ToString());
			//FuelSDKManager.Instance.OnUserValues (userValuesInfo); //TODO Implement a feedback for the player
			
		}
		
		public override void OnVirtualGoodList (Dictionary<string, object> data)
		{
			Debug.Log ("OnVirtualGoodList");
			
			// data must be defined or else its an error
			if (data == null) {
				Debug.Log ("OnVirtualGoodRollback - null message");
				return;
			}
			
			if (string.IsNullOrEmpty (data["transactionID"].ToString())) {
				Debug.Log ("OnVirtualGoodRollback - undefined transaction id");
				return;
			}
			
			if( data.ContainsKey("transactionID") ) {
				string transactionId = (string) data["transactionID"];
				
				List<object> virtualGoodsList = new List<object> ();
				if( data.ContainsKey("virtualGoods") ) {
					virtualGoodsList = data ["virtualGoods"] as List<object>;
				}
				
				Debug.Log ("OnVirtualGoodList transactionId = " + transactionId);
				
				
				FuelHandler.Instance.OnPropellerSDKVirtualGoodList( virtualGoodsList );
				FuelSDK.AcknowledgeVirtualGoods(transactionId, true);
			}
			else {
				Debug.Log ("OnVirtualGoodList. Imposible to parse the virtual good because there aren't transactionID in the data. ");
			}
		}
		
		public override void OnVirtualGoodRollback (Dictionary<string, object> data)
		{
			Debug.Log ("OnVirtualGoodRollback");
			
			// data must be defined or else its an error
			if (data == null) {
				Debug.Log ("OnVirtualGoodRollback - null message");
				return;
			}
			
			if (string.IsNullOrEmpty (data["transactionID"].ToString ())) {
				Debug.Log ("OnVirtualGoodRollback - undefined transaction id");
				return;
			}
			
			string transactionId = data["transactionID"].ToString ();
			
			FuelHandler.Instance.OnPropellerSDKVirtualGoodRollback (transactionId);
			
		}
		
		public override void OnIgniteEventsReceive( List<object> listEvents ) {
			FuelHandler.Instance.OnIgniteEventsReceive( listEvents );
		}
		
		public override void OnIgniteLeaderBoardReceive( Dictionary<string,object> leaderBoard ) {
			FuelHandler.Instance.OnIgniteLeaderBoardReceive( leaderBoard );
		}
		
		public override void OnIgniteMissionReceive(  Dictionary<string,object> mission  ) {
			FuelHandler.Instance.OnIgniteMissionReceive( mission );
		}
		
		public override void OnIgniteQuestReceive( Dictionary<string,object> quest) {
			FuelHandler.Instance.OnIgniteQuestReceive();
		}
		
		public override void OnDataReciever(Dictionary<string, object> message ) {
			Debug.Log( "FuelSDKListener - OnDataReciever. Action: "+message["action"] );
			
			Dictionary<string, object> data = message["data"] as Dictionary<string,object>;
			
			switch( (DataReceiverAction) message["action"] ) {
				
			case DataReceiverAction.fuelSDKIgniteEvents:
				OnIgniteEventsReceive( data["events"] as List<object> );
				break;
			case DataReceiverAction.fuelSDKNotificationEnabled:
				OnNotificationEnabled(data);
				break;
			case DataReceiverAction.fuelSDKNotificationDisabled:
				OnNotificationDisabled(data);
				break;
			case DataReceiverAction.fuelSDKImplicitLaunchRequest:
				OnImplicitLaunch(data);
				break;
			case DataReceiverAction.fuelSDKUserValues:
				OnUserValues(data);
				break;
			case DataReceiverAction.fuelSDKCompeteTournamentInfo:
				OnTournamentInfo(data);
				break;
			case DataReceiverAction.fuelSDKCompeteChallengeCount:
				OnChallengeCountChanged(data);
				break;
			case DataReceiverAction.fuelSDKCompeteUICompletedMatch:
				OnSdkCompletedWithMatch(data);
				break;
			case DataReceiverAction.fuelSDKCompeteUICompletedFail:
				OnSdkFailed(data);
				break;
			case DataReceiverAction.fuelSDKCompeteUICompletedExit:
				OnSdkCompletedWithExit();
				break;
			case DataReceiverAction.fuelSDKSocialShareRequest:
				OnSdkSocialShare(data);
				break;
			case DataReceiverAction.fuelSDKSocialInviteRequest:
				OnSdkSocialInvite(data);
				break;
			case DataReceiverAction.fuelSDKSocialLoginRequest:
				OnSdkSocialLogin(data);
				break;
			case DataReceiverAction.fuelSDKVirtualGoodRollback:
				OnVirtualGoodRollback(data);
				break;
			case DataReceiverAction.fuelSDKVirtualGoodList:
				OnVirtualGoodList(data);
				break;
			case DataReceiverAction.fuelSDKIgniteLeaderBoard:
				OnIgniteLeaderBoardReceive(data["leaderBoard"] as Dictionary<string,object>);
				break;
			case DataReceiverAction.fuelSDKIgniteMission:
				OnIgniteMissionReceive(data["mission"] as Dictionary<string,object>);
				break;
			case DataReceiverAction.fuelSDKIgniteQuest:
				OnIgniteQuestReceive(data["quest"] as Dictionary<string,object>);
				break;
			default:
				break;
			}
		}
	}
}