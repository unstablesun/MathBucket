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

	private void AddEquationForm(eEquationType eqForm, int minRange, int maxRange, int operandBias)
	{
		switch( eqForm )
		{
			case eEquationType.compexity1://a+b=c, a*b=c, etc...
			{

				int inputA = UnityEngine.Random.Range(minRange, maxRange);


				//NOTE *!* defer these steps unit inputA, inputB & resultC have been worked out see comments below
				int[] digits = getDigits(inputA);
				int num = digits.GetLength(0);
				for(int i = 0; i < num; i++)
				{
					AddFormulaToken (digits[i], MathBall.eFunction.Digit);
				}



				int inputB = UnityEngine.Random.Range(minRange, maxRange);

				//get random operand
				//is input acceptible for minus or divide

				//for division do a mult first then reverse example 4 * 5 = 20 : 20 / 4 = 5 or 20 / 5 = 4.
				//more ex. 3 * 8 = 24 : 24 / 8 = 3.  12 * 8 = 96 : 96 / 12 = 8 or 96 / 8 = 12.

				//for subtraction do an add first then reverse example 8+12=20 : 20-12=8 or 20-8=12 or even 12=20-8


				//MathBall.eOperator operand = getRandomOperand();

				AddFormulaToken ((int)MathBall.eOperator.Plus,  MathBall.eFunction.Operand);
				AddFormulaToken (2, MathBall.eFunction.Digit);

				AddFormulaToken ((int)MathBall.eOperator.Equals,  MathBall.eFunction.Operand);



				int resultC = applyOperand(MathBall.eOperator.Plus, inputA, inputB);

				int[] resultDigits = getDigits(inputA);
				num = resultDigits.GetLength(0);
				for(int i = 0; i < num; i++)
				{
					AddFormulaToken (resultDigits[i], MathBall.eFunction.Digit);
				}

			}
			break;

			case eEquationType.compexity2:
			{
				int inputA = UnityEngine.Random.Range(minRange, maxRange);
				
				int[] digits = getDigits(inputA);
				int num = digits.GetLength(0);
				for(int i = 0; i < num; i++)
				{
					AddFormulaToken (digits[i], MathBall.eFunction.Digit);
				}
			}
			break;
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

		return digits;
	}

	private bool isInputAcceptableForMinus(int inputA, int inputB)
	{
		int result = inputA - inputB;
		if(result >= 0)
			return true;

		return false;
	}

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

	private MathBall.eOperator getRandomOperand(int[] fromThese)
	{
		int range = fromThese.GetLength(0);
		int randIdx =  UnityEngine.Random.Range(0, range);
		return (MathBall.eOperator)fromThese[randIdx];
	}

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
