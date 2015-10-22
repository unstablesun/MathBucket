using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FuelSDKSimpleJSON;


public class FuelSDK : MonoBehaviour {

	#region ===================================== data enums =====================================

	public enum ContentOrientation
	{
		landscape,
		portrait,
		auto
	}
	
	public enum NotificationType
	{
		none 	= 0x0,
		all 	= 0x3,
		push 	= 1 << 0,
		local 	= 1 << 1
	}



	#endregion

	#region ===================================== Editor Variables =====================================
	
	[SerializeField]
	private string GameKey;
	[SerializeField]
	private string GameSecret;
	[SerializeField]
	private bool UseTestServers = false;
	[SerializeField]
	private ContentOrientation Orientation = ContentOrientation.landscape;
	[SerializeField]
	private string HostGameObjectName;
	[SerializeField]
	private bool iOSGameHandleLogin;
	[SerializeField]
	private bool iOSGameHandleInvite;
	[SerializeField]
	private bool iOSGameHandleShare;
	[SerializeField]
	private string AndroidNotificationIcon = "notify_icon";
	[SerializeField]
	private string AndroidGCMSenderID;
	[SerializeField]
	private bool AndroidGameHandleLogin;
	[SerializeField]
	private bool AndroidGameHandleInvite;
	[SerializeField]
	private bool AndroidGameHandleShare;
	[SerializeField]
	private bool IgniteEnabled;
	[SerializeField]
	private bool CompeteEnabled;
	[SerializeField]
	private bool DynamicsEnabled;

	[SerializeField]
	private bool createTestData = true;

	#endregion

	#region ===================================== Static Local Variables =====================================

	public static bool Initialized {
		get{ 
			return m_bInitialized;
		}
	}

	private static FuelSDKPlatform platformSelected;
	private static bool m_bInitialized;
	private static FuelSDKListener m_listener;

	#endregion

	#region ===================================== MonoBehaviour =====================================

	void Start() {

		if(CompeteEnabled) {
			InitializeCompeteUI();
			SetOrientationUICompete (Orientation);
		}
		
		if(IgniteEnabled) {
			InitializeIgniteUI();
			SetOrientationUIIgnite (Orientation);
		}
	}
	
	void Awake () {
		if (Application.isEditor && createTestData) {
			gameObject.AddComponent<TestFuelSDK>();
		}
	
		platformSelected = FuelSDKPlatform.setupPlatform ();
		
		if (!m_bInitialized) {
			GameObject.DontDestroyOnLoad (gameObject);
			
			if (!Application.isEditor) {
				
				bool gameHandleLogin = false;
				bool gameHandleInvite = false;
				bool gameHandleShare = false;
				
				#if UNITY_IPHONE
				gameHandleLogin = iOSGameHandleLogin;
				gameHandleInvite = iOSGameHandleInvite;
				gameHandleShare = iOSGameHandleShare;
				#elif UNITY_ANDROID
				gameHandleLogin = AndroidGameHandleLogin;
				gameHandleInvite = AndroidGameHandleInvite;
				gameHandleShare = AndroidGameHandleShare;				
				#endif
				
				if (UseTestServers) {
					platformSelected.UseSandbox();
				}
				
				platformSelected.Initialize(GameKey, GameSecret, gameHandleLogin, gameHandleInvite, gameHandleShare);
				
				if(CompeteEnabled) {
					InitializeCompete();
				}
				
				if(IgniteEnabled) {
					InitializeIgnite();
				}
				
				if(DynamicsEnabled) {
					InitializeDynamics();
				}
				
				platformSelected.SetNotificationIcon(AndroidNotificationIcon);
				
				platformSelected.InitializeGCM(AndroidGCMSenderID);
			}

//			if (!string.IsNullOrEmpty (HostGameObjectName)) {
//				
//				GameObject common = new GameObject();
//				common.name = "PropellerCommon";
//				common.AddComponent<PropellerCommon>();
//			}
			
			m_bInitialized = true;
		} else {
			GameObject.Destroy (gameObject);	
		}
		
	}
	
