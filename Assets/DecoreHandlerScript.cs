using UnityEngine;
using System.Collections;

public class DecoreHandlerScript : MonoBehaviour {

	public GameObject DecoreGO;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.J)) {
			MakeDecore();
		}
	}

	public bool CanMakeDecore(){
		return this.transform.childCount <= 5;
	}

	public void MakeDecore(){
		GameObject aDecore = Instantiate (DecoreGO);
		float x;
		float Direction = Random.Range (0, 10);
		float Speed;
		Debug.LogError (Direction);

		if (Direction <= 5) {
			Direction = 0;
			x = 18;
			Speed = Random.Range(-1,-5);
			Vector3 scale = aDecore.transform.localScale;
			scale.x *= -1;
			aDecore.transform.localScale = scale;
		} else {
			Direction = 1;

			x = -18;
			Speed = Random.Range(1,5);
		}
		Vector3 scaley = aDecore.transform.localScale;
		scaley.y += Random.value-0.25f;
		aDecore.transform.localScale = scaley;
		float y = Random.Range (12, 9.5f);



		aDecore.transform.position = new Vector3 (x,y, 0);
		aDecore.GetComponent<CloudScript> ().Speed = Speed;
		aDecore.GetComponent<CloudScript> ().Direction = Direction;
		aDecore.transform.SetParent (this.transform);
	}
}
