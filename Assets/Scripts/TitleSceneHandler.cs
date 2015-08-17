using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class TitleSceneHandler : MonoBehaviour 
{
	public float screenTimeout = 6.0f;
	private float _elaspedTime = 0.0f;

	void Start () 
	{
		_elaspedTime = 0.0f;

	}	

	void Update () 
	{
		_elaspedTime += Time.deltaTime;
		
		if(_elaspedTime > screenTimeout)
		{
			_elaspedTime = 0.0f;
			Application.LoadLevel("MainMenu");
		}
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
