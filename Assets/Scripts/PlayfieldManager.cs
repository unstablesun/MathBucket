using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

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
	private GameObject GamePlayOverlay = null;


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

		GamePlayOverlay = GameObject.Find("CanvasGamePlay");
		if (GamePlayOverlay != null)
		{
			GamePlayOverlay.SetActive(false);
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
				SetCircularLoadAmount(1.0f);
				break;

			case ePlayfieldState.gameplay:

				//get timing value from gameplay manager

				float remainingTime = GameCommon.getGameplayManagerClass().GetPuzzleTimeLeft();
				SetCircularLoadAmount(remainingTime);

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
	public void SetPuzzleResults (int score, int level) 
	{
		_playfieldState = ePlayfieldState.displayResults;
		SetOverlayScoreAndLevel(score, level);
	}



	//GUI
	//this gets called by the GUI button "Ready"
	public void StartMatch () 
	{
		//remove canvas
		if (StartGameOverlay != null)
			StartGameOverlay.SetActive(false);

		if (GamePlayOverlay != null)
			GamePlayOverlay.SetActive(true);



		_playfieldState = ePlayfieldState.gameplay;

		GameCommon.getGameplayManagerClass().StartGame();
	}

	//this gets called by the GUI button "Results"
	public void GotoMainMenu () 
	{
		MainMenuHandler.sComingFromGame = true;

		Application.LoadLevel("MainMenu");
	}



	private void SetOverlayScoreAndLevel (int score, int level) 
	{
		Text[] texts = GamePlayOverlay.GetComponentsInChildren<Text>();
		foreach (Text text in texts)
		{
			Debug.Log ("text found = " + text.name);
			if(text.name == "InGameScoreText")
			{
				//text.enabled = enabled;
				text.text = "Score : " + score;
			}

			if(text.name == "InGameLevelText")
			{
				//text.enabled = enabled;
				text.text = "Level : " + level;
			}
		}
	}

	private void SetCircularLoadAmount (float amount) 
	{
		Image[] images = GamePlayOverlay.GetComponentsInChildren<Image>();
		foreach (Image image in images)
		{
			if(image.name == "CircularLoadImage")
			{
				image.fillAmount = amount;
			}
			
		}
	}






}
