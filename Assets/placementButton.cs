using UnityEngine;
using System.Collections;

public class placementButton : MonoBehaviour {
	public string type;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetPlacementType(){
		Camera.main.GetComponent<Singleton_editor> ().PlacementType = type;
		Camera.main.GetComponent<Singleton_editor> ().SetMode (2);
		Camera.main.GetComponent<Singleton_editor> ().SelectUpdate ();
		//Debug.Log ("Set PlacementType : " + type);
	}
}
