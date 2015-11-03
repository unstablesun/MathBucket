using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionPopupController : MonoBehaviour {


	[SerializeField]
	public Text score;

	[SerializeField]
	public Text title;

	// Use this for initialization
	void Awake () {

		title.text = "Math Challenge";
		
		score.text = "123";
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
