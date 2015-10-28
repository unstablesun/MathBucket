using UnityEngine;
using System;

namespace FUEL.SDK {

	public struct MissionData {
		public string Id { get; set; }
		public double Progress { get; set; }
		public double OldProgress { get; set; }
		public System.Collections.Generic.Dictionary<string,RuleData> Rules { get; set; }
		public MissionMetadata Metadata { get; set; }
		
		public MissionData Copy( double OldProgress ) {
			MissionData returnValue = new MissionData();
			returnValue.Id = Id;
		
			returnValue.OldProgress = ( Progress != OldProgress )?OldProgress:-1;
			returnValue.Progress = Progress;

			returnValue.Rules = Rules;

			returnValue.Metadata = Metadata;
			return returnValue;
		}

		public static MissionData ParseFromDictionary ( System.Collections.Generic.Dictionary<string,object> missionDict ) {
			MissionData missionData = new MissionData();
			missionData.OldProgress = -1;
			if( missionDict.ContainsKey("id") ) {
				missionData.Id = Convert.ToString( missionDict["id"] );
			}
			if( missionDict.ContainsKey("progress") ){
				missionData.Progress = Math.Round( Convert.ToDouble(missionDict["progress"]), 2);
			}
			if( missionDict.ContainsKey( "metadata" ) ) {
				System.Collections.Generic.Dictionary<string,object> missionMetadataDict = missionDict["metadata"] as System.Collections.Generic.Dictionary<string,object>;
				missionData.Metadata = MissionMetadata.ParseFromDictionary(missionMetadataDict);
				
			}
			if( missionDict.ContainsKey("rules") ) {
				missionData.Rules = new System.Collections.Generic.Dictionary<string, RuleData>();
				System.Collections.Generic.List<object> rulesList = missionDict["rules"] as System.Collections.Generic.List<object>;
				foreach(object rule in rulesList ) {
					System.Collections.Generic.Dictionary<string,object> ruleDict = rule as System.Collections.Generic.Dictionary<string, object>;
					RuleData ruleData = RuleData.ParseFromDictionary( ruleDict );
					missionData.Rules.Add( ruleData.Id, ruleData);
				}
			}

			Debug.Log (	
			           "Ignite Mission Data" + "\n" +
			           "Id = " + missionData.Id + "\n" +
			           "Progress = " + missionData.Progress + "\n" +
			           "Metadata = " + missionData.Metadata + "\n" +
			           "Rules = " + missionData.Rules + "\n"
			           );

			return missionData;
		}
	}
	
	public struct MissionMetadata {
		public string Name { get; set; }
		public string GameData { get; set; }
		public VirtualGoodData VirtualGood { get; set; }

		public static MissionMetadata ParseFromDictionary ( System.Collections.Generic.Dictionary<string,object> missionMetadataDict ) {
			MissionMetadata missionMetadata = new MissionMetadata();
			if( missionMetadataDict.ContainsKey( "name" ) ) {
				missionMetadata.Name = Convert.ToString( missionMetadataDict["name"] );
			}
			if( missionMetadataDict.ContainsKey( "virtualGood" ) ) {
				VirtualGoodData virtualGood = new VirtualGoodData();
				System.Collections.Generic.Dictionary<string,object> virtualGoodDict = missionMetadataDict["virtualGood"] as System.Collections.Generic.Dictionary<string,object>;
				if( virtualGoodDict.ContainsKey( "iconUrl" ) ) {
					virtualGood.IconUrl = Convert.ToString( virtualGoodDict["iconUrl"]);
				}
				if( virtualGoodDict.ContainsKey( "description" ) ) {
					virtualGood.Description = Convert.ToString( virtualGoodDict["description"] );
				}
				if( virtualGoodDict.ContainsKey( "id" ) ) {
					virtualGood.Id = Convert.ToString( virtualGoodDict["id"] );
				}
				if( virtualGoodDict.ContainsKey( "goodId" ) ) {
					virtualGood.GoodId = Convert.ToString( virtualGoodDict["goodId"] );
				}
				virtualGood.Init();
				missionMetadata.VirtualGood = virtualGood;

				Debug.Log (	
				           "Ignite Mission Virtual Good" + "\n" +
				           "Id = " + virtualGood.Id + "\n" +
				           "Description = " + virtualGood.Description + "\n" +
				           "GoodId = " + virtualGood.GoodId + "\n" +
				           "IconUrl = " + virtualGood.IconUrl + "\n" 
				           );

			}
			if( missionMetadataDict.ContainsKey( "gamedata" ) ) {
				missionMetadata.GameData = Convert.ToString( missionMetadataDict["gamedata"] );
			}

			Debug.Log (	
			           "Ignite Mission MetaData" + "\n" +
			           "Name = " + missionMetadata.Name + "\n" +
			           "GameData = " + missionMetadata.GameData + "\n"
			           );

			return missionMetadata;
		}


	}

