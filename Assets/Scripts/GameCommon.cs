using UnityEngine;
using System.Collections;
using System;

public class GameCommon : MonoBehaviour 
{

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

	static public ParticleDepot getParticleDepotClass()
	{
		GameObject _particleDepot = GameObject.Find("ParticleDepot");
		if (_particleDepot != null) {
			ParticleDepot _particleDepotScript = _particleDepot.GetComponent<ParticleDepot> ();
			if(_particleDepotScript != null) {
				return _particleDepotScript;
			}
			throw new Exception();
		}
		throw new Exception();
	}

	static public AudioDepot getAudioDepotClass()
	{
		GameObject _audioDepot = GameObject.Find("AudioDepot");
		if (_audioDepot != null) {
			AudioDepot _audioDepotScript = _audioDepot.GetComponent<AudioDepot> ();
			if(_audioDepotScript != null) {
				return _audioDepotScript;
			}
			throw new Exception();
		}
		throw new Exception();
	}

	static public MainMenuHandler getMainMenuClass()
	{
		GameObject _mainmenu = GameObject.Find("MainMenuHandler");
		if (_mainmenu != null) {
			MainMenuHandler _mainmenutScript = _mainmenu.GetComponent<MainMenuHandler> ();
			if(_mainmenutScript != null) {
				return _mainmenutScript;
			}
			throw new Exception();
		}
		throw new Exception();
	}


}
