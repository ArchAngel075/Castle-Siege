using UnityEngine;
using System.Collections;

public class Singleton_editor : MonoBehaviour {
	public GameObject PolygonColliderObject;
	public System.Collections.Generic.List<string> levelFileLines = new System.Collections.Generic.List<string>();

	public System.Collections.Generic.List<Vector2> polygonPoints = new System.Collections.Generic.List<Vector2>();

	public int Mode = 0;
	public GameObject inspectedObject;

	public System.Collections.Generic.List<GameObject> inspectedObjectsList = new System.Collections.Generic.List<GameObject>();

	public GameObject inspectorMods;

	public string PlacementType;

	public bool isOnUIElement = true;
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
			obj.GetComponent<Rigidbody2D>().isKinematic = !GameObject.Find("isSimulated").GetComponent<UnityEngine.UI.Toggle>().isOn;
		}
		//UnityEngine.Analytics.Analytics.
	}

	void PlaceCollidableAtMouse(){
		GameObject newCollidable = Instantiate(Camera.main.GetComponent<LevelWorkerScript> ().collidablesP[PlacementType]);
		newCollidable.transform.position = (Camera.main.ScreenToWorldPoint (Input.mousePosition)+new Vector3(0,0,10));

		newCollidable.transform.SetParent (GameObject.Find ("GameCollidables").transform);
		newCollidable.GetComponent<collidableObjectScript> ().isValid = true;
		newCollidable.GetComponent<Rigidbody2D>().isKinematic = GameObject.Find("isSimulated").GetComponent<UnityEngine.UI.Toggle>().isOn;
	}

	public void DeleteInspected(){
		if (inspectedObject != null) {
			Destroy(inspectedObject);
		}

	}

	void Start () {
		Screen.SetResolution(800, 600, true);

		GameObject Inspector = GameObject.Find("Inspector");

		GameObject.Find ("SaveFileDialog").GetComponent<Canvas> ().enabled = false;
		//Debug.Log (Application.persistentDataPath);
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
		if (Input.GetMouseButtonDown (0) && !isOnUIElement) {
			Ray MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D HIT = Physics2D.Raycast(MouseRay.origin,MouseRay.direction);
			if(HIT.collider != null){
				//Debug.LogError(HIT.collider.name);
				if(HIT.collider.CompareTag("collidable")){
					if(Input.GetKey(KeyCode.LeftShift)){
						if(HIT.collider.GetComponent<Edit_ObjectScript>().isSelected){
							if(inspectedObjectsList.Contains(HIT.collider.gameObject)){
								inspectedObjectsList.Remove(HIT.collider.gameObject);
								HIT.collider.GetComponent<Edit_ObjectScript>().OnDeselect();
							}
						} else {
							if(!inspectedObjectsList.Contains(HIT.collider.gameObject)){
								inspectedObjectsList.Add(HIT.collider.gameObject);
							}
							HIT.collider.GetComponent<Edit_ObjectScript>().OnSelect();
						}
					} else {
						if(inspectedObject != null){
							inspectedObject.GetComponent<Edit_ObjectScript>().OnDeselect();
						}
						foreach(GameObject selectedObject in inspectedObjectsList){
							selectedObject.GetComponent<Edit_ObjectScript>().OnDeselect();
						}
						inspectedObjectsList.Clear();
						HIT.collider.GetComponent<Edit_ObjectScript>().OnSelect();
						inspectedObject = HIT.collider.gameObject;
						if(!inspectedObjectsList.Contains(inspectedObject)){
							inspectedObjectsList.Add(inspectedObject);
						}
						inspectorMods.GetComponent<InspectedModsScript>().Inspected = inspectedObject;
					}
				} else if(HIT.collider.gameObject.name == "RotationMods"){
					inspectorMods.GetComponent<InspectedModsScript>().isRotating = true;
				}
			} else {
				if(inspectedObject != null){
					inspectedObject.GetComponent<Edit_ObjectScript>().OnDeselect();
					foreach (GameObject obj in inspectedObjectsList) {
						obj.GetComponent<Edit_ObjectScript>().OnDeselect();
					}
					inspectedObjectsList.Clear();
				}
				inspectedObject = null;
				inspectorMods.GetComponent<InspectedModsScript>().Inspected = null;
			}
		}

//		if (Input.GetMouseButton (1) && inspectedObject != null && !inspectorMods.GetComponent<InspectedModsScript>().isRotating) {
//					Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//					mpos.z = inspectedObject.transform.position.z;
//					Vector3 diff = Vector3.MoveTowards(inspectedObject.transform.position,mpos,1f);
//					inspectedObject.transform.position = diff;
//		}

		if (Input.GetMouseButtonUp (0) && inspectorMods.GetComponent<InspectedModsScript> ().isRotating) {
			inspectorMods.GetComponent<InspectedModsScript> ().isRotating = false;
		}
		float magn = 0.05f;
		if(Input.GetKey(KeyCode.LeftShift)){
			magn = 0.001f;
		}
			if(inspectedObjectsList.Count == 0 && inspectedObject != null){
				if (Input.GetKey (KeyCode.A)) {
					Vector3 possave = inspectedObject.transform.position;
					possave.x += -magn;
					inspectedObject.transform.position = possave;
				}
				if (Input.GetKey (KeyCode.D)) {
					Vector3 possave = inspectedObject.transform.position;
					possave.x += magn;
					inspectedObject.transform.position = possave;
				}
				if (Input.GetKey (KeyCode.W)) {
					Vector3 possave = inspectedObject.transform.position;
					possave.y += magn;
					inspectedObject.transform.position = possave;
				}
				if (Input.GetKey (KeyCode.S)) {
					Vector3 possave = inspectedObject.transform.position;
					possave.y += -magn;
					inspectedObject.transform.position = possave;
				}
			} else {
				foreach (GameObject obj in inspectedObjectsList) {
					if (Input.GetKey (KeyCode.A)) {
						Vector3 possave = obj.transform.position;
						possave.x += -magn;
						obj.transform.position = possave;
					}
					if (Input.GetKey (KeyCode.D)) {
						Vector3 possave = obj.transform.position;
						possave.x += magn;
						obj.transform.position = possave;
					}
					if (Input.GetKey (KeyCode.W)) {
						Vector3 possave = obj.transform.position;
						possave.y += magn;
						obj.transform.position = possave;
						
					}
					if (Input.GetKey (KeyCode.S)) {
						Vector3 possave = obj.transform.position;
						possave.y += -magn;
						obj.transform.position = possave;
					}
				}
		}
	}

	void LateUpdate(){
		//GetComponent<Canvas>().
		if (Input.GetKey (KeyCode.LeftArrow)) {
			Camera.main.transform.Translate(-0.05f,0,0);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			Camera.main.transform.Translate(0.05f,0,0);
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			Camera.main.transform.Translate(0,0.05f,0);
			
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			Camera.main.transform.Translate(0,-0.05f,0);
		}
		Camera.main.orthographicSize -= Input.mouseScrollDelta.y * 1f;
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize * 1f,1,5);
	}

	public void SetMode(int mode){
		Mode = mode;
	}

	public void WeldSelected(){
		foreach (GameObject objA in inspectedObjectsList) {
			foreach (GameObject objB in inspectedObjectsList) {
				if(objA != objB){
					objA.GetComponent<Edit_ObjectScript>().weldTo(objB);
				}
				
			}

		}

	}

	public void UnweldSelected(){
		foreach (GameObject objA in inspectedObjectsList) {
			foreach (GameObject objB in inspectedObjectsList) {
				if(objA != objB){
					objA.GetComponent<Edit_ObjectScript>().UnweldTo(objB);
					objB.GetComponent<Edit_ObjectScript>().UnweldTo(objA);
				}
				
			}
			
		}
		
	}

	void UpdateMode1(){
		if (Input.GetMouseButtonDown (0)) {
			polygonPoints.Add (Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
		if (Input.GetMouseButtonDown (1) && !isOnUIElement) {
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

	public void UpdateInspected_rotation(){
		string input = GameObject.Find("RotationInput").GetComponent<UnityEngine.UI.InputField>().text;
		if (inspectedObject != null) {
			inspectedObject.GetComponent<Transform>().rotation = Quaternion.Euler(0,0,((float)System.Convert.ToDouble(input)));
		}
	}

	public void UpdateInspected_scale(){
		string inputx = GameObject.Find("ScaleXInput").GetComponent<UnityEngine.UI.InputField>().text;
		string inputy = GameObject.Find("ScaleYInput").GetComponent<UnityEngine.UI.InputField>().text;
		if (inspectedObject != null) {
			inspectedObject.GetComponent<Transform>().localScale = new Vector2(
				(float)System.Convert.ToDouble(inputx),
			    (float)System.Convert.ToDouble(inputy));
		}
	}

	public void ToMainMenu(){
		Application.LoadLevel("MainMenuScene");
	}


}