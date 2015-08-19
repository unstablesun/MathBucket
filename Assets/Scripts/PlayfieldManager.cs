using UnityEngine;
using System.Collections;
using System;

public class PlayfieldManager : MonoBehaviour 
{
	private GameObject EndGameOverlay = null;
	private GameObject StartGameOverlay = null;

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

	}
	
	void Update () 
	{
	
	}



	public void StartMatch () 
	{
		//remove canvas
		if (StartGameOverlay != null)
			StartGameOverlay.SetActive(false);

		//start the balls falling
		GameCommon.getBallManagerClass().SetState(1);
	}



	public void SetMatchOverWithScore (int score) 
	{
		if (EndGameOverlay != null)
			EndGameOverlay.SetActive(true);
		
		GameCommon.getFuelHandlerClass ().SetMatchScore (score);
	}


	public void GotoMainMenu () 
	{
		MainMenuHandler.sComingFromGame = true;

		Application.LoadLevel("MainMenu");
	}


}
