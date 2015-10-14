using UnityEngine;
using System.Collections;

public class CannonScript : MonoBehaviour {
	private GameObject Cannon;
	private Singleton Singleton;
	public GameObject Ball;
	public int BallIndex = 1;
	public bool isPressing = false;
	public float Power = 0f;
	public bool touchOne;

	public System.Collections.Generic.List<string> BallsK;
	public System.Collections.Generic.List<GameObject> BallsV;
	public System.Collections.Generic.List<int> BallsNumShotsLeft;
	public System.Collections.Generic.Dictionary<string,GameObject> BallsP;



	float PowerMax;
	float PowerMin;

	// Use this for initialization
	void Start () {
		PowerMax = 4000 * 10;
		PowerMin = 900 * 10;
		Cannon = GameObject.Find ("Cannon");

		Singleton = GameObject.Find ("Ground").GetComponent<Singleton> ();
		//Singleton.isWindows
		UpdateBall ();
	}
	
	// Update is called once per frame
	void Update () {

		if (!Singleton.isWindows) {
			//UpdateAndroidDevice ();
			UpdateDesktopDevice ();
		} else {
			UpdateDesktopDevice ();
		}
		UpdateCurrentBallDisplay ();
	}

	void UpdateDesktopDevice ()
	{
		bool isMouseAboveUI = (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > Cannon.GetComponent<Rigidbody2D> ().position.y);
		bool isMouseLeftOfCannon = (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < Cannon.GetComponent<Rigidbody2D> ().position.x);
		float additive = 0f;
		if (isMouseLeftOfCannon) {
			additive = -180f;
		}
		if (!Singleton.isBall && isMouseAboveUI) {

			if (Input.GetMouseButton (0)) {
				Cannon.transform.rotation = Quaternion.Euler(
					Cannon.transform.rotation.eulerAngles.x,
					Cannon.transform.rotation.y,
					GetAngleOf(Cannon.GetComponent<Rigidbody2D> ().position,Camera.main.ScreenToWorldPoint(Input.mousePosition))+additive);
				GameObject.Find ("AngleSlider").GetComponent<UnityEngine.UI.Slider> ().value = Mathf.Rad2Deg*Cannon.transform.rotation.z;

				UpdatePower();
				isPressing = true;
			}
			if (!Input.GetMouseButton (0) && isPressing && BallsNumShotsLeft[BallIndex] > 0) {
				ShootBall();
				isPressing = false;
				BallsNumShotsLeft[BallIndex] -= 1;
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
					GameObject.Find ("AngleSlider").GetComponent<UnityEngine.UI.Slider> ().value = Mathf.Rad2Deg*Cannon.transform.rotation.z;
					UpdatePower();
				}
				if (Input.GetTouch(0).phase == TouchPhase.Ended && BallsNumShotsLeft[BallIndex] > 0) {
					ShootBall();
					BallsNumShotsLeft[BallIndex] -= 1;
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
		Power = 0;
		GameObject.Find ("Ground").GetComponent<Singleton> ().OnShoot ();
	}

	void UpdatePower(){
		Vector2 refPoint;
		if (touchOne) { 
			refPoint = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
		} else {
			refPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}

		Power = Vector2.Distance(Cannon.GetComponent<Rigidbody2D> ().position,refPoint)*25;
		Power *= 90;

		//Debug.Log("Power " + Vector2.Distance(Cannon.GetComponent<Rigidbody2D> ().position,refPoint)*500);

		if (Power < PowerMin) {
			Power = PowerMin;
		}
		if (Power > PowerMax) {
			Power = PowerMax;
		}
		GameObject.Find ("PowerSlider").GetComponent<UnityEngine.UI.Slider> ().value = Power / PowerMax;
	}

	void UpdateBall(){
		Ball = BallsV [BallIndex];
	}

	public void NextBallType(){
		BallIndex += 1;
		if (BallIndex == BallsK.Count) {
			BallIndex = 0;
		}
		UpdateBall ();
	}

	void UpdateCurrentBallDisplay(){
		GameObject.Find ("CurrentBallDisplay").GetComponent<UnityEngine.UI.Image> ().sprite = Ball.GetComponent<SpriteRenderer> ().sprite;
		GameObject.Find ("CurrentBallDisplay").GetComponent<UnityEngine.UI.Image> ().color = Ball.GetComponent<SpriteRenderer> ().color;
		GameObject.Find ("CBD_Text").GetComponent<UnityEngine.UI.Text> ().text = BallsK[BallIndex];
		GameObject.Find ("CBD_Text2").GetComponent<UnityEngine.UI.Text> ().text = BallsNumShotsLeft[BallIndex].ToString();
	}

	float GetAngleOf(Vector2 v1,Vector2 v2){
		return Mathf.Atan ((v2.y - v1.y) / (v2.x - v1.x)) * Mathf.Rad2Deg;
	}
	
	
	void GenericUpdate(){
		
		
	}
}
