using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CentralCalculator : MonoBehaviour
{
	private int _calcScore;
	public int calcScore
	{
		get { return _calcScore; }
	}



	public class CalcToken 
	{
		public int value { get; set; }
		public MathBall.eFunction function { get; set; }
		public string key { get; set; }
	}


	public static List <CalcToken> CalcTokens = new List<CalcToken>();

	private int errorResult;
	private int keyIndex;

	void Start ()
	{
		keyIndex = 0;
		errorResult = 0;
	}
	
	public void ResetCalcTokenList () 
	{
		keyIndex = 0;
		errorResult = 0;
		CalcTokens.Clear ();
		_calcScore = 0;
	}

	public void AddCalcToken (int value, MathBall.eFunction function) 
	{
		CalcToken newToken = new CalcToken ();
		
		newToken.key = "k"+keyIndex;
		newToken.value = value;
		newToken.function = function;

		//Debug.Log ("Token : key = " + newToken.key + " value = " + newToken.value.ToString() + " function = " + (MathBall.eFunction)newToken.function);

		CalcTokens.Add(newToken);

		keyIndex++;
	}
		
	public bool AnalyseCalcTokenList () 
	{
		Debug.Log ("AnalyseCalcTokenList");
		bool result = false;

		//equation
		int eix = 0;
		int[] equation = new int[128];

		//fill equation
		int nix = 0;
		string[] numberStrings = new string[32];

		int _index = 0;
		string _key = "k" + _index;
		CalcToken _ctoken = GetCalcToken (_key);

		int pMode = 0;

		while (_ctoken != null) 
		{
			MathBall.eFunction _function = _ctoken.function;
			int _value = _ctoken.value;

			if(_function == MathBall.eFunction.Digit)
			{
				numberStrings [nix] += _value.ToString ();
				pMode = 1;
			}
			else if(_function == MathBall.eFunction.Operand)
			{
				if (pMode == 1) 
				{
					equation [eix] = Int32.Parse (numberStrings [nix]);
					eix++;
					nix++;
					pMode = 0;
				}
				else
				{
					//Debug.Log ("Setting errorResult in loop...");
					errorResult = 1;
					return result;
				}

				equation [eix++] = (int)_value;
			}
				
			//get next
			_index++;
			_key = "k" + _index;
			_ctoken = GetCalcToken (_key);
			if (_ctoken == null) 
			{
				if (pMode == 1) 
				{
					equation [eix] = Int32.Parse (numberStrings [nix]);
					eix++;
				}
			}
		}

		//evaluate the equation
		bool bothSides = false;
		int count = eix;
		int leftSide = 0;
		int rightSide = 9999;
		int score = 0;

		int idx = 0;
		MathBall.eOperator currentOp = MathBall.eOperator.Plus;

		int value = equation [idx];
		idx++;
		while (idx < count) 
		{
			if (idx % 2 == 0) 
			{
				//number
				switch (currentOp) 
				{
					case MathBall.eOperator.Plus:
						value += equation [idx];
						score += 4;
						break;
					case MathBall.eOperator.Minus:
						value -= equation [idx];
						score += 5;
						break;
					case MathBall.eOperator.Multiply:
						value *= equation [idx];
						score += 6;
						break;
					case MathBall.eOperator.Divide:
						value /= equation [idx];
						score += 7;
						break;
					case MathBall.eOperator.Power:
						value = (int)Math.Pow(value, equation [idx]);
						score += 8;
						break;
					case MathBall.eOperator.Modulus:
						value %= equation [idx];
						score += 9;
						break;
				}
			} 
			else 
			{
				//operand
				currentOp = (MathBall.eOperator)equation[idx];
		
				if( currentOp == MathBall.eOperator.Equals)
				{
					if (bothSides == false) 
					{
						leftSide = value;

						idx++;
						if(idx < count)//advance to right side if there is data there
						{
							value = equation [idx];
							bothSides = true;
						}
					}
				}
			}

			idx++;
		}

		if (bothSides == true)
		{
			rightSide = value;
		}

		Debug.Log ("leftSide="+leftSide+":rightSide="+rightSide);

		if (bothSides == true && leftSide == rightSide) 
		{
			result = true;
		}

		_calcScore = score;

		return result;
	}

	public int ErrorReport () 
	{
		return errorResult;
		
	}
		
	private CalcToken GetCalcToken(string key) 
	{
		foreach(CalcToken gObj in CalcTokens)
		{
			if(gObj.key == key)
			{
				return gObj;
			}
		}
		return null;
	}
		

	public void UnitTests()
	{
		
		Console.WriteLine ("12 + 34 = 46");
		AddCalcToken (1, MathBall.eFunction.Digit);
		AddCalcToken (2, MathBall.eFunction.Digit);
		AddCalcToken ((int)MathBall.eOperator.Plus,  MathBall.eFunction.Operand);
		AddCalcToken (3, MathBall.eFunction.Digit);
		AddCalcToken (4, MathBall.eFunction.Digit);
		AddCalcToken ((int)MathBall.eOperator.Equals,  MathBall.eFunction.Operand);
		AddCalcToken (46, MathBall.eFunction.Digit);
		if (AnalyseCalcTokenList () == true) {
			Debug.Log ("...success");
		} else {
			Debug.Log ("...fail");
		}


		ResetCalcTokenList ();

		Console.WriteLine ("2 ^ 5 = 32");
		AddCalcToken (2, MathBall.eFunction.Digit);
		AddCalcToken ((int)MathBall.eOperator.Power,  MathBall.eFunction.Operand);
		AddCalcToken (5, MathBall.eFunction.Digit);
		AddCalcToken ((int)MathBall.eOperator.Equals,  MathBall.eFunction.Operand);
		AddCalcToken (3, MathBall.eFunction.Digit);
		AddCalcToken (1, MathBall.eFunction.Digit);
		if (AnalyseCalcTokenList () == true) {
			Debug.Log ("...success");
		} else {
			Debug.Log ("...fail");
		}


		ResetCalcTokenList ();

		Console.WriteLine ("8 / 2 = 4");
		AddCalcToken (8, MathBall.eFunction.Digit);
		AddCalcToken ((int)MathBall.eOperator.Divide,  MathBall.eFunction.Operand);
		AddCalcToken (2, MathBall.eFunction.Digit);
		AddCalcToken ((int)MathBall.eOperator.Equals,  MathBall.eFunction.Operand);
		AddCalcToken (4, MathBall.eFunction.Digit);
		if (AnalyseCalcTokenList () == true) {
			Debug.Log ("...success");
		} else {
			Debug.Log ("...fail");
		}


		ResetCalcTokenList ();

		Console.WriteLine ("8 * 6 = 48");
		AddCalcToken (8, MathBall.eFunction.Digit);
		AddCalcToken ((int)MathBall.eOperator.Multiply,  MathBall.eFunction.Operand);
		AddCalcToken (6, MathBall.eFunction.Digit);
		AddCalcToken ((int)MathBall.eOperator.Equals,  MathBall.eFunction.Operand);
		AddCalcToken (4, MathBall.eFunction.Digit);
		AddCalcToken (8, MathBall.eFunction.Digit);
		if (AnalyseCalcTokenList () == true) {
			Debug.Log ("...success");
		} else {
			Debug.Log ("...fail");
		}

	}

}
