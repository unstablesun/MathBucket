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
		Debug.Log ("FormulaFactory Start()");
	}


	public void SetFormulaSeed (int seed) 
	{
		//UnityEngine.Random.seed = seed;
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





	public enum eEquationType 
	{
		compexity1,//a+b=c
		compexity2,
	};

	public enum eOperandBias 
	{
		any,
		forcePlus,
		forceMinus,
		forceMult,
		forceDivide,
		forcePower,
		forceModulus,
		plusOrMinus,
		multOrDivide,
	};

	//deprecated
	//public int InitEquationSet()
	//{
	//	keyIndex = 0;
	//	AddEquation(1, 1);
	//	return keyIndex;
	//}

	
	public void AddEquation(int level, int minRange, int maxRange, eOperandBias operandBias)
	{
		//use level to adjust values
		AddEquationForm(eEquationType.compexity1, minRange, maxRange, operandBias );
	}
	
	private void AddEquationForm(eEquationType eqForm, int minRange, int maxRange, eOperandBias operandBias)
	{
		switch( eqForm )
		{
			case eEquationType.compexity1://a+b=c, a*b=c, etc...
			{
				int inputA, inputB, resultC;
				inputA = UnityEngine.Random.Range(minRange, maxRange);
				inputB = UnityEngine.Random.Range(minRange, maxRange);

				resultC = 0;

				if(operandBias == eOperandBias.forcePlus)
				{
					resultC = inputA + inputB;
					Debug.Log (resultC + " = " + inputA + " + " + inputB);
					AddFormulaToken ((int)MathBall.eOperator.Plus,  MathBall.eFunction.Operand);
				}
				else if(operandBias == eOperandBias.forceMinus)
				{
					int resultX = inputA + inputB;
					resultC = inputA;
					inputA = resultX;
					Debug.Log (resultC + " = " + inputA + " - " + inputB);
					AddFormulaToken ((int)MathBall.eOperator.Minus,  MathBall.eFunction.Operand);
				}
				else if(operandBias == eOperandBias.forceMult)
				{
					resultC = inputA * inputB;
					Debug.Log (resultC + " = " + inputA + " * " + inputB);
					AddFormulaToken ((int)MathBall.eOperator.Multiply,  MathBall.eFunction.Operand);
				}
				else if(operandBias == eOperandBias.forceDivide)
				{
					int resultX = inputA * inputB;
					resultC = inputA;
					inputA = resultX;
					Debug.Log (resultC + " = " + inputA + " / " + inputB);
					AddFormulaToken ((int)MathBall.eOperator.Divide,  MathBall.eFunction.Operand);
				}

				convertDigitsToFormulaTokens(inputA);
				convertDigitsToFormulaTokens(inputB);
				convertDigitsToFormulaTokens(resultC);

				AddFormulaToken ((int)MathBall.eOperator.Equals,  MathBall.eFunction.Operand);

			}
			break;

			case eEquationType.compexity2://a*b=c/d, etc...
			{
				int inputA = UnityEngine.Random.Range(minRange, maxRange);
				
			}
			break;
		}



	}

	private void convertDigitsToFormulaTokens(int input)
	{
		int[] digits = getDigits(input);
		int num = digits.GetLength(0);
		for(int i = 0; i < num; i++)
		{
			AddFormulaToken (digits[i], MathBall.eFunction.Digit);
		}
	}

	private int[] getDigits(int fromThis)
	{
		int[] digits = new int[16];
		int idx = 0;

		if(fromThis >= 100)
		{
			digits[idx++] = fromThis/100;
			int tens = fromThis%100;
			if(tens >= 10)
			{
				digits[idx++] = tens/10;
				digits[idx++] = tens%10;
			}
			else
			{
				digits[idx++] = 0;
				digits[idx++] = tens;
			}
		}
		else if(fromThis >= 10)
		{
			digits[idx++] = fromThis/10;
			digits[idx++] = fromThis%10;
		}
		else
		{
			digits[idx++] = fromThis;
		}

		int[] finaldigits = new int[idx];

		for(int i = 0; i< idx; i++)
		{
			finaldigits[i] = digits[i];
		}


		return finaldigits;
	}


















	//unit tests
	/*
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
		*/









	//deprecated
	private bool isInputAcceptableForMinus(int inputA, int inputB)
	{
		int result = inputA - inputB;
		if(result >= 0)
			return true;

		return false;
	}

	//deprecated
	private int isInputAcceptableForDivide(int inputA, int inputB)
	{
		if(inputA > inputB)
		{
			int result = inputA % inputB;
			if(result == 0)
				return 1;//yes
		}
		else if(inputA < inputB)
		{
			int result = inputB % inputA;
			if(result == 0)
				return 2;//yes but switch for result calculation
		}

		return 0;
	}

	//deprecated
	private int applyOperand(MathBall.eOperator operand, int inputA, int inputB)
	{
		int resultC = 0;
		switch (operand) 
		{
		case MathBall.eOperator.Plus:
			resultC = inputA + inputB;
			break;
		case MathBall.eOperator.Minus:
			resultC = inputA - inputB;
			break;
		case MathBall.eOperator.Multiply:
			resultC = inputA * inputB;
			break;
		case MathBall.eOperator.Divide:
			resultC = inputA / inputB;
			break;
		case MathBall.eOperator.Power:
			resultC = inputA ^ inputB;
			break;
		case MathBall.eOperator.Modulus:
			resultC = inputA % inputB;
			break;
		}

		return resultC;
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