	public enum RuleType {
		incremental = 0,
		spot       	= 1,
	}

	public struct RuleData {
		public string Id { get; set; }
		public int Score { get; set; }
		public float OldProgress { get; set; }
		public int Target { get; set; }
		public bool Achieved { get; set; }
		public string Variable { get; set; }
		public RuleType Kind { get; set; }
		public RuleMetadata Metadata { get; set; }
		
		public RuleData Copy( float oldScore ) {
			RuleData returnValue = new RuleData();
			returnValue.Id = Id;
			returnValue.OldProgress = ( Progress != oldScore )?oldScore:-1;
			returnValue.Score = Score;
			returnValue.Target = Target;
			returnValue.Achieved = Achieved;
			returnValue.Variable = Variable;
			returnValue.Kind = Kind;
			returnValue.Metadata = Metadata;
			return returnValue;
		}

		public float Progress {
			get {
				double progressValue = (Target > 0)?Math.Round( (double)Score/(double)Target, 2):0;
				if( progressValue > 1 ) {
					progressValue = 1;
				}
				return (float)progressValue;
			}
		}

		public static RuleData ParseFromDictionary ( System.Collections.Generic.Dictionary<string,object> ruleDict ) {
			RuleData ruleData = new RuleData();
			ruleData.OldProgress = -1;
			if( ruleDict.ContainsKey("id") ) {
				ruleData.Id = Convert.ToString( ruleDict["id"] );
			}
			if( ruleDict.ContainsKey("score") ) {
				ruleData.Score = Convert.ToInt32( ruleDict["score"] );
			}
			if( ruleDict.ContainsKey("target") ) {
				ruleData.Target = Convert.ToInt32( ruleDict["target"] );
			}
			if( ruleDict.ContainsKey("achieved") ) {
				ruleData.Achieved = Convert.ToBoolean( ruleDict["achieved"] );
			}
			if( ruleDict.ContainsKey("variable") ) {
				ruleData.Variable = Convert.ToString( ruleDict["variable"] );
			}
			if( ruleDict.ContainsKey("kind") ) {
				ruleData.Kind = (RuleType) Enum.Parse( typeof(RuleType) , Convert.ToString( ruleDict["kind"] ) );
			}
			if( ruleDict.ContainsKey( "metadata" ) ) {
				string metadataString = Convert.ToString( ruleDict["metadata"] );
				metadataString = (string.IsNullOrEmpty(metadataString))?"{}":metadataString;
				System.Collections.Generic.Dictionary<string,object> metadataDict = FuelSDKCommon.Deserialize( metadataString ) as System.Collections.Generic.Dictionary<string,object>;
				ruleData.Metadata = RuleMetadata.ParseFromDictionary( metadataDict );
			}

			Debug.Log (	
			           "Ignite Mission Rule Data" + "\n" +
			           "Id = " + ruleData.Id + "\n" +
			           "Score = " + ruleData.Score + "\n" +
			           "Target = " + ruleData.Target + "\n" +
			           "Variable = " + ruleData.Variable + "\n" +
			           "Kind = " + ruleData.Kind + "\n" +
			           "Achieved = " + ruleData.Achieved + "\n" 
			           );

			return ruleData;
		}
	}
	
	public struct RuleMetadata {
		public string name { get; set; }

		public static RuleMetadata ParseFromDictionary ( System.Collections.Generic.Dictionary<string,object> metadataDict ) {
			RuleMetadata ruleDataMetadata = new RuleMetadata();
			if( metadataDict != null ) {
				if( metadataDict.ContainsKey( "name" ) ) {
					ruleDataMetadata.name = Convert.ToString( metadataDict["name"] );
				}
			}
			return ruleDataMetadata;
		}
	}

}