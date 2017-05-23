using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	
	public GameObject PlayerCarPrefab;

	public Transform PlayerDestinations;

	private Transform mCurrentCar = null;

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
				mCurrentCar.GetComponent<RunOver> ().Resume ();
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
		Debug.Log ("Initializing");

		StartCoroutine (LateLoad(pPlayerCarIndex));


	}

	private IEnumerator LateLoad(int pPlayerCarIndex)
	{
		yield return new WaitForEndOfFrame ();

		if (mCurrentCar != null) {
			Destroy (mCurrentCar.gameObject);
		}
		mCurrentCar = Instantiate(PlayerCarPrefab).transform;

		PlayerDestinations = GameObject.Find ("Waypoints Jogador").transform;



		mCurrentCar.GetComponent<RunOver> ().Initialize(PlayerDestinations.GetChild (pPlayerCarIndex)); 
		ChangeState(GameStates.DRIVING);
	}
}