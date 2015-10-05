using UnityEngine;
using System.Collections;

public class PlacementPanelScript : MonoBehaviour {
	public GameObject placementGO;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddPlacement(GameObject GO,string Name){
		GameObject newPlacement = Instantiate (placementGO);
		newPlacement.transform.SetParent (this.transform);
		newPlacement.GetComponent<placementButton> ().type = Name;

		newPlacement.GetComponent<UnityEngine.UI.Image> ().sprite = Sprite.Create(
			GO.GetComponent<SpriteRenderer> ().sprite.texture,
			GO.GetComponent<SpriteRenderer> ().sprite.rect,
			new Vector2(0,0)
		);
		newPlacement.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
		//newPlacement.GetComponent<UnityEngine.UI.Image> ().sprite.rect.size /= 8;
		newPlacement.GetComponentInChildren<UnityEngine.UI.Text> ().text = Name;
	}
}
