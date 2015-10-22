using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuelListenerExample : FuelSDKListener {


	private FuelExample m_fuelExample;


	public FuelListenerExample(FuelExample fuelExample) {

		m_fuelExample = fuelExample;

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
		
		m_fuelExample.SdkOnNotificationEnabled (notificationType);
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
		
		m_fuelExample.SdkOnNotificationDisabled (notificationType);
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
		
		if (m_fuelExample == null) {
			Debug.Log ("OnImplicitLaunch - undefined host game object");
			return;
		}
		

		m_fuelExample.SDKImplicitLaunch(message);
	}


	public override void OnUserValues (Dictionary<string, object> data)
	{

		// message must be defined or else its an error
		if (data == null) {
			Debug.Log ("OnChallengeCountChanged - null message");
			return;
		}

		if (m_fuelExample == null) {
			Debug.Log ("OnSdkSocialShare - undefined listener");
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

		Debug.Log ("OnUserValues!");

		m_fuelExample.OnFuelSDKUserValues (userValuesInfo);
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
	
		
		if (m_fuelExample == null) {
			Debug.Log ("OnTournamentInfo - undefined host game object");
			return;
		}
		
		m_fuelExample.SendMessage ("OnFuelSDKTournamentInfo", tournamentInfo);
	}


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
		
		Debug.Log ("OnChallengeCountChanged - " + data["count"]);
		
		if (m_fuelExample == null) {
			Debug.Log ("OnChallengeCountChanged - undefined host game object");
			return;
		}
		
		m_fuelExample.SendMessage ("OnChallengeCountChanged", data["count"]);
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
		
		if (m_fuelExample == null) {
			Debug.Log ("OnSdkCompletedWithMatch - undefined listener");
			return;
		}
		
		m_fuelExample.SdkCompletedWithMatch (matchInfo);
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


		if (m_fuelExample == null) {
			Debug.Log ("OnSdkFailed - undefined listener");
			return;
		}
		
		m_fuelExample.SdkFailed (failReason);
	}


	public override void OnSdkCompletedWithExit ()
	{
		Debug.Log ("OnSdkCompletedWithExit");
		m_fuelExample.SdkCompletedWithExit ();
	}

	public override void OnSdkSocialShare (Dictionary<string, object> data)
	{
		Debug.Log ("OnSdkSocialShare");

		// data must be defined or else its an error
		if (data == null) {
			Debug.Log ("OnSdkSocialShare - null message");
			return;
		}

		if (m_fuelExample == null) {
			Debug.Log ("OnSdkSocialShare - undefined listener");
			return;
		}

		m_fuelExample.SdkSocialShare (data);
	}


	public override void OnSdkSocialInvite (Dictionary<string, object> data)
	{
		Debug.Log ("OnSdkSocialInvite");

		// data must be defined or else its an error
		if (data == null) {
			Debug.Log ("OnSdkSocialInvite - null message");
			return;
		}
		
		if (m_fuelExample == null) {
			Debug.Log ("OnSdkSocialInvite - undefined listener");
			return;
		}

		m_fuelExample.SdkSocialInvite (data);
	}


	public override void OnSdkSocialLogin (Dictionary<string, object> data)
	{
		Debug.Log ("OnSdkSocialLogin");
		
		// data must be defined or else its an error
		if (data == null) {
			Debug.Log ("OnSdkSocialLogin - null message");
			return;
		}
		
		if (m_fuelExample == null) {
			Debug.Log ("OnSdkSocialLogin - undefined listener");
			return;
		}
		
		bool allowCache = System.Convert.ToBoolean (data["allowCache"].ToString());
		m_fuelExample.SdkSocialLogin (allowCache);
	}

	public override void OnVirtualGoodRollback (Dictionary<string, object> data)
	{
		Debug.Log ("OnVirtualGoodRollback");
		
		// data must be defined or else its an error
		if (data == null) {
			Debug.Log ("OnVirtualGoodRollback - null message");
			return;
		}
		
		if (m_fuelExample == null) {
			Debug.Log ("OnVirtualGoodRollback - undefined listener");
			return;
		}
	
		if (string.IsNullOrEmpty (data["transactionID"].ToString ())) {
			Debug.Log ("OnVirtualGoodRollback - undefined transaction id");
			return;
		}

		string transactionId = data["transactionID"].ToString ();

		m_fuelExample.OnFuelSDKVirtualGoodRollback (transactionId);
	}


	public override void OnVirtualGoodList (Dictionary<string, object> data)
	{
		Debug.Log ("OnVirtualGoodList");

		// data must be defined or else its an error
		if (data == null) {
			Debug.Log ("OnVirtualGoodRollback - null message");
			return;
		}
		
		if (m_fuelExample == null) {
			Debug.Log ("OnVirtualGoodRollback - undefined listener");
			return;
		}

		if (string.IsNullOrEmpty (data["transactionID"].ToString())) {
			Debug.Log ("OnVirtualGoodRollback - undefined transaction id");
			return;
		}
		Dictionary<string, object> virtualGoodInfo = new Dictionary<string, object> ();
		virtualGoodInfo.Add ("transactionID", data["transactionID"].ToString());
		
		List<string> virtualGoods = new List<string> ();
		virtualGoods = data ["virtualGoods"] as List<string>;

		virtualGoodInfo.Add ("virtualGoods", virtualGoods);

		m_fuelExample.OnFuelSDKVirtualGoodList (virtualGoodInfo);

	}


	public override void OnIgniteEventsReceive( List<object> listEvents ) {

		foreach (object value in listEvents) {
			Debug.Log (value.ToString());
		}
		//FuelSDKManager.Instance.OnIgniteEventsReceive( listEvents );
	}

	public override  void OnIgniteLeaderBoardReceive( Dictionary<string,object> leaderBoard ) {
		//FuelSDKManager.Instance.OnIgniteLeaderBoardReceive();
	}

	public override  void OnIgniteMissionReceive(  Dictionary<string,object> mission  ) {
		//FuelSDKManager.Instance.OnIgniteMissionReceive( mission );
	}

	public override  void OnIgniteQuestReceive( Dictionary<string,object> quest) {
		//FuelSDKManager.Instance.OnIgniteQuestReceive();
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
