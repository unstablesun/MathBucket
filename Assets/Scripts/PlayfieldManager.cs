using UnityEngine;
using System.Collections;
using System;

public class PlayfieldManager : MonoBehaviour 
{

	public enum ePlayfieldState 
	{
		init,
		waitingToStart,
		gameplay,
		displayResults,
		showingResults,
		gameResults,
		waitingToExit,
	};
	public ePlayfieldState _playfieldState = ePlayfieldState.init;

	private GameObject EndGameOverlay = null;
	private GameObject StartGameOverlay = null;


	private float _elaspedTime = 0.0f;
	private float _puzzleResultsTime = 1.0f;

	void Start () 
	{
		EndGameOverlay = GameObject.Find("CanvasEndGame");
		if (EndGameOverlay != null)
		{
			EndGameOverlay.SetActive(false);
		}

		StartGameOverlay = GameObject.Find("CanvasStartGame");
		if (StartGameOverlay != null)
		{
			StartGameOverlay.SetActive(true);
		}

		_playfieldState = ePlayfieldState.waitingToStart;

	}
	
	void Update () 
	{

		switch (_playfieldState)
		{
			case ePlayfieldState.init:
				break;

			case ePlayfieldState.waitingToStart:
				break;

			case ePlayfieldState.gameplay:
				break;
				
			case ePlayfieldState.displayResults:
			{
				_playfieldState = ePlayfieldState.showingResults;

			}
			break;

			case ePlayfieldState.showingResults:
			{
				_elaspedTime += Time.deltaTime;
				
				if(_elaspedTime > _puzzleResultsTime)
				{
					//start next puzzle - call gameplay manager
					Debug.Log ("_playfieldState : StartNextPuzzle");

					GameCommon.getGameplayManagerClass().StartNextPuzzle();

					_playfieldState = ePlayfieldState.gameplay;

					_elaspedTime = 0.0f;
				}
			}
			break;


		}
		
	}



	public void SetMatchOverWithScore (int score) 
	{
		if (EndGameOverlay != null)
			EndGameOverlay.SetActive(true);
		
		GameCommon.getFuelHandlerClass ().SetMatchScore (score);
	}

	//only for last puzzle - wait and trigger next puzzle
	public void SetPuzzleResults (int score) 
	{
		_playfieldState = ePlayfieldState.displayResults;

	}



	//GUI
	//this gets called by the GUI button "Ready"
	public void StartMatch () 
	{
		//remove canvas
		if (StartGameOverlay != null)
			StartGameOverlay.SetActive(false);

		_playfieldState = ePlayfieldState.gameplay;

		GameCommon.getGameplayManagerClass().StartGame();
	}

	//this gets called by the GUI button "Results"
	public void GotoMainMenu () 
	{
		MainMenuHandler.sComingFromGame = true;

		Application.LoadLevel("MainMenu");
	}


}
