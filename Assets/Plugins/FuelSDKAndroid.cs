using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FuelSDKSimpleJSON;

#if UNITY_ANDROID
public class FuelSDKAndroid : FuelSDKPlatform {

	private AndroidJavaClass m_jniFuelSDKUnity = null;

	public FuelSDKAndroid() {
		m_jniFuelSDKUnity = new AndroidJavaClass( "com.fuelpowered.lib.fuelsdk.unity.FuelSDKUnitySingleton" );
	}

	#region Fuel intefaces

	public override void Initialize (string key, string secret, bool gameHasLogin, bool gameHasInvite, bool gameHasShare) {
		m_jniFuelSDKUnity.CallStatic("initialize", key, secret, gameHasLogin, gameHasInvite, gameHasShare);
	}

	public override void UseSandbox () {
		m_jniFuelSDKUnity.CallStatic ("useSandbox");
	}
	
	public override void SetNotificationToken (string notificationToken) {
		m_jniFuelSDKUnity.CallStatic ("setNotificationToken", notificationToken);
	}
	
	public override bool EnableNotification (FuelSDK.NotificationType notificationType) {
	
		AndroidJavaObject fuelSDKNotificationType = getFuelSDKNotificationType (notificationType);

		if (fuelSDKNotificationType == null)
		{
			FuelSDKCommon.Log (FuelSDKCommon.LogLevel.ERROR, "invalid notification type");
			return false;
		}

		return m_jniFuelSDKUnity.CallStatic<bool> ("enableNotification", fuelSDKNotificationType);


	}
	
	public override bool DisableNotification (FuelSDK.NotificationType notificationType) {

		AndroidJavaObject fuelSDKNotificationType = getFuelSDKNotificationType (notificationType);
		
		if (fuelSDKNotificationType == null)
		{
			FuelSDKCommon.Log (FuelSDKCommon.LogLevel.ERROR, "invalid notification type");
			return false;
		}
		
		return m_jniFuelSDKUnity.CallStatic<bool> ("disableNotification", fuelSDKNotificationType);

	}
	
	public override bool IsNotificationEnabled (FuelSDK.NotificationType notificationType) {

		AndroidJavaObject fuelSDKNotificationType = getFuelSDKNotificationType (notificationType);
		
		if (fuelSDKNotificationType == null)
		{
			FuelSDKCommon.Log (FuelSDKCommon.LogLevel.ERROR, "invalid notification type");
			return false;
		}
		
		return m_jniFuelSDKUnity.CallStatic<bool> ("isNotificationEnabled", fuelSDKNotificationType);
	}
	
	public override void SetLanguageCode (string langCode) {
		m_jniFuelSDKUnity.CallStatic ("setLanguageCode", langCode);
	}
	
	public override bool SetNotificationIcon (int iconResId) {
		return m_jniFuelSDKUnity.CallStatic<bool>("setNotificationIcon", iconResId);
	}
	
	public override bool SetNotificationIcon (string iconName) {
		return m_jniFuelSDKUnity.CallStatic<bool>("setNotificationIcon", iconName);
	}
	
	public override bool SyncVirtualGoods () {
		return m_jniFuelSDKUnity.CallStatic<bool>("syncVirtualGoods");
	}
	
	public override bool AcknowledgeVirtualGoods( string transactionId, bool consumed) {
		return m_jniFuelSDKUnity.CallStatic<bool>("acknowledgeVirtualGoods", transactionId, consumed);
	}
	
