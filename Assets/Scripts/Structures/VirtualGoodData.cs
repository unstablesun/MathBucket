using System;
using FUEL.UserProfile;

namespace FUEL.SDK {

	public struct VirtualGoodData {
		public enum RewardType {
			None = 0,
			Coins = 1
		}
		
		public string Id { get; set; }
		public RewardType Type { get; set; }
		public string GoodId { get; set; }
		public string Description { get; set; }
		public string IconUrl { get; set; }
		public int Value { get; set; }

		public void Init () {
			Type = RewardType.None;
			if( GoodId == "1" ) {
				Type = RewardType.Coins;
				Value = 5;
			}
			if( GoodId == "2" ) {
				Type = RewardType.Coins;
				Value = 15;
			}
		}

		public void GetReward() {
			switch( Type ) {
			case RewardType.Coins:
				UserProfileManager.Instance.currentUser.player.coins += Value;
				break;
			default:
				break;
			}
		}

		public static VirtualGoodData ParseFromDictionary ( System.Collections.Generic.Dictionary<string,object> virtualGoodDict ) {
			VirtualGoodData virtualGoodsData = new VirtualGoodData();
			if( virtualGoodDict.ContainsKey("id") ){
				virtualGoodsData.Id = Convert.ToString( virtualGoodDict["id"] );
			}
			if( virtualGoodDict.ContainsKey("goodId") ){
				virtualGoodsData.GoodId = Convert.ToString( virtualGoodDict["goodId"] );
			}
			if( virtualGoodDict.ContainsKey("description") ){
				virtualGoodsData.Description = Convert.ToString( virtualGoodDict["description"] );
			}
			virtualGoodsData.Init();
			return virtualGoodsData;
		}
	}

}