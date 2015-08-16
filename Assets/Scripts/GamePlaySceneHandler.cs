using UnityEngine;
using System.Collections;
using System;

public class GamePlaySceneHandler : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}



	public void SetWin()
	{
		MainMenuHandler.sComingFromGame = true;

		getFuelHandlerClass ().SetMatchScore (1);

		Application.LoadLevel("Title");
	}
	public void SetLose()
	{
		MainMenuHandler.sComingFromGame = true;

		getFuelHandlerClass ().SetMatchScore (0);

		Application.LoadLevel("Title");
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
