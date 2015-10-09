using UnityEngine;
using System.Collections;

public class InspectedModsScript : MonoBehaviour {
	public GameObject Inspected;

	public bool isRotating = false;

	public GameObject rotationMods;

	public Vector3 Mouse = Vector3.zero;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Inspected != null){
			this.transform.position = Inspected.transform.position;
			if(isRotating){
				//get angle -
				//
				transform.rotation = Quaternion.Euler(0,0,Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z,GetAngleOf(Camera.main.WorldToScreenPoint(transform.position),Input.mousePosition),360)+180f);
				Inspected.transform.rotation = transform.rotation;

			} else {
				rotationMods.transform.parent.rotation = Inspected.transform.rotation;

			}
		}
	}

	float GetAngleOf(Vector2 v1,Vector3 v2){
		bool isLeft = (v1.x < v2.x);
		float anglebf = Mathf.Atan ((v2.y - v1.y) / (v2.x - v1.x)) * Mathf.Rad2Deg;
		if (isLeft) {
			anglebf -= 180;
		}
		return anglebf;
	}
}
