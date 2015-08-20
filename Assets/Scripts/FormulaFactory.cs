using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FormulaFactory : MonoBehaviour 
{
	private int _ball_value;
	public int ball_value
	{
		get { return _ball_value; }
		set { _ball_value = value; }
	}
	
	private MathBall.eFunction _ball_function;
	public MathBall.eFunction ball_function
	{
		get { return _ball_function; }
		set { _ball_function = value; }
	}

	private int keyIndex;

	public class FormulaToken 
	{
		public int value { get; set; }
		public MathBall.eFunction function { get; set; }
		public string key { get; set; }
	}
	
	public static List <FormulaToken> FormulaTokens = new List<FormulaToken>();
	
	void Start ()
	{
	}
	
	public void ResetFormulaTokenList () 
	{
		keyIndex = 0;
		FormulaTokens.Clear ();
	}

	public void AddFormulaToken (int value, MathBall.eFunction function) 
	{
		//Debug.Log ("AddFormulaToken = " + value);
		FormulaToken newToken = new FormulaToken ();
		
		newToken.key = "k"+keyIndex;
		newToken.value = value;
		newToken.function = function;
		
		//Debug.Log ("Token : key = " + newToken.key + " value = " + newToken.value.ToString() + " function = " + (MathBall.eFunction)newToken.function);
		
		FormulaTokens.Add(newToken);
		
		keyIndex++;

		//Debug.Log ("AddFormulaToken : keyIndex = " + keyIndex);
	}
	
	public int InitEquationSet()
	{
		keyIndex = 0;
		AddEquation(1, 1);
		return keyIndex;
	}

	private void AddEquation(int numEquations, int complexityCurve)
	{
		//unit tests
		AddFormulaToken (1, MathBall.eFunction.Digit);
		AddFormulaToken ((int)MathBall.eOperator.Plus,  MathBall.eFunction.Operand);
		AddFormulaToken (2, MathBall.eFunction.Digit);
		AddFormulaToken ((int)MathBall.eOperator.Equals,  MathBall.eFunction.Operand);
		AddFormulaToken (3, MathBall.eFunction.Digit);

		AddFormulaToken (5, MathBall.eFunction.Digit);
		AddFormulaToken ((int)MathBall.eOperator.Multiply,  MathBall.eFunction.Operand);
		AddFormulaToken (4, MathBall.eFunction.Digit);
		AddFormulaToken ((int)MathBall.eOperator.Equals,  MathBall.eFunction.Operand);
		AddFormulaToken (2, MathBall.eFunction.Digit);
		AddFormulaToken (0, MathBall.eFunction.Digit);

		//int numLeftDigits = UnityEngine.Random.Range(2, 4);
		//int operand = UnityEngine.Random.Range(0, 5);
	}

	private FormulaToken GetFormulaToken(string key) 
	{
		foreach(FormulaToken gObj in FormulaTokens)
		{
			if(gObj.key == key)
			{
				return gObj;
			}
		}
		return null;
	}

	public bool getNextBallValue () 
	{
		//Debug.Log ("getNextBallValue : keyIndex = " + keyIndex);

		if(keyIndex < 0)
			return false;

		string _key = "k" + (keyIndex - 1);
		keyIndex--;

		FormulaToken _ftoken = GetFormulaToken (_key);

		if( _ftoken != null) 
		{
			_ball_value = _ftoken.value;
			_ball_function = _ftoken.function;


			return true;
		}

		return false;
	}


	//deprecated
	public void setNextBallValues () 
	{
		int chance = UnityEngine.Random.Range(0, 100);

		if(chance < 50)
		{
			int rn = UnityEngine.Random.Range(0, 9);
			_ball_value = rn;

			_ball_function = MathBall.eFunction.Digit;
			
		}
		else if(chance < 80)
		{
			int operand = UnityEngine.Random.Range(0, 5);
			_ball_value = operand;

			_ball_function = MathBall.eFunction.Operand;
		}
		else
		{
			_ball_value = (int)MathBall.eOperator.Equals;
			_ball_function = MathBall.eFunction.Operand;
		}

	}
	
}
