using UnityEngine;
using System.Collections;

public class collidableObjectScript : MonoBehaviour {
	public float Life;
	public bool isValid = false;
	public string type = "name";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!isValid) {
			Destroy (this.gameObject);
		}
		if (Life <= 0) {
			Destroy (this.gameObject);
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
		Life -= 1;
		Color oldColor = GetComponent<SpriteRenderer> ().material.color;
		oldColor.g -= 1-(1*(Life/10));
		oldColor.b -= 1-(1*(Life/10));
		GetComponent<SpriteRenderer> ().material.color = oldColor;


	}
}
