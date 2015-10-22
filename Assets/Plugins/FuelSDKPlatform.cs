using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class  FuelSDKPlatform  {

	public static FuelSDKPlatform setupPlatform() {
		#if UNITY_ANDROID && !UNITY_EDITOR
			return new FuelSDKAndroid();
		#elif UNITY_IPHONE && !UNITY_EDITOR
			return new FuelSDKiOS();
		#else
			return new FuelSDKEditor();
		#endif

	}

	// -- Fuel intefaces
	public abstract void Initialize(string key, string secret, bool gameHasLogin, bool gameHasInvite, bool gameHasShare);

	public abstract void UseSandbox ();
	
	public abstract void SetNotificationToken (string notificationToken);
	
	public abstract bool EnableNotification (FuelSDK.NotificationType notificationType);
	
	public abstract bool DisableNotification (FuelSDK.NotificationType notificationType);
	
	public abstract bool IsNotificationEnabled (FuelSDK.NotificationType notificationType);
	
	public abstract void SetLanguageCode (string langCode);
	
	public abstract bool SetNotificationIcon (int iconResId);
	
	public abstract bool SetNotificationIcon (string iconName);
	
	public abstract bool SyncVirtualGoods ();
	
	public abstract bool AcknowledgeVirtualGoods( string transactionId, bool consumed);
	
	public abstract bool SdkSocialLoginCompleted (Dictionary<string, string> loginData);
	
	public abstract bool SdkSocialInviteCompleted ();
	
	public abstract bool SdkSocialShareCompleted ();

	// -- Compete intefaces
	public abstract void InitializeCompete();

	public abstract bool SubmitMatchResult (Dictionary<string, object> matchResult);
	
	public abstract void SyncChallengeCounts ();
	
	public abstract void SyncTournamentInfo ();

	// -- Competeui interfaces
	public abstract void InitializeCompeteUI();

	public abstract void SetOrientationUICompete (FuelSDK.ContentOrientation orientation);
	
	public abstract bool Launch ();

	// -- Ingnite intefaces
	public abstract void InitializeIgnite();

	public abstract bool ExecMethod (string method, string parameters);

	public abstract void SendProgress (string progress, string ruleTags);
	
	public abstract bool GetEvents (string eventTags);
	
	public abstract bool GetLeaderBoard (string boardID);
	
	public abstract bool GetMission (string missionID);
	
	public abstract bool GetQuest (string questID);

	// -- Ingniteui intefaces
	public abstract void InitializeIgniteUI();
	
	public abstract  void SetOrientationUIIgnite (FuelSDK.ContentOrientation orientation);

	//--Dynamics methods
	public abstract void InitializeDynamics ();
	
	public abstract bool SetUserConditions (Dictionary<string, object> userConditions);
	
	public abstract bool SyncUserValues ();

	public abstract void OnPause();

	public abstract void OnResume();

	public abstract void OnQuit();

	public abstract void RestoreAllLocalNotifications();

	public abstract void InitializeGCM(string googleProjectNumber);

}
