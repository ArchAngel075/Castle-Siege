using UnityEngine;
using System.Collections;

public class SelectButtonScript : MonoBehaviour {
	public string fullpath;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadLevel(){
		GameObject.Find ("SelectPanel").GetComponent<LevelSelectPanel> ().LoadLevel (fullpath);
	}
}
