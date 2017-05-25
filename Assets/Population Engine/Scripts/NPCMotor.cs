/*
	POPULATION ENGINE
	-----------------------
	Custom Script Integration sample

	NPCMotor.cs
	
	REQUIREMENTS: "NPC EVENTS" component must be attached to the NPC player
	
	DESCIRPTION: Shows how to use waypoint data retrieved from the Population Generator
	to move your actor towards the waypoints.
	It uses the NPC Events component, which the Population Generator uses to communicate with custom scripts
	such as this one.

	(C) 2014-2016 AIBotSystem.com
	Support: aibotsystem@gmail.com

	TERMS: Your usage of this script and all associated assets of this product
	means you accept the Unity Asset Store terms and conditions.
	Please know that you have purchased a LICENSE to use this product and associated scripts
	did not purchase the source code itself, including this script.
	Therefore, resale of this script or any part of this product by itself is strictly prohibited.
	Commercial usage is allowed if you integrate this product and scripts into your gaming projects.

*/

using UnityEngine;
using System.Collections;
using PopulationEngine;

namespace PopulationEngine{

[RequireComponent (typeof (PopulationEngine.NPCEvents))]

public class NPCMotor : MonoBehaviour {
	private Animator avatar;
	private UnityEngine.AI.NavMeshAgent agent;	// if using navmesh...not necessary in most cases
	private PopulationEngine.NPCEvents npcEvents;


	public bool useNavMeshAgent = false;	// NavMesh support is a new feature under development
											// We do not recommend using this if you have a very large population
											// for performance reasons



	public bool useAnimator = false;

	[Header("Random Idle Animation State Names (optional)")]
	public string[] randomIdleAnims;
	public string randomIdleStopAnim = "Stop Idle";
	public bool randomIdleEnabled = false;

	// sets random min / max speeds. to set constant speed,
	// simply set min and max to same number
	public float minSpeed = 1;
	public float maxSpeed = 3;
	private float currentSpeed = 0;
	private float originalSpeed = 0;	// holds original speed to rebound from idle


	// sets random min / max rotation speeds
	public float minTurn = 0.1f;
	public float maxTurn = 3;
	private float currentTurnSpeed = 0;


	
	// how long to idle at waypoint before moving onto next?
	public float minIdleTime = 0;	// random min idle time
	public float maxIdleTime = 6;	// random max idle time
	private float idleTime = 3;	

	
	public float stopDistance = 1.5f;

	public Vector3 currentDestination;

	private Transform self_transform;	// cache self for better performance
	public int currentWaypointIndex = -1;	// holds the current index of the waypoint in route
											// this is used by the Population Generator module
											// to figure out what the next waypoint should be
											// To ensure accuracy, this must never be changed at runtime unless you know what you're doing!

	[Header("Ignore Y Position? (Uncheck for airplanes)")]
	public bool ignoreY = true;


	[Header("HEAD LOOK AND FOCUS")]
	public bool headLookActive = false; // set to true to turn head towards object
	public bool focusOnIdle = false;	// set to true to turn entire body to custom focus object on idle only
	public float focusDistance = 5;		// when to start focusing on object?
	public Transform customFocusObject;

	[Header("Debug use")]
	public Transform currentWaypoint;

	private bool firstWaypoint = true;

	//Fernando vars
	private Vector3 mOriginalPos;
	private float mTimer = 0;
	private const float cMaxTimeOnSpot = 0.5f;

		public enum NPCStates
		{
			WALKING		=		0,
			WAITING		=		1,
			HIT			=		2,
			SHOCKED		=		3
		}

		private NPCStates mCurrentState;

		private bool mWaitingBehavior;
		private float mWaitingTimer;
		private const float cMaxWaitingTimer = 2;
	//end Fernando vars

