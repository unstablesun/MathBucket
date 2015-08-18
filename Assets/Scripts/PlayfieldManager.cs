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

		//start the balls falling
		GameCommon.getBallManagerClass().SetState(1);
	}



	public void SetMatchOverWithScore (int score) 
	{
		MainMenuHandler.sComingFromGame = true;
		
		GameCommon.getFuelHandlerClass ().SetMatchScore (score);
		
		Application.LoadLevel("MainMenu");
	}




}
