using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class TitleSceneHandler : MonoBehaviour 
{
	public float screenTimeout = 6.0f;
	private float _elaspedTime = 0.0f;

	void Start () 
	{
		_elaspedTime = 0.0f;

	}	

	void Update () 
	{
		_elaspedTime += Time.deltaTime;
		
		if(_elaspedTime > screenTimeout)
		{
			_elaspedTime = 0.0f;
			Application.LoadLevel("MainMenu");
		}
	}

}