	public override bool SdkSocialLoginCompleted (Dictionary<string, string> loginData) {

		bool succeeded = false;

		using (AndroidJavaObject hashMap = new AndroidJavaObject("java.util.HashMap"))
		{
			if (loginData != null)
			{
				System.IntPtr hashMapPutMethodId = AndroidJNI.GetMethodID (
					hashMap.GetRawClass (),
					"put",
					"(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
				
				foreach (string key in loginData.Keys)
				{
					using (AndroidJavaObject hashMapPutMethodArgKey = new AndroidJavaObject("java.lang.String", key))
					{
						using (AndroidJavaObject hashMapPutMethodArgValue = new AndroidJavaObject ("java.lang.String", loginData [key]))
						{
							object[] args = new object[2];
							args [0] = hashMapPutMethodArgKey;
							args [1] = hashMapPutMethodArgValue;
							
							jvalue[] hashMapPutMethodArgs = AndroidJNIHelper.CreateJNIArgArray (args);
							
							AndroidJNI.CallObjectMethod (
								hashMap.GetRawObject (),
								hashMapPutMethodId,
								hashMapPutMethodArgs);
						}
					}
				}
			}
			
			succeeded = m_jniFuelSDKUnity.CallStatic<bool>("sdkSocialLoginCompleted", hashMap);
		}
			
		return succeeded;

	}
	
	public override bool SdkSocialInviteCompleted () {
		return m_jniFuelSDKUnity.CallStatic<bool>( "sdkSocialInviteCompleted");
	}
	
	public override bool SdkSocialShareCompleted () {
		return m_jniFuelSDKUnity.CallStatic<bool>( "sdkSocialShareCompleted");
	}
	#endregion

	#region Compete intefaces

	public override void InitializeCompete ()
	{
		m_jniFuelSDKUnity.CallStatic ("initializeCompete");
	}

	public override bool SubmitMatchResult (string matchResultJSONString) {
		bool succeeded = false;

		using (AndroidJavaObject matchResultJavaJSONString = new AndroidJavaObject("java.lang.String", matchResultJSONString))
		{
			succeeded = m_jniFuelSDKUnity.CallStatic<bool> ("submitMatchResult", matchResultJavaJSONString);
		}

		return succeeded;
	}
	
	public override void SyncChallengeCounts () {
		m_jniFuelSDKUnity.CallStatic( "syncChallengeCounts");
	}
	
	public override void SyncTournamentInfo () {
		m_jniFuelSDKUnity.CallStatic( "syncTournamentInfo");
	}
	#endregion

	#region Competeui intefaces

	public override void InitializeCompeteUI ()
	{
		m_jniFuelSDKUnity.CallStatic( "initializeCompeteUI");
	}

	public override void SetOrientationUICompete (FuelSDK.ContentOrientation orientation) {

		AndroidJavaObject fuelSDKOrientationType = getFuelSDKOrientationType (orientation);
		
		if (fuelSDKOrientationType == null)
		{
			FuelSDKCommon.Log (FuelSDKCommon.LogLevel.ERROR, "invalid orientation type");
		}

		m_jniFuelSDKUnity.CallStatic ("setOrientationuiCompete", fuelSDKOrientationType);
	}
	
	public override bool Launch ()
	{
		return m_jniFuelSDKUnity.CallStatic<bool>("Launch");
	}
	#endregion

	#region Ignite intefaces

	public override void InitializeIgnite ()
	{
		m_jniFuelSDKUnity.CallStatic ("initializeIgnite");
	}


	public override bool ExecMethod (string method, string parameters) 
	{
		return m_jniFuelSDKUnity.CallStatic <bool>("execMethod", method, parameters);
	}
	
	public override void SendProgress (string progress, string tags) 
	{
		m_jniFuelSDKUnity.CallStatic( "sendProgress", progress, tags);
	}
	
	public override bool GetEvents (string eventTags) 
	{
		return m_jniFuelSDKUnity.CallStatic<bool> ("getEvents", eventTags);
	}
	
	public override bool JoinEvent (string eventID) 
	{
		return m_jniFuelSDKUnity.CallStatic<bool>("joinEvent", eventID);
	}
	
	public override bool GetLeaderBoard (string boardID) 
	{
		return m_jniFuelSDKUnity.CallStatic<bool>("getLeaderBoard", boardID);
	}
	
	public override bool GetMission (string missionID) 
	{
		return m_jniFuelSDKUnity.CallStatic<bool>("getMission", missionID);
	}
	
	public override bool GetQuest (string questID) {
		return m_jniFuelSDKUnity.CallStatic<bool>("getQuest", questID);
	}
	#endregion

	#region Ingniteui intefaces
	public override void InitializeIgniteUI () 
	{
		m_jniFuelSDKUnity.CallStatic ("initializeIgniteUI");
	}
	
	public override  void SetOrientationUIIgnite (FuelSDK.ContentOrientation orientation) 
	{
		AndroidJavaObject fuelSDKOrientationType = getFuelSDKOrientationType (orientation);
		
		if (fuelSDKOrientationType == null)
		{
			FuelSDKCommon.Log (FuelSDKCommon.LogLevel.ERROR, "invalid orientation type");
		}
		
		m_jniFuelSDKUnity.CallStatic ("setOrientationuiIgnite", fuelSDKOrientationType);
	}

	#endregion

	#region Dynamics intefaces

	public override void InitializeDynamics () 
	{
		m_jniFuelSDKUnity.CallStatic ("initializeDynamics");
	}
	
	public override bool SetUserConditions (string userConditions) 
	{
		bool succeeded = false;
		succeeded = m_jniFuelSDKUnity.CallStatic<bool> ("setUserConditions", userConditions);

		return succeeded;

	}
	
	public override bool SyncUserValues () 
	{
		return m_jniFuelSDKUnity.CallStatic<bool>("syncUserValues");
	}
	#endregion


	/// <summary>
	/// Retrieves the AndroidJavaObject PropellerSDKNotificationType equivalent to the given NotificationType
	/// </summary>
	/// <returns>
	/// The AndroidJavaObject PropellerSDKNotificationType equivalent to the given notification type, null otherwise
	/// </returns>
	/// <param name='notificationType'>
	/// The notification type whose equivalent will be retrieved
	/// </param>
	private static AndroidJavaObject getFuelSDKNotificationType(FuelSDK.NotificationType notificationType)
	{
		int notificationTypeValue = (int)notificationType;
		
		AndroidJavaClass fuelSDKNotificationTypeClass = new AndroidJavaClass("com.fuelpowered.lib.fuelsdk.fuelnotificationtype");
		
		return fuelSDKNotificationTypeClass.CallStatic<AndroidJavaObject>("findByValue", notificationTypeValue);
	}


	/// <summary>
	/// Retrieves the AndroidJavaObject PropellerSDKNotificationType equivalent to the given NotificationType
	/// </summary>
	/// <returns>
	/// The AndroidJavaObject PropellerSDKNotificationType equivalent to the given notification type, null otherwise
	/// </returns>
	/// <param name='notificationType'>
	/// The notification type whose equivalent will be retrieved
	/// </param>
	private static AndroidJavaObject getFuelSDKOrientationType(FuelSDK.ContentOrientation orientation)
	{
		string orientationTypeValue = orientation.ToString();
		
		AndroidJavaClass fuelSDKNotificationTypeClass = new AndroidJavaClass("com.fuelpowered.lib.fuelsdk.fuelorientationtype");
		
		return fuelSDKNotificationTypeClass.CallStatic<AndroidJavaObject>("findByValue", orientationTypeValue);
	}

	/// <summary>
	/// Dictionaries to map.
	/// </summary>
	/// <returns>The to map.</returns>
	/// <param name="dictionary">Dictionary.</param>
	private static AndroidJavaObject dictionaryToMap(Dictionary<string, object> dictionary) 
	{
		using (AndroidJavaObject hashMap = new AndroidJavaObject("java.util.HashMap")) {
			if (dictionary != null) {
				System.IntPtr hashMapPutMethodId = AndroidJNI.GetMethodID (
					hashMap.GetRawClass (),
					"put",
					"(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
				foreach (string key in dictionary.Keys) {
					using (AndroidJavaObject hashMapPutMethodArgKey = new AndroidJavaObject("java.lang.String", key)) {
						using (AndroidJavaObject hashMapPutMethodArgValue = new AndroidJavaObject ("java.lang.String", dictionary [key])) {
							object[] args = new object[2];
							args [0] = hashMapPutMethodArgKey;
							args [1] = hashMapPutMethodArgValue;
							
							jvalue[] hashMapPutMethodArgs = AndroidJNIHelper.CreateJNIArgArray (args);
							AndroidJNI.CallObjectMethod (
								hashMap.GetRawObject (),
								hashMapPutMethodId,
								hashMapPutMethodArgs);
						}
					}
				}
			}
			return hashMap;
		}
	}


	public override void OnPause ()
	{
		m_jniFuelSDKUnity.CallStatic ("onPause");
	}

	public override void OnQuit ()
	{
		m_jniFuelSDKUnity.CallStatic ("onQuit");
	}

	public override void OnResume ()
	{
		m_jniFuelSDKUnity.CallStatic ("onResume");
	}

	public override void RestoreAllLocalNotifications ()
	{
		// unused by Android";
	}

	public override void InitializeGCM (string googleProjectNumber)
	{
		m_jniFuelSDKUnity.CallStatic("initializeGCM", googleProjectNumber);
	}

}

#endif 
