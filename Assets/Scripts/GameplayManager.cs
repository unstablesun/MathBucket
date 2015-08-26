using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour 
{
	//each game and final score is divided into a series of equation puzzles

	public enum eGameDifficulty 
	{
		learning,
		easy,
		hard,
		genius,
	};
	public eGameDifficulty _gameDifficulty = eGameDifficulty.easy;

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
	private float _singleEquationTime = 15.0f;
	private int _gameLevel = 1;
	private int _gameScore = 0;

	void Start () 
	{
	
	}

	void Reset () 
	{
		_gameLevel = 1;
		_gameScore = 0;
		_gameState = eGameState.init;
	}


	public void StartGame () 
	{

		_gameState = eGameState.selectFormulas;


		//select random paramters based on difficulty

		int difficulty = 1;

		bool parameterStrict = true;

		//set default values
		int minMaxThres = 4;
		int min = 1;
		int max = 5;

		int operandPref = (int)FormulaFactory.eOperandBias.any;
		int prefProb = 80;
		int sameProb = 90;

		int numEquaThres = 6;
		int numEqua = 1;
		int numEquaCap = 2;

		if (_gameDifficulty == eGameDifficulty.easy) 
		{


		}
		else if (_gameDifficulty == eGameDifficulty.hard) 
		{
			max = 8;
			
		}
		else if (_gameDifficulty == eGameDifficulty.genius) 
		{
			min = 0;
			max = 12;
			
		}
		else if (_gameDifficulty == eGameDifficulty.learning) 
		{
			prefProb = 100;
			sameProb = 100;

		}

		SetupPuzzleCurveData (minMaxThres, min, max, //minMaxThreshold, min, max
		                      operandPref, prefProb, sameProb, //operand, prefprob, prob of same in two+ equarions
		                      numEquaThres, numEqua, numEquaCap); //num equation threshold, starting num equations

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


	void Update () 
	{

		switch (_gameState)
		{
		case eGameState.init:
			break;

		case eGameState.selectFormulas:
			AdvancePuzzleCurve();
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

			GameCommon.getPlayfieldManagerClass().SetPuzzleResults(_gameScore, _gameLevel+1);

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






	
	
	public struct PuzzleCurveData 
	{
		public int minMaxThreshold { get; set; }
		public int minRange { get; set; }
		public int maxRange { get; set; }

		public int operatorPreference { get; set; } //do we want all plus
		public int operatorPreferenceProb { get; set; } //probability of preferred operator
		public int sameOperatorProb { get; set; } //for 2 or more equations, probability they will be the same

		public int numEquationThreshold { get; set; }
		public int numEquations { get; set; }
		public int numEquationsCap { get; set; }

		public int resetRangeAfterEquationInc { get; set; }

	}

	private PuzzleCurveData mPuzzleCurveData;

	//set once at the start of the game
	public void SetupPuzzleCurveData (	int minMaxThreshold, int minRange, int maxRange, 
	                           			int operatorPreference, int operatorPreferenceProb, int sameOperatorProb,
	                           			int numEquationThreshold, int numEquations, int numEquationsCap) 
	{
		mPuzzleCurveData.minMaxThreshold = minMaxThreshold;
		mPuzzleCurveData.minRange = minRange;
		mPuzzleCurveData.maxRange = maxRange;

		mPuzzleCurveData.operatorPreference = operatorPreference;
		mPuzzleCurveData.operatorPreferenceProb = operatorPreferenceProb;
		mPuzzleCurveData.sameOperatorProb = sameOperatorProb;

		mPuzzleCurveData.numEquationThreshold = numEquationThreshold;
		mPuzzleCurveData.numEquations = numEquations;
		mPuzzleCurveData.numEquationsCap = numEquationsCap;
	}

	private void AdvancePuzzleCurve () 
	{
		FormulaFactory _formulaFactory = GameCommon.getFormulaFactoryClass();

		int minMaxThreshold = mPuzzleCurveData.minMaxThreshold;
		int minRange = mPuzzleCurveData.minRange;
		int maxRange = mPuzzleCurveData.maxRange;

		FormulaFactory.eOperandBias operatorPref = (FormulaFactory.eOperandBias)mPuzzleCurveData.operatorPreference;
		int operatorPreferenceProb = mPuzzleCurveData.operatorPreferenceProb;
		int sameOperatorProb = mPuzzleCurveData.sameOperatorProb;

		FormulaFactory.eOperandBias operand = FormulaFactory.eOperandBias.forcePlus;
		FormulaFactory.eOperandBias firstOperand = FormulaFactory.eOperandBias.forcePlus;

		int numEquationThreshold = mPuzzleCurveData.numEquationThreshold;
		int numEquations = mPuzzleCurveData.numEquations;
		int numEquationsCap = mPuzzleCurveData.numEquationsCap;

		for (int i = 0; i < numEquations; i++) 
		{


			bool operatorSet = false;
			if(i > 0)
			{
				int chanceSame = UnityEngine.Random.Range(0, 100);
				if(chanceSame < sameOperatorProb)
				{
					operand = firstOperand;
					operatorSet = true;
				}
			}

			if(operatorSet == false)
			{
				if(operatorPref == FormulaFactory.eOperandBias.any)
				{
					operand = (FormulaFactory.eOperandBias)UnityEngine.Random.Range(1, 5);//1-4
				}
				else
				{
					int chancePref = UnityEngine.Random.Range(0, 100);

					if(chancePref < operatorPreferenceProb)
					{
						operand = operatorPref;
					}
					else
					{
						operand = (FormulaFactory.eOperandBias)UnityEngine.Random.Range(1, 5);//1-4
					}
				}
			}

			if(i == 0)
			{
				firstOperand = operand;
			}
			


			//add random select bucket
			
			
			_formulaFactory.AddEquation(0, minRange, maxRange, operand );

		}

		_elaspedPuzzleTime = 0.0f;
		_puzzleDurationTime = _singleEquationTime * (float)numEquations;


		//update thresholds
		if (_gameLevel % minMaxThreshold == 0) {

			//crossed threshold
			//advance some curve data

			maxRange++;
			mPuzzleCurveData.maxRange = maxRange;

		}

		if (_gameLevel % numEquationThreshold == 0) {
			
			//crossed threshold
			//advance some curve data

			if(numEquations < numEquationsCap)
			{
				numEquations++;
				mPuzzleCurveData.numEquations = numEquations;
			}
		}

	}

}
