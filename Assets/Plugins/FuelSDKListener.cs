using UnityEngine;
using System.Collections.Generic;

public abstract class FuelSDKListener
{
	public enum DataReceiverAction
	{
		none,
		fuelSDKVirtualGoodList,
		fuelSDKVirtualGoodRollback,
		fuelSDKNotificationEnabled,
		fuelSDKNotificationDisabled,
		fuelSDKSocialLoginRequest,
		fuelSDKSocialInviteRequest,
		fuelSDKSocialShareRequest,
		fuelSDKImplicitLaunchRequest,
		fuelSDKUserValues,
		fuelSDKCompeteChallengeCount,
		fuelSDKCompeteTournamentInfo,
		fuelSDKCompeteUICompletedExit,
		fuelSDKCompeteUICompletedMatch,
		fuelSDKCompeteUICompletedFail,
		fuelSDKIgniteEvents,
		fuelSDKIgniteLeaderBoard,
		fuelSDKIgniteMission,
		fuelSDKIgniteQuest
	}

	public abstract void OnNotificationEnabled(Dictionary<string, object> data);

	public abstract void OnNotificationDisabled (Dictionary<string, object> data);

	public abstract void OnImplicitLaunch (Dictionary<string, object> data);

	public abstract void OnUserValues (Dictionary<string, object> data);

	public abstract void OnTournamentInfo (Dictionary<string, object> data);

	public abstract void OnChallengeCountChanged (Dictionary<string, object> data);

	public abstract void OnSdkCompletedWithMatch (Dictionary<string, object> data);

	public abstract void OnSdkFailed (Dictionary<string, object> data);

	public abstract void OnSdkCompletedWithExit ();

	public abstract void OnSdkSocialShare (Dictionary<string, object> data);

	public abstract void OnSdkSocialInvite (Dictionary<string, object> data);

	public abstract void OnSdkSocialLogin (Dictionary<string, object> data);

	public abstract void OnVirtualGoodRollback (Dictionary<string, object> data);

	public abstract void OnVirtualGoodList (Dictionary<string, object> data);

	public abstract void OnIgniteEventsReceive (List<object> listEvents);

	public abstract void OnIgniteLeaderBoardReceive (Dictionary<string,object> leaderBoard);

	public abstract void OnIgniteMissionReceive (Dictionary<string,object> mission);

	public abstract void OnIgniteQuestReceive (Dictionary<string,object> quest);

	public abstract void OnDataReciever( Dictionary<string, object> message );

}
