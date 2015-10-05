using UnityEngine;
using System.Collections;

public class LevelButtonScript : MonoBehaviour {
	public string fullpath;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void readLevel(){
		Camera.main.GetComponent<Singleton_editor> ().readLevelFile (fullpath);
	}
}
