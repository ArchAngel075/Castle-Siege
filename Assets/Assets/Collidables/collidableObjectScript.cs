using UnityEngine;
using System.Collections;

public class collidableObjectScript : MonoBehaviour {
	public float Life;
	public bool isValid = false;
	public string type = "name";
	public bool isRoped = false;
	public bool isPointGiven = false;
	public int[] IndexesWeldedTo;
	public System.Collections.Generic.List<GameObject> ObjectsWeldedTo = new System.Collections.Generic.List<GameObject> ();
	public int Index;
	public GameObject PointText;
	public System.Collections.Generic.List<DistanceJoint2D> Welds = new System.Collections.Generic.List<DistanceJoint2D> ();
	public GameObject PointTextContainer;
	public bool isWeakToFire = true;
	public bool isWeakToWater = true;
	public bool isWeakToImpact = true;
	public bool isWeakToExplode = true;
	// Use this for initialization
	void Start () {
		PointTextContainer = GameObject.Find ("PointsAccumed");
	}

	public void StartupWelds(){
		foreach (int indexer in IndexesWeldedTo) {
			GameObject partnerWelded = null;
			foreach(GameObject tested in GameObject.FindGameObjectsWithTag("collidable")){
				if(tested.GetComponent<collidableObjectScript>().Index == indexer){
					partnerWelded = tested;
				}
			}
			if(partnerWelded != null && !ObjectsWeldedTo.Contains (partnerWelded)){
				GetComponent<Edit_ObjectScript>().weldTo(partnerWelded);
//				DistanceJoint2D	aWeld = this.gameObject.AddComponent<DistanceJoint2D>();
//				aWeld.distance = Vector2.Distance(new Vector2(this.transform.position.x,this.transform.position.y),
//				                                  new Vector2(partnerWelded.transform.position.x,partnerWelded.transform.position.y));
//				aWeld.enableCollision = true;
//				aWeld.connectedBody = partnerWelded.GetComponent<Rigidbody2D>();
//				Welds.Add(aWeld);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isValid) {
			Destroy (this.gameObject);
		}
		if (Life <= 0) {
			Destroy (this.gameObject);
			isPointGiven = true;
		}
		foreach (DistanceJoint2D welding in Welds) {
			if(welding != null && (welding.connectedBody == null || welding.connectedBody.gameObject == null)){
				Destroy(welding);
			}
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{

		//Debug.Log (col.gameObject.name);
		if(col.gameObject.name == "Ball(Clone)" && col.relativeVelocity.magnitude > 1f)
		{
			foreach (DistanceJoint2D welding in Welds) {
				Destroy(welding);
			}
			col.gameObject.GetComponent<BallScript>().Lifetime -= 1;
			LoseLife(3);
		}
		if (col.gameObject.name == "Ground" && col.relativeVelocity.magnitude > 1f) {
			LoseLife(2);
		}

	}

	public void LoseLife(){
		LoseLife (1);
	}

	public void LoseLife(int scale){
		Life -= scale;
		Color oldColor = GetComponent<SpriteRenderer> ().material.color;
		oldColor.g -= scale-(scale*(Life/10));
		oldColor.b -= scale-(scale*(Life/10));
		GetComponent<SpriteRenderer> ().material.color = oldColor;
		if (Life <= 0) {
			GameObject newPointText = Instantiate(PointText);
			newPointText.transform.position = Camera.main.WorldToScreenPoint(transform.position);
			
			newPointText.transform.SetParent(PointTextContainer.transform);
			isPointGiven = true;
			newPointText.GetComponent<pointGetScript>().SetPointGot(1);
			Destroy (this.gameObject);
		}

	}

	void OnDestroy(){
		if (isPointGiven) {
			//GameObject.Find ("Ground").GetComponent<Singleton> ().PointsObtained += 1;
			GameObject.Find ("Ground").GetComponent<Singleton> ().OncolldableBreak();
			GameObject.Find ("PointsText").GetComponent<UnityEngine.UI.Text> ().text = GameObject.Find ("Ground").GetComponent<Singleton> ().PointsObtained.ToString ();

		}
	}
}
