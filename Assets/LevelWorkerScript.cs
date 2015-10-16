using UnityEngine;
using System.Collections;

public class LevelWorkerScript : MonoBehaviour {
	public GameObject PolygonColliderObject;

	public System.Collections.Generic.List<string> levelFileLines = new System.Collections.Generic.List<string>();

	public System.Collections.Generic.List<GameObject> collidablesV = new System.Collections.Generic.List<GameObject>();

	public System.Collections.Generic.List<string> collidablesK = new System.Collections.Generic.List<string>();

	public System.Collections.Generic.Dictionary<string,GameObject> collidablesP = new System.Collections.Generic.Dictionary<string, GameObject> ();
	
	
	public System.Collections.Generic.List<GameObject> collidables = new System.Collections.Generic.List<GameObject>();

	// Use this for initialization
	void Start () {

		UpdateCollidableIndexes ();
		for (int i = 0; i < collidablesK.Count; i++) {
			collidablesP.Add(collidablesK[i],collidablesV[i]);
			//Debug.Log(collidablesP.Count);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject BuildCollidableFromLine(string Line){
		GameObject newCollidable = Instantiate(PolygonColliderObject);
		
		string[] lines = Line.Split (';');
		System.Collections.Generic.Dictionary<string,string> Vars = new System.Collections.Generic.Dictionary<string, string> ();
		foreach (string line in lines) {
			string type = line.Substring(0,line.IndexOf('='));
			string value = line.Substring(line.IndexOf('=')+1,line.Length-line.IndexOf('=')-1);
			Vars.Add(type,value);
			//Debug.Log("Var [" + type + "]:[" + value + "]");
		}
		
		//type :
		if (collidablesP.ContainsKey (Vars ["type"])) {
			newCollidable = Instantiate (collidablesP [Vars ["type"]]);
			newCollidable.GetComponent<collidableObjectScript> ().isValid = true;
			newCollidable.GetComponent<Rigidbody2D> ().isKinematic = false;
		} else {
			Debug.LogError ("Unknown collidable type " + Vars ["type"]);
		}
		//position it
		//convert to proper vec2 :
		int commaSep = Vars ["position"].IndexOf (",");
		string xstr = Vars ["position"].Substring (1, commaSep - 1);
		string ystr = Vars ["position"].Substring (commaSep+1,Vars ["position"].Length-(2+commaSep));
		float x;
		float y;
		if(xstr.Substring(0,1) == "-"){
			x = (float)(System.Convert.ToDouble(xstr.Substring(1,xstr.Length-1))*-1);
		}else{
			x = (float)(System.Convert.ToDouble(xstr.Substring(0,xstr.Length)));
		}
		
		if(ystr.Substring(0,1) == "-"){
			y = (float)(System.Convert.ToDouble(ystr.Substring(1,ystr.Length-1))*-1);
		}else{
			y = (float)(System.Convert.ToDouble(ystr.Substring(0,ystr.Length)));
		}
		
		//Debug.Log ("x,y [" + x + "," + y + "]");
		
		newCollidable.transform.position = new Vector2 ((float)x, (float)y);
		//
		if (Vars.ContainsKey ("scale")) {
			commaSep = Vars ["scale"].IndexOf (",");
			xstr = Vars ["scale"].Substring (1, commaSep - 1);
			ystr = Vars ["scale"].Substring (commaSep + 1, Vars ["scale"].Length - (2 + commaSep));
			
			if (xstr.Substring (0, 1) == "-") {
				x = (float)(System.Convert.ToDouble (xstr.Substring (1, xstr.Length - 1)) * -1);
			} else {
				x = (float)(System.Convert.ToDouble (xstr.Substring (0, xstr.Length)));
			}
			
			if (ystr.Substring (0, 1) == "-") {
				y = (float)(System.Convert.ToDouble (ystr.Substring (1, ystr.Length - 1)) * -1);
			} else {
				y = (float)(System.Convert.ToDouble (ystr.Substring (0, ystr.Length)));
			}
			newCollidable.transform.localScale = new Vector2 ((float)x, (float)y);
		}
		if (Vars.ContainsKey ("rotation")) {
			newCollidable.transform.Rotate( new Vector3(0,0,(float)(System.Convert.ToDouble (Vars ["rotation"]))));
		}
		if (Vars.ContainsKey ("index")) {
			newCollidable.GetComponent<collidableObjectScript> ().Index = System.Convert.ToInt32 (Vars ["index"]);
		} else {
			throw new MissingReferenceException ("level is outdated. Either fetch an updated version or contanct Developers");
		}

		if (Vars.ContainsKey ("welds")) {

			System.Collections.Generic.List<int> indexesWeldedTo = new System.Collections.Generic.List<int> ();
			foreach (string str in Vars["welds"].Split(',')) {
				indexesWeldedTo.Add (System.Convert.ToInt16 (str));
			}
			newCollidable.GetComponent<collidableObjectScript> ().IndexesWeldedTo = indexesWeldedTo.ToArray ();
		}
		//
		return newCollidable;
	}

	public void readLevelFile(string path){
		GameObject[] colls = GameObject.FindGameObjectsWithTag ("collidable");
		foreach (GameObject obj in colls) {
			Destroy(obj);
		}
		levelFileLines.Clear ();
		foreach (string line in System.IO.File.ReadAllLines(path)) {
			GameObject newCollidable = GetComponent<LevelWorkerScript>().BuildCollidableFromLine(line);
			newCollidable.transform.SetParent (GameObject.Find ("GameCollidables").transform);
			newCollidable.GetComponent<Rigidbody2D>().isKinematic = true;
			collidables.Add(newCollidable);
		}
	}

	public void UpdateCollidableIndexes(){
		int counter = -1;
		foreach (GameObject tested in GameObject.FindGameObjectsWithTag("collidable")) {
			counter++;
			tested.GetComponent<collidableObjectScript>().Index = counter;
		}
	}

	public void SaveAll(){
		string path = Application.persistentDataPath + "/Levels/";
		path += GameObject.Find ("FileNameInput").GetComponent<UnityEngine.UI.InputField> ().text;
		path += ".cl";

		//Save all collidables and states to [path]
		GameObject[] colls = GameObject.FindGameObjectsWithTag ("collidable");
		string workingSave = "";
		string nl = System.Environment.NewLine;
		//type=square;position={0,3};rotation=90;scale={-0.5,-0.5}
		int indexCounter = -1;
		foreach (GameObject obj in colls) {
			indexCounter++;
			Vector2 scale = obj.GetComponent<collidableObjectScript>().transform.localScale;
			Vector2 posi = obj.GetComponent<collidableObjectScript>().transform.position;
			string weldString = "";
			if(obj.GetComponent<collidableObjectScript>().ObjectsWeldedTo.Count > 0){
				foreach (GameObject item in obj.GetComponent<collidableObjectScript>().ObjectsWeldedTo) {
					weldString += item.GetComponent<collidableObjectScript>().Index.ToString() + ",";
				}
				Debug.LogError("weld string BEFORE :" + weldString);
				weldString = weldString.Substring(0,weldString.Length-1);
				Debug.LogError("weld string AFTER :" + weldString);
			}

			workingSave += "type=" + obj.GetComponent<collidableObjectScript>().type + ";";
			workingSave += "rotation=" + obj.GetComponent<collidableObjectScript>().transform.rotation.eulerAngles.z.ToString() + ";";
			workingSave += "scale={" + scale.x + "," + scale.y + "};";
			workingSave += "position={" + posi.x + "," + posi.y + "};";
			workingSave += "index=" + indexCounter;
			if(weldString.Length >= 1){
				workingSave += ";";
				workingSave += "welds=" + weldString;
			}

			workingSave += nl;
		}
		if ( (!System.IO.File.Exists (path)) || ( System.IO.File.Exists (path) && GameObject.Find ("OverwriteToggle").GetComponent<UnityEngine.UI.Toggle> ().isOn)) {
			System.IO.File.WriteAllText (path, workingSave);
		}
		GameObject.Find ("LevelsPanel").GetComponent<LevelPanelScript> ().UpdateAll ();

	}
}
