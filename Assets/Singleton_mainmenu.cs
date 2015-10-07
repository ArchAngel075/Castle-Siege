using UnityEngine;
using System.Collections;

public class Singleton_mainmenu : MonoBehaviour {
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	
	}

	public void ToLevelSelect(){
		Application.LoadLevel("SelectScene");
	}

	public void ToEditor(){
		Application.LoadLevel("EditorScene");
	}


}
