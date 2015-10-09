using UnityEngine;
using System.Collections;

public class AudioSourcesScript : MonoBehaviour {
	public GameObject sourceGO;
	private int channels = 16;


	// Use this for initialization
	void Start () {
		for (int i = 0; i<channels; i++) {
			GameObject newSource = Instantiate(sourceGO);
			newSource.transform.SetParent(this.transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Play(){
		GameObject available = null;
		for (int i = 0; (i < this.transform.childCount); i++) {
			GameObject getten = this.transform.GetChild(i).gameObject;
			if(!getten.GetComponent<AudioSource>().isPlaying){
				available = getten;
			}
		}
		if (available != null) {
			available.GetComponent<AudioSource> ().Play ();
		} else {
			//Debug.LogError("Not enough");
		}
	}
}
