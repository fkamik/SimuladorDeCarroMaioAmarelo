using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
	public enum TrafficStates
	{
		REGULAR		=		0,
		ACCIDENT	=		1
	}
	
	private TrafficStates mCurrentState;
	
	private Vector2 cDefaultOpenClosedTimes = new Vector2(8, 6);
	
	public List<StopStation> Traffic;
	
	private List<float> mTimers;
	
	/// <summary>
	/// Times in seconds that each stop sign will remain open(x)/closed(y)
	/// </summary>
	public List<Vector2> OpenClosedTimes;

	private static TrafficManager sInstance = null;
	
	public static TrafficManager Instance
	{
		get{return sInstance;}
		private set{}
	}
	
	void Awake()
	{
		if(sInstance == null)
		{
			sInstance = GetComponent<TrafficManager>();
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	private void ChangeState(TrafficStates pNewState)
	{
		switch(pNewState)
		{
			case TrafficStates.REGULAR:
			default:

				int openClosedTimesCount = OpenClosedTimes.Count;

				for (int i = 0; i < Traffic.Count - openClosedTimesCount; ++i)
				{
					OpenClosedTimes.Add (cDefaultOpenClosedTimes);
				}


				mTimers = new List<float>();
				for(int i = 0; i < Traffic.Count; ++i)
				{
					mTimers.Add(0);
				}
			break;
			case TrafficStates.ACCIDENT:
				
			break;
		}
		
		mCurrentState = pNewState;
	}

	void Start()
	{
		Initialize ();
	}
	
	void Update()
	{
		switch(mCurrentState)
		{
			case TrafficStates.REGULAR:
				default:
				for(int i = 0; i < Traffic.Count; ++i)
				{
					mTimers[i] += Time.deltaTime;
					if(Traffic[i].AreCarsMoving())
					{
						if(mTimers[i] > OpenClosedTimes[i].x)
						{
							mTimers[i] = 0;
							Traffic[i].Walk();
						}
					}
					else
					{
						if(mTimers[i] > OpenClosedTimes[i].y)
						{
							mTimers[i] = 0;
							Traffic[i].Stop();
						}
					}
				}
			break;
			case TrafficStates.ACCIDENT:
			break;
		}
	}
	
	public void Initialize()
	{
		ChangeState(TrafficStates.REGULAR);
	}
	
	public void Pause()
	{
		ChangeState(TrafficStates.ACCIDENT);
	}
}