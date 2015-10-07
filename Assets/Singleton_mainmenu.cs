using UnityEngine;
using System.Collections;

public class Singleton_mainmenu : MonoBehaviour {
	public GoogleAnalyticsV3 googleAnalytics;
	// Use this for initialization
	void Start () {
		googleAnalytics.StartSession();
		googleAnalytics.LogScreen("Main Menu");
		UnityEngine.Analytics.Analytics.SetUserGender (UnityEngine.Analytics.Gender.Male);
		googleAnalytics.DispatchHits ();
	}

	// Update is called once per frame
	void Update () {
	
	}


}
