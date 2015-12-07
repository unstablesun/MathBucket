//
//  fueltypes.h
//  fuelsdk
//
//  Created by Alan Price on 2015-07-31.
//  Copyright (c) 2015 Fuel. All rights reserved.
//

#ifndef fuelsdk_fueltypes_h
#define fuelsdk_fueltypes_h

#define FSDK_MATCH_RESULT_TOURNAMENT_KEY @"tournamentID"
#define FSDK_MATCH_RESULT_MATCH_KEY @"matchID"
#define FSDK_MATCH_RESULT_PARAMS_KEY @"params"

#define FSDK_MATCH_POST_TOURNAMENT_KEY @"tournamentID"
#define FSDK_MATCH_POST_MATCH_KEY @"matchID"
#define FSDK_MATCH_POST_SCORE_KEY @"score"
#define FSDK_MATCH_POST_VISUALSCORE_KEY @"visualScore"
#define FSDK_MATCH_POST_MATCHDATA_KEY @"matchData"
#define FSDK_MATCH_POST_CURRENCIES_KEY @"currencies"
#define FSDK_MATCH_POST_GAMEDATA_KEY @"gameData"
#define FSDK_MATCH_POST_CURRENCYID_KEY @"id"

// Fuel Notifications

#define FSDK_BROADCAST_VG_LIST @"fuelSDKVirtualGoodList"
#define FSDK_BROADCAST_VG_ROLLBACK @"fuelSDKVirtualGoodRollback"
#define FSDK_BROADCAST_NOTIFICATION_ENABLED @"fuelSDKNotificationEnabled"
#define FSDK_BROADCAST_NOTIFICATION_DISABLED @"fuelSDKNotificationDisabled"

#define FSDK_BROADCAST_SOCIAL_LOGIN_REQUEST @"fuelSDKSocialLoginRequest"
#define FSDK_BROADCAST_SOCIAL_INVITE_REQUEST @"fuelSDKSocialInviteRequest"
#define FSDK_BROADCAST_SOCIAL_SHARE_REQUEST @"fuelSDKSocialShareRequest"

#define FSDK_BROADCAST_IMPLICIT_LAUNCH_REQUEST @"fuelSDKImplicitLaunchRequest"

#define FSDK_BROADCAST_USER_CHANGED @"fuelSDKUserChanged"


// Dynnamics Notifications

#define FSDK_BROADCAST_USER_VALUES @"fuelSDKUserValues"

// Compete Notifications

#define FSDK_BROADCAST_COMPETE_CHALLENGE_COUNT @"fuelSDKCompeteChallengeCount"
#define FSDK_BROADCAST_COMPETE_TOURNAMENT_INFO @"fuelSDKCompeteTournamentInfo"

// CompeteUI Notifications

#define FSDK_BROADCAST_COMPETE_EXIT @"fuelSDKCompeteUICompletedExit"
#define FSDK_BROADCAST_COMPETE_MATCH @"fuelSDKCompeteUICompletedMatch"
#define FSDK_BROADCAST_COMPETE_FAIL @"fuelSDKCompeteUICompletedFail"

// Ignite Notifications

#define FSDK_BROADCAST_IGNITE_EVENTS @"fuelSDKIgniteEvents"
#define FSDK_BROADCAST_IGNITE_LEADERBOARD @"fuelSDKIgniteLeaderBoard"
#define FSDK_BROADCAST_IGNITE_MISSION @"fuelSDKIgniteMission"
#define FSDK_BROADCAST_IGNITE_QUEST @"fuelSDKIgniteQuest"
#define FSDK_BROADCAST_IGNITE_JOIN_EVENT @"fuelSDKIgniteJoinEvent"

typedef enum
{
    fuelnotificationtypenone = 0x0,
    fuelnotificationtypeall = 0x3,
    fuelnotificationtypepush = 1 << 0,
    fuelnotificationtypelocal = 1 << 1
} fuelnotificationtype;

typedef enum
{
    fuelgameorientationportrait,
    fuelgameorientationlandscape
} fuelgameorientation;

#endif
