using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BallManager : MonoBehaviour 
{
	public enum eSystemState 
	{
		noop,
		reboot,
		active,
		done,
	};
	private eSystemState _systemState = eSystemState.noop;

	public int NumPreloaded = 32;
	public int NumAnimsPreloaded = 32;

	public static List <GameObject> BallObjects = new List<GameObject>();
	public static List <GameObject> BallAnimObjects = new List<GameObject>();


	private float _elaspedTime = 0.0f;
	private float _emmiterTime = 0.01f;

	private int _score = 0;


	public  Color ballColorBackDigit;
	public  float ballScaleDigit = 0.3f;

	public  Color ballColorBackOperand;
	public  float ballScaleOperand = 0.25f;

	public  Color ballColorBackEquals;
	public  float ballScaleEquals = 0.2f;

	public  Color ballColorHilite;	



	public static BallManager Instance;

	void Awake () 
	{
		Instance = this;

	}




	public void SetState(eSystemState state)
	{
		_systemState = state;
	}

	void Start () 
	{
		//default colors & scales
		//ballColorBackDigit = new Color(0.9f, 0.9f, 0.9f, 1f);
		//ballColorBackOperand = new Color(0.5f, 0.5f, 0.5f, 1f);
		//ballColorBackEquals = new Color(0.3f, 0.3f, 0.3f, 1f);
		//ballColorHilite = new Color(0.9f, 0.9f, 0.9f, 1f);

		//clear
		BallObjects.Clear ();
		BallAnimObjects.Clear ();

		PreLoadBallPrefabs();
		PreLoadBallAnimPrefabs();

		//debug
		GetNumBallsInBucket();

	}
	
	void Update () 
	{
		switch (_systemState)
		{
			case eSystemState.noop:
			break;

			case eSystemState.reboot:
				_score = 0;
				_systemState = eSystemState.active;
			break;

			case eSystemState.active:
			{
				_elaspedTime += Time.deltaTime;
				
				if(_elaspedTime > _emmiterTime)
				{
					//insert ball into bucket
					AddBallToBucket();
					
					_elaspedTime = 0.0f;
				}
				
				
				
				if (Input.GetMouseButtonDown (0)) 
				{
					//Debug.Log ("Clicked");
					Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
					RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
					// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
					if(hitInfo)
					{
						//Debug.Log( hitInfo.transform.gameObject.name );
						
						GameObject _gObj = hitInfo.transform.gameObject;
						MathBall _mathBallScript = _gObj.GetComponent<MathBall> ();
						if(_mathBallScript.setBallSelected() == false)
						{
							ClearSelectedBalls();
							//CentralCalculator _centralCalculator = GameCommon.getCentralCalculatorClass();
							CentralCalculator.Instance.ResetCalcTokenList();
							
							//is ball already selected?
							//clear with zonk
							//GameCommon.getAudioDepotClass().PlaySfx(AudioDepot.eSfxID.matchReset);
							AudioDepot.Instance.PlaySfx(AudioDepot.eSfxID.matchReset);
						}
						else
						{
							//value & function
							MathBall.eFunction function = _mathBallScript._function;
							int value = _mathBallScript.ball_value;
							
							//CentralCalculator _centralCalculator = GameCommon.getCentralCalculatorClass();
							
							CentralCalculator.Instance.AddCalcToken(value, function);
							
							if(CentralCalculator.Instance.AnalyseCalcTokenList() == true)
							{
								//success
								
								//get score
								_score += CentralCalculator.Instance.calcScore;
								
								//clear selected balls
								Debug.Log ("SUCCESSFULL CALCULATION!!");
								RemoveSelectedBalls();
								CentralCalculator.Instance.ResetCalcTokenList();

								if(GetNumBallsInBucket() == 0)
								{
									//GameCommon.getGameplayManagerClass().PuzzleCompete(_score);
									GameplayManager.Instance.PuzzleCompete(_score);
									//GameCommon.getAudioDepotClass().PlaySfx(AudioDepot.eSfxID.puzzleDone);
									AudioDepot.Instance.PlaySfx(AudioDepot.eSfxID.puzzleDone);

									//GameCommon.getParticleDepotClass().PlayBonus();
									ParticleDepot.Instance.PlayBonus ();
								}
								else
								{
									//GameCommon.getAudioDepotClass().PlaySfx(AudioDepot.eSfxID.matchMade);
									AudioDepot.Instance.PlaySfx(AudioDepot.eSfxID.matchMade);

								}
							}
							else if(CentralCalculator.Instance.ErrorReport() > 0)
							{
								//clear with zonk
								Debug.Log ("ERROR ... ERROR ... ERROR!!");
								ClearSelectedBalls();
								CentralCalculator.Instance.ResetCalcTokenList();
								
							}
						}
					}
				}
			}
			break;


			case eSystemState.done:
			break;

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
		int spanx = UnityEngine.Random.Range(-1, 1);
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



		//FormulaFactory _formulaFactory = GameCommon.getFormulaFactoryClass();

		//FormulaFactory.Instance.getNextBallValue()

		if(FormulaFactory.Instance.getNextBallValue() == true)
		{

			MathBall _mathBallScript = mathBall.GetComponent<MathBall> ();
			_mathBallScript._state = MathBall.eState.InBucket;

			//debug
			GetNumBallsInBucket();

			MathBall.eFunction _ballFunction = FormulaFactory.Instance.ball_function;
			int _ballValue = FormulaFactory.Instance.ball_value;

			setBallFunctionAndValue(mathBall, _ballFunction, _ballValue);


			if(_ballFunction == MathBall.eFunction.Digit)
			{
				setBallColor(mathBall, ballColorBackDigit);
				setBallScale(mathBall, ballScaleDigit);

				setBallHiliteColor(mathBall, ballColorBackDigit);
			}
			else if(_ballFunction == MathBall.eFunction.Operand)
			{
				if(_ballValue == (int)MathBall.eOperator.Equals)
				{
					setBallColor(mathBall, ballColorBackEquals);
					setBallScale(mathBall, ballScaleEquals);
					setBallHiliteColor(mathBall, ballColorBackEquals);
				}
				else
				{
					setBallColor(mathBall, ballColorBackOperand);
					setBallScale(mathBall, ballScaleOperand);
					setBallHiliteColor(mathBall, ballColorBackOperand);
				}
			}


			mathBall.SetActive(true);
			setAllChildrenActive(mathBall, true);
		}
	}


	private int GetNumBallsInBucket() 
	{
		int count = 0;
		foreach(GameObject gObj in BallObjects)
		{
			MathBall _mathBallScript = gObj.GetComponent<MathBall> ();
			if(_mathBallScript._state == MathBall.eState.InBucket)
			{
				count++;
			}
		}
		//Debug.Log ("GetNumBallsInBucket = " + count);
		return count;
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
				_mathBallAnim.transform.position =  new Vector3(gObj.transform.position.x, gObj.transform.position.y, -4);
				_mathBallAnim.transform.localScale = gObj.transform.localScale;

				int ball_value = _mathBallScript.ball_value;

				//use ball_value to select correct particle
				ParticleDepot.Instance.PlayAtPosition(ball_value, new Vector3(gObj.transform.position.x, gObj.transform.position.y, -5));
				MathBallAnim _mathBallAnimScript = _mathBallAnim.GetComponent<MathBallAnim> ();
				_mathBallAnimScript.setBallText(ball_value.ToString());
				_mathBallAnimScript.startAnim();

				_mathBallAnim.SetActive(true);
				setAllChildrenActive(_mathBallAnim, true);

				_mathBallAnimScript.setBallRemovalColors();


				//remove from bucket
				int spanx = UnityEngine.Random.Range(-1, 1);
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

	private void setBallHiliteColor(GameObject _gObj, Color _c)
	{
		MathBall _mathBallScript = _gObj.GetComponent<MathBall> ();
		_mathBallScript.setBallHiliteColor(_c);
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
		_mathBallScript.setBallTextColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
		
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
