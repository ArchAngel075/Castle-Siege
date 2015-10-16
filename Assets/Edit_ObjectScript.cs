using UnityEngine;
using System.Collections;

public class Edit_ObjectScript : MonoBehaviour {
	public bool isSelected = false;
	public
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

	public void UnweldTo(GameObject objB){
		if (GetComponent<collidableObjectScript> ().ObjectsWeldedTo.Contains (objB)) {
			DistanceJoint2D aWeld = null;
			foreach(DistanceJoint2D welding in GetComponent<collidableObjectScript>().Welds){
				if(welding.connectedBody.gameObject == objB.gameObject){
					aWeld = welding;
				}
			}
			if(aWeld != null){
				GetComponent<collidableObjectScript> ().ObjectsWeldedTo.Remove (objB);
				GetComponent<collidableObjectScript> ().Welds.Remove (aWeld);
				Destroy(aWeld);
			}
		}
	}

	public void weldTo(GameObject objB){
		if (!GetComponent<collidableObjectScript> ().ObjectsWeldedTo.Contains (objB)) {
			DistanceJoint2D	aWeld = this.gameObject.AddComponent<DistanceJoint2D>();
			aWeld.enableCollision = true;
			aWeld.connectedBody = objB.GetComponent<Rigidbody2D>();
			aWeld.distance = Vector2.Distance(new Vector2(this.transform.position.x,this.transform.position.y),
			                                  new Vector2(objB.transform.position.x,objB.transform.position.y));
			GetComponent<collidableObjectScript>().Welds.Add(aWeld);
			GetComponent<collidableObjectScript> ().ObjectsWeldedTo.Add (objB);
		}

	}

	public void OnDeselect(){
		isSelected = false;
		GetComponent<SpriteRenderer> ().color = Color.white;
		GameObject.Find ("RotationInput").GetComponent<UnityEngine.UI.InputField> ().text = "NA";
		GameObject.Find ("ScaleXInput").GetComponent<UnityEngine.UI.InputField> ().text = "NA";
		GameObject.Find ("ScaleYInput").GetComponent<UnityEngine.UI.InputField> ().text = "NA";
		GetComponent<Rigidbody2D> ().isKinematic = !GameObject.Find("isSimulated").GetComponent<UnityEngine.UI.Toggle>().isOn;;
	}
}
