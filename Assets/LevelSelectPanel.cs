using UnityEngine;
using System.Collections;

public class LevelSelectPanel : MonoBehaviour {
	public GameObject LevelButton;
	public System.Collections.Generic.List<GameObject> levelList = new System.Collections.Generic.List<GameObject>();
	public System.Collections.Generic.List<string> staticLevelList = new System.Collections.Generic.List<string>();
	
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
			aNewLevelButton.GetComponentInChildren<UnityEngine.UI.Text>().text = System.IO.Path.GetFileNameWithoutExtension( staticLevelList[i]);
			aNewLevelButton.GetComponentInChildren<SelectButtonScript>().fullpath = staticLevelList[i];
			aNewLevelButton.transform.SetParent(this.transform);
			aNewLevelButton.transform.localScale = new Vector3(1,1,1);
			
		}
		
	}
	
	public void updateStaticList(){
		staticLevelList.Clear ();
		string[] files = System.IO.Directory.GetFiles (Application.persistentDataPath + "/Levels","*cl",System.IO.SearchOption.AllDirectories);
		foreach (string file in files) {
			staticLevelList.Add(file);
		}
	}

	public void LoadLevel(string path){
		//Application.persistentDataPath + "/Levels/_load.hidden"
		System.IO.File.WriteAllText (Application.persistentDataPath + "/Levels/_load.hidden", path);
		//change scene to game scene (main)
		Application.LoadLevel ("Main");
	}
	
	
	// Use this for initialization
	void Start () {
		updateStaticList ();
		UpdatePanel ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void QuitGame(){
		Application.Quit ();
	}
}
