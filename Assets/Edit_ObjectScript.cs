using UnityEngine;
using System.Collections;

public class Edit_ObjectScript : MonoBehaviour {
	public bool isSelected = false;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (isSelected && Input.GetKeyDown (KeyCode.Delete)) {
			Destroy (this.gameObject);
		}
	}

	void UpdateInspection(){
		GameObject.Find ("RotationInput").GetComponent<UnityEngine.UI.InputField> ().text = GetComponent<Rigidbody2D> ().rotation.ToString ();
		GameObject.Find ("ScaleXInput").GetComponent<UnityEngine.UI.InputField> ().text = transform.localScale.x.ToString ();
		GameObject.Find ("ScaleYInput").GetComponent<UnityEngine.UI.InputField> ().text = transform.localScale.y.ToString ();
	}

	public void OnSelect(){
		isSelected = true;
		GetComponent<SpriteRenderer> ().color = Color.blue;
		GetComponent<Rigidbody2D> ().isKinematic = true;;
		UpdateInspection ();
	}

	public void OnDeselect(){
		isSelected = false;
		GetComponent<SpriteRenderer> ().color = Color.white;
		GameObject.Find ("RotationInput").GetComponent<UnityEngine.UI.InputField> ().text = "NA";
		GameObject.Find ("ScaleXInput").GetComponent<UnityEngine.UI.InputField> ().text = "NA";
		GameObject.Find ("ScaleYInput").GetComponent<UnityEngine.UI.InputField> ().text = "NA";
		GetComponent<Rigidbody2D> ().isKinematic = GameObject.Find("isSimulated").GetComponent<UnityEngine.UI.Toggle>().isOn;;
	}
}
