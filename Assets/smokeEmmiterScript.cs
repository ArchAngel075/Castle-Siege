using UnityEngine;
using System.Collections;

public class smokeEmmiterScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<ParticleSystem> ().isStopped) {
			Destroy (this.gameObject);
		}
	}
}
