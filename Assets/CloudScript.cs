using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour {
	public float Direction;

	public float Speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = this.transform.position;
		pos.x = pos.x += Speed * Time.deltaTime;
		this.transform.position = pos;

		if(Direction == 1 && pos.x >= 18){
			Destroy(this.gameObject);
		} else if ( Direction == 0 && pos.x <= -18){
			Destroy(this.gameObject);
		}
	}
}
