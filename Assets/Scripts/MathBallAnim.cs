using UnityEngine;
using System.Collections;

public class MathBallAnim : MonoBehaviour 
{
	[System.Serializable]
	public class Move
	{
		public float velcity = 25.0f;
		public float animTime = 1.0f;
		public bool x_axis = false;
		public bool y_axis = false;
		public bool z_axis = false;
		public bool _direction = false;
	}
	public Move move;


	public enum eState 
	{
		Create,
		InPool, 
		Animating  
	};
	public eState _state = eState.Create;
	
	private float _elaspedTime;


	// Use this for initialization
	void Start () 
	{
		_elaspedTime = 0.0f;

	}

	public void startAnim()
	{
		_elaspedTime = 0.0f;
		_state = eState.Animating;
	}

	
	// Update is called once per frame
	void Update () 
	{
		if(_state == eState.Animating)
		{
			//Translate-----------------------------------------------------------
			if(move.y_axis == true)
			{
				if(move._direction == false)
					transform.Translate(Vector3.up * Time.deltaTime * move.velcity);
				else
					transform.Translate(Vector3.down * Time.deltaTime * move.velcity);
			}
			
			if(move.x_axis == true)
			{
				if(move._direction == false)
					transform.Translate(Vector3.right * Time.deltaTime * move.velcity);
				else
					transform.Translate(Vector3.left * Time.deltaTime * move.velcity);
			}
			
			if(move.z_axis == true)
			{
				if(move._direction == false)
					transform.Translate(Vector3.forward * Time.deltaTime * move.velcity);
				else
					transform.Translate(Vector3.back * Time.deltaTime * move.velcity);
			}
			
			_elaspedTime += Time.deltaTime;
			if(_elaspedTime > move.animTime)
			{
				//reset animation is over

				_state = eState.InPool;
			}
		}
	
	}


	private GameObject _getChildGameObject(string withName) 
	{
		foreach (Transform child in transform)
		{
			if (child.gameObject.name == withName)
				return child.gameObject;
		}
		return null;
	}

	public void setBallText(string t)
	{
		GameObject _ballText = _getChildGameObject("text1");
		_ballText.GetComponent<TextMesh>().text = t;
	}
	public void setBallTextColor(Color _c)
	{
		GameObject _ballText = _getChildGameObject("text1");
		_ballText.GetComponent<Renderer>().material.color = _c;
	}
	
	public void setBallColor(Color _c)
	{
		GameObject image = _getChildGameObject("image1");
		image.GetComponent<Renderer>().material.color = _c;
	}
	
	public void setBallHiliteColor(Color _c)
	{
		GameObject image = _getChildGameObject("hilite1");
		image.GetComponent<Renderer>().material.color = _c;
	}
	
	public void SetBallScale (float _s) 
	{
		transform.localScale = new Vector3(_s, _s, _s);
	}
	public void SetBallAlpha (float _a) 
	{
		GameObject image = _getChildGameObject("image1");
		GameObject text = _getChildGameObject("text1");
		GameObject hilite = _getChildGameObject("hilite1");

	}		


}
