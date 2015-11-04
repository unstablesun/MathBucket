using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FUEL.SDK;

public class MissionPopupController : MonoBehaviour {


	[SerializeField]
	public Text score;

	[SerializeField]
	public Text title;

	[SerializeField]
	public Text progress;

	[SerializeField]
	public Text target;

	public static MissionData missionData;

	public static EventData igniteEventData;


	public static void PopulateIgniteEvent ( EventData eventData ) {
		igniteEventData = eventData;
	}
	public static void PopulateIgniteMisssion ( MissionData mission ) {
		missionData = mission;
	}

	// Use this for initialization
	void Awake () {

		title.text = missionData.Metadata.Name;
		
		score.text = igniteEventData.Score.ToString();

		progress.text = missionData.Progress.ToString ();

		//target.text = missionData.Rules.
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void InitMission( MissionData missionData ) {
		Debug.Log ("MainIgniteUIPopupController. Init Mission");
		if (string.IsNullOrEmpty (missionData.Id)) {
			return;
		}

	}

}
