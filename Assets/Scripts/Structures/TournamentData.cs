using System;
using FUEL.Utils;

namespace FUEL.SDK {

	public struct TournamentData {
		public bool Enable { get; set; }
		public string Name { get; set; }
		public string CampaignName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Logo { get; set; }

		public static TournamentData ParseFromDictionary ( System.Collections.Generic.Dictionary<string, string> tournamentDict ) {
			TournamentData tournamentData = new TournamentData();
			tournamentData.Enable = true;
			if( tournamentDict.ContainsKey("name") ) {
				tournamentData.Name = tournamentDict["name"];
			}
			if( tournamentDict.ContainsKey("campaignName") ) {
				tournamentData.CampaignName = tournamentDict["campaignName"];
			}
			if( tournamentDict.ContainsKey("startDate") ) {
				long t = Convert.ToInt64 (tournamentDict["startDate"]);
				tournamentData.StartDate = TimeUtility.FromUnixTime (t);
			}
			if( tournamentDict.ContainsKey("endDate") ) {
				long t = Convert.ToInt64 (tournamentDict["endDate"]);
				tournamentData.EndDate = TimeUtility.FromUnixTime (t);
			}
			if( tournamentDict.ContainsKey("logo") ) {
				tournamentData.Logo = tournamentDict["logo"];
			}
			return tournamentData;
		}
	}

}