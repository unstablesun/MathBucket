using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FUEL.Utils;

namespace FUEL.SDK {

	public enum IgniteEventType
	{
		none         = -1,
		leaderBoard 	= 0,
		mission      = 1,
		quest        = 2
	}

	public struct EventData {
		public string Id { get; set; }
		public DateTime StartTime { get; set; }
		public bool Authorized { get; set; }
		public string EventId { get; set; }
		public string State { get; set; }
		public int Score { get; set; }
		public int OldScore { get; set; }
		public IgniteEventType Type { get; set; }
		public DateTime EndTime { get; set; }
		public EventMetadata Metadata  { get; set; }

		public bool Active {
			get{
				if( State != "active" ) {
					return false;
				}
				if( TimeUtility.TimeIsInThePast(StartTime) && TimeUtility.TimeIsInTheFuture(EndTime) ) {
					return true;
				}
				return false;
			}
		}
		
		public bool CommingSoon {
			get{
				if(TimeUtility.TimeIsInTheFuture(StartTime)) {
					return true;
				}
				return false;
			}
		}
		
		public void Copy( EventData eventDataCopy ) {
			if( this.Id != eventDataCopy.Id ) {
				return;
			}
			this.StartTime = eventDataCopy.StartTime;
			this.Authorized = eventDataCopy.Authorized;
			this.EventId = eventDataCopy.EventId;
			this.State = eventDataCopy.State;
			this.EndTime = eventDataCopy.EndTime;
			this.OldScore = ( this.Score != eventDataCopy.Score )?this.Score:-1;
			this.Score = eventDataCopy.Score;
			this.Type = eventDataCopy.Type;
			this.EndTime = eventDataCopy.EndTime;
			this.Metadata = eventDataCopy.Metadata;
		}

		public static EventData ParseFromDictionary ( System.Collections.Generic.Dictionary<string,object> eventDict ) {
			EventData eventData = new EventData();
			eventData.OldScore = -1;
			if( eventDict.ContainsKey( "id" ) ) {
				eventData.Id = Convert.ToString( eventDict["id"] );//Activity ID
			}
			if( eventDict.ContainsKey( "startTime" ) ) {
				long t = Convert.ToInt64 (eventDict["startTime"]);
				eventData.StartTime = TimeUtility.FromUnixTime (t);
			}
			if( eventDict.ContainsKey( "authorized" ) ) {
				eventData.Authorized = Convert.ToBoolean( eventDict["authorized"] );
			}
			if( eventDict.ContainsKey( "eventId" ) ) {
				eventData.EventId = Convert.ToString( eventDict["eventId"] );
			}
			if( eventDict.ContainsKey( "state" ) ) {
				eventData.State = Convert.ToString( eventDict["state"] );
			}
			if( eventDict.ContainsKey( "score" ) ) {
				eventData.Score = Convert.ToInt32( eventDict["score"] );
			}
			if( eventDict.ContainsKey( "type" ) ) {
				eventData.Type = (IgniteEventType) Enum.Parse( typeof(IgniteEventType) , Convert.ToString( eventDict["type"] ) );
			}
			if( eventDict.ContainsKey( "endTime" ) ) {
				long t = Convert.ToInt64 (eventDict["endTime"]);
				eventData.EndTime = TimeUtility.FromUnixTime (t);
			}
			if( eventDict.ContainsKey( "metadata" ) ) {
				System.Collections.Generic.Dictionary<string,object> eventMetadataDict = eventDict["metadata"] as System.Collections.Generic.Dictionary<string,object>;
				EventMetadata eventMetadata = new EventMetadata();
				if( eventMetadataDict.ContainsKey( "iconUrl" ) ) {
					eventMetadata.IconUrl = Convert.ToString( eventMetadataDict["iconUrl"] );
				}
				if( eventMetadataDict.ContainsKey( "name" ) ) {
					eventMetadata.Name = Convert.ToString( eventMetadataDict["name"] );
				}
				if( eventMetadataDict.ContainsKey( "gamedata" ) ) {
					eventMetadata.GameData = Convert.ToString( eventMetadataDict["gamedata"] );
				}
				eventData.Metadata = eventMetadata;
			}

			Debug.Log (	
			           "Ignite Event Data" + "\n" +
			           "Id = " + eventData.Id + "\n" +
			           "StartTime = " + eventData.StartTime + "\n" +
			           "EndTime = " + eventData.EndTime + "\n" +
			           "Authorized = " + eventData.Authorized + "\n" +
			           "EventId = " + eventData.EventId + "\n" +
			           "State = " + eventData.State + "\n" +
			           "Score = " + eventData.Score + "\n" +
			           "Type = " + eventData.Type + "\n" +
			           "Metadata = " + eventData.Metadata + "\n" 
			           );

			return eventData;
		}

	}
	
	public struct EventMetadata {
		public string Name { get; set; }
		public string IconUrl { get; set; }
		public string GameData { get; set; }
	}

}
