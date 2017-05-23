using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunOver : MonoBehaviour
{
	public Transform Destinations;
	
	public float Speed = 3;

	public float CurrentTurnSpeed = 1.5f;
	
	public int mCurrentDestinationIndex = 0;

	private float mStopTimer;

	private const float cMaxStopTime = 3;

	public enum States
	{
		RIDING		=		0,
		STOP		=		1,
		FINISH		=		2
	}
	
	private States mCurrentState;
	
	private void ChangeState(States pNewState)
	{
		switch(pNewState)
		{
			case States.RIDING:
			default:
				
			break;
			case States.STOP:
				
			break;
			case States.FINISH:
			break;
		}
		
		mCurrentState = pNewState;
	}
	
	void Update()
	{
		switch(mCurrentState)
		{
			case States.RIDING:
			default:
				transform.position = Vector3.MoveTowards(transform.position, Destinations.GetChild(mCurrentDestinationIndex).position, Speed * Time.deltaTime);
				RotateTowardsDest(Destinations.GetChild(mCurrentDestinationIndex).position);
			if(Vector3.Distance(transform.position, Destinations.GetChild(mCurrentDestinationIndex).position) < Speed)
				{
					++mCurrentDestinationIndex;
					if(mCurrentDestinationIndex >= Destinations.childCount)
					{
						mCurrentDestinationIndex = 0;
					}
				}
			break;
			case States.STOP:
			mStopTimer += Time.deltaTime;
			if (mStopTimer > cMaxStopTime)
			{
				mStopTimer = 0;
				ChangeState (States.RIDING);
			}
			break;
			case States.FINISH:
			break;
		}
	}


	//Fernando script
	//if this is a vehicle and it collides with any Vehicle or NPC, 
	//do not allow it to move(pedestrians should always have priority)
	void OnCollisionEnter(Collision pCollision)
	{
		if (transform.CompareTag ("Vehicle"))
		{
			if (pCollision.transform.CompareTag ("Vehicle") || pCollision.transform.CompareTag ("NPC"))
			{
				ChangeState (States.STOP);
			}
		}
		else if(pCollision.transform.CompareTag("AccidentNPC"))
		{
			ChangeState(States.FINISH);
		}
	}

	//if this is a vehicle and it was colliding with any Vehicle or NPC, resume moving now
	void OnCollisionExit(Collision pCollision)
	{
		if (transform.CompareTag ("Vehicle"))
		{
			if (pCollision.transform.CompareTag ("Vehicle") || pCollision.transform.CompareTag ("NPC"))
			{
				ChangeState (States.RIDING);
			}
		}
	}
	//end Fernando script
	
	void RotateTowardsDest(Vector3 dest){
		Vector3 targetDir = Vector3.zero;
	
		targetDir = dest;

		Quaternion rotation = Quaternion.LookRotation(targetDir - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * CurrentTurnSpeed);						        

	}

	public void Stop()
	{
		ChangeState (States.STOP);
	}

	public void Resume()
	{
		ChangeState (States.RIDING);
	}

	public void Initialize(Transform pDestination, int pChildIndex = 0)
	{
		Destinations = pDestination;
		transform.position = pDestination.GetChild (pChildIndex).position;
	}
}