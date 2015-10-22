using UnityEngine;
using System.Collections;
using System;

public class GamePlaySceneHandler : MonoBehaviour 
{

	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}
	
	public void SetWin()
	{
		MainMenuHandler.sComingFromGame = true;

		//GameCommon.getFuelHandlerClass ().SetMatchScore (1);

		Application.LoadLevel("Title");
	}
	public void SetLose()
	{
		MainMenuHandler.sComingFromGame = true;

		//GameCommon.getFuelHandlerClass ().SetMatchScore (0);

		Application.LoadLevel("Title");
	}


}
