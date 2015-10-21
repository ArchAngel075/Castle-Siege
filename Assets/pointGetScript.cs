using UnityEngine;
using System.Collections;

public class pointGetScript : MonoBehaviour {
	int stage = -1;
	Vector3 target;
	Vector3 falling = Vector3.up*50f;
	float fluxi = 2f;
	Color targetColor;
	bool Destroythis = false;

	int PointsGiven = 0;

	float targetScale = 1f;
	// Use this for initialization
	void Start () {
	
	}

	public void SetPointGot(int point){
		PointsGiven = point;
		this.GetComponent<UnityEngine.UI.Text> ().text = point.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Destroythis) {
			Destroy (this.gameObject);
		}
		if (stage == -1) {
			target = transform.position-(falling);
			target.y += Random.Range(-fluxi*80,0);
			target.x += Random.Range(-fluxi*80,fluxi*80);
			stage = 0;
		}
		if (stage == 0) {
			Vector3 moved = Vector3.MoveTowards (transform.position, target, 400f*Time.deltaTime);
			transform.position = moved;
			if(Vector3.Distance(transform.position,target) <= 1){
				target = GameObject.Find("PointsText").transform.position;
				targetScale = 3f;
				targetColor = new Color(Random.value,Random.value,Random.value);
				stage = 1;
			}
		}
		if (stage == 1) {
			float dscale = Mathf.MoveTowards(transform.localScale.x,targetScale,1f*Time.deltaTime);
			Vector3 moved = Vector3.MoveTowards (transform.position, target, 400f*Time.deltaTime);
			transform.position = moved;
			transform.localScale = new Vector3(dscale,dscale,1);
			GetComponent<UnityEngine.UI.Text>().color = Color.Lerp(GetComponent<UnityEngine.UI.Text>().color,targetColor,0.2f);
			if(Vector3.Distance(transform.position,target) <= 0.25f){
				GameObject.Find ("Ground").GetComponent<Singleton> ().OnPointGet(PointsGiven);
				Destroythis = true;
				stage = 2;
			}
		}

	}
}
