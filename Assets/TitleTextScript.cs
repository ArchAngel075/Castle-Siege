using UnityEngine;
using System.Collections;

public class TitleTextScript : MonoBehaviour {
	float rotTimer = 1f;
	float scaleTimer = 1f;

	bool isRight = true;
	bool isShrink = false;

	Color targColor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		scaleTimer -= Time.deltaTime;
		rotTimer -= Time.deltaTime;
		if (scaleTimer <= 0) {
			scaleTimer = 0.8f;
			isShrink = !isShrink;
		}
		if (rotTimer <= 0) {
			rotTimer = 1.6f;
			isRight = !isRight;
		}
		if (isShrink) {
			this.transform.localScale = Vector3.Scale (this.transform.localScale, Vector3.one * (1+Time.deltaTime));
		} else {
			this.transform.localScale = Vector3.Scale (this.transform.localScale, Vector3.one * (1-Time.deltaTime));
		}

		if (isRight) {
			this.transform.Rotate(0,0,Time.deltaTime*45f);
			//GetComponent<UnityEngine.UI.Text>().
			//this.transform.localScale = Vector3.Scale (this.transform.localScale, Vector3.one * (1+Time.deltaTime));
		} else {
			this.transform.Rotate(0,0,-Time.deltaTime*45f);
			//this.transform.localScale = Vector3.Scale (this.transform.localScale, Vector3.one * (1-Time.deltaTime));
		}

		if (GetComponent<UnityEngine.UI.Text> ().color == targColor) {
			targColor = new Color(Random.value,Random.value,Random.value,1);
			targColor.a = 1;
		}
		GetComponent<UnityEngine.UI.Text> ().color = Color.Lerp (GetComponent<UnityEngine.UI.Text> ().color, targColor, Time.deltaTime*16f);

	}
}
