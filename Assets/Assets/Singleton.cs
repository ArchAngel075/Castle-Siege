using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour {
	public bool isBall = false;
	public GameObject Pos_Shooting;
	public GameObject Pos_Looking;

	public float depth_Shooting;
	public float depth_Looking;

	public Transform targetTransform;
	public float targetDepth = 8f;
	public float CameraShake = 0;

	public System.Collections.Generic.List<Ray2D> Ray2DList = new System.Collections.Generic.List<Ray2D>();
	public Ray2D lookRay;

	public Vector3 Mouse;

	public bool isWindows = false;

	private bool isInitKine = false;

	public int PointsTotalPossible = 0;
	public int PointsObtained = 0;

	public GameObject ping01;
	public GameObject ping02;
	public AudioSource boomSource;


	// Use this for initialization
	void Start () {
		Screen.SetResolution (800, 600,true);


		GameObject.Find ("AppDataText").GetComponent<UnityEngine.UI.Text> ().text = "";//Application.persistentDataPath;
		if(!System.IO.Directory.Exists(Application.persistentDataPath + "/Levels/")){
			System.IO.Directory.CreateDirectory (Application.persistentDataPath + "/Levels");
		}

		Camera.main.GetComponent<LevelWorkerScript> ().readLevelFile (System.IO.File.ReadAllText (Application.persistentDataPath + "/Levels/_load.hidden"));
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("collidable")) {
			PointsTotalPossible += 1;
			//obj.GetComponent<collidableObjectScript>().StartupWelds ();
		}


		Camera.main.GetComponent<analyticsScript> ().UpdatePlayedLevel (System.IO.Path.GetFileNameWithoutExtension( System.IO.File.ReadAllText (Application.persistentDataPath + "/Levels/_load.hidden")));		
		targetTransform = Pos_Shooting.transform;
		if (Application.platform == RuntimePlatform.Android) {
			isWindows = false;
		} else {
			isWindows = true;
			//UpdateDesktopDevice();
		}
	}
	
	// Update is called once per frame
	void Update () {
		GameObject.Find ("PointsText").GetComponent<UnityEngine.UI.Text> ().text = PointsObtained.ToString ();
		UpdateScreenShake ();
		if (!isInitKine) {
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("collidable")) {
				obj.GetComponent<Rigidbody2D>().isKinematic = false;
			}
			isInitKine = true;
		}
		//if (Vector3.Distance (Camera.main.transform.position, targetTransform.position) > 0) {
			Camera.main.transform.position = Vector3.MoveTowards (Camera.main.transform.position, targetTransform.position, Time.deltaTime * 2);
		//}
		Camera.main.GetComponent<Camera> ().orthographicSize = Mathf.MoveTowards (Camera.main.GetComponent<Camera> ().orthographicSize, targetDepth, Time.deltaTime * 1.5f);

	}
	
	public void setLooking(){
		targetTransform = Pos_Looking.transform;
		targetDepth = depth_Looking;
		
	}
	
	public void setShooting(){
		targetTransform = Pos_Shooting.transform;
		targetDepth = depth_Shooting;
		
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		foreach (Ray2D rayd in Ray2DList) {
			//Gizmos.DrawSphere (rayd.origin, 0.01f);
			//Gizmos.DrawRay(rayd.origin,rayd.direction*-1);
		}
		Gizmos.color = Color.blue;
		Gizmos.DrawRay (lookRay.origin, lookRay.direction);
		Gizmos.color = Color.red;
		Gizmos.DrawRay (lookRay.origin, lookRay.direction*-1);
		Gizmos.color = Color.cyan;
		//Gizmos.DrawLine (GameObject.Find ("Cannon").GetComponent<Rigidbody2D>().position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
		
	}

	public void QuitToSelection(){
		Application.LoadLevel ("SelectScene");
	}

	public void OnPointGet(int points){
		PointsObtained += points;
		ping01.GetComponent<AudioSourcesScript>().Play ();

	}

	public void OncolldableBreak(){
		ping02.GetComponent<AudioSourcesScript> ().Play ();
		doScreenShake (0.8f);

	}

	public void OnShoot(){
		boomSource.Play ();

	}

	void UpdateScreenShake(){
		if (CameraShake > 0) {
			Vector3 now = Camera.main.transform.position;
			Vector3 want = now + new Vector3 (Mathf.Clamp( Random.Range(-1,1) * CameraShake,-0.15f,0.15f), 
			                                  Mathf.Clamp( Random.Range(-1,1) * CameraShake,-0.15f,0.15f),
			                                  Mathf.Clamp( Random.Range(-1,1) * CameraShake,-0.15f,0.15f));
			Vector3 after = Vector3.Lerp (now, want, 0.5f);
			CameraShake -= Time.deltaTime;
			if( Vector3.Distance(targetTransform.position,Camera.main.transform.position) < 2f){
				Camera.main.transform.position = after;
			}
		} else {
			CameraShake = 0;
		}
	}

	public void doScreenShake(float intensity){
		CameraShake += Random.Range(0,8)*intensity;
		CameraShake = Mathf.Clamp (CameraShake, 0, 0.7f);

	}
}
