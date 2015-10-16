using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class CanvasScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {// required interface when using the OnPointerEnter method.

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnPointerEnter(PointerEventData data){
		Camera.main.GetComponent<Singleton_editor> ().isOnUIElement = true;
	//	Debug.LogError ("IN");
	}
	
	public void OnPointerExit (PointerEventData eventData) 
	{
		Debug.Log ("The cursor exited the selectable UI element.");
		Camera.main.GetComponent<Singleton_editor> ().isOnUIElement = false;
	//	Debug.LogError ("OUT");
	}
}
