/*
    POPULATION ENGINE
    -----------------------
    Custom Script Integration sample

    CustomPopulateScript.cs
    
    DESCIRPTION: Demo script that auto destroys any NPCs that come near it.
    NPC must have NPC events attached. 
    This script is used in the ENEMY WAVES demo

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

public class Destroyer : MonoBehaviour {

    public GameObject explodeFX;
    
	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter(Collider other){
        if (other.gameObject.GetComponent<PopulationEngine.NPCEvents>() != null){
            GameObject fx = Instantiate(explodeFX, other.gameObject.transform.position, Quaternion.identity) as GameObject;
            Destroy(other.gameObject);
        }
    }
}
