using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {
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
	void OnCollisionEnter2D (Collision2D col)
	{	
		if (col.gameObject.name == "Boundary") {
			DestroyBall ();
		} else {
			if(col.gameObject.CompareTag("collidable")){
				col.gameObject.GetComponent<collidableObjectScript>().LoseLife(1);
			}
			Singleton.Ray2DList.Clear ();
			GameObject[] list = GameObject.FindGameObjectsWithTag ("collidable");
			foreach (GameObject obj in list) {
				float Distance = Vector2.Distance (this.transform.position, obj.gameObject.transform.position);
				if (Distance < 4f) {
					Vector2 direction = this.transform.position - obj.gameObject.transform.position;
					obj.GetComponent<Rigidbody2D> ().AddForce (direction * (-850 * (Distance / 4)));
					Singleton.Ray2DList.Add (new Ray2D (this.transform.position, direction));
					obj.GetComponent<collidableObjectScript> ().LoseLife (2);
				}

			}
			DestroyBall ();
		}

	}

	void DestroyBall(){
		Destroy(this.gameObject);
		Singleton.isBall = false;
		Singleton.setShooting();

	}
}
