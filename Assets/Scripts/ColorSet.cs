using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;


[System.Serializable]
public class ColorSet
{
	[SerializeField]
	private Color mColor1 = Color.white;
	[SerializeField]
	private Color mColor2 = Color.white;
	[SerializeField]
	private Color mColor3 = Color.white;
	[SerializeField]
	private Color mColor4 = Color.white;
	[SerializeField]
	private Color mColor5 = Color.white;
	[SerializeField]
	private Color mColor6 = Color.white;
	[SerializeField]
	private Color mColor7 = Color.white;
	[SerializeField]
	private Color mColor8 = Color.white;
	[SerializeField]
	private Color mColor9 = Color.white;
	[SerializeField]
	private Color mColor10 = Color.white;
	[SerializeField]
	private Color mColor11 = Color.white;
	[SerializeField]
	private Color mColor12 = Color.white;
	[SerializeField]
	private Color mColorText = Color.white;
	[SerializeField]
	private Color mColorAux1 = Color.white;
	[SerializeField]
	private Color mColorAux2 = Color.white;
	[SerializeField]
	private Color mColorAux3 = Color.white;


	[SerializeField]
	private int[] mColorCodes = null; 
	public int GetColorCode(int index)
	{
		return( mColorCodes[index] );
	}

	public Color Color1
	{
		get { return mColor1; }
		set { mColor1 = value; }
	}

	public Color Color2
	{
		get { return mColor2; }
		set { mColor2 = value; }
	}

	public Color Color3
	{
		get { return mColor3; }
		set { mColor3 = value; }
	}

	public Color Color4
	{
		get { return mColor4; }
		set { mColor4 = value; }
	}

	public Color Color5
	{
		get { return mColor5; }
		set { mColor5 = value; }
	}

	public Color Color6
	{
		get { return mColor6; }
		set { mColor6 = value; }
	}

	public Color Color7
	{
		get { return mColor7; }
		set { mColor7 = value; }
	}

	public Color Color8
	{
		get { return mColor8; }
		set { mColor8 = value; }
	}

	public Color Color9
	{
		get { return mColor9; }
		set { mColor9 = value; }
	}

	public Color Color10
	{
		get { return mColor10; }
		set { mColor10 = value; }
	}

	public Color Color11
	{
		get { return mColor11; }
		set { mColor11 = value; }
	}

	public Color Color12
	{
		get { return mColor12; }
		set { mColor12 = value; }
	}


	public Color ColorText
	{
		get { return mColorText; }
		set { mColorText = value; }
	}

	public Color ColorAux1
	{
		get { return mColorAux1; }
		set { mColorAux1 = value; }
	}

	public Color ColorAux2
	{
		get { return mColorAux2; }
		set { mColorAux2 = value; }
	}

	public Color ColorAux3
	{
		get { return mColorAux3; }
		set { mColorAux3 = value; }
	}

}
