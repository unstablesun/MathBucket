using UnityEngine;
using System.Collections;

public class ParticleUnitTest : MonoBehaviour 
{
	public GameObject _emitter1 = null;

	private float _elaspedTime = 0.0f;
	private float _emmiterTime = 1.0f;

	void Start () 
	{
	
	}
	
	void Update () 
	{
		_elaspedTime += Time.deltaTime;
		
		if(_elaspedTime > _emmiterTime)
		{
			if(_emitter1 != null)
			{
				//if(_emitter1.GetComponent<ParticleSystem>().isPlaying == false)
				//{
					float xpos = (float)UnityEngine.Random.Range(-4, 4);
					float ypos = (float)UnityEngine.Random.Range(-4, 4);

					Vector3 temp = new Vector3(xpos, ypos, -1);
					_emitter1.transform.position = temp;
					_emitter1.GetComponent<ParticleSystem>().Play();
				//}
			}


			_elaspedTime = 0.0f;
		}

	}
}
