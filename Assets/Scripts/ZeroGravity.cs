using UnityEngine;
using System.Collections;

public class ZeroGravity : MonoBehaviour 
{
	private float forceUpTime;


	void Start ()
	{
		forceUpTime = Time.time + 0.25f;
	}

	void Update () 
	{
		if (Time.time < forceUpTime) 
		{
			//Get all the rigid bodies that belong to the ragdoll
			Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody> ();

			//Add the RagdollPartScript to all the gameobjects that also have the a rigid body
			foreach (Rigidbody body in rigidBodies) 
			{
				body.useGravity = false;
				body.AddForce (Vector3.up * 0.5f, ForceMode.Impulse);
			}
		}

		if (Input.GetButtonUp ("Fire2")) 
		{
			Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody> ();

			//Add the RagdollPartScript to all the gameobjects that also have the a rigid body
			foreach (Rigidbody body in rigidBodies) 
			{
				body.useGravity = true;
			}
			Destroy (GetComponent<ZeroGravity> ());
		}
	}
}
