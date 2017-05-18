using UnityEngine;
using System.Collections;

public class RagdollPartScript1 : MonoBehaviour 
{
	//Declare a reference to the main script (of type StairDismount).
	//This will be set by the code that adds this script to all ragdoll parts
	public RagdollController1 mainScript1;

	// Use this for initialization
	void Start () 
	{
	
	}
	void OnCollisionEnter(Collision collision)
	{
		//Increase score if this ragdoll part collides with something else
		//than another ragdoll part with sufficient velocity. 
		//If the colliding object is another ragdoll part, it will have the same root, hence the inequality check.
		if (transform.root != collision.transform.root) 
		{			
			//Check that we are colliding with sufficient velocity
			if (collision.relativeVelocity.magnitude > 4.0f) 
			{
				//compute score
				int score = Mathf.RoundToInt ((collision.relativeVelocity.magnitude)/2);
/*
				//increase the main script's score variable (see StairDismount.cs)
				mainScript.score += score;
				
				//Instantiate a text object
				GameObject scoreText = Instantiate (mainScript.scoreTextTemplate) as GameObject;
				
				//Update the text to show the score
				scoreText.GetComponent<TextMesh> ().text = score.ToString ();
				
				//position the text 1m above this ragdoll part
				scoreText.transform.position = transform.position;
				scoreText.transform.Translate (0, 1, 0);
*/
				if (collision.gameObject.tag == "Ground") 
				{
					float fallDamage = (collision.relativeVelocity.magnitude)/2;
					SendMessageUpwards ("Damage", fallDamage, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

//	void OnCollisionStay (Collision other)
//	{
//		if (gameObject.tag == "AllyBody" | gameObject.tag == "EnemyBody") 
//		{
//			if (other.gameObject.tag == "Ground" )
//				&& !GetComponentInParent<HealthWithStandUp>().dead
//				&& !GetComponentInParent<HealthWithStandUp>().uncounscious
//				&& !GetComponentInParent<HealthWithStandUp>().dead
//				&& !GetComponentInParent<HealthWithStandUp>().uncounscious) 
//			{
//				GetComponentInParent<RagdollController1> ().fallDown = false;
//			}
//		}
//	}


}
