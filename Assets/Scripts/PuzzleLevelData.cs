using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;


[System.Serializable]
public class PuzzleLevelData
{
	[SerializeField]
	private int difficulty;

	[SerializeField]
	private int minRange;
	[SerializeField]
	private int maxRange;

	[SerializeField]
	private FormulaFactory.eOperandBias operatorPreference; //do we want all plus
	[SerializeField]
	private int operatorPreferenceProb; //probability of preferred operator
	[SerializeField]
	private int sameOperatorProb; //for 2 or more equations, probability they will be the same

	[SerializeField]
	private int numEquations;

	[SerializeField]
	private int repeats;


	public int Difficulty
	{
		get { return difficulty; }
		set { difficulty = value; }
	}

	public int MinRange
	{
		get { return minRange; }
		set { minRange = value; }
	}

	public int MaxRange
	{
		get { return maxRange; }
		set { maxRange = value; }
	}

	public FormulaFactory.eOperandBias OperatorPreference
	{
		get { return operatorPreference; }
		set { operatorPreference = value; }
	}

	public int OperatorPreferenceProb
	{
		get { return operatorPreferenceProb; }
		set { operatorPreferenceProb = value; }
	}

	public int SameOperatorProb
	{
		get { return sameOperatorProb; }
		set { sameOperatorProb = value; }
	}

	public int NumEquations
	{
		get { return numEquations; }
		set { numEquations = value; }
	}

	public int Repeats
	{
		get { return repeats; }
		set { repeats = value; }
	}

}
