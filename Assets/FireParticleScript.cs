using UnityEngine;
using System.Collections;

public class FireParticleScript : MonoBehaviour {
	bool isArmed = false;
	float lifetime = 4f;
	// Use this for initialization
	void Start () {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ignoreFire")) {
			Physics2D.IgnoreCollision(this.GetComponent<CircleCollider2D>(),obj.GetComponent<CircleCollider2D>(),true);
		} 
	}
	
	// Update is called once per frame
	void Update () {
		lifetime -= Time.deltaTime;
		if (lifetime <= 0) {
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{	
		if (!(col.gameObject.CompareTag ("collidable") || col.gameObject.CompareTag ("ignoreFire"))) {
			//Debug.Log(col.gameObject.tag);
			Destroy (this.gameObject);
		}
	}
}