		//Fernando script
		private void ChangeState(NPCStates pNewState)
		{
//			Debug.Log ("Changing to " + pNewState);

			avatar = avatar ?? GetComponent<Animator>();

			switch (pNewState)
			{
				case NPCStates.WALKING:
				default:
					ResetMove ();
					break;
				case NPCStates.WAITING:
					npcEvents.canMove = false;	// temporary stop movement

					currentSpeed = 0;
				try
				{
					if (useAnimator)
					{
						avatar.CrossFade (randomIdleAnims [Mathf.Min(randomIdleAnims.Length - 1, Random.Range (0, randomIdleAnims.Length))], 0.3f);
					}
				}
				catch(System.Exception ex) {
					
				}
					mWaitingTimer = 0;
					break;
				case NPCStates.SHOCKED:
				//avatar.SetBool("shocked", true);
					break;
				case NPCStates.HIT:
					break;
			}

			mCurrentState = pNewState;
		}
		//end Fernando script

		public void Walking()
		{
//			if (!mCurrentState.Equals (NPCStates.WALKING))
//			{
				ChangeState (NPCStates.WALKING);
//			}
		}

		public void Waiting()
		{
			if (!mCurrentState.Equals (NPCStates.HIT))
			{
				ChangeState (NPCStates.WAITING);
			}
		}

		public void Dead()
		{
			ChangeState (NPCStates.HIT);
		}

	// Use this for initialization
	void Start () {
		// set random speed:
		currentSpeed = Random.Range(minSpeed, maxSpeed);
		originalSpeed = currentSpeed;

		// set random turn speed:
		currentTurnSpeed = Random.Range(minTurn, maxTurn);		

		// cache for better performance
		self_transform = transform;
		mOriginalPos = self_transform.position;

		// get animator if needed
		if (useAnimator){
			avatar = GetComponent<Animator>();
		}

		// use NavMesh Agent if needed
		if (useNavMeshAgent){
			agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
			if (agent == null){
				Debug.Log("You selected NavMesh Agent navigation, but NavMesh agent is missing on population NPC!");
				useNavMeshAgent = false;
			}
		}

		npcEvents = GetComponent<PopulationEngine.NPCEvents>();

		GetRandomIdleTime();	// set random idle time

		// check for headlook component:
		if (headLookActive){
			if (GetComponent<HeadLookController>() == null)	{
				headLookActive = false;
			}
		}
		
			//Fernando script
			ChangeState (NPCStates.WALKING);

			mWaitingBehavior = transform.CompareTag ("Vehicle");


			//end Fernando script
	}


	void FindClosestObjectToFocus(){
		// loop through each object from NPCEvents component and find the closest by comparing distances:
		// REQUIRES NPC EVENTS COMPONENT attached

		float tempDistance = 9999;
		GameObject tempFocusObject = null;

		// if there's only 1 object, then grab the first index:
		if (npcEvents.objectsToFocus.Length != 0){
//			foreach(GameObject x in npcEvents.objectsToFocus){
			for (int i = 0; i < npcEvents.objectsToFocus.Length; ++i)
			{
				if (npcEvents.objectsToFocus [i])
				{
					float newTempDist = Vector3.Distance (self_transform.position, npcEvents.objectsToFocus [i].transform.position);
					if ((newTempDist < tempDistance) && newTempDist <= focusDistance)
					{

						// this one is closer so we store it:
						tempDistance = newTempDist;
						tempFocusObject = npcEvents.objectsToFocus [i];
					}
				}
			}
		}
		

		if (tempFocusObject == null){
			customFocusObject = null;
		} else {
			customFocusObject = tempFocusObject.transform;	// load the closest object up			
		}

		
	}
	

	void FindClosestTagsToFocus(){
		// get list of objects from tag, then loop through each object and find closest
		// loop through each object and find the closest by comparing distances:
		// REQUIRES NPC EVENTS COMPONENT attached

		GameObject[] listOfObjectsWithTag = GameObject.FindGameObjectsWithTag(npcEvents.tagToFocus);

		float tempDistance = 9999;
		GameObject tempFocusObject = null;

		// if there's only 1 object, then grab the first index:
		if (listOfObjectsWithTag.Length != 0){

			foreach(GameObject x in listOfObjectsWithTag){
				float newTempDist = Vector3.Distance(self_transform.position, x.transform.position);
				
				if ((newTempDist < tempDistance) && newTempDist <= focusDistance){
					// this one is closer so we store it:
					tempDistance = newTempDist;
					tempFocusObject = x;
				}
			}
		}

		if (tempFocusObject == null){
			customFocusObject = null;
		} else {
			customFocusObject = tempFocusObject.transform;	// load the closest object up			
		}
	}



