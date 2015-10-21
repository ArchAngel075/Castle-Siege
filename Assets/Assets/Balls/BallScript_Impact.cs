using UnityEngine;
using System.Collections;

public class BallScript_Impact : MonoBehaviour {
	public float Lifetime = 1;
	private Singleton Singleton;
	public bool useEmmiter = false;
	private int damage = 36;
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
		if (Lifetime <= 0 || damage <= 0) {
			DestroyBall();
		}
		
	}
	bool OnCollisionEnter2D (Collision2D col)
	{	
		Debug.Log (col.relativeVelocity.magnitude + "," + col.gameObject.name);
		if (col.gameObject.name == "Boundary") {
			DestroyBall ();
			return true;
		} else if (col.gameObject.CompareTag ("collidable")) {
			if(col.gameObject.GetComponent<collidableObjectScript> ().Life > damage){
				col.gameObject.GetComponent<collidableObjectScript> ().LoseLife (damage);
				DestroyBall();
			} else {
				//Debug.Log(col.relativeVelocity);
				damage -= System.Convert.ToInt16(col.gameObject.GetComponent<collidableObjectScript> ().Life);
				if(col.relativeVelocity.magnitude > 0.25f){
					col.gameObject.GetComponent<collidableObjectScript> ().LoseLife (System.Convert.ToInt16(col.gameObject.GetComponent<collidableObjectScript> ().Life));
					Physics2D.IgnoreCollision(GetComponent<Collider2D>(),col.collider,true);
					//Physics2D.
					GetComponent<Rigidbody2D> ().AddForce (col.relativeVelocity*2f,ForceMode2D.Impulse);
				}

			}
			return false;
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
