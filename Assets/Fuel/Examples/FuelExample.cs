using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class FuelExample : MonoBehaviour {

	private FuelSDKListener m_listener;
	private bool m_bInitialized;

	private string m_tournamentID;
	private string m_matchID;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void launch() {
		Debug.Log ("launch");
		FuelSDK.Launch ();
	}

	public void initializeCompete() {
		Debug.Log ("initializeCompete");
		FuelSDK.InitializeCompete ();
	}

	public void initializeIgnite() {
		Debug.Log ("initializeIgnite");
		FuelSDK.InitializeIgnite ();
	}

	public void testLogin() {
		Debug.Log ("Calling testLogin");
		Invoke ("SendLoginDetails", 0.5f);
	}


	private void SendLoginDetails ()
	{
		Debug.Log ("Calling SendingLoginDetails");
		
		Dictionary<string, string> loginInfo = new Dictionary<string, string> ();
		loginInfo.Add ("provider", "facebook");
		loginInfo.Add ("email", "testguy@fuelpowered.com");
		loginInfo.Add ("id", "testguyid");
		loginInfo.Add ("nickname", "testguy445");
		loginInfo.Add ("token", "testguy445");

		FuelSDK.SdkSocialLoginCompleted (loginInfo);
		// Return null in the case of a failure
		//PropellerSDK.SdkSocialLoginCompleted(null);
	}


	public void syncChallengeCounts() {
		Debug.Log ("Calling syncChallengeCounts");
		FuelSDK.SyncChallengeCounts ();
		
	}

	public void syncTournamentInfo() {
		Debug.Log ("Calling syncTournamentInfo");
		FuelSDK.SyncTournamentInfo ();
		
	}

	public void syncVirtualGoods() {
		Debug.Log ("Calling syncVirtualGoods");
		FuelSDK.SyncVirtualGoods ();
	}

	public void syncUserValues() {
		Debug.Log ("Calling syncUserValues");

		bool succedded = FuelSDK.SyncUserValues ();

		Debug.Log ("response from syncUserValues" + succedded.ToString());

	}

	public void enableLocalNotification() {
		FuelSDK.EnableNotification (FuelSDK.NotificationType.local);
	}

	public void enableRemoteNotification() {
		FuelSDK.EnableNotification (FuelSDK.NotificationType.push);
	}

	public void disableLocalNotification() {
		FuelSDK.DisableNotification (FuelSDK.NotificationType.local);
	}

	public void disableRemoteNotification() {
		FuelSDK.DisableNotification (FuelSDK.NotificationType.push);
	}

	public void isLocalNotificationEnabled() {
		FuelSDK.IsNotificationEnabled (FuelSDK.NotificationType.local);
	}

	public void isRemoteNotificationEnabled() {
		FuelSDK.IsNotificationEnabled (FuelSDK.NotificationType.push);
	}


	public void execMethod() {
	
		List<object> tags = new List<object>();

		tags.Add("allowedMissions");
		tags.Add("allowedQuests");

//		List<Object> parameters = new List<Object>();
//		parameters.Add((Object)tags);
//
		FuelSDK.ExecMethod ("GetEvents", tags);

	}

	public void getEvents() {
		List<object> tags = new List<object>();
		tags.Add("allowedMissions");
		tags.Add("allowedQuests");
		FuelSDK.GetEvents (tags);
	}


	public void sendProgress() {

		Dictionary<string, object> progress = new Dictionary<string, object>();
		progress.Add("score", "499");

		List<object> tags = new List<object>();
		tags.Add("wizard");

		//progress.Add("score", "30");

		FuelSDK.SendProgress ( progress,tags);
	}

	//Callbacks
	public void OnFuelSDKImplicitLaunch (string message) {
		
		//TODO implement
		Debug.Log ("OnFuelSDKImplicitLaunch in FuelExample : " + message);
		
	}

	public void OnFuelSDKVirtualGoodList (Dictionary<string, object> virtualGoodInfo) {

		//TODO implement
		Debug.Log ("OnFuelSDKVirtualGoodList in FuelExample");

	}

	public void OnFuelSDKVirtualGoodRollback (string transactionId) {

		//TODO implement
		Debug.Log ("OnFuelSDKVirtualGoodRollback in FuelExample : " + transactionId);
	
	}


	public void OnFuelSDKChallengeCountUpdated (string count) {
		//TODO implement
		Debug.Log ("OnFuelSDKChallengeCountUpdated in FuelExample : " + count);
	}

	public void OnFuelSDKTournamentInfo (Dictionary<string, string> tournamentInfo) {
		//TODO implement
		Debug.Log ("OnFuelSDKTournamentInfo in FuelExample" );
	}


	public void InvokeLoginDetails ()
	{
		Debug.Log ("FuelExample - InvokeLoginDetails in FuelExample");
		Invoke ("SendLoginDetails", 0.5f);
	}

	public void InvokeMatchDetails ()
	{
		Debug.Log ("FuelExample - InvokeMatchDetails in FuelExample");
		Invoke ("SendMatchDetails", 0.5f);
	}

	private void SendMatchDetails ()
	{
		Debug.Log ("FuelExample - SendMatchDetails in FuelExample");

		Dictionary<string, object> matchResult = new Dictionary<string, object> ();
		matchResult.Add ("matchID", m_matchID);
		matchResult.Add ("tournamentID", m_tournamentID);
		matchResult.Add ("score", "55");
		
		FuelSDK.SubmitMatchResult (matchResult);
		FuelSDK.Launch ();
	}

	public void InvokeInviteCompleted ()
	{
		Debug.Log ("FuelExample - InvokeInviteCompleted in FuelExample");
		Invoke ("SendInviteCompleted", 0.5f);
	}

	private void SendInviteCompleted ()
	{
		Debug.Log ("FuelExample - SendInviteCompleted in FuelExample");
		FuelSDK.SdkSocialInviteCompleted ();
	}

	public void InvokeShareCompleted ()
	{
		Debug.Log ("FuelExample - InvokeShareCompleted in FuelExample");
		Invoke ("SendShareCompleted", 0.5f);
	}

	private void SendShareCompleted ()
	{
		Debug.Log ("FuelExample - SendShareCompleted in FuelExample");
		FuelSDK.SdkSocialShareCompleted ();
	}


	public void SdkSocialInvite (Dictionary<string, object> inviteDetail)
	{
		StringBuilder stringBuilder = new StringBuilder ();

		bool first = true;

		foreach (KeyValuePair<string,object> entry in inviteDetail) {
			if (first) {
				first = false;
			} else {
				stringBuilder.Append (", ");
			}

			stringBuilder.Append (entry.Key);
			stringBuilder.Append ("=");
			stringBuilder.Append (entry.Value.ToString());
		}

		Debug.Log ("SdkSocialInvite - " + stringBuilder.ToString ());

		// Calling InvokeInviteCompleted() to fake a social invite
		this.InvokeInviteCompleted ();
	}

	public void SdkSocialShare (System.Collections.Generic.Dictionary<string, object> shareDetail)
	{
		StringBuilder stringBuilder = new StringBuilder ();
		
		bool first = true;
		
		foreach (KeyValuePair<string,object> entry in shareDetail) {
			if (first) {
				first = false;
			} else {
				stringBuilder.Append (", ");
			}
			
			stringBuilder.Append (entry.Key);
			stringBuilder.Append ("=");
			stringBuilder.Append (entry.Value.ToString());
		}
		
		Debug.Log ("FuelExample - SdkSocialShare - " + stringBuilder.ToString ());
		
		// Calling InvokeShareCompleted() to fake a social share
		this.InvokeShareCompleted ();
	}

	public void SdkCompletedWithExit ()
	{
		Debug.Log ("FuelExample - SdkCompletedWithExit");
		
		// Clean exit
	}

	public void SdkCompletedWithMatch (System.Collections.Generic.Dictionary<string, string> matchInfo)
	{
		string tournamentID = matchInfo ["tournamentID"];
		string matchID = matchInfo ["matchID"];
		
		Debug.Log ("FuelExample - SdkCompletedWithMatch - " + tournamentID + " - " + matchID);
		Debug.Log ("Params - " + matchInfo ["paramsJSON"]);
		
		// Caching match information for later
		m_tournamentID = tournamentID;
		m_matchID = matchID;
		
		// Calling InvokeMatchDetails() to fake a match result
		this.InvokeMatchDetails ();
	}

	public void OnFuelSDKUserValues (Dictionary<string, string> userValues)
	{
		Debug.Log ("FuelExample - OnFuelSDKUserValues");


	}

	public void SdkSocialLogin (bool allowCache)
	{
		//base.SdkSocialLogin (allowCache);
		Debug.Log ("FuelExample - SdkSocialLogin - " + allowCache);
		
		// Calling InvokeLoginDetails() to fake a social login
		this.InvokeLoginDetails ();
	}

	public void SdkOnNotificationEnabled(FuelSDK.NotificationType type) 
	{
		Debug.Log ("FuelExample - SdkOnNotificationEnabled");
	}

	public void SdkOnNotificationDisabled(FuelSDK.NotificationType type) 
	{
		Debug.Log ("FuelExample - SdkOnNotificationEnabled");
	}

	public void SDKImplicitLaunch(string message){
	
		Debug.Log ("SDKImplicitLaunch -> " + message);
	}

	public void SdkFailed (string reason)
	{
		Debug.Log ("FuelExample - SdkFailed - " + reason);
		
		// Handle the failure as necessary
	}

	private void Awake ()
	{
		if (!m_bInitialized) {
			GameObject.DontDestroyOnLoad (gameObject);
			
			if (!Application.isEditor) {
				Initialize ();
			}
			
			m_bInitialized = true;
		} else {
			GameObject.Destroy (gameObject);
		}
	}

	private void Initialize() {
		m_listener = new FuelListenerExample (this);
		FuelSDK.setListener (m_listener);
	}
}
