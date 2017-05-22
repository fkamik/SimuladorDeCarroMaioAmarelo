using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	
	/// <summary>
	/// Cars that will be spawned with the camera. Only one of these will be spawned at a time, 
	/// chosen before the transition to the game scene
	/// </summary>
	public List<GameObject> PlayerCars;

	private Transform mCurrentCar;

	public enum GameStates
	{
		DRIVING		=		0,
		ACCIDENT	=		1
	}

	private GameStates mCurrentState;

	private void ChangeState(GameStates pNewState)
	{
		switch (pNewState)
		{
			case GameStates.DRIVING:
				default:
				TrafficManager.Instance.Initialize ();
				break;
			case GameStates.ACCIDENT:
				break;
		}

		mCurrentState = pNewState;
	}

	private static GameManager sInstance = null;
	public static GameManager Instance
	{
		get{return sInstance;}
		private set{}
	}
	
	void Awake()
	{
		if(sInstance == null)
		{
			sInstance = GetComponent<GameManager>();
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}


	public void Initialize(int pPlayerCarIndex)
	{
		mCurrentCar = Instantiate(PlayerCars[pPlayerCarIndex]).transform;
		ChangeState(GameStates.DRIVING);
	}
}