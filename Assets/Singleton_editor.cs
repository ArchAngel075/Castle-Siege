using UnityEngine;
using System.Collections;

public class Singleton_editor : MonoBehaviour {
	public GameObject PolygonColliderObject;
	public System.Collections.Generic.List<string> levelFileLines = new System.Collections.Generic.List<string>();

	public System.Collections.Generic.List<Vector2> polygonPoints = new System.Collections.Generic.List<Vector2>();

	public int Mode = 0;
	public GameObject inspectedObject;

	public string PlacementType;
	/*
		Mode 0 : None - Selection Mode
		Mode 1 : Polygon - Add polygonObject
		Mode 2 : Palcement - Place the current Palcement Type at mouse-
	*/
	// Use this for initialization
	public void readLevelFile(string path){
		Camera.main.GetComponent<LevelWorkerScript> ().readLevelFile (path);
	}

	public void SelectUpdate(){
		GameObject.Find("CursorImage").GetComponent<UnityEngine.UI.Image>().sprite = Camera.main.GetComponent<LevelWorkerScript> ().collidablesP [PlacementType].GetComponent<SpriteRenderer> ().sprite;
	}

	public void UpdateSimulate(){
		GameObject[] colls = GameObject.FindGameObjectsWithTag ("collidable");
		foreach (GameObject obj in colls) {
			obj.GetComponent<Rigidbody2D>().simulated = GameObject.Find("isSimulated").GetComponent<UnityEngine.UI.Toggle>().isOn;
		}

	}

	void PlaceCollidableAtMouse(){
		GameObject newCollidable = Instantiate(Camera.main.GetComponent<LevelWorkerScript> ().collidablesP[PlacementType]);
		newCollidable.transform.position = (Camera.main.ScreenToWorldPoint (Input.mousePosition)+new Vector3(0,0,10));

		newCollidable.transform.SetParent (GameObject.Find ("GameCollidables").transform);
		newCollidable.GetComponent<collidableObjectScript> ().isValid = true;
		newCollidable.GetComponent<Rigidbody2D>().simulated = GameObject.Find("isSimulated").GetComponent<UnityEngine.UI.Toggle>().isOn;
	}

	void Start () {

		GameObject.Find ("SaveFileDialog").GetComponent<Canvas> ().enabled = false;
		Debug.Log (Application.persistentDataPath);
		PlacementPanelScript thePlacementPanel = GameObject.Find ("PlacementsPanel").GetComponent<PlacementPanelScript> ();
		for (int i = 0; i < Camera.main.GetComponent<LevelWorkerScript> ().collidablesK.Count; i++) {
			//Camera.main.GetComponent<LevelWorkerScript> ().collidablesP.Add(Camera.main.GetComponent<LevelWorkerScript> ().collidablesK[i],Camera.main.GetComponent<LevelWorkerScript> ().collidablesV[i]);
			thePlacementPanel.AddPlacement(Camera.main.GetComponent<LevelWorkerScript> ().collidablesV[i],Camera.main.GetComponent<LevelWorkerScript> ().collidablesK[i]);
		}
		//readLevelFile (Application.persistentDataPath + "/Levels/Level01.cl");

	}
	
	// Update is called once per frame
	void Update () {
		Vector2 point = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		GameObject.Find ("CursorImage").transform.position = point;
		if (Mode == 1) {
			UpdateMode1();	
		}
		if (Mode == 2) {
			if(Input.GetMouseButtonDown(1)){
				PlaceCollidableAtMouse();
			}
		}

	}

	public void SetMode(int mode){
		Mode = mode;
	}

	void UpdateMode1(){
		if (Input.GetMouseButtonDown (0)) {
			polygonPoints.Add (Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
		if (Input.GetMouseButtonDown (1)) {
			polygonPoints.Add (polygonPoints[0]);
			SetMode(0);
		}
	}

	void OnDrawGizmos() {
		if (polygonPoints.Count >= 2) {
			Gizmos.color = Color.yellow;
			for (int i = 0; i < polygonPoints.Count; i++) {
				if (i == 0) {
					Gizmos.DrawLine (polygonPoints [0], polygonPoints [1]);
				} else if(i == polygonPoints.Count-1) {
					Gizmos.DrawLine (polygonPoints [polygonPoints.Count-2], polygonPoints [polygonPoints.Count-1]);
					Gizmos.DrawLine (polygonPoints [polygonPoints.Count-1], Camera.main.ScreenToWorldPoint(Input.mousePosition));
				} else {
					Gizmos.DrawLine (polygonPoints [i-1], polygonPoints [i]);
				}
			}
		}
	}

	public void OpenSaveDialog(){
		GameObject.Find ("SaveFileDialog").GetComponent<Canvas> ().enabled = true;
	}


}