using UnityEngine;
using System.Collections;
using System.Threading;

public class BallScript_Fire : MonoBehaviour {
	public float Lifetime = 24;
	private Singleton Singleton;
	public GameObject fireParticle;
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
		for (int scatI = 0; scatI <= Random.Range(4,16); scatI++) {
			ScatterFire (col);
		}

		if (col.gameObject.name == "Boundary") {
			DestroyBall ();
		} else {
			if(col.gameObject.CompareTag("collidable")){
				col.gameObject.GetComponent<collidableObjectScript>().LoseLife(1);
			}
			Singleton.Ray2DList.Clear ();
			GameObject[] list = GameObject.FindGameObjectsWithTag ("collidable");
			foreach (GameObject obj in list) {

				
			}
			DestroyBall ();
		}
		
	}

	void ScatterFire(Collision2D col){
		foreach (ContactPoint2D point in col.contacts) {
			GameObject aFire = Instantiate(fireParticle);
			Vector2 normal = point.normal;
			Vector2 relVec = col.relativeVelocity;
			aFire.transform.position = point.point + normal*0.15f;

			relVec += new Vector2(Random.Range(-4f,2f), Random.Range(1f,6.85f) );
			relVec.x = Mathf.Clamp(relVec.x,0,1.5f);

			normal += new Vector2(Random.Range(-8f,16f), Random.Range(-1f,1f) );
			normal.x = Mathf.Clamp(normal.x,-1.25f,1f);
			//Debug.Log(normal.y);
			aFire.GetComponent<Rigidbody2D>().AddForce(
				Vector2.Scale(relVec,normal),
				ForceMode2D.Impulse);
		}

	}
	
	void DestroyBall(){
		Destroy(this.gameObject);
		Singleton.isBall = false;
		Singleton.setShooting();
	}
}
