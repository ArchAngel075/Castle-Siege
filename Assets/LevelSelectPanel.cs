using UnityEngine;
using System.Collections;

public class LevelSelectPanel : MonoBehaviour {
	public GameObject LevelButton;
	public System.Collections.Generic.List<GameObject> levelList = new System.Collections.Generic.List<GameObject>();
	public System.Collections.Generic.List<string> staticLevelList = new System.Collections.Generic.List<string>();
	System.Collections.Generic.List<string> SearchLocations = new System.Collections.Generic.List<string> ();
	public WWW myWWW;
	string ChapterPath;

	public int size = 10;
	public int start = 0;
	
	public void UpdatePanel(){
		levelList.Clear ();
		foreach (RectTransform child in this.gameObject.GetComponentsInChildren<RectTransform>()) {
			if(child != this.GetComponent<RectTransform>()){
				Destroy(child.gameObject);
			}
		}
		//
		for (int i = start; (i < start+size) && i < staticLevelList.Count; i++) {
			//Debug.Log ("add " + staticLevelList[i]);
			GameObject aNewLevelButton = Instantiate(LevelButton);
			aNewLevelButton.name = "LevelButton";
			string name = "ERROR";
			if(ChapterPath != null){
				name = System.IO.Path.GetFileNameWithoutExtension( staticLevelList[i]);
			}else{
				System.IO.DirectoryInfo ChapterDirectory = new System.IO.DirectoryInfo(staticLevelList[i] + "/");
				Debug.LogError(ChapterDirectory.Name);
				name = ChapterDirectory.Name.TrimStart('@');
			}

			aNewLevelButton.GetComponentInChildren<UnityEngine.UI.Text>().text = name;
			aNewLevelButton.GetComponentInChildren<SelectButtonScript>().fullpath = staticLevelList[i];
			aNewLevelButton.transform.SetParent(this.transform);
			aNewLevelButton.transform.localScale = new Vector3(1,1,1);
			
		}
		
	}

	string[] GetChapters(){
		Debug.LogError ("Chapters :");
		System.Collections.Generic.List<string> total = new System.Collections.Generic.List<string>();
		foreach (string Location in SearchLocations) {


			string[] files = System.IO.Directory.GetDirectories (Location,"@*",System.IO.SearchOption.TopDirectoryOnly);
			foreach (string file in files) {
				total.Add(file);
			}	
		}

		return total.ToArray();
	}

	string[] GetLevels(){
		string[] files = System.IO.Directory.GetFiles (ChapterPath, "*cl", System.IO.SearchOption.AllDirectories);
		foreach (string file in files) {
			staticLevelList.Add (file);
		}
		return files;
	}
	
	public void updateStaticList(){
		staticLevelList.Clear ();
		if (ChapterPath != null) {
			string[] levels = GetLevels();
			foreach (string level in levels) {
				staticLevelList.Add (level);
			}

		} else {
			//addChapters instead :
			string[] Chapters = GetChapters();
			foreach (string chapter in Chapters) {
				staticLevelList.Add (chapter);
			}

		}
	}

	public void LoadLevel(string path){
		if (System.IO.Directory.Exists (path)) {
			ChapterPath = path;
			updateStaticList ();
			UpdatePanel ();
		} else {
			//Application.persistentDataPath + "/Levels/_load.hidden"
			System.IO.File.WriteAllText (Application.persistentDataPath + "/Levels/_load.hidden", path);
			//change scene to game scene (main)
			Application.LoadLevel ("Main");
		}
	}
	
	
	// Use this for initialization
	void Start () {
		myWWW = new WWW ("file:///" + Application.streamingAssetsPath + "/Levels.jar");
		Debug.LogError (Application.dataPath);
		Debug.LogError (Application.streamingAssetsPath);
		SearchLocations.Add (Application.streamingAssetsPath + "/Levels/");
		GameObject.Find ("DEBUGTEXT").GetComponent<UnityEngine.UI.Text> ().text = Application.streamingAssetsPath;
		SearchLocations.Add (Application.persistentDataPath + "/Levels/");
		updateStaticList ();
		UpdatePanel ();
	}
	
	// Update is called once per frame
	void Update () {
		if (myWWW.isDone) {
			Debug.Assert(myWWW.assetBundle);
			foreach (string item in myWWW.assetBundle.GetAllAssetNames()) {
				Debug.Log(item);
			}
			Debug.Break ();
		}
	}

	public void back(){
		if (ChapterPath != null) {
			ChapterPath = null;
			updateStaticList ();
			UpdatePanel ();
		} else {
			ToMainMenu();
		}

	}

	void ToMainMenu(){
		Application.LoadLevel("MainMenuScene");
	}
}