	void FixedUpdate(){
			switch (mCurrentState)
			{
				case NPCStates.WALKING:
					
					WalkingBehavior ();
					break;
				case NPCStates.WAITING:
					WaitingBehavior ();
					break;
			default:
				break;
			}
		
	}

		private void WaitingBehavior()
		{
			
//			if (mWaitingBehavior)
//			{
//				Debug.Log ("Waiting behavior");
				mWaitingTimer += Time.deltaTime;
				if (mWaitingTimer > cMaxWaitingTimer)
				{
					mWaitingTimer = 0;
					ChangeState (NPCStates.WALKING);
				}
//			}
		}

		private void WalkingBehavior()
		{
			// GET FOCUS OBJECTS, IF NEEDED:
			if (npcEvents.focusObjects && npcEvents.objectsToFocus.Length != 0){
				FindClosestObjectToFocus();
			}

			// GET TAG TO FOCUS:
			else if (npcEvents.focusTag && npcEvents.tagToFocus != ""){
				FindClosestTagsToFocus();
			}



			// UPDATE ANIMATION CONTROLLER SPEED:
			if (useAnimator && avatar != null){
				avatar.SetFloat("Speed", currentSpeed);
			}

			// HEADLOOK, only if customFocusObject not null:
			// NOTE: This integrates with Unity's popular HeadlookController script

			if (headLookActive && customFocusObject != null){
				GetComponent<HeadLookController>().target = customFocusObject.position;
				GetComponent<HeadLookController>().effect = 1;
			}

			else if (headLookActive && customFocusObject == null){
				GetComponent<HeadLookController>().target = Vector3.zero;
				GetComponent<HeadLookController>().effect = 0;
			}


			// MOVEMENT FUNCTIONALITY
			if (npcEvents.canMove){

				// very first time?
				if (firstWaypoint){

					// get initial waypoint index from NPC Events (set by Population Generator component in scene):
					currentWaypointIndex = npcEvents.initialWaypointIndex;

					if (currentWaypointIndex <= -1){
						currentWaypointIndex = 0;
					}

					//Fernando modification
					if (!npcEvents.popGen) {
						npcEvents.popGen = transform.parent.GetComponent<PopulationGenerator> ();
					}
					//end Fernando modification
					// get waypoint object (for debug purposes):
					currentWaypoint = npcEvents.popGen.GetWaypointByID(currentWaypointIndex);	
					currentDestination = npcEvents.popGen.GetCurrentWaypointPosition(currentWaypointIndex);


					firstWaypoint = false;	// set flag to false so we can move on

					// if (useNavMeshAgent && agent != null){
					// 	agent.SetDestination(currentDestination);
					// 	agent.Resume();				
					// }
				}


				// are we at waypoint?
				// Checks remaining distance (navmesh) or (distance between self and current destination)
				else if ((useNavMeshAgent && agent.remainingDistance <= stopDistance) || (Vector3.Distance(self_transform.position, currentDestination) <= stopDistance)){
					// we're at the waypoint, so let's check if we should continue down path:
					// we need to check if the NEXT waypoint exists before fetching it
					// so use the DoesRouteEnd(waypoint Index) method, but add 1 to it to signal the next waypoint
					if (npcEvents.popGen.DoesRouteEnd(currentWaypointIndex+1) == false){

						// stop navmesh agent, if using:
						// if (useNavMeshAgent){
						// 	agent.Stop();
						// }



						// path does not end, let's get next waypoint position:
						// by sending over current waypoint index
						currentDestination = npcEvents.popGen.GetNextWaypointPosition(currentWaypointIndex);

						// get waypoint object (for debug purposes):
						currentWaypoint = npcEvents.popGen.GetWaypointByID(currentWaypointIndex);	

						// path does not end, let's get next waypoint index: (this line must be called last out of these 3)
						currentWaypointIndex = npcEvents.popGen.GetNextWaypointIndex(currentWaypointIndex);




						// check if we need to IDLE or move:
						if (idleTime > 0){
							npcEvents.canMove = false;	// temporary stop movement
							GetRandomIdleTime();	// get random idle time

							currentSpeed = 0;	// set current spd to 0 to stop


							// look at object (focus)
							if (focusOnIdle && customFocusObject != null){
								Vector3 focusPosition = new Vector3(customFocusObject.position.x, self_transform.position.y, customFocusObject.position.z);
								self_transform.LookAt(focusPosition);
							}


							// play random idle?
							if (randomIdleEnabled && randomIdleAnims.Length != 0 && avatar != null){
								avatar.CrossFade(randomIdleAnims[Random.Range(0, randomIdleAnims.Length)], 0.3f);
							}

							Invoke("ResetMove", idleTime);	// start idling
						} 				

					}

				} 

				else {

					// we're not at next waypoint yet, so we keep moving:
					// but only if we're not using navmesh agent:
					if (!useNavMeshAgent){
						Move();		
					}

					// using navmesh agent:
					// else {
					// 	if (agent.velocity == Vector3.zero){
					// 		// we're stopped, so set a new destination:
					// 		agent.SetDestination(currentDestination);
					// 		agent.Resume();
					// 	}
					// }

				}


			}
		}

	void ResetMove(){
		npcEvents.canMove = true;

		// reset current speed:
		currentSpeed = originalSpeed;

		// if using random idle, reset it:
		if (randomIdleEnabled && randomIdleAnims.Length != 0 && avatar != null){
			avatar.Play(randomIdleStopAnim);
		}		
	}


	void Move(){
//		self_transform.position += self_transform.forward * currentSpeed * Time.deltaTime;
			self_transform.position = Vector3.MoveTowards(self_transform.position, new Vector3(currentDestination.x, self_transform.position.y, currentDestination.z), currentSpeed * Time.deltaTime);


			//Fernando modifications:
			if (Vector3.Distance (mOriginalPos, self_transform.position) < 0.01f)
			{
				//if is too long on the smae spot, lift the character up so that he/she can keep walking
				mTimer += Time.deltaTime;
				if (mTimer > cMaxTimeOnSpot)
				{
					self_transform.position += self_transform.up * 0.5f;
					mTimer = 0;
				}
			}
			mOriginalPos = self_transform.position;
			//end Fernando modifications

		RotateTowardsDest(currentDestination);
	}


	void RotateTowardsDest(Vector3 dest){
		Vector3 targetDir = Vector3.zero;


		if (ignoreY){
			targetDir = new Vector3(dest.x, self_transform.position.y, dest.z);
		} else {
			targetDir = dest;
		}

		Quaternion rotation = Quaternion.LookRotation(targetDir - self_transform.position);
		self_transform.rotation = Quaternion.Slerp(self_transform.rotation, rotation, Time.deltaTime * currentTurnSpeed);						        

	}

	void GetRandomIdleTime(){
		idleTime = Random.Range(minIdleTime, maxIdleTime);
	}

	//Fernando script
		//if this is a vehicle and it collides with any Vehicle or NPC, 
		//do not allow it to move(pedestrians should always have priority)
		void OnCollisionEnter(Collision pCollision)
		{
//			if (transform.CompareTag ("Vehicle"))
//			{
				if (pCollision.transform.CompareTag ("Vehicle") || pCollision.transform.CompareTag ("NPC"))
				{
					Waiting ();
				}
//			}
		}

		//if this is a vehicle and it was colliding with any Vehicle or NPC, resume moving now
		void OnCollisionExit(Collision pCollision)
		{
//			if (transform.CompareTag ("Vehicle"))
//			{
				if (pCollision.transform.CompareTag ("Vehicle") || pCollision.transform.CompareTag ("NPC"))
				{
					Walking ();
				}
//			}
		}
	//end Fernando script
}



}