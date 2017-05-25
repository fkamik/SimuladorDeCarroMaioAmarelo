using UnityEngine;
using System.Collections;

public class RagdollController1 : MonoBehaviour
{
	//Current score
	public int score;

	public bool fallDown;

	//Fernando vars
	private PopulationEngine.NPCMotor mMotor = null;

	private RagdollHelper1 mRDHelper = null;

	private float mFlyAwayTimer;

	private const float cMaxFlyAwayTimer = 0.5f;

	private Vector3 mCollisionOrigin;

	private bool mFlyAway = false;

	private Rigidbody mRigidbody = null;
	//end Fernando vars

	// Use this for initialization
	void Start ()
	{
		mRigidbody = mRigidbody ?? GetComponent<Rigidbody> ();

		mRDHelper = mRDHelper ?? GetComponent<RagdollHelper1> ();
		mMotor = mMotor ?? GetComponent<PopulationEngine.NPCMotor> ();

		//Get all the rigid bodies that belong to the ragdoll
		Rigidbody[] rigidBodies=GetComponentsInChildren<Rigidbody>();
		
		//Add the RagdollPartScript to all the gameobjects that also have the a rigid body
		foreach (Rigidbody body in rigidBodies)
		{
			RagdollPartScript1 rps1=body.gameObject.AddComponent<RagdollPartScript1>();
			
			//Set the scripts mainScript reference so that it can access
			//the score and scoreTextTemplate member variables above
			rps1.mainScript1=this;
		}

	}

	public void TurnOnRagdoll(bool pRagdollMode, Vector3 pCollisionOrigin)
	{	

		fallDown = pRagdollMode;

		if (pRagdollMode)
		{
//			if (!mFlyAway)
//			{
				mCollisionOrigin = pCollisionOrigin;
				
				mMotor.Waiting ();
				mFlyAwayTimer = 0;
				mFlyAway = true;

				mRDHelper.ragdolled = true;
//			}
		} else
		{
			mRDHelper.ragdolled = pRagdollMode;
			mMotor.Walking ();
		}
	}

	public void ToggleRagdoll()
	{
		
		TurnOnRagdoll (!fallDown, GetComponent<PopulationEngine.NPCMotor>().currentDestination);
	}

	//Delete this after debugging
	void Update()
	{
//		TurnOnRagdoll (fallDown, GetComponent<PopulationEngine.NPCMotor>().currentDestination);

		if(mFlyAway)
		{
			mFlyAwayTimer += Time.deltaTime;
			mRigidbody.AddForce (1000 * (Vector3.Normalize(transform.position - mCollisionOrigin) + Vector3.up));
			if (mFlyAwayTimer > cMaxFlyAwayTimer)
			{
				mFlyAway = false;

//				mMotor.enabled = false;
				mMotor.Dead();
//				GetComponent<Animator> ().enabled = false;
//				mRDHelper.ragdolled = false;
			}
		}
	}

}
