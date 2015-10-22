using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using FuelSDKSimpleJSON;

#if UNITY_IPHONE

public class FuelSDKiOS : FuelSDKPlatform {

	[DllImport ("__Internal")]
	private static extern void iOSInitialize(string key, string secret, bool gameHasLogin, bool gameHasInvite, bool gameHasShare);
	[DllImport ("__Internal")]
	private static extern void iOSSetLanguageCode(string languageCode);
	[DllImport ("__Internal")]
	private static extern bool iOSLaunch();
	[DllImport ("__Internal")]
	private static extern bool iOSSubmitMatchResult(string delimitedMatchInfo);
	[DllImport ("__Internal")]
	private static extern void iOSSyncChallengeCounts();
	[DllImport ("__Internal")]
	private static extern void iOSSyncTournamentInfo();
	[DllImport ("__Internal")]
	private static extern bool iOSSyncVirtualGoods();
	[DllImport ("__Internal")]
	private static extern bool iOSAcknowledgeVirtualGoods(string transactionId, bool consumed);
	[DllImport ("__Internal")]
	private static extern bool iOSEnableNotification(FuelSDK.NotificationType notificationType);
	[DllImport ("__Internal")]
	private static extern bool iOSDisableNotification(FuelSDK.NotificationType notificationType);
	[DllImport ("__Internal")]
	private static extern bool iOSIsNotificationEnabled(FuelSDK.NotificationType notificationType);
	[DllImport ("__Internal")]
	private static extern bool iOSSdkSocialLoginCompleted(string loginInfo);
	[DllImport ("__Internal")]
	private static extern bool iOSSdkSocialInviteCompleted();
	[DllImport ("__Internal")]
	private static extern bool iOSSdkSocialShareCompleted();
	[DllImport ("__Internal")]
	private static extern void iOSRestoreAllLocalNotifications();
	[DllImport ("__Internal")]
	private static extern void iOSUseSandbox();
	[DllImport ("__Internal")]
	private static extern void iOSSetNotificationToken();
	[DllImport ("__Internal")]
	private static extern void iOSInitializeCompete();
	[DllImport ("__Internal")]
	private static extern void iOSInitializeCompeteUI();
	[DllImport ("__Internal")]
	private static extern void iOSInitializeIgnite();
	[DllImport ("__Internal")]
	private static extern void iOSInitializeIgniteUI();
	[DllImport ("__Internal")]
	private static extern void iOSInitializeDynamics ();
	[DllImport ("__Internal")]
	private static extern bool iOSSyncUserValues ();
	[DllImport ("__Internal")]
	private static extern void iOSSetOrientationuiCompete (string orientation);
	[DllImport ("__Internal")]
	private static extern void iOSSetOrientationuiIgnite (string orientation);
	[DllImport ("__Internal")]
	private static extern bool iOSExecMethod (string method, string parameters);
	[DllImport ("__Internal")]
	private static extern void iOSSendProgress (string progress, string ruleTags);
	[DllImport ("__Internal")]
	private static extern bool iOSIgniteGetEvent (string tags);
	[DllImport ("__Internal")]
	private static extern bool iOSIgniteGetLeaderBoard (string boardID);
	[DllImport ("__Internal")]
	private static extern bool iOSIgniteGetMission (string missionID);
	[DllImport ("__Internal")]
	private static extern bool iOSIgniteGetQuest (string questID);



	public FuelSDKiOS() {
		
	}

	#region Fuel intefaces
	
	public override void Initialize (string key, string secret, bool gameHasLogin, bool gameHasInvite, bool gameHasShare) {
		iOSInitialize(key, secret, gameHasLogin, gameHasInvite, gameHasShare);
	}
	
	public override void UseSandbox () {
		iOSUseSandbox ();
	}
	
	public override void SetNotificationToken (string notificationToken) {
		iOSSetNotificationToken();
	}
	
	public override bool EnableNotification (FuelSDK.NotificationType notificationType) {
		return iOSEnableNotification(notificationType);
	}
	
	public override bool DisableNotification (FuelSDK.NotificationType notificationType) {
		return iOSDisableNotification(notificationType);
	}
	
	public override bool IsNotificationEnabled (FuelSDK.NotificationType notificationType) {
		return iOSIsNotificationEnabled(notificationType);
	}
	
	public override void SetLanguageCode (string langCode) {
		iOSSetLanguageCode(langCode);
	}
	
	public override bool SetNotificationIcon (int iconResId) {
		Debug.Log ("SetNotificationIcon - unused by iOS");
		return false;
	}
	
	public override bool SetNotificationIcon (string iconName) {
		Debug.Log ("SetNotificationIcon - unused by iOS");
		return false;
	}
	
	public override bool SyncVirtualGoods () {
		return iOSSyncVirtualGoods();
	}
	
	public override bool AcknowledgeVirtualGoods( string transactionId, bool consumed) {
		return iOSAcknowledgeVirtualGoods(transactionId, consumed);
	}
	
	public override bool SdkSocialLoginCompleted (Dictionary<string, string> loginData) {
		
		string urlEncodedString = null;
		
		if (loginData != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			
			bool first = true;
			
			foreach ( KeyValuePair<string,string> entry in loginData )
			{
				if (first)
				{
					first = false;
				}
				else
				{
					stringBuilder.Append( "&" );
				}
				
				stringBuilder.Append( WWW.EscapeURL (entry.Key) );
				stringBuilder.Append( "=" );
				stringBuilder.Append( WWW.EscapeURL (entry.Value) );
			}
			
			urlEncodedString = stringBuilder.ToString();
		}
		
		return iOSSdkSocialLoginCompleted( urlEncodedString );
	}
	
	public override bool SdkSocialInviteCompleted () {
		return iOSSdkSocialInviteCompleted();
	}
	
	public override bool SdkSocialShareCompleted () {
		return iOSSdkSocialShareCompleted();
	}
	#endregion

	#region Compete intefaces
	
	public override void InitializeCompete()
	{
		iOSInitializeCompete ();
	}
	
	public override bool SubmitMatchResult (Dictionary<string, object> matchResult) {
		Debug.Log ("SubmitMatchResult - start");
		
		bool succeeded = false;
		
		if (!Application.isEditor) {
			
			JSONClass matchResultJSON = FuelSDKCommon.toJSONClass (matchResult);
			
			if (matchResultJSON == null) {
				Debug.Log ("SubmitMatchResult - match result parse error");
			} else {
				succeeded = iOSSubmitMatchResult( matchResultJSON.ToString () );
			}
		}
		Debug.Log ("SubmitMatchResult - end");
		
		return succeeded;
	}
	
	public override void SyncChallengeCounts () {
		iOSSyncChallengeCounts();
	}
	
	public override void SyncTournamentInfo () {
		iOSSyncTournamentInfo();
	}
	#endregion
	
	#region Competeui intefaces
	public override void InitializeCompeteUI()
	{
		iOSInitializeCompeteUI();
	}

	public override void SetOrientationUICompete (FuelSDK.ContentOrientation orientation) {
		iOSSetOrientationuiCompete (orientation.ToString ());
	}
	
	public override bool Launch ()
	{
		return iOSLaunch();
	}
	#endregion
	
	#region Ignite intefaces
	
	public override void InitializeIgnite ()
	{
		iOSInitializeIgnite ();
	}
	
	
	public override bool ExecMethod (string method, string parameters) {

		Debug.Log ("ExecMethod");
		return iOSExecMethod(method, parameters);
	}
	
	public override void SendProgress (string progress, string tags) {

		Debug.Log ("SendProgress");
		iOSSendProgress(progress , tags);
	}
	
	public override bool GetEvents (string eventTags) {
		return iOSIgniteGetEvent( eventTags );
	}
	
	public override bool GetLeaderBoard (string boardID) {
		Debug.Log ("GetLeaderBoard");
		return iOSIgniteGetLeaderBoard(boardID);
	}
	
	public override bool GetMission (string missionID) {
		Debug.Log ("GetMission");
		return iOSIgniteGetMission(missionID);
	}
	
	public override bool GetQuest (string questID) {
		Debug.Log ("GetMission");
		return iOSIgniteGetQuest(questID);

	}
	#endregion
	
	#region Ingniteui intefaces
	public override void InitializeIgniteUI () {
		iOSInitializeIgniteUI ();
	}
	
	public override  void SetOrientationUIIgnite (FuelSDK.ContentOrientation orientation) {
		iOSSetOrientationuiIgnite(orientation.ToString());
	}
	
	#endregion
	
	#region Dynamics intefaces
	
	public override void InitializeDynamics () {
		iOSInitializeDynamics ();
	}
	
	public override bool SetUserConditions (Dictionary<string, object> userConditions) {
		Debug.Log ("SetUserConditions");
		throw new System.NotImplementedException ();
	}
	
	public override bool SyncUserValues () {
		return iOSSyncUserValues ();
	}
	#endregion

	/// <summary>
	/// Restores cancelled Propeller SDK local notifications
	/// </summary>
	public override void RestoreAllLocalNotifications ()
	{
		Debug.Log ("RestoreAllLocalNotifications - start");
		
		if (!Application.isEditor) {
			iOSRestoreAllLocalNotifications();
		}
		Debug.Log ("RestoreAllLocalNotifications - end");
	}


	public override void OnPause ()
	{
		Debug.Log ("OnPause - unused by iOS");
	}

	public override void OnQuit ()
	{
		Debug.Log ("OnQuit - unused by iOS");
	}

	public override void OnResume ()
	{
		Debug.Log ("OnResume - unused by iOS");
	}

	public override void InitializeGCM (string googleProjectNumber)
	{
		Debug.Log ("InitializeGCM - unused by iOS");
	}	
}
#endif
