using UnityEngine;
using System.Collections;

public class Ungravity : MonoBehaviour 
{

	//Declare a member variables for distributing the impacts over several frames
	float ungravityStartTime = 0;
	Rigidbody ungravityTarget = null;
	Vector3 impact;
	private bool hasZeroGrav = false;

	public AudioClip ungravitySound;


	void Update ()
	{
		if (Input.GetButtonDown("Fire2"))
		{
			//Get a ray going from the camera through the mouse cursor
			Ray ray = Camera.main.ViewportPointToRay(new Vector3 (0.5f, 0.5f));

			//check if the ray hits a physic collider
			RaycastHit hit; //a local variable that will receive the hit info from the Raycast call below
			if (Physics.Raycast(ray,out hit))
			{
				//check if the raycast target has a rigid body (belongs to the ragdoll)
				if (hit.rigidbody != null)
				{
					//find the RagdollHelper component and activate ragdolling
					hit.rigidbody.SendMessageUpwards ("ExplodeAway", true, SendMessageOptions.DontRequireReceiver);
					//set the impact target to whatever the ray hit
					ungravityTarget = hit.rigidbody;
				
					GetComponent<AudioSource> ().volume = 1;
					GetComponent<AudioSource> ().clip = ungravitySound;
					GetComponent<AudioSource> ().loop = true;
					GetComponent<AudioSource> ().Play ();

					ungravityStartTime=Time.time+0.25f;

				}
			}
		}

		//Check if we need to apply an impact
		if (Time.time<ungravityStartTime && !hasZeroGrav)
		{

			hasZeroGrav = true;

			if (ungravityTarget.tag == "Untagged") 
			{
				ungravityTarget.gameObject.AddComponent<ZeroGravity> ();
			}

			if (ungravityTarget.tag != "Untagged") 
			{
				ungravityTarget.transform.root.gameObject.AddComponent<ZeroGravity> ();
			}
		}

		if (Input.GetButtonUp ("Fire2"))
		{
			GetComponent<AudioSource> ().Stop ();
			GetComponent<AudioSource> ().loop = false;
			GetComponent<AudioSource> ().clip = null;

			hasZeroGrav = false;
		}

	}




}
