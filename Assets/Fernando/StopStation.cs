using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stop station script. It should make the NPC move/stop accordingly
/// </summary>
public class StopStation : MonoBehaviour 
{

//	private SphereCollider mCollider = null;
//
//	private const float cOriginalColliderRadius = 2;
//	private const float cRadiusIncreaseSpeed = 2f;

	private List<Collider> mColliders;

	private int mCounter;

	/// <summary>
	/// States are named always referencing the pedestrians
	/// </summary>
	public enum States
	{
		STOP,
		WALK
	}

	private States mCurrentState;

	private void ChangeState(States pNewState)
	{
//		mCollider = mCollider ?? GetComponent<SphereCollider> ();

		switch (pNewState)
		{
			case States.STOP:
			default:
//				mCollider.radius = cOriginalColliderRadius;
				break;
			case States.WALK:
//				mCollider.radius = cOriginalColliderRadius;
				mCounter = 0;
				break;
		}

		mCurrentState = pNewState;
	}

	// Use this for initialization
	void Start () {
		mColliders = new List<Collider> ();
	}
	
	// Update is called once per frame
	void Update () {
		switch (mCurrentState)
		{
			case States.WALK:
//				if (mCounter < 2)
//				{
//
//					if (mCollider.radius < 1.25f * cOriginalColliderRadius)
//					{
//						mCollider.radius += cRadiusIncreaseSpeed * Time.deltaTime;
//					} else
//					{
//						++mCounter;
//						mCollider.radius = 0.7f * cOriginalColliderRadius;
//					}
//				}

				for (int i = 0; i < mColliders.Count; ++i)
				{
					if(mColliders[i].transform.CompareTag("NPC"))
					{
						mColliders[i].transform.GetComponent<PopulationEngine.NPCMotor> ().Walking ();
					}
					else
					{
						mColliders[i].transform.GetComponent<PopulationEngine.NPCMotor> ().Waiting ();
					}
				}
				break;
			case States.STOP:
				default:
				for (int i = 0; i < mColliders.Count; ++i)
				{
					if(mColliders[i].transform.CompareTag("NPC"))
					{
						mColliders[i].transform.GetComponent<PopulationEngine.NPCMotor> ().Waiting ();
					}
					else
					{
						mColliders[i].transform.GetComponent<PopulationEngine.NPCMotor> ().Walking ();
					}
				}
				break;
		}
	}

	void OnTriggerEnter(Collider pCollision)
	{
		//cars and pedestrians have reversed behaviours. If StopStation is on WALK, 
		//pedestrians can walk and cars have to stop. If it's on STOP,
		//pedestrians have to stop and cars can keep going through their trajectory
		if (pCollision.transform.CompareTag ("NPC"))
		{
			mColliders.Add (pCollision);
			
			switch (mCurrentState)
			{
				case States.WALK:
					pCollision.transform.GetComponent<PopulationEngine.NPCMotor> ().Walking ();
					break;
				case States.STOP:
				default:
					pCollision.transform.GetComponent<PopulationEngine.NPCMotor> ().Waiting ();
					break;
			}
//			Physics.IgnoreCollision (mCollider, pCollision, true);
		}
		else
		{
			if(pCollision.transform.CompareTag ("Vehicle"))
			{
				mColliders.Add (pCollision);
				switch (mCurrentState)
				{
					case States.WALK:
						pCollision.transform.GetComponent<PopulationEngine.NPCMotor> ().Waiting ();
						break;
					case States.STOP:
					default:
						pCollision.transform.GetComponent<PopulationEngine.NPCMotor> ().Walking ();
						break;
				}
			}
		}
	}

	void OnTriggerExit(Collider pCollision)
	{
//		Physics.IgnoreCollision (mCollider, pCollision, false);
		mColliders.Remove (pCollision);
	}

	public void Stop()
	{
		ChangeState (States.STOP);
	}

	public void Walk()
	{
		ChangeState (States.WALK);
	}
	
	public bool AreCarsMoving()
	{
		return mCurrentState.Equals(States.STOP);
	}
}
