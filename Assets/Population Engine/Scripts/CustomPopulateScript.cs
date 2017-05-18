/*
	POPULATION ENGINE
	-----------------------
	Custom Script Integration sample

	CustomPopulateScript.cs
	
	DESCIRPTION: Custom script showing how to use the 'CUSTOM' setting on the Population Generator
	This is a great way to integrate your own triggers and methods with the population generator.

	1) Attach this to anything, such as your player. 
	2) On the Population Generator object, set RUNTIME METHOD to "Custom"
	3) Run the scene. Notice that nothing is spawned because it will be awaiting activation by custom scripts.
	4) When you press the spacebar, it will activate the spawner.


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

public class CustomPopulateScript : MonoBehaviour {

	[Header("Press (SPACE) to generate at runtime")]
	public PopulationEngine.PopulationGenerator populationGen;

	// Use this for initialization
	void Start () {
		if (populationGen == null){
			Debug.Log("Your need to fill in Population Gen slot in CustomPopulateScript!");
		}
	}

	void Update(){
		if (Input.GetKeyUp(KeyCode.Space)){
			populationGen.StartGeneratingInstant();	 // activate instant generation (uses your settings on Population Generator)

			// populationGen.StartGeneratingTimed(); // activate timed generation  (uses your settings on Population Generator)
		}
	}
	
}
