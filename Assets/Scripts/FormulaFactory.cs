using UnityEngine;
using System.Collections;

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

	public void InitFormula(int[] values, int[]operands)
	{
	}






	/*
	public enum eFunction 
	{
		Digit,
		Number,
		Operand,
		Point
	};

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
	*/

	private void CreateEquation(int complextiy)
	{



		int operand = UnityEngine.Random.Range(0, 5);

	}


}
