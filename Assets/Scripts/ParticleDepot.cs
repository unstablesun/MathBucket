using UnityEngine;
using System.Collections;

public class ParticleDepot : MonoBehaviour 
{
	
	public GameObject _emitter0 = null;
	public GameObject _emitter1 = null;
	public GameObject _emitter2 = null;
	public GameObject _emitter3 = null;
	public GameObject _emitter4 = null;
	public GameObject _emitter5 = null;
	public GameObject _emitter6 = null;
	public GameObject _emitter7 = null;
	public GameObject _emitter8 = null;
	public GameObject _emitter9 = null;

	public GameObject _emitterBonus = null;

	public static ParticleDepot Instance;

	void Awake () 
	{
		Instance = this;

	}


	public void PlayAtPosition(int ballValue, Vector3 epos)
	{

		switch(ballValue)
		{
		case 1:
			if(_emitter1 != null)
			{
				_emitter1.transform.position = epos;
				_emitter1.GetComponent<ParticleSystem>().Play();
			}
			break;
		case 2:
			if(_emitter2 != null)
			{
				_emitter2.transform.position = epos;
				_emitter2.GetComponent<ParticleSystem>().Play();
			}
			break;
		case 3:
			if(_emitter3 != null)
			{
				_emitter3.transform.position = epos;
				_emitter3.GetComponent<ParticleSystem>().Play();
			}
			break;
		case 4:
			if(_emitter4 != null)
			{
				_emitter4.transform.position = epos;
				_emitter4.GetComponent<ParticleSystem>().Play();
			}
			break;
		case 5:
			if(_emitter5 != null)
			{
				_emitter5.transform.position = epos;
				_emitter5.GetComponent<ParticleSystem>().Play();
			}
			break;
		case 6:
			if(_emitter6 != null)
			{
				_emitter6.transform.position = epos;
				_emitter6.GetComponent<ParticleSystem>().Play();
			}
			break;
		case 7:
			if(_emitter7 != null)
			{
				_emitter7.transform.position = epos;
				_emitter7.GetComponent<ParticleSystem>().Play();
			}
			break;
		case 8:
			if(_emitter8 != null)
			{
				_emitter8.transform.position = epos;
				_emitter8.GetComponent<ParticleSystem>().Play();
			}
			break;
		case 9:
			if(_emitter9 != null)
			{
				_emitter9.transform.position = epos;
				_emitter9.GetComponent<ParticleSystem>().Play();
			}
			break;
		case 0:
			if(_emitter0 != null)
			{
				_emitter0.transform.position = epos;
				_emitter0.GetComponent<ParticleSystem>().Play();
			}
			break;
		}
		
	}

	public void PlayBonus()
	{
		if(_emitterBonus != null)
		{
			_emitterBonus.GetComponent<ParticleSystem>().Play();
		}

	}


}
