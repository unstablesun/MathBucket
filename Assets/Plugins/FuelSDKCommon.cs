using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FuelSDKSimpleJSON;
using FuelSDKMiniJSON;

public class FuelSDKCommon : MonoBehaviour
{
	private enum DataType
	{
		intType,
		longType,
		floatType,
		doubleType,
		boolType,
		stringType
	}
	;

	private static bool m_bInitialized;

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	public static extern bool iOSSetUserConditions(string conditions);
	[DllImport ("__Internal")]
	public static extern bool iOSSyncUserValues();
	#endif
	
	static FuelSDKCommon ()
	{
		m_bInitialized = false;
	}


	private void Awake ()
	{
		if (!m_bInitialized) {
			GameObject.DontDestroyOnLoad (gameObject);
			m_bInitialized = true;
		} else {
			GameObject.Destroy (gameObject);	
		}
	}



	#region private utility methods

	public enum LogLevel
	{
		DEBUG,
		INFO,
		WARN,
		ERROR
	};

	public static void Log (LogLevel level, string message)
	{
		if (message == null)
		{
			return;
		}

		string prefix = "Fuel SDK [" + level.ToString () + "]";

//		StackTrace stacktrace = new StackTrace();
//		MethodBase methodBase = stacktrace.GetFrame(1).GetMethod();
//		string caller = methodBase.Name;
//
//		// Fuel SDK [INFO]: Log () - some message to print
//		string log = prefix + ": " + caller + " () - " + message;

		string log = prefix + ": " + message;

		switch (level)
		{
		case LogLevel.DEBUG:
		{
#if FUEL_SDK_DEBUG
			UnityEngine.Debug.Log (log);
#endif
			break;
		}
		case LogLevel.INFO:
		{
			UnityEngine.Debug.Log (log);
			break;
		}
		case LogLevel.WARN:
		{
			UnityEngine.Debug.LogWarning (log);
			break;
		}
		case LogLevel.ERROR:
		{
			UnityEngine.Debug.LogError (log);
			break;
		}
		}
	}
	public static JSONClass toJSONClass (Dictionary<string, object> dictionary)
	{
		if (dictionary == null) {
			return null;
		}
		
		JSONClass jsonClass = new JSONClass ();
		
		foreach (KeyValuePair<string, object> item in dictionary) {
			JSONNode jsonNode;
			
			if (item.Value is List<object>) {
				jsonNode = toJSONArray ((List<object>)item.Value);
			} else if (item.Value is Dictionary<string, object>) {
				jsonNode = toJSONClass ((Dictionary<string, object>)item.Value);
			} else {
				jsonNode = toJSONValue (item.Value);
			}
			
			if (jsonNode == null) {
				continue;
			}
			
			jsonClass.Add (item.Key, jsonNode);
		}
		
		return jsonClass;
	}
	
	public static JSONArray toJSONArray (List<object> list)
	{
		if (list == null) {
			return null;
		}
		
		JSONArray jsonArray = new JSONArray ();
		
		foreach (object item in list) {
			if (item == null) {
				continue;
			}
			
			JSONNode jsonNode;
			
			if (item is List<object>) {
				jsonNode = toJSONArray ((List<object>)item);
			} else if (item is Dictionary<string, object>) {
				jsonNode = toJSONClass ((Dictionary<string, object>)item);
			} else {
				jsonNode = toJSONValue (item);
			}
			
			if (jsonNode == null) {
				continue;
			}
			
			jsonArray.Add (jsonNode);
		}
		
		return jsonArray;
	}
	
	public static JSONClass toJSONValue (object data)
	{
		if (data == null) {
			return null;
		}
		
		DataType type;
		
		if (data is int) {
			type = DataType.intType;
		} else if (data is long) {
			type = DataType.longType;
		} else if (data is float) {
			type = DataType.floatType;
		} else if (data is double) {
			type = DataType.doubleType;
		} else if (data is bool) {
			type = DataType.boolType;
		} else if (data is string) {
			type = DataType.stringType;
		} else {
			return null;
		}
		
		JSONData jsonDataChecksum = new JSONData ("faddface");
		
		// Data type is stored as a string instead of remaining an
		// integer because of a bug in SimpleJSON where by the
		// JSONData.ToString() method surrounds the value with
		// double quotes, making it appear like a JSON string
		JSONData jsonDataType = new JSONData (((int)type).ToString ());
		
		JSONData jsonDataValue = new JSONData (data.ToString ());
		
		JSONClass jsonClass = new JSONClass ();
		jsonClass.Add ("checksum", jsonDataChecksum);
		jsonClass.Add ("type", jsonDataType);
		jsonClass.Add ("value", jsonDataValue);
		
		return jsonClass;
	}

	public static object Deserialize(string message) 
	{

		return Json.Deserialize (message);
	}

	public static string Serialize(object message) 
	{
		if( message == null ) {
			return null;
		}
		return Json.Serialize(message);
	}

	public static bool TryParseEnum<T> (string value, out T returnValue)
	{
		try {
			returnValue = (T)Enum.Parse (typeof (T), value);
			return true;
		} catch {
			returnValue = default (T);
			return false;
		}
	}
	
	public static Dictionary<string, string> ToStringDictionary<TKey, TValue> (Dictionary<TKey, TValue> dictionary)
	{
		if (dictionary == null) {
			return null;
		}
		
		Dictionary<string, string> result = new Dictionary<string, string> ();
		
		foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
		{
			result.Add(keyValuePair.Key.ToString (), keyValuePair.Value.ToString ());
		}
		
		return result;
	}
	
	#endregion

	
}


