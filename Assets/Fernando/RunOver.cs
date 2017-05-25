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

	private bool mStartRunningOver = false;

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
			mStopTimer = 0;
			break;
			case States.FINISH:
				mStartRunningOver = false;
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
					if(mCurrentDestinationIndex >= Destinations.childCount - 3)
					{
					mStartRunningOver = true;
//						mCurrentDestinationIndex = 0;
					}

					if (mCurrentDestinationIndex >= Destinations.childCount) {
						ChangeState (States.FINISH);
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
			ScenesManager.Instance.GoToLoadPath ();
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
				
				if (mStartRunningOver) {
					Debug.Log ("running over!");
					if (pCollision.transform.CompareTag ("NPC")) {
						
						pCollision.transform.GetComponent<RagdollController1> ().TurnOnRagdoll (true, transform.position);
//						pCollision.transform.GetComponent<RagdollController1> ().ToggleRagdoll ();
					}
				} else {
					Debug.Log ("stopping from RunOver");
					ChangeState (States.STOP);
				}
			}
		}

	}

	//if this is a vehicle and it was colliding with any Vehicle or NPC, resume moving now
	void OnCollisionExit(Collision pCollision)
	{
		if (!mStartRunningOver) {
			if (transform.CompareTag ("Vehicle")) {
				if (pCollision.transform.CompareTag ("Vehicle") || pCollision.transform.CompareTag ("NPC")) {
//					ChangeState (States.RIDING);
				}
			}
		}
	}
	//end Fernando script
	
	void RotateTowardsDest(Vector3 dest){
		Vector3 targetDir = Vector3.zero;
	
		targetDir = dest;

		Quaternion rotation = Quaternion.LookRotation(transform.position - targetDir);
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