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

	public System.Collections.Generic.List<Ray2D> Ray2DList = new System.Collections.Generic.List<Ray2D>();
	public Ray2D lookRay;

	public Vector3 Mouse;

	public bool isWindows = false;

	// Use this for initialization
	void Start () {
		GameObject.Find ("AppDataText").GetComponent<UnityEngine.UI.Text> ().text = "";//Application.persistentDataPath;
		System.IO.Directory.CreateDirectory (Application.persistentDataPath + "/Levels");
		//Camera.main.GetComponent<LevelWorkerScript> ().readLevelFile (System.IO.File.ReadAllText (Application.persistentDataPath + "/Levels/_load.hidden"));
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("collidable")) {
			obj.GetComponent<Rigidbody2D>().simulated = true;
		}
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
		if (Vector3.Distance (Camera.main.transform.position, targetTransform.position) > 0) {
			Camera.main.transform.position = Vector3.MoveTowards (Camera.main.transform.position, targetTransform.position, Time.deltaTime * 2);
		}
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
}
