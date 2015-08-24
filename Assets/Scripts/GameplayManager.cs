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
	private int _gameLevel = 1;
	private int _gameScore = 0;

	void Start () 
	{
	
	}

	void Reset () 
	{
		_gameLevel = 0;
		_gameState = eGameState.init;
	}


	public void StartGame () 
	{

		_gameState = eGameState.selectFormulas;

	}

	public void PuzzleCompete (int score) 
	{
		//get time bonus
		float remainingTime = 1.0f;
		float rtime = _puzzleDurationTime - _elaspedPuzzleTime;
		remainingTime = rtime * 1.0f / _puzzleDurationTime;

		float bonusFactor = (2.0f - remainingTime) * (float)_gameLevel;

		float pScore = (float)score * bonusFactor;


		_gameScore += (int)pScore;
		_gameState = eGameState.puzzleResults;
		
	}

	public void StartNextPuzzle () 
	{
		Debug.Log ("StartNextPuzzle");

		FormulaFactory _formulaFactory = GameCommon.getFormulaFactoryClass();
		_formulaFactory.ResetFormulaTokenList();

		_gameState = eGameState.selectFormulas;

		_gameLevel++;
		
	}

	public float GetPuzzleTimeLeft () 
	{
		float remainingTime = 1.0f;
		float rtime = _puzzleDurationTime - _elaspedPuzzleTime;
		remainingTime = rtime * 1.0f / _puzzleDurationTime;

		return remainingTime;
	}


	void SetupPuzzle () 
	{
		Debug.Log ("SetupPuzzle");

		FormulaFactory _formulaFactory = GameCommon.getFormulaFactoryClass();

		_elaspedPuzzleTime = 0.0f;
		switch( _gameLevel )
		{
		case 1:

			_formulaFactory.AddEquation(0, 1, 10, FormulaFactory.eOperandBias.forcePlus );
			_puzzleDurationTime = 30.0f;
			break;

		case 2:
			
			_formulaFactory.AddEquation(0, 1, 18, FormulaFactory.eOperandBias.forceMinus );

			_puzzleDurationTime = 30.0f;
			break;

		case 3:
			
			_formulaFactory.AddEquation(0, 1, 10, FormulaFactory.eOperandBias.forcePlus );
			_formulaFactory.AddEquation(0, 1, 10, FormulaFactory.eOperandBias.forceMinus );

			_puzzleDurationTime = 50.0f;
			break;

		case 4:
			
			_formulaFactory.AddEquation(0, 1, 10, FormulaFactory.eOperandBias.forceMult );
			_formulaFactory.AddEquation(0, 1, 10, FormulaFactory.eOperandBias.forcePlus );

			_puzzleDurationTime = 50.0f;
			break;

		case 5:

			_formulaFactory.AddEquation(0, 1, 10, FormulaFactory.eOperandBias.forceMult );
			_formulaFactory.AddEquation(0, 1, 10, FormulaFactory.eOperandBias.forceDivide );

			_puzzleDurationTime = 50.0f;
			break;

		case 6:

			_formulaFactory.AddEquation(0, 1, 10, FormulaFactory.eOperandBias.forcePlus );
			_formulaFactory.AddEquation(0, 1, 10, FormulaFactory.eOperandBias.forceMult );
			_formulaFactory.AddEquation(0, 1, 10, FormulaFactory.eOperandBias.forceDivide );

			_puzzleDurationTime = 100.0f;
			break;
		}


	}


	void Update () 
	{

		switch (_gameState)
		{
		case eGameState.init:
			break;

		case eGameState.selectFormulas:
			SetupPuzzle();
			_gameState = eGameState.puzzleReady;
			break;

		case eGameState.puzzleReady:
			GameCommon.getBallManagerClass().SetState(BallManager.eSystemState.reboot);
			_gameState = eGameState.puzzlePlaying;
			break;

		case eGameState.puzzlePlaying:
		{
		
			_elaspedPuzzleTime += Time.deltaTime;
			
			if(_elaspedPuzzleTime > _puzzleDurationTime)
			{
				//puzzle playing time elasped
				GameCommon.getBallManagerClass().SetState(BallManager.eSystemState.done);
				_gameState = eGameState.gameover;

				_elaspedPuzzleTime = 0.0f;
			}

		}
		break;

		case eGameState.puzzleResults:
			//send data to playfield about last puzzle

			Debug.Log ("eGameState.puzzleResults");

			GameCommon.getPlayfieldManagerClass().SetPuzzleResults(_gameScore, _gameLevel);

			_gameState = eGameState.waitingForPlayfield;
			break;

		case eGameState.waitingForPlayfield:
			//waiting for playfield to do it's overlays and tell us to continue (or not)
			break;

		case eGameState.gameover:
			//tell playfield no more puzzles coming - send final data

			GameCommon.getPlayfieldManagerClass().SetMatchOverWithScore(_gameScore);

			_gameState = eGameState.waitingForPlayfield;
			break;
		}

	}
}
