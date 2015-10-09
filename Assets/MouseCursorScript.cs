using UnityEngine;
using System.Collections;

public class MouseCursorScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag ("collidable")) {
			//col.gameObject.GetComponent<Edit_ObjectScript>().isMouseOver = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D col){
		if (col.CompareTag ("collidable")) {
			//col.gameObject.GetComponent<Edit_ObjectScript>().isMouseOver = false;
		}
	}
}
