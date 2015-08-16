using UnityEngine;
using System.Collections;
using System;

public class PlayfieldManager : MonoBehaviour 
{

	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}



	public void StartMatch () 
	{
		//remove canvas
		GameObject go = GameObject.Find("CanvasStartGame");
		if (!go)
			return;

		go.SetActive(false);



		getBallManagerClass().SetState(1);
	}






	private BallManager getBallManagerClass()
	{
		GameObject _ballManagerHandler = GameObject.Find("BallManager");
		if (_ballManagerHandler != null) {
			BallManager _ballManagerScript = _ballManagerHandler.GetComponent<BallManager> ();
			if(_ballManagerScript != null) {
				return _ballManagerScript;
			}
			throw new Exception();
		}
		throw new Exception();
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
