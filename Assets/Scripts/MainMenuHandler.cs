using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour 
{
	void Start () 
	{
	}
	
	void Update () 
	{
	
	}
		
	public void LaunchEasy () 
	{
		//GameplayManager.Instance.SetGameDifficulty (GameplayManager.eGameDifficulty.easy);

		GameplayManager._gameDifficulty = GameplayManager.eGameDifficulty.easy;

		LoadGame ();
	}

	public void LaunchHard () 
	{
		//GameplayManager.Instance.SetGameDifficulty (GameplayManager.eGameDifficulty.hard);

		GameplayManager._gameDifficulty = GameplayManager.eGameDifficulty.hard;

		LoadGame ();
	}

	public void LaunchExpert () 
	{
		//GameplayManager.Instance.SetGameDifficulty (GameplayManager.eGameDifficulty.expert);

		GameplayManager._gameDifficulty = GameplayManager.eGameDifficulty.expert;

		LoadGame ();
	}

	public void LoadGame () 
	{
		SceneManager.LoadScene("PlayField");
	}
		
}
