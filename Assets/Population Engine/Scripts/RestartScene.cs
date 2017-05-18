using UnityEngine;
using System.Collections;

namespace PopulationEngine{
public class RestartScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Restart(){
		Application.LoadLevel(Application.loadedLevel);
	}
}
}