using System;
using System.Linq;

namespace FUEL.SDK {

	public struct LeaderBoardData  {
		public string Id { get; set; }
		public int Progress { get; set; }
		public System.Collections.Generic.List<LeaderBoardRowData> LeaderBoardList { get; set; }
		public LeaderBoardMetadata Metadata { get; set; }
		
		public void Copy( LeaderBoardData leaderBoardDataCopy ) {
			if( this.Id != leaderBoardDataCopy.Id ) {
				return;
			}
			this.LeaderBoardList = leaderBoardDataCopy.LeaderBoardList;
			this.Metadata = leaderBoardDataCopy.Metadata;
		}

		public static LeaderBoardData ParseFromDictionary ( System.Collections.Generic.Dictionary<string,object> leaderBoardDict ) {
			LeaderBoardData leaderBoardData = new LeaderBoardData();
			if( leaderBoardDict.ContainsKey( "id" ) ) {
				leaderBoardData.Id = Convert.ToString( leaderBoardDict["id"] );
			}
			
			LeaderBoardMetadata leaderBoardMetadata = new LeaderBoardMetadata();
			leaderBoardMetadata.Name = "Collect the most treasure to win!";
			
			leaderBoardData.Metadata = leaderBoardMetadata;
			
			LeaderBoardRowData userData = new LeaderBoardRowData();
			if( leaderBoardDict.ContainsKey( "userPos" ) ) {
				System.Collections.Generic.Dictionary<string,object> userPosDict = leaderBoardDict["userPos"] as System.Collections.Generic.Dictionary<string,object>;
				if( userPosDict.ContainsKey("user_id") ){
					userData.UserID = Convert.ToString( userPosDict["user_id"] );
				}
				if( userPosDict.ContainsKey("nickname") ){
					userData.Nickname = Convert.ToString( userPosDict["nickname"] );
				}
				if( userPosDict.ContainsKey("avatar") ){
					userData.Avatar = Convert.ToString( userPosDict["avatar"] );
				}
				if( userPosDict.ContainsKey("score") ){
					userData.Score = Convert.ToInt32( userPosDict["score"] );
				}
				if( userPosDict.ContainsKey("rank") ){
					userData.Rank = Convert.ToInt32( userPosDict["rank"] );
				}
			}
			
			leaderBoardData.LeaderBoardList = new System.Collections.Generic.List<LeaderBoardRowData>();
			bool listContainTheUser = false;
			if( leaderBoardDict.ContainsKey( "leaderboard" ) ) {
				LeaderBoardRowData leaderBoardRowData = new LeaderBoardRowData();
				System.Collections.Generic.List<object> leaderBoardList = leaderBoardDict["leaderboard"] as System.Collections.Generic.List<object>;
				foreach( object leaderBoardObject in leaderBoardList ) {
					System.Collections.Generic.Dictionary<string,object> leaderBoardRowsDict = leaderBoardObject as System.Collections.Generic.Dictionary<string,object>;
					if( leaderBoardRowsDict.ContainsKey("user_id") ){
						leaderBoardRowData.UserID = Convert.ToString( leaderBoardRowsDict["user_id"] );
						if( !string.IsNullOrEmpty(userData.UserID) && userData.UserID == leaderBoardRowData.UserID ) {
							leaderBoardRowData.IsYou = true;
							listContainTheUser = true;
						}
					}
					if( leaderBoardRowsDict.ContainsKey("nickname") ){
						leaderBoardRowData.Nickname = Convert.ToString( leaderBoardRowsDict["nickname"] );
					}
					if( leaderBoardRowsDict.ContainsKey("avatar") ){
						leaderBoardRowData.Avatar = Convert.ToString( leaderBoardRowsDict["avatar"] );
					}
					if( leaderBoardRowsDict.ContainsKey("score") ){
						leaderBoardRowData.Score = Convert.ToInt32( leaderBoardRowsDict["score"] );
					}
					if( leaderBoardRowsDict.ContainsKey("rank") ){
						leaderBoardRowData.Rank = Convert.ToInt32( leaderBoardRowsDict["rank"] );
					}
					leaderBoardData.LeaderBoardList.Add( leaderBoardRowData );
				}
			}
			if( !listContainTheUser ) {
				leaderBoardData.LeaderBoardList.Add( userData );
			}
			leaderBoardData.LeaderBoardList = leaderBoardData.LeaderBoardList.OrderByDescending( data => data.Rank ).ToList();
			return leaderBoardData;
		}
	}
	
	public struct LeaderBoardRowData {
		public string UserID { get; set; }
		public bool IsYou { get; set; }
		public string Nickname { get; set; }
		public string Avatar { get; set; }
		public int Score { get; set; }
		public int Rank { get; set; }
	}
	
	public struct LeaderBoardMetadata {
		public string Name { get; set; }
	}

}