using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FuelSDKMiniJSON;

public class TestFuelSDK : MonoBehaviour {

	#region ===================================== Editor Variables =====================================
	
	[SerializeField]
	protected string messageStringTest;

	#endregion

	#region ===================================== Editor Variables =====================================

	protected FuelSDK fuelSDKInstance;

	#endregion

	#region ===================================== MonoBehaviour =====================================

	void Awake () {
		fuelSDKInstance = GetComponent<FuelSDK>();
	}

	void OnGUI() {
		if (GUI.Button(new Rect(10, 10, 130, 30), "Test Fuel Message")) {
			TestFuelMessage();
		}
		
	}
	
	#endregion

	#region ===================================== Buttons =====================================

	void TestFuelMessage() {
		if( fuelSDKInstance != null && !String.IsNullOrEmpty(messageStringTest) ) {
			fuelSDKInstance.gameObject.SendMessage( "DataReceiver" , messageStringTest );
		}
	}

	#endregion
}
