using UnityEngine;
using System.Collections;

public class AudioDepot : MonoBehaviour 
{

	public AudioSource _sfxMatchMade = null;
	public AudioSource _sfxMatchReset = null;
	public AudioSource _sfxPuzzleDone = null;
	public AudioSource _sfxCollide1 = null;

	public enum eSfxID 
	{
		matchMade,
		matchReset,
		puzzleDone,
		collide1
	};


	public static AudioDepot Instance;

	void Awake () 
	{
		Instance = this;

	}


	public void PlaySfx(eSfxID sfxID)
	{
		
		switch(sfxID)
		{
		case eSfxID.matchMade:
			if(_sfxMatchMade != null)
			{
				_sfxMatchMade.Play();
			}
			break;
		case eSfxID.matchReset:
			if(_sfxMatchReset != null)
			{
				_sfxMatchReset.Play();
			}
			break;
		
		case eSfxID.puzzleDone:
			if(_sfxPuzzleDone != null)
			{
				_sfxPuzzleDone.Play();
			}
			break;
	
		case eSfxID.collide1:
			if(_sfxCollide1 != null)
			{
				_sfxCollide1.Play();
			}
			break;
		}
	}


}
