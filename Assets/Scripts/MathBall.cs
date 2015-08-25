using UnityEngine;
using System.Collections;

public class MathBall : MonoBehaviour 
{

	public enum eFunction 
	{
		Digit,
		Number,
		Operand,
		Point
	};
	public eFunction _function = eFunction.Digit;

	public enum eOperator 
	{
		Plus,
		Minus,
		Multiply,
		Divide,
		Power,
		Modulus,
		Equals
	};
	public eOperator _operator = eOperator.Plus;

	public enum eState 
	{
		Create,
		InPool, 
		InBucket,  
		Selected, 
		Animating
	};
	public eState _state = eState.Create;

	//unique id for bucket
	private int _ball_index;
	public int ball_index
	{
		get { return _ball_index; }
		set { _ball_index = value; }
	}
	
	//ball_value holds the value of a digit, number, and operand
	private int _ball_value;
	public int ball_value
	{
		get { return _ball_value; }
		set { _ball_value = value; }
	}

	//the order the ball was selected
	private int _select_order;
	public int select_order
	{
		get { return _select_order; }
		set { _select_order = value; }
	}


	public void SetFunctionAndValue (eFunction f, int value) 
	{

		_function = f;
		_ball_value = value;


		if (f == eFunction.Digit) 
		{

			setBallText(value.ToString());
			setBallTextColor(new Color(0.0f, 0.0f, 0.0f, 1f));

		}
		else if (f == eFunction.Operand) 
		{

			_operator = (eOperator)value;

			string operatorText = "e";
			if(_operator == MathBall.eOperator.Plus)
			{
				operatorText = "+";
				setBallTextColor(new Color(0.25f, 0.25f, 1f, 1f));
			}
			else if(_operator == MathBall.eOperator.Minus)
			{
				operatorText = "-";
				setBallTextColor(new Color(1f, 0.25f, 1f, 1f));
			}
			else if(_operator == MathBall.eOperator.Multiply)
			{
				operatorText = "*";
				setBallTextColor(new Color(0.25f, 1f, 0.25f, 1f));
			}
			else if(_operator == MathBall.eOperator.Divide)
			{
				operatorText = "/";
				setBallTextColor(new Color(1.0f, 0.25f, 0.25f, 1f));
			}
			else if(_operator == MathBall.eOperator.Power)
			{
				operatorText = "^";
				setBallTextColor(new Color(1.0f, 1.0f, 0.25f, 1f));
			}
			else if(_operator == MathBall.eOperator.Equals)
			{
				operatorText = "=";
				setBallTextColor(new Color(1.0f, 1.0f, 1.0f, 1f));
			}

			setBallText(operatorText);

		}
		else if (f == eFunction.Number) 
		{

		}

	}



	void Start () 
	{

		setBallHiliteColor(new Color(1.0f, 1.0f, 1.0f, 0f));
	}
	
	void Update () 
	{


	
	}



	public bool setBallSelected() 
	{
		if(_state == eState.Selected)
		{
			return false;
		}
		else
		{
			setBallHiliteColor(new Color(1.0f, 0.2f, 0.1f, 1f));
			_state = eState.Selected;
		}


		return true;
	}

	public void clearSelected() 
	{
		setBallHiliteColor(new Color(1.0f, 1.0f, 1.0f, 1f));
		_state = eState.InBucket;
	}
	public void removeSelected() 
	{
		setBallHiliteColor(new Color(1.0f, 1.0f, 1.0f, 1f));
		_state = eState.InPool;
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
	private void setBallTextColor(Color _c)
	{
		GameObject _ballText = _getChildGameObject("text1");
		_ballText.GetComponent<Renderer>().material.color = _c;
	}
	
	private void setBallColor(Color _c)
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



}
