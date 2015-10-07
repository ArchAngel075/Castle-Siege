using UnityEngine;
using System.Collections;

public class BallScript_Impact : MonoBehaviour {
	public float Lifetime = 24;
	private Singleton Singleton;
	public bool useEmmiter = false;
	// Use this for initialization
	void Start () {
		Singleton = GameObject.Find ("Ground").GetComponent<Singleton> ();
		if(!useEmmiter){
			this.gameObject.GetComponentInChildren<ParticleSystem> ().Stop ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (useEmmiter) {
			if (GetComponent<Rigidbody2D> ().velocity.magnitude > 1) {
				this.gameObject.GetComponentInChildren<ParticleSystem> ().Play ();
			} else {
				this.gameObject.GetComponentInChildren<ParticleSystem> ().Stop ();
			}
		}
		Lifetime -= Time.deltaTime;
		if (Lifetime <= 0) {
			DestroyBall();
		}
		
	}
	bool OnCollisionEnter2D (Collision2D col)
	{	
		Lifetime -= 8;
		if (col.gameObject.name == "Boundary") {
			DestroyBall ();
			return true;
		} else if (col.gameObject.CompareTag ("collidable")) {
			GetComponent<Rigidbody2D> ().AddForce (col.relativeVelocity);
			col.gameObject.GetComponent<collidableObjectScript> ().LoseLife (6);
			return true;
		} else {
			return true;
		}

		
	}
	
	void DestroyBall(){
		Destroy(this.gameObject);
		Singleton.isBall = false;
		Singleton.setShooting();
		
	}
}
