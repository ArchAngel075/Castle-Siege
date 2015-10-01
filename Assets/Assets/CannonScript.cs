using UnityEngine;
using System.Collections;

public class CannonScript : MonoBehaviour {
	private GameObject Cannon;
	private Singleton Singleton;
	private GameObject PowerSlider;
	public GameObject Ball;
	public bool isPressing = false;
	public float Power = 0f;
	public bool touchOne;
	public bool touchTwo;

	public Vector3 v1;
	public Vector3 v2;

	// Use this for initialization
	void Start () {
		Cannon = GameObject.Find ("Cannon");
		PowerSlider = GameObject.Find ("SliderCanvas");

		Singleton = GameObject.Find ("Ground").GetComponent<Singleton> ();
		//Singleton.isWindows
	}
	
	// Update is called once per frame
	void Update () {

		if (!Singleton.isWindows) {
			UpdateAndroidDevice ();
		} else {
			UpdateDesktopDevice ();
		}
	}

	void UpdateDesktopDevice ()
	{
		bool isMouseLeftOfCannon = (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < Cannon.GetComponent<Rigidbody2D> ().position.x);
		float additive = 0f;
		if (isMouseLeftOfCannon) {
			additive = -180f;
		}
		if (!Singleton.isBall) {
			PowerSlider.GetComponent<Canvas>().enabled = false;

			if (Input.GetMouseButton (0)) {
				Cannon.transform.rotation = Quaternion.Euler(
					Cannon.transform.rotation.eulerAngles.x,
					Cannon.transform.rotation.y,
					GetAngleOf(Cannon.GetComponent<Rigidbody2D> ().position,Camera.main.ScreenToWorldPoint(Input.mousePosition))+additive);

				UpdatePower();
				isPressing = true;
			}
			if (!Input.GetMouseButton (0) && isPressing) {
				ShootBall();
				isPressing = false;
			}
		}
	}

	void UpdateAndroidDevice(){
		bool isMouseLeftOfCannon = (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < Cannon.GetComponent<Rigidbody2D> ().position.x);
		float additive = 0f;
		if (isMouseLeftOfCannon) {
			additive = -180f;
		}
		touchOne = false;
		if (Input.touchCount == 1) {
			touchOne = true;
		}
		if (touchOne) {
			if (!Singleton.isBall) {
				if (touchOne) {
					Cannon.transform.rotation = Quaternion.Euler(
						Cannon.transform.rotation.eulerAngles.x,
						Cannon.transform.rotation.y,
						GetAngleOf(Cannon.GetComponent<Rigidbody2D> ().position,Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position))+additive);
					UpdatePower();
				}
				if (Input.GetTouch(0).phase == TouchPhase.Ended) {
					ShootBall();
				}
			}
		}

	}

	void ShootBall(){
		//we fired a ball! :
		GameObject aBall = GameObject.Instantiate (Ball);
		aBall.transform.position = GameObject.Find ("spawnnode").transform.position;
		
		float angle = Cannon.GetComponent<Rigidbody2D> ().rotation;
		Vector2 forceComps = new Vector2 (
			Mathf.Cos (Mathf.Deg2Rad * angle),
			Mathf.Sin (Mathf.Deg2Rad * angle)
			);
		forceComps *= (Power);
		aBall.GetComponent<Rigidbody2D> ().AddForce (forceComps);
		Singleton.isBall = true;
		Singleton.setLooking ();
		PowerSlider.GetComponent<Canvas> ().enabled = false;
		Power = 0;
		
	}

	void UpdatePower(){
		Vector2 refPoint;
		if (touchOne) { 
			refPoint = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
		} else {
			refPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
		//PowerSlider.GetComponent<Canvas>().enabled = true;
		Power = Vector2.Distance(Cannon.GetComponent<Rigidbody2D> ().position,refPoint)*500;

		//Debug.Log("Power " + Vector2.Distance(Cannon.GetComponent<Rigidbody2D> ().position,refPoint)*500);

		if (Power < 900) {
			Power = 900;
		}
		if (Power > 4000f) {
			Power = 4000f;
		}
		//PowerSlider.GetComponentInChildren<UnityEngine.UI.Slider>().value = (Power/4000f);
	}

	float GetAngleOf(Vector2 v1,Vector2 v2){
		return Mathf.Atan ((v2.y - v1.y) / (v2.x - v1.x)) * Mathf.Rad2Deg;
	}
	
	
	void GenericUpdate(){
		
		
	}
}
