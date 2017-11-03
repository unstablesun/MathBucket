using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

	public TextMeshProUGUI _inGameScoreText;
	public TextMeshProUGUI _inGameRoundText;
	public TextMeshProUGUI _inGameBaseScoreText;
	public TextMeshProUGUI _inGameDiffMultText;
	public TextMeshProUGUI _inGameRoundMultText;
	public TextMeshProUGUI _inGameRoundScoreText;
	public TextMeshProUGUI _inGameBlitzMultText;

	private GameObject EndGameOverlay = null;
	private GameObject StartGameOverlay = null;
	private GameObject GamePlayOverlay = null;
	private GameObject GameBackingOverlay = null;

	public GameObject BlitzMeterDial = null;

	private float _elaspedTime = 0.0f;
	private float _puzzleResultsTime = 0.1f;



	public static PlayfieldManager Instance;

	void Awake () 
	{
		Instance = this;

	}


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

		GameBackingOverlay = GameObject.Find("GameBackingLayer");
		if (GameBackingOverlay != null)
		{
			GameBackingOverlay.SetActive(true);
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

				//float remainingTime = GameCommon.getGameplayManagerClass ().GetPuzzleTimeLeft ();
				float remainingTime = GameplayManager.Instance.GetPuzzleTimeLeft ();

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

					//GameCommon.getGameplayManagerClass().StartNextPuzzle();
					GameplayManager.Instance.StartNextPuzzle();
					_playfieldState = ePlayfieldState.gameplay;

					_elaspedTime = 0.0f;
				}
			}
			break;


		}
		
	}



	public void SetMatchOverWithScore (int score) 
	{
		if (EndGameOverlay != null) {
			EndGameOverlay.SetActive (true);
		}


		SetOverlayFinalScore(score);
		
		//GameCommon.getFuelHandlerClass ().SetMatchScore (score);
		//set early score reporting here!

		//FuelHandler.Instance.SendFinishedMatchDetails (score);
	}

	//only for last puzzle - wait and trigger next puzzle
	public void SetPuzzleResults (int score, int level) 
	{
		_playfieldState = ePlayfieldState.displayResults;

		SetOverlayPostGameValues(score, level);
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

		GameplayManager.Instance.StartGame();
	}

	//this gets called by the GUI button "Results"
	public void GotoMainMenu () 
	{

		SceneManager.LoadScene("MainMenu");
	}


	private void SetOverlayFinalScore (int score) 
	{
		Text[] texts = EndGameOverlay.GetComponentsInChildren<Text>();
		foreach (Text text in texts)
		{
			Debug.Log ("text found = " + text.name);
			if(text.name == "FinalScoreText")
			{
				//text.enabled = enabled;
				text.text = "Score : " + score;
			}
		}
	}


	private void SetOverlayPostGameValues(int score, int roundScore) 
	{
		_inGameScoreText.SetText ("Score : " + score.ToString());
		_inGameRoundScoreText.SetText ("= " + roundScore.ToString());
	}

	public void SetOverlayPregameValues (float diffMult, float roundMult, int level) 
	{
		_inGameDiffMultText.SetText ("* " + diffMult.ToString());
		_inGameRoundMultText.SetText ("* " + roundMult.ToString());
		_inGameRoundText.SetText ("Round : " + level.ToString());
	}

	public void SetOverlayBaseScoreValue (int baseScore) 
	{
		_inGameBaseScoreText.SetText (baseScore.ToString() + " *");
	}

	public void SetOverlayBlitzMultValue (int mult) 
	{
		_inGameBlitzMultText.SetText (mult.ToString());
	}



	private void SetCircularLoadAmount (float amount) 
	{
		//Debug.Log ("amount = " + amount);
		if(BlitzMeterDial != null) {
			Image[] images = BlitzMeterDial.GetComponentsInChildren<Image>();
			foreach (Image image in images)
			{
				if(image.name == "BlitzMeter")
				{
					image.fillAmount = amount;
				}
			
			}
		}
	}






}
