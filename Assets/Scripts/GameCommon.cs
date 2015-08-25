using UnityEngine;
using System.Collections;
using System;

public class GameCommon : MonoBehaviour 
{
	public static Color ballColorBackDigit;
	public static float ballScaleDigit;

	public static Color ballColorBackOperand;
	public static float ballScaleOperand;

	public static Color ballColorBackEquals;
	public static float ballScaleEquals;

	public static Color ballColorHilite;	

	public static void InitDefaults()
	{
		//default colors & scales
		ballColorBackDigit = new Color(0.9f, 0.9f, 0.9f, 1f);
		ballScaleDigit = 0.3f;

		ballColorBackOperand = new Color(0.5f, 0.5f, 0.5f, 1f);
		ballScaleOperand = 0.25f;

		ballColorBackEquals = new Color(0.3f, 0.3f, 0.3f, 1f);
		ballScaleEquals = 0.2f;

		ballColorHilite = new Color(0.9f, 0.9f, 0.9f, 1f);

	}

	static public CentralCalculator getCentralCalculatorClass()
	{
		GameObject _centralCalculatorObject = GameObject.Find("CentralCalculator");
		if (_centralCalculatorObject != null) 
		{
			CentralCalculator _centralCalculatorScript = _centralCalculatorObject.GetComponent<CentralCalculator> ();
			if(_centralCalculatorScript != null) 
			{
				return _centralCalculatorScript;
			}
			Debug.Log("_centralCalculatorScript == null");
			throw new Exception();
		}
		Debug.Log("_centralCalculatorObject = null");
		throw new Exception();
	}

	static public FormulaFactory getFormulaFactoryClass()
	{
		GameObject _formulaFactoryObject = GameObject.Find("FormulaFactory");
		if (_formulaFactoryObject != null) 
		{
			FormulaFactory _formulaFactoryScript = _formulaFactoryObject.GetComponent<FormulaFactory> ();
			if(_formulaFactoryScript != null) 
			{
				return _formulaFactoryScript;
			}
			Debug.Log("_formulaFactoryScript == null");
			throw new Exception();
		}
		Debug.Log("_formulaFactoryObject = null");
		throw new Exception();
	}

	static public BallManager getBallManagerClass()
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
	
	static public FuelHandler getFuelHandlerClass()
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

	static public PlayfieldManager getPlayfieldManagerClass()
	{
		GameObject _playfieldManager = GameObject.Find("PlayfieldManager");
		if (_playfieldManager != null) {
			PlayfieldManager _playfieldManagerScript = _playfieldManager.GetComponent<PlayfieldManager> ();
			if(_playfieldManagerScript != null) {
				return _playfieldManagerScript;
			}
			throw new Exception();
		}
		throw new Exception();
	}

	static public GameplayManager getGameplayManagerClass()
	{
		GameObject _gameplayManager = GameObject.Find("GameplayManager");
		if (_gameplayManager != null) {
			GameplayManager _gameplayManagerScript = _gameplayManager.GetComponent<GameplayManager> ();
			if(_gameplayManagerScript != null) {
				return _gameplayManagerScript;
			}
			throw new Exception();
		}
		throw new Exception();
	}

}
