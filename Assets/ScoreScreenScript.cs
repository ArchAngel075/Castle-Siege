using UnityEngine;
using System.Collections;

public class ScoreScreenScript : MonoBehaviour {
	public UnityEngine.UI.Text DestroyedCountText;
	public UnityEngine.UI.Text DestroyedCountValue;
	int DestroyedCount = 0;

	public UnityEngine.UI.Text BallBonusText;
	public UnityEngine.UI.Text BallBonusValue;
	int BallBonusCount = 0;

	public UnityEngine.UI.Text ComboText;
	public UnityEngine.UI.Text ComboValue;
	int Combo = 0;

	public UnityEngine.UI.Text TOTALPOINTSTEXT;
	int Total = 0;

	public Singleton singleton;

	int State = -1;
	float timer = 0;

	float MaxH = 328.34f;
	float MinH = 104f;

	float TargetH = 104f;


	public void InitPointCounter(){
		State = 0;
	}

	// Use this for initialization
	void Start () {
	
	}

	void UpdateText(){
		DestroyedCountValue.text = DestroyedCount.ToString () + " of " + singleton.PointsTotalPossible.ToString ();

		BallBonusValue.text = BallBonusCount.ToString () + " of " + GameObject.Find("Cannon").GetComponent<CannonScript>().GetTotalBallsPossible().ToString();

		ComboValue.text = Combo.ToString ();

		TOTALPOINTSTEXT.text = Total.ToString ();

	}

	void LerpTotalText(int clamped){
		TOTALPOINTSTEXT.resizeTextMaxSize += 2;
		TOTALPOINTSTEXT.resizeTextMaxSize = Mathf.Clamp (TOTALPOINTSTEXT.resizeTextMaxSize, 50, clamped);



	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.H)) {
			InitPointCounter();
		}

		UpdateText ();
		if (State == 0) {
			timer -= Time.deltaTime;
			if(timer <= 0){
				timer += 0.1f;
				Debug.Log("DestroyedCount");
				Debug.LogError(singleton.PointsObtained);
				if(DestroyedCount != singleton.PointsObtained){
					DestroyedCount++;
				} else {
					Debug.LogError("ASDF");
					State = 1;
				}
			}
		}
		if (State == 1) {
			timer -= Time.deltaTime;
			if(timer <= 0){
				timer += 0.01f;
				Debug.Log("BallBonusCount");
				if(BallBonusCount != GameObject.Find("Cannon").GetComponent<CannonScript>().GetTotalBallsLeft()){
					BallBonusCount++;
				} else {
					State = 2;
				}
			}
		}
		if (State == 2) {
			timer -= Time.deltaTime;
			if(timer <= 0){
				timer += 0.12f;
				if(Combo != GameObject.Find("Cannon").GetComponent<CannonScript>().BonusPoints){
					Combo++;
				} else {
					State = 3;
					TOTALPOINTSTEXT.enabled = true;
				}
			}
		}

		if (State == 3) {

			timer -= Time.deltaTime;
			if(timer <= 0){
				timer += 0.12f;
				if(DestroyedCount != 0){
					DestroyedCount--;
					Total++;
					LerpTotalText(105);
					if(TOTALPOINTSTEXT.resizeTextMaxSize >= 215){
						DestroyedCountText.enabled = false;
						DestroyedCountValue.enabled = false;
					}
				} else {
					State = 4;
				}
			}
		}
		if (State == 4) {
			timer -= Time.deltaTime;
			if(timer <= 0){
				timer += 0.12f;
				if(BallBonusCount != 0){
					BallBonusCount--;
					Total++;
					LerpTotalText(160);
					if(TOTALPOINTSTEXT.resizeTextMaxSize >= 215){
						BallBonusText.enabled = false;
						BallBonusValue.enabled = false;
					}
				} else {
					State = 5;
				}
			}
		}

		if (State == 5) {
			timer -= Time.deltaTime;
			if(timer <= 0){
				timer += 0.12f;
				if(Combo != 0){
					Combo--;
					Total++;
					LerpTotalText(215);
					if(TOTALPOINTSTEXT.resizeTextMaxSize >= 215){
						ComboText.enabled = false;
						ComboValue.enabled = false;
					}
				}
			}
		}

	}
}

























