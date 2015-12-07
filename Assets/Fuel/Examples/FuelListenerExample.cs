using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuelListenerExample : FuelSDKListener {

	private FuelExample m_fuelExample;

	public FuelListenerExample(FuelExample fuelExample) {
		m_fuelExample = fuelExample;
	}
	
	public override void OnVirtualGoodList (string transactionID, List<object> virtualGoods)
	{
		m_fuelExample.OnVirtualGoodList (transactionID, virtualGoods);
	}
	
	public override void OnVirtualGoodRollback (string transactionID)
	{
		m_fuelExample.OnVirtualGoodRollback (transactionID);
	}
	
	public override void OnNotificationEnabled (FuelSDK.NotificationType notificationType)
	{
		m_fuelExample.OnNotificationEnabled (notificationType);
	}

	public override void OnNotificationDisabled (FuelSDK.NotificationType notificationType)
	{
		m_fuelExample.OnNotificationDisabled (notificationType);
	}

	public override void OnSocialLogin (bool allowCache)
	{
		m_fuelExample.OnSocialLogin (allowCache);
	}
	
	public override void OnSocialInvite (Dictionary<string, string> data)
	{
		m_fuelExample.OnSocialInvite (data);
	}
	
	public override void OnSocialShare (Dictionary<string, string> data)
	{
		m_fuelExample.OnSocialShare (data);
	}
	
	public override void OnImplicitLaunch (FuelSDK.ApplicationState applicationState)
	{
		m_fuelExample.OnImplicitLaunch(applicationState);
	}

	public override void OnUserValues (Dictionary<string, string> conditions, Dictionary<string, string> variables)
	{
		m_fuelExample.OnUserValues (conditions, variables);
	}

	public override void OnCompeteTournamentInfo (Dictionary<string, string> tournamentInfo)
	{
		m_fuelExample.OnCompeteTournamentInfo (tournamentInfo);
	}

	public override void OnCompeteChallengeCount (int count)
	{
		m_fuelExample.OnCompeteChallengeCount (count);
	}

	public override void OnCompeteUICompletedWithExit ()
	{
		m_fuelExample.OnCompeteUICompletedWithExit ();
	}
	
	public override void OnCompeteUICompletedWithMatch (Dictionary<string, object> matchInfo)
	{
		m_fuelExample.OnCompeteUICompletedWithMatch (matchInfo);
	}

	public override void OnCompeteUIFailed (string reason)
	{
		m_fuelExample.OnCompeteUIFailed (reason);
	}

	public override void OnIgniteEvents (List<object> events)
	{
		m_fuelExample.OnIgniteEvents (events);
	}

	public override void OnIgniteLeaderBoard (Dictionary<string, object> leaderBoard)
	{
		m_fuelExample.OnIgniteLeaderBoard (leaderBoard);
	}

	public override void OnIgniteMission (Dictionary<string, object> mission)
	{
		m_fuelExample.OnIgniteMission (mission);
	}

	public override void OnIgniteQuest (Dictionary<string, object> quest)
	{
		m_fuelExample.OnIgniteQuest (quest);
	}

	public override void OnIgniteJoinEvent (string eventID, bool joinStatus)
	{
		m_fuelExample.OnIgniteJoinEvent (eventID, joinStatus);
	}

}
