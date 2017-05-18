using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunOver : MonoBehaviour
{
	public List<Transform> Destinations;
	
	public float Speed = 3;

	public float CurrentTurnSpeed = 1.5f;
	
	private int mCurrentDestinationIndex;

	public enum States
	{
		RIDING		=		0,
		STOP		=		1
	}
	
	private States mCurrentState;
	
	private void ChangeState(States pNewState)
	{
		switch(pNewState)
		{
			case States.RIDING:
			default:
				mCurrentDestinationIndex = 0;
			break;
			case States.STOP:
				
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
				transform.position = Vector3.MoveTowards(transform.position, Destinations[mCurrentDestinationIndex].position, Speed * Time.deltaTime);
				RotateTowardsDest(Destinations[mCurrentDestinationIndex].position);
				if(Vector3.Distance(transform.position, Destinations[mCurrentDestinationIndex].position) < Speed * Time.deltaTime)
				{
					++mCurrentDestinationIndex;
					if(mCurrentDestinationIndex >= Destinations.Count)
					{
						mCurrentDestinationIndex = 0;
					}
				}
			break;
			case States.STOP:
				
			break;
		}
	}

	void OnCollisionEnter(Collision pCollision)
	{
		if(pCollision.transform.CompareTag("AccidentNPC"))
		{
			ChangeState(States.STOP);
		}
	}
	
	void RotateTowardsDest(Vector3 dest){
		Vector3 targetDir = Vector3.zero;
	
		targetDir = dest;

		Quaternion rotation = Quaternion.LookRotation(targetDir - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * CurrentTurnSpeed);						        

	}
}