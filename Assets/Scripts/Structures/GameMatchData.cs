using System;
using FuelSDKSimpleJSON;

namespace FUEL.SDK {

	public struct GameMatchData {
		public bool ValidMatchData { get; set; }
		public bool MatchComplete { get; set; }
		public int MatchRound { get; set; }
		public int MatchScore { get; set; }
		public string TournamentID { get; set; }
		public string MatchID { get; set; }
		public string YourNickname { get; set; }
		public string YourAvatarURL { get; set; }
		public string TheirNickname { get; set; }
		public string TheirAvatarURL { get; set; }

		public static GameMatchData ParseFromDictionary ( System.Collections.Generic.Dictionary<string,string> gameMatchDict ) {
			GameMatchData gameMatchData = new GameMatchData();

			gameMatchData.ValidMatchData = true;

			if( gameMatchDict.ContainsKey( "tournamentID" ) ) {
				gameMatchData.TournamentID = gameMatchDict ["tournamentID"];
			}
			if( gameMatchDict.ContainsKey( "matchID" ) ) {
				gameMatchData.MatchID = gameMatchDict ["matchID"];
			}

			if( gameMatchDict.ContainsKey( "paramsJSON" ) ) {
				// extract the params data
				string paramsJSON = gameMatchDict ["paramsJSON"];
				JSONNode json = JSONNode.Parse (paramsJSON);
				
				gameMatchData.MatchRound = json ["round"].AsInt;
				
				JSONClass you = json ["you"].AsObject;
				gameMatchData.YourNickname = you ["name"];
				gameMatchData.YourAvatarURL = you ["avatar"];
				
				JSONClass them = json ["them"].AsObject;
				gameMatchData.TheirNickname = them ["name"];
				gameMatchData.TheirAvatarURL = them ["avatar"];
			}
			
			gameMatchData.MatchComplete = false;

			return gameMatchData;
		}
	}

}