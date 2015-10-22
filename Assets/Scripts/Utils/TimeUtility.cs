using UnityEngine;
using System.Collections;
using System;

namespace FUEL.Utils
{
	public static class TimeUtility {

		public static DateTime FromUnixTime(long unixTime)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return epoch.AddSeconds(unixTime);
		}

		public static bool TimeIsInThePast( DateTime time ) {
			if (time.CompareTo (DateTime.UtcNow) <= 0) {
				return true;
			}
			return false;
		}

		public static bool TimeIsInTheFuture( DateTime time ) {
			if (time.CompareTo (DateTime.UtcNow) >= 0) {
				return true;
			}
			return false;
		}

		public static string RemainingTimeString ( DateTime time ) {
			int days=0, hours=0, minutes=0, seconds=0;
			if (TimeIsInTheFuture (time)) {
				TimeSpan remainingTime = time.Subtract( DateTime.UtcNow );
				days = remainingTime.Days;
				hours = remainingTime.Hours;
				minutes = remainingTime.Minutes;
				seconds = remainingTime.Seconds;
			}
			if( days > 0 ) {
				return ((days < 10) ? "0" + days.ToString () : days.ToString ())+ "d "+
					((hours < 10) ? "0" + hours.ToString () : hours.ToString ()) + "h";
			}
			else if( hours > 0 ) {
				return ((hours < 10) ? "0" + hours.ToString () : hours.ToString ())+ "h "+
					((minutes < 10) ? "0" + minutes.ToString () : minutes.ToString ()) + "m";
			}
			else if( minutes > 0 ) {
				return ((minutes < 10) ? "0" + minutes.ToString () : minutes.ToString ())+ "m "+
					((seconds < 10) ? "0" + seconds.ToString () : seconds.ToString ()) + "s";
			}
			return ((seconds < 10) ? "0" + seconds.ToString () : seconds.ToString ()) + "s";
		}

		public static string ElapsedTimeString ( DateTime time ) {
			int days=0, hours=0, minutes=0, seconds=0;
			if (TimeIsInTheFuture (time)) {
				TimeSpan elapsedTime = DateTime.UtcNow.Subtract( time );
				days = elapsedTime.Days;
				hours = (days > 0)?days*24+elapsedTime.Hours:elapsedTime.Hours;
				minutes = elapsedTime.Minutes;
				seconds = elapsedTime.Seconds;
			}
			if( days > 0 ) {
				return ((days < 10) ? "0" + days.ToString () : days.ToString ())+ "d "+
					((hours < 10) ? "0" + hours.ToString () : hours.ToString ()) + "h";
			}
			else if( hours > 0 ) {
				return ((hours < 10) ? "0" + hours.ToString () : hours.ToString ())+ "h "+
					((minutes < 10) ? "0" + minutes.ToString () : minutes.ToString ()) + "m";
			}
			else if( minutes > 0 ) {
				return ((minutes < 10) ? "0" + minutes.ToString () : minutes.ToString ())+ "m "+
					((seconds < 10) ? "0" + seconds.ToString () : seconds.ToString ()) + "s";
			}
			return ((seconds < 10) ? "0" + seconds.ToString () : seconds.ToString ()) + "s";
		}
	}
}