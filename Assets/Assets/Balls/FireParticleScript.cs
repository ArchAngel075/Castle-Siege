using UnityEngine;
using System.Collections;

public class FireParticleScript : MonoBehaviour {
	int isArmed = 0;
	float ticks = 0f;
	float lifetime = 4f;
	public GameObject SmokeEmitter;
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
			isArmed++;
		}
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.CompareTag ("collidable")) {
			ticks+=Time.deltaTime;
			if(ticks >= 0.25f){
				isArmed++;
				ticks = 0.25f-ticks;
				if(coll.gameObject.GetComponent<collidableObjectScript>().isWeakToFire){
					GameObject newSmoker = Instantiate(SmokeEmitter);
					newSmoker.transform.position = coll.contacts[0].point;
					coll.gameObject.GetComponent<collidableObjectScript>().LoseLife(2);
				}
			}
			if(isArmed > 2){
				Destroy (this.gameObject);
			}
		}
	}
}
