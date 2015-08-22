using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour 
{
	//each game and final score is divided into a series of equation puzzles


	public enum eGameState 
	{
		init,
		selectFormulas,
		puzzleReady,
		puzzlePlaying,
		puzzleResults,
		waitingForPlayfield,
		gameover,
	};
	public eGameState _gameState = eGameState.init;

	private float _elaspedPuzzleTime = 0.0f;
	private float _puzzleDurationTime = 5.0f;
	private int _gameLevel = 0;

	void Start () 
	{
	
	}

	void Reset () 
	{
		_gameLevel = 0;
	}

	void SetupPuzzle () 
	{
		_elaspedPuzzleTime = 0.0f;



	}


	void Update () 
	{

		switch (_gameState)
		{
		case eGameState.init:
			break;
		case eGameState.selectFormulas:
			break;
		case eGameState.puzzleReady:
			break;

		case eGameState.puzzlePlaying:
		{
		
			_elaspedPuzzleTime += Time.deltaTime;
			
			if(_elaspedPuzzleTime > _puzzleDurationTime)
			{
				//puzzle playing time elasped
				//tell ballmanger
				
				_elaspedPuzzleTime = 0.0f;
			}

		}
		break;

		case eGameState.puzzleResults:
			//send data to playfield about last puzzle

			_gameState = eGameState.waitingForPlayfield;
			break;

		case eGameState.waitingForPlayfield:
			//waiting for playfield to do it's overlays and tell us to continue (or not)
			break;

		case eGameState.gameover:
			//tell playfield no more puzzles coming - send final data

			_gameState = eGameState.waitingForPlayfield;
			break;
		}

	}
}
