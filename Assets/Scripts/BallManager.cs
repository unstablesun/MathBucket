using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BallManager : MonoBehaviour 
{


	public int NumPreloaded = 32;
	public int NumAnimsPreloaded = 32;

	public static List <GameObject> BallObjects = new List<GameObject>();
	public static List <GameObject> BallAnimObjects = new List<GameObject>();


	private float _elaspedTime = 0.0f;
	private float _emmiterTime = 0.05f;

	void Start () 
	{
	
		GameCommon.InitDefaults();
		PreLoadBallPrefabs();
		PreLoadBallAnimPrefabs();
	}
	
	void Update () 
	{


		_elaspedTime += Time.deltaTime;
		
		if(_elaspedTime > _emmiterTime)
		{
			//insert ball into bucket
			AddBallToBucket();

			_elaspedTime = 0.0f;
		}

	

		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Clicked");
			Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
			// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
			if(hitInfo)
			{
				Debug.Log( hitInfo.transform.gameObject.name );

				GameObject _gObj = hitInfo.transform.gameObject;
				MathBall _mathBallScript = _gObj.GetComponent<MathBall> ();
				if(_mathBallScript.setBallSelected() == false)
				{
					ClearSelectedBalls();
					CentralCalculator _centralCalculator = GameCommon.getCentralCalculatorClass();
					_centralCalculator.ResetCalcTokenList();

					//is ball already selected?
					//clear with zonk
				}
				else
				{
					//value & function
					MathBall.eFunction function = _mathBallScript._function;
					int value = _mathBallScript.ball_value;

					CentralCalculator _centralCalculator = GameCommon.getCentralCalculatorClass();

					_centralCalculator.AddCalcToken(value, function);

					if(_centralCalculator.AnalyseCalcTokenList() == true)
					{
						//success

						//get score

						//clear selected balls
						Debug.Log ("SUCCESSFULL CALCULATION!!");
						RemoveSelectedBalls();
						_centralCalculator.ResetCalcTokenList();
					}
					else if(_centralCalculator.ErrorReport() > 0)
					{
						//clear with zonk
						Debug.Log ("ERROR ... ERROR ... ERROR!!");
						ClearSelectedBalls();
						_centralCalculator.ResetCalcTokenList();

					}
				}
			}
		}
		

	}


	//------------------------------------------------------------------
	/*
										BALLS
	*/
	//------------------------------------------------------------------
	void PreLoadBallPrefabs() 
	{
		for(int i = 1; i < NumPreloaded; i++)
		{
			LoadBallPrefab("MathButton",  i++);
		}
	}

	void LoadBallPrefab(string prefabName, int index) 
	{
		//Debug.Log ("creating ball " + " " + index);

		GameObject mathBall = Instantiate(Resources.Load(prefabName, typeof(GameObject))) as GameObject;
		
		mathBall.name = "mathBall" + index;
		int spanx = UnityEngine.Random.Range(-2, 2);
		mathBall.transform.position = new Vector3((float)spanx, 20f, 0f);
		
		MathBall _mathBallScript = mathBall.GetComponent<MathBall> ();
		_mathBallScript.ball_index = index;
		_mathBallScript._state = MathBall.eState.InPool;

		mathBall.SetActive(false);
		setAllChildrenActive(mathBall, false);
		
		BallObjects.Add(mathBall);


		setBallColor(mathBall, new Color(1f, 1f,1f, 1f));
		
		//WARNING! when you load a prefab at this point "void Start()" is still to be called and may overwrite anything you set here.
	}



	void AddBallToBucket() 
	{
		GameObject mathBall = GetAvailableBall();

		if(mathBall == null)
			return;


		MathBall _mathBallScript = mathBall.GetComponent<MathBall> ();
		_mathBallScript._state = MathBall.eState.InBucket;


		FormulaFactory _formulaFactory = GameCommon.getFormulaFactoryClass();
		_formulaFactory.setNextBallValues();
		MathBall.eFunction _ballFunction = _formulaFactory.ball_function;
		int _ballValue = _formulaFactory.ball_value;

		setBallFunctionAndValue(mathBall, _ballFunction, _ballValue);


		if(_ballFunction == MathBall.eFunction.Digit)
		{
			setBallColor(mathBall, GameCommon.ballColorBackDigit);
			setBallScale(mathBall, GameCommon.ballScaleDigit);
		}
		else if(_ballFunction == MathBall.eFunction.Operand)
		{
			if(_ballValue == (int)MathBall.eOperator.Equals)
			{
				setBallColor(mathBall, GameCommon.ballColorBackEquals);
				setBallScale(mathBall, GameCommon.ballScaleEquals);
			}
			else
			{
				setBallColor(mathBall, GameCommon.ballColorBackOperand);
				setBallScale(mathBall, GameCommon.ballScaleOperand);
			}
		}


		mathBall.SetActive(true);
		setAllChildrenActive(mathBall, true);
	}



	GameObject GetAvailableBall() 
	{
		foreach(GameObject gObj in BallObjects)
		{
			MathBall _mathBallScript = gObj.GetComponent<MathBall> ();
			if(_mathBallScript._state == MathBall.eState.InPool)
			{
				return gObj;
				
			}
		}
		return null;
	}
	
	void ClearSelectedBalls() 
	{
		foreach(GameObject gObj in BallObjects)
		{
			MathBall _mathBallScript = gObj.GetComponent<MathBall> ();
			if(_mathBallScript._state == MathBall.eState.Selected)
			{
				_mathBallScript.clearSelected();
			}
		}
	}

	void RemoveSelectedBalls() 
	{
		foreach(GameObject gObj in BallObjects)
		{
			MathBall _mathBallScript = gObj.GetComponent<MathBall> ();
			if(_mathBallScript._state == MathBall.eState.Selected)
			{
				//setup anim ball
				GameObject _mathBallAnim = GetAvailableBallAnim();
				_mathBallAnim.transform.position =  new Vector3(gObj.transform.position.x, gObj.transform.position.y, gObj.transform.position.z);
				_mathBallAnim.transform.localScale = gObj.transform.localScale;

				MathBallAnim _mathBallAnimScript = _mathBallAnim.GetComponent<MathBallAnim> ();
				_mathBallAnimScript.startAnim();

				_mathBallAnim.SetActive(true);
				setAllChildrenActive(_mathBallAnim, true);


				//remove from bucket
				int spanx = UnityEngine.Random.Range(-5, 5);
				gObj.transform.position = new Vector3((float)spanx, 20f, 0f);
				gObj.SetActive(false);
				setAllChildrenActive(gObj, false);
				_mathBallScript.removeSelected();
			}
		}
	}

	//------------------------------------------------------------------
	/*
								COMMON
	*/
	//------------------------------------------------------------------
	
	private GameObject getChildGameObject(GameObject fromGameObject, string withName) 
	{
		foreach (Transform child in fromGameObject.transform)
		{
			if (child.gameObject.name == withName)
				return child.gameObject;
		}
		return null;
	}
	
	private void setAllChildrenActive(GameObject forGameObject, bool state) 
	{
		foreach (Transform child in forGameObject.transform)
		{
			child.gameObject.SetActive(state);
		}
	}


	private void setBallFunctionAndValue(GameObject _gObj, MathBall.eFunction _f, int _v)
	{
		MathBall _mathBallScript = _gObj.GetComponent<MathBall> ();
		_mathBallScript.SetFunctionAndValue(_f, _v);
	}



	private void setBallText(GameObject _gObj, string t)
	{
		MathBall _mathBallScript = _gObj.GetComponent<MathBall> ();
		_mathBallScript.setBallText(t);
	}
	private void setBallTextColor(GameObject _gObj, Color _c)
	{
		GameObject _ballText = getChildGameObject(_gObj, "text1");
		_ballText.GetComponent<Renderer>().material.color = _c;
	}
	
	private void setBallColor(GameObject _gObj, Color _c)
	{
		GameObject image = getChildGameObject(_gObj, "image1");
		image.GetComponent<Renderer>().material.color = _c;
	}
	
	void setBallScale(GameObject gObj, float _scale) 
	{
		MathBall _mathBallScript = gObj.GetComponent<MathBall> ();
		_mathBallScript.SetBallScale(_scale);
	}



	//deprecated
	private void setRandomPrimaryColor(GameObject _gObj)
	{
		float r = 1, g = 1, b = 1;
		float dim = 0.6f;
		int rn = UnityEngine.Random.Range(0, 6);
		if(rn == 0){r = dim;}
		else if(rn == 1){g = dim;}
		else if(rn == 2){b = dim;}
		else if(rn == 3){r = dim;g = dim;}
		else if(rn == 4){g = dim;b = dim;}
		else if(rn == 5){b = dim;r = dim;}
		GameObject image = getChildGameObject(_gObj, "image1");
		image.GetComponent<Renderer>().material.color = new Color(r, g, b, 1f);
		
	}
	





	//------------------------------------------------------------------
	/*
									ANIMATIONS
	*/
	//------------------------------------------------------------------

	void PreLoadBallAnimPrefabs() 
	{
		for(int i = 1; i < NumAnimsPreloaded; i++)
		{
			LoadBallAnimPrefab("MathBallAnim",  i++);
		}
	}
	
	void LoadBallAnimPrefab(string prefabName, int index) 
	{
		//Debug.Log ("creating ball " + " " + index);
		
		GameObject mathBallAnim = Instantiate(Resources.Load(prefabName, typeof(GameObject))) as GameObject;
		
		mathBallAnim.name = "mathBallAnim" + index;
		mathBallAnim.transform.position = new Vector3(0f, 20f, 0f);
		
		MathBallAnim _mathBallScript = mathBallAnim.GetComponent<MathBallAnim> ();
		_mathBallScript._state = MathBallAnim.eState.InPool;
		
		mathBallAnim.SetActive(false);
		setAllChildrenActive(mathBallAnim, false);
		
		BallAnimObjects.Add(mathBallAnim);
		
		
		//setBallColor(mathBallAnim, new Color(0.5f, 1f,1f, 0.5f));

		_mathBallScript.setBallColor(new Color(0.5f, 0.5f, 0.5f, 0.5f));
		_mathBallScript.setBallTextColor(new Color(0.5f, 0.0f, 0.5f, 0.5f));
		
		//WARNING! when you load a prefab at this point "void Start()" is still to be called and may overwrite anything you set here.
	}

	GameObject GetAvailableBallAnim() 
	{
		foreach(GameObject gObj in BallAnimObjects)
		{
			MathBallAnim _mathBallAnimScript = gObj.GetComponent<MathBallAnim> ();
			if(_mathBallAnimScript._state == MathBallAnim.eState.InPool)
			{
				return gObj;
				
			}
		}
		return null;
	}

}
