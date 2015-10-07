using UnityEngine;
using System.Collections;

public class Edit_ObjectScript : MonoBehaviour {
	public bool isSelected = false;
	public bool isMouseOver = false;
	public Vector2 MouseDelta = Vector2.zero;
	public Vector2 MouseLastFrame = Vector2.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && isMouseOver) {
			if(Camera.main.GetComponent<Singleton_editor>().inspectedObject != null){
				Camera.main.GetComponent<Singleton_editor>().inspectedObject.GetComponent<Edit_ObjectScript>().OnDeselect();
				Camera.main.GetComponent<Singleton_editor>().inspectedObject.GetComponent<Edit_ObjectScript>().isSelected = false;
				Camera.main.GetComponent<Singleton_editor>().inspectedObject = null;
			}
			OnSelect();
			isSelected = true;
			Camera.main.GetComponent<Singleton_editor>().inspectedObject = this.gameObject;
		}
		if (isMouseOver && isSelected && Input.GetMouseButton (0)) {
			MouseDelta = new Vector2(Input.mousePosition.x-MouseLastFrame.x,Input.mousePosition.y-MouseLastFrame.y)*0.03f;

			GetComponent<Transform>().position = new Vector2(GetComponent<Transform>().position.x + MouseDelta.x,GetComponent<Transform>().position.y + MouseDelta.y);
		}
		if (Input.GetMouseButtonDown (0) && isSelected && !isMouseOver) {
			isSelected = false;
			if(Camera.main.GetComponent<Singleton_editor>().inspectedObject != null){
				Camera.main.GetComponent<Singleton_editor>().inspectedObject = null;
			}
			OnDeselect();
		}
		MouseLastFrame = Input.mousePosition;
	}

	void UpdateInspection(){
		GameObject.Find ("RotationInput").GetComponent<UnityEngine.UI.InputField> ().text = GetComponent<Rigidbody2D> ().rotation.ToString ();
	}

	void OnSelect(){
		GetComponent<SpriteRenderer> ().color = Color.blue;
		GetComponent<Rigidbody2D> ().isKinematic = true;;
		UpdateInspection ();
	}

	public void OnDeselect(){
		GetComponent<SpriteRenderer> ().color = Color.white;
		GameObject.Find ("RotationInput").GetComponent<UnityEngine.UI.InputField> ().text = "NA";
		GetComponent<Rigidbody2D> ().isKinematic = GameObject.Find("isSimulated").GetComponent<UnityEngine.UI.Toggle>().isOn;;
	}
}
