using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PropellerSDKSimpleJSON;



public class FuelListener : PropellerSDKListener
{

    private string m_tournamentID;
    private string m_matchID;

    public FuelListener ()
    {
        m_tournamentID = "";
        m_matchID = "";
    }





    public override void SdkCompletedWithMatch (Dictionary<string, string> matchResult)
    {
		/*
        // Sdk completed with a match.

        // Extract the match information and cache if for
        // later when posting the match score.
        m_tournamentID = matchResult ["tournamentID"];
		m_matchID = matchResult ["matchID"];

        // Extract the params data.
        string paramsJSON = matchResult ["paramsJSON"];
        JSONNode json = JSONNode.Parse (paramsJSON);

        // Extract the match seed value to be used for any
        // randomization seeding. The seed value will be
        // the same for each match player.
        long seed = 0;

        // Must parse long values manually since SimpleJSON
        // doesn't yet provide this function automatically.
        if (!long.TryParse(json ["seed"], out seed))
        {
            // invalid string encoded long value, defaults to 0
        }

        // Extract the match round value.
        int round = json ["round"].AsInt;

        // Extract the ads allowed flag to be used to
        // determine if in-game ads should be allowed in
        // this match.
        bool adsAllowed = json ["adsAllowed"].AsBool;

        // Extract the fair play flag to be used to
        // determine if a level playing field between the
        // match players should be enforced.
        bool fairPlay = json ["fairPlay"].AsBool;

        // Extract the options data.
        JSONClass options = json ["options"].AsObject;

        // Extract the player's public profile data.
        JSONClass you = json ["you"].AsObject;
        string yourNickname = you ["name"];
        string yourAvatarURL = you ["avatar"];

        // Extract the opponent's public profile data.
        JSONClass them = json ["them"].AsObject;
        string theirNickname = them ["name"];
        string theirAvatarURL = them ["avatar"];

        // Play the game and pass any extracted match
        // data as necessary.
        //startMultiplayerGame();

		*/
		getFuelHandlerClass ().LaunchMultiplayerGame ( matchResult );
    }


    public override void SdkCompletedWithExit ()
    {
        // Sdk completed gracefully with no further action.
        // The game should handle this event by returning the
        // player to the game’s main scene.
    }


    public override void SdkFailed (string reason)
    {
        // Sdk has failed with an unrecoverable error.
        // Refer to the supplied message for details.
        // The game should handle this event gracefully by
        // by returning the player to the game’s main scene
        // and possibly report the error.
    }




	private FuelHandler getFuelHandlerClass()
	{
		GameObject _dynamicsHandler = GameObject.Find("FuelHandler");
		if (_dynamicsHandler != null) {
			FuelHandler _fuelHandlerScript = _dynamicsHandler.GetComponent<FuelHandler> ();
			if(_fuelHandlerScript != null) {
				return _fuelHandlerScript;
			}
			throw new Exception();
		}
		throw new Exception();
	}

	



}