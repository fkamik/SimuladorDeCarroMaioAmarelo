using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalCollidersScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision pCollision)
	{
		//			if (transform.CompareTag ("Vehicle"))
		//			{
		if (pCollision.transform.CompareTag ("internal_collider") || pCollision.transform.CompareTag ("NPC"))
		{
			

			PopulationEngine.NPCMotor motor = transform.parent.parent.GetComponent<PopulationEngine.NPCMotor>();
			RunOver runOver = transform.parent.parent.GetComponent<RunOver>();
			if(motor)
			{
				Debug.Log("collision from " + transform.parent.name + " with " + pCollision.transform.name +", stopping");
				motor.Waiting ();
			}
			else
			{
				if(runOver)
				{
					runOver.Stop();
				}
			}
		}
		//			}
	}

	//if this is a vehicle and it was colliding with any Vehicle or NPC, resume moving now
	void OnCollisionExit(Collision pCollision)
	{
		//			if (transform.CompareTag ("Vehicle"))
		//			{
		if (pCollision.transform.CompareTag ("internal_collider") || pCollision.transform.CompareTag ("NPC"))
		{
			PopulationEngine.NPCMotor motor = transform.parent.parent.GetComponent<PopulationEngine.NPCMotor>();
			RunOver runOver = transform.parent.parent.GetComponent<RunOver>();

			if(motor)
			{
				motor.Walking ();
			}
			else
			{
				if(runOver)
				{
					runOver.Resume();
				}	 
			}
		}
		//			}
	}
}