	private void OnApplicationPause (bool paused)
	{
		if (!Application.isEditor) {
			if (paused) {
				platformSelected.OnPause();
			} else {
				platformSelected.OnResume();
			}
		}
		
	}
	
	private void OnApplicationQuit ()
	{
		if (!Application.isEditor) {
			platformSelected.OnQuit(); //Only for Android
		}
	}

	#endregion

	#region ===================================== Static Methods =====================================

	static FuelSDK(){
		m_bInitialized = false;
	}

	//FUEL SDK
	public static void UseSandbox() {
		platformSelected.UseSandbox ();
	}

	#endregion

	#region ===================================== Notifications =====================================

	public static void SetNotificationToken (string notificationToken) {
		platformSelected.SetNotificationToken(notificationToken);
	}

	public static void EnableNotification (NotificationType notificationType)
	{
		Debug.Log ("enableNotification - start");
		
		if (!Application.isEditor) {
			Debug.Log ("EnableNotification - " + notificationType);
			platformSelected.EnableNotification(notificationType);
		}

		Debug.Log ("enableNotification - end");
	}

	public static void DisableNotification (NotificationType notificationType)
	{
		Debug.Log ("disableNotification - start");
		
		if (!Application.isEditor) {
			Debug.Log ("DisableNotification - " + notificationType);
			platformSelected.DisableNotification(notificationType);
		}
		
		Debug.Log ("disableNotification - end");
	}

	public static bool IsNotificationEnabled (NotificationType notificationType)
	{
		Debug.Log ("isNotificationEnabled - start");
		
		bool succeed = false;
		
		if (!Application.isEditor) {
			succeed = platformSelected.IsNotificationEnabled(notificationType);
			Debug.Log ("IsNotificationEnabled - " + notificationType + ":" + succeed);
		}
		
		Debug.Log ("isNotificationEnabled - end");
		
		return succeed;
	}

	#endregion
	
	#region ===================================== Language =====================================

	/// <summary>
	/// Sets the language code for the Propeller SDK online content. Must be compliant to ISO 639-1.
	/// If the language code is not supported, then the content will default to English (en)
	/// </summary>
	/// <param name="languageCode">
	/// Two character string language code to set the Fuel SDK online content to.
	/// </param>
	public static void SetLanguageCode (string languageCode)
	{
		Debug.Log ("setLanguageCode - start");
		
		if (!Application.isEditor) {
			Debug.Log ("setLanguageCode - " + languageCode);
			platformSelected.SetLanguageCode(languageCode);
		}
		
		Debug.Log ("setLanguageCode - end");
	}

	#endregion
	
	#region ===================================== Virtual Goods =====================================

	/// <summary>
	/// Begins an asynchronous operation to request the virtual goods from Propeller.
	/// </summary>
	public static void SyncVirtualGoods ()
	{
		Debug.Log ("syncVirtualGoods - start");
		
		if (!Application.isEditor) {
			platformSelected.SyncVirtualGoods();
		}
		
		Debug.Log ("syncVirtualGoods - end");
	}

	/// <summary>
	/// Begins an asynchronous operation to acknowledge the received virtual goods from Fuel.
	/// </summary>
	/// <param name="transactionId">
	/// The transaction ID being acknowledged
	/// </param>
	/// <param name="consumed">
	/// Flags whether or not the virutal good were consumed
	/// </param>
	public static void AcknowledgeVirtualGoods (string transactionId, bool consumed)
	{
		Debug.Log ("acknowledgeVirtualGoods - start");
		
		if (!Application.isEditor) {
			platformSelected.AcknowledgeVirtualGoods(transactionId, consumed);
		}

		Debug.Log ("acknowledgeVirtualGoods - end");
	}

	#endregion
	
	#region ===================================== Login =====================================

	/// <summary>
	/// Begins an asynchronous operation to indicate login info is complete.
	/// </summary>
	/// <param name="loginInfo">Login info.</param>
	public static void SdkSocialLoginCompleted (Dictionary<string, string> loginInfo)
	{
		Debug.Log ("sdkSocialLoginCompleted - start");
		
		if (!Application.isEditor) {

			platformSelected.SdkSocialLoginCompleted(loginInfo);
		}

		Debug.Log ("sdkSocialLoginCompleted - end");
	}

	#endregion
	
	#region ===================================== Social =====================================

	/// <summary>
	/// Begins an asynchronous operation to indicate invite info is complete.
	/// </summary>
	public static void SdkSocialInviteCompleted ()
	{
		Debug.Log ("sdkSocialInviteCompleted - start");
		
		if (!Application.isEditor) {
			platformSelected.SdkSocialInviteCompleted();
		}
		
		Debug.Log ("sdkSocialInviteCompleted - end");
	}

	/// <summary>
	/// Begins an asynchronous operation to indicate sharing info is complete.
	/// </summary>
	public static void SdkSocialShareCompleted ()
	{
		Debug.Log ("sdkSocialShareCompleted - start");
		
		if (!Application.isEditor) {
			platformSelected.SdkSocialShareCompleted();
		}
		
		Debug.Log ("sdkSocialShareCompleted - end");
	}


	#endregion
	
	#region ===================================== Compete =====================================

	/// <summary>
	/// Initializes compete.
	/// </summary>
	public static void InitializeCompete() {
		Debug.Log ("initializeCompete - start");

		platformSelected.InitializeCompete ();

		Debug.Log ("initializeCompete - end");
	}
	
	/// <summary>
	/// Submits the results of the match. You must stuff the match result into a dictionary that will be parsed and passed to the SDK API servers..
	/// Current parameters:
	/// 	matchID
	/// 	tournamentID
	/// 	score
	/// </summary>
	/// <returns>
	/// True if the match results were submitted.
	/// </returns>
	/// <param name="matchResult">
	/// A dictionary filled with values the SDK needs to use to properly pass the result to the API servers. Examples: score, tournamentID, matchID, etc.
	/// </param>
	public static bool SubmitMatchResult (Dictionary<string, object> matchResult) {
		Debug.Log ("submitMatchResult - start");
		
		bool succeeded = false;
		
		if (!Application.isEditor) {
			succeeded = platformSelected.SubmitMatchResult(matchResult);
		}
		
		Debug.Log ("submitMatchResult - end");
		
		return succeeded;
	}

	/// <summary>
	/// Begins an asynchronous operation to request the player's challenge counts from Propeller.
	/// </summary>
	public static void SyncChallengeCounts ()
	{
		Debug.Log ("syncChallengeCounts - start");
		
		if (!Application.isEditor) {
			platformSelected.SyncChallengeCounts();
		}
		
		Debug.Log ("syncChallengeCounts - end");
	}

	/// <summary>
	/// Begins an asynchronous operation to request the tournament information from Propeller.
	/// </summary>
	public static void SyncTournamentInfo ()
	{
		Debug.Log ("syncTournamentInfo - start");
		
		if (!Application.isEditor) {
			platformSelected.SyncTournamentInfo();
		}
		
		Debug.Log ("syncTournamentInfo - end");
	}

	//Compete UI

	public static void InitializeCompeteUI() {
		Debug.Log ("InitializeCompeteUI - start");
		
		platformSelected.InitializeCompeteUI ();
		
		Debug.Log ("InitializeCompeteUI - end");
	}

	/// <summary>
	/// Sets the orientationui compete.
	/// </summary>
	/// <param name="orientation">Orientation.</param>
	public static void SetOrientationUICompete(ContentOrientation orientation) {

		Debug.Log ("SetOrientationUICompete - start");

		platformSelected.SetOrientationUICompete (orientation);

		Debug.Log ("SetOrientationUICompete - end");

	}

	/// <summary>
	/// Launch the SDK with the provided listener to handle callbacks.
	/// </summary>
	/// <param name='listener'>
	/// A class that subclasses the PropellerSDKListener abstract class that will receive various callbacks.
	/// </param>
	public static bool Launch ()
	{
		Debug.Log ("launch - start");

		bool succeeded = false;
		
		if (!Application.isEditor) {
			succeeded = platformSelected.Launch();
		}
		
		Debug.Log ("launch - end");
		
		return succeeded;
	}

	#endregion
	
	#region ===================================== Ignite =====================================

	/// <summary>
	/// Initializes ignite.
	/// </summary>
	public static void InitializeIgnite() {

		Debug.Log ("initializeIgnite - start");

		platformSelected.InitializeIgnite ();

		Debug.Log ("initializeIgnite - end");
	}

	/// <summary>
	/// Execs the method.
	/// </summary>
	/// <returns><c>true</c>, if method was execed, <c>false</c> otherwise.</returns>
	/// <param name="method">Method.</param>
	/// <param name="parameters">Parameters.</param>
	public static bool ExecMethod (string method, List<object> parameters) {

		Debug.Log ("execMethod - start");

		bool succeeded = false; 

		string parametersString = FuelSDKCommon.Serialize(parameters);

		succeeded = platformSelected.ExecMethod (method, parametersString);

		Debug.Log ("execMethod - end");

		return succeeded;
	}

	/// <summary>
	/// Sends the progress.
	/// </summary>
	/// <param name="progress">Progress.</param>
	/// <param name="tags">Tags.</param>
	public static void SendProgress (Dictionary<string, object> progress, List<object> tags) {

		Debug.Log ("sendProgress - start");

		string progressString = FuelSDKCommon.Serialize(progress);
		string tagsString = FuelSDKCommon.Serialize(tags);

		if (progressString == null) {
			Debug.Log ("SendProgress - end with error: imposible to parse the progress");
		} else {
			platformSelected.SendProgress (progressString, tagsString);
			Debug.Log ("sendProgress - end");
		}
	}

	/// <summary>
	/// Gets the events.
	/// </summary>
	/// <returns><c>true</c>, if events was gotten, <c>false</c> otherwise.</returns>
	public static bool GetEvents (List<object> eventTags) {

		Debug.Log ("getEvents - start");
		bool succeeded = false;

		string eventTagsString = FuelSDKCommon.Serialize(eventTags);

		succeeded =  platformSelected.GetEvents (eventTagsString);

		Debug.Log ("getEvents - end");

		return succeeded;
	}
	
	/// <summary>
	/// Gets the leader board.
	/// </summary>
	/// <returns><c>true</c>, if leader board was gotten, <c>false</c> otherwise.</returns>
	/// <param name="boardID">Board I.</param>
	public static bool GetLeaderBoard (string boardID) {

		Debug.Log ("getLeaderBoard - start");
		bool succeeded = false;

		succeeded = platformSelected.GetLeaderBoard (boardID);

		Debug.Log ("getLeaderBoard - end");
		return succeeded;
	}

	/// <summary>
	/// Gets the mission.
	/// </summary>
	/// <returns><c>true</c>, if mission was gotten, <c>false</c> otherwise.</returns>
	/// <param name="missionID">Mission I.</param>
	public static bool GetMission (string missionID) {

		Debug.Log ("getMission - start");
		bool succeeded = false;

		succeeded = platformSelected.GetMission (missionID);

		Debug.Log ("getMission - end");
		return succeeded;
	}

	/// <summary>
	/// Gets the quest.
	/// </summary>
	/// <returns><c>true</c>, if quest was gotten, <c>false</c> otherwise.</returns>
	/// <param name="questID">Quest I.</param>
	public static bool GetQuest (string questID) {

		Debug.Log ("getQuest - start");

		bool succeeded = false;

		succeeded = platformSelected.GetQuest (questID);

		Debug.Log ("getQuest - end");

		return succeeded;

	}

	//Ingnite UI

	/// <summary>
	/// Initializes the ingite U.
	/// </summary>
	public static void InitializeIgniteUI () {

		Debug.Log ("InitializeIgniteUI - start");

		platformSelected.InitializeIgniteUI ();

		Debug.Log ("InitializeIgniteUI - end");
	}

	/// <summary>
	/// Sets the orientationui ignite.
	/// </summary>
	/// <param name="orientation">Orientation.</param>
	public static void SetOrientationUIIgnite (ContentOrientation orientation) {

		Debug.Log ("setOrientationuiIgnite - start");

		platformSelected.SetOrientationUIIgnite (orientation);

		Debug.Log ("setOrientationuiIgnite - end");
	}

	//Dynamics

	/// <summary>
	/// Initializes dynamics.
	/// </summary>
	public static void InitializeDynamics () {

		Debug.Log ("initializeDynamics - start");

		platformSelected.InitializeDynamics ();

		Debug.Log ("initializeDynamics - end");
	}

	/// <summary>
	/// FUEL DYNAMICS - SetUserConditions
	/// 
	/// </summary>
	/// <returns>
	/// True if the call to the SDK succeeded.
	/// </returns>
	/// <param name="conditions">
	/// A dictionary filled with values the SDK needs to use to properly pass the result to the Propeller servers. Examples: score, tournamentID, matchID, etc.
	/// </param>
	public static bool SetUserConditions (Dictionary<string, object> conditions)
	{
		Debug.Log ("setUserConditions - start");
		
		bool succeeded = false;
		
		if (!Application.isEditor) {
			succeeded = platformSelected.SetUserConditions(conditions);
		}
		
		Debug.Log ("setUserConditions - end");
		
		return succeeded;
	}

	/// <summary>
	/// Begins an asynchronous operation to request the user values from Propeller.
	/// </summary>
	/// <returns><c>true</c>, if user values was synced, <c>false</c> otherwise.</returns>
	public static bool SyncUserValues ()
	{
		Debug.Log ("syncUserValues - start");
		
		bool succeeded = false;
		
		if (!Application.isEditor) {
			succeeded = platformSelected.SyncUserValues();
		}
		
		Debug.Log ("syncUserValues - end");
		
		return succeeded;
	}

	#endregion
	
	#region ===================================== Callbacks =====================================

	/// <summary>
	/// Sets the listener.
	/// </summary>
	/// <param name="listener">Listener.</param>
	public static void setListener(FuelSDKListener listener)
	{
		m_listener = listener;
	}

	/// <summary>
	/// Restores cancelled Propeller SDK local notifications
	/// </summary>
	public static void RestoreAllLocalNotifications ()
	{
		Debug.Log ("RestoreAllLocalNotifications - start");
		
		if (!Application.isEditor) {
			platformSelected.RestoreAllLocalNotifications();
		}
		
		Debug.Log ("RestoreAllLocalNotifications - end");
	}
	
	/// <summary>
	/// Data Receiver
	/// </summary>
	/// <param name="message">data</param>
	private void DataReceiver (string message)
	{
		Debug.Log ("DataReceiver - start");
		
		Dictionary<string, object> messageObject = FuelSDKCommon.Deserialize( message ) as Dictionary<string,object>;

		Debug.Log ("Message deserialized : " + messageObject.ToString());

		if( !messageObject.ContainsKey("action") ) {
			Debug.Log( "FuelSDK - DataReceiver error. No specific action in the response object" );
			return;
		}
		if( !messageObject.ContainsKey("data") ) {
			Debug.Log( "FuelSDK - DataReceiver error. No specific data in the response object" );
			return;
		}

		Debug.Log ("Message action : " + messageObject["action"].ToString());

		messageObject["action"] = (FuelSDKListener.DataReceiverAction) Enum.Parse( typeof(FuelSDKListener.DataReceiverAction) , messageObject["action"].ToString() );
		m_listener.OnDataReciever(messageObject);

		Debug.Log ("DataReceiver - end");
	}

	#endregion

}
