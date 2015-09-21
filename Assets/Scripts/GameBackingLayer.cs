using UnityEngine;
using System.Collections;

public class GameBackingLayer : MonoBehaviour 
{
	[System.Serializable]
	public class Move
	{
		public float velcity = 8.0f;
		public float velcity2 = 16.0f;
		public float animTime = 0.5f;
		public float animTime2 = 1.5f;
		public bool x_axis = false;
		public bool y_axis = false;
		public bool z_axis = false;
		public bool _direction = false;
	}
	public Move move;


	public GameObject _collisionEdge1 = null;
	public GameObject _defaultPosEdge1 = null;

	public GameObject _collisionEdge2 = null;
	public GameObject _defaultPosEdge2 = null;

	public GameObject _collisionEdge3 = null;
	public GameObject _defaultPosEdge3 = null;

	public GameObject _collisionEdge4 = null;
	public GameObject _defaultPosEdge4 = null;


	private float _elaspedTime;
	//private GameObject _elaspedTime;

	void Start () 
	{
		_elaspedTime = 0f;
	}
	
	void Update () 
	{
		//Translate-----------------------------------------------------------
		if(move.y_axis == true)
		{
			if(move._direction == false)
				transform.Translate(Vector3.up * Time.deltaTime * move.velcity2);
			else
				transform.Translate(Vector3.down * Time.deltaTime * move.velcity2);
		}
		
		if(move.x_axis == true)
		{
			if(move._direction == false)
				transform.Translate(Vector3.right * Time.deltaTime * move.velcity2);
			else
				transform.Translate(Vector3.left * Time.deltaTime * move.velcity2);
		}
		
		if(move.z_axis == true)
		{
			if(move._direction == false)
				transform.Translate(Vector3.forward * Time.deltaTime * move.velcity2);
			else
				transform.Translate(Vector3.back * Time.deltaTime * move.velcity2);
		}


		_elaspedTime += Time.deltaTime;
		if(_elaspedTime > move.animTime)
		{
			//move to animation ZoomOutAnim
			_elaspedTime = 0.0f;
			
		}

	
	}
}
