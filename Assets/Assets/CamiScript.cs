using UnityEngine;
using System.Collections;

public class CamiScript : MonoBehaviour {
	public GameObject Pos_Shooting;
	public GameObject Pos_Looking;

	public float depth_Shooting = 6f;
	public float depth_Looking = 3f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setLooking(){
		Camera.main.transform.position = Pos_Looking.transform.position;
		Camera.main.GetComponent<Camera> ().depth = depth_Looking;

	}

	public void setShooting(){
		Camera.main.transform.position = Pos_Shooting.transform.position;
		Camera.main.GetComponent<Camera> ().depth = depth_Shooting;
		
	}


}
