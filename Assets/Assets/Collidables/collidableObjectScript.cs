using UnityEngine;
using System.Collections;

public class collidableObjectScript : MonoBehaviour {
	public float Life;
	public bool isValid = false;
	public string type = "name";
	public bool isRoped = false;
	public bool isPointGiven = false;
	public GameObject PointText;
	public GameObject PointTextContainer;
	// Use this for initialization
	void Start () {
		PointTextContainer = GameObject.Find ("PointsAccumed");
	}
	
	// Update is called once per frame
	void Update () {
		if (!isValid) {
			Destroy (this.gameObject);
		}
		if (Life <= 0) {
			Destroy (this.gameObject);
			isPointGiven = true;
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{

		//Debug.Log (col.gameObject.name);
		if(col.gameObject.name == "Ball(Clone)" && col.relativeVelocity.magnitude > 1f)
		{
			col.gameObject.GetComponent<BallScript>().Lifetime -= 1;
			LoseLife(3);
		}
		if (col.gameObject.name == "Ground" && col.relativeVelocity.magnitude > 1f) {
			LoseLife(2);
		}

	}

	public void LoseLife(){
		LoseLife (1);
	}

	public void LoseLife(int scale){
		Life -= scale;
		Color oldColor = GetComponent<SpriteRenderer> ().material.color;
		oldColor.g -= scale-(scale*(Life/10));
		oldColor.b -= scale-(scale*(Life/10));
		GetComponent<SpriteRenderer> ().material.color = oldColor;
		if (Life <= 0) {
			Destroy (this.gameObject);
			isPointGiven = true;
		}

	}

	void OnDestroy(){
		if (isPointGiven) {
			GameObject.Find ("Ground").GetComponent<Singleton> ().PointsObtained += 1;
			GameObject.Find ("Ground").GetComponent<Singleton> ().OncolldableBreak();
			GameObject.Find ("PointsText").GetComponent<UnityEngine.UI.Text> ().text = GameObject.Find ("Ground").GetComponent<Singleton> ().PointsObtained.ToString ();
			GameObject newPointText = Instantiate(PointText);
			newPointText.transform.position = Camera.main.WorldToScreenPoint(transform.position);
			newPointText.transform.SetParent(PointTextContainer.transform);
		}
	}
}
