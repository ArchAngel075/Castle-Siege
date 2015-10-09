using UnityEngine;
using System.Collections;

public class analyticsScript : MonoBehaviour {
	public GoogleAnalyticsV3 googleAnalytics;
	// Use this for initialization
	void Start () {
		googleAnalytics.StartSession();
		googleAnalytics.LogScreen(Application.loadedLevelName);
		UnityEngine.Analytics.Analytics.SetUserGender (UnityEngine.Analytics.Gender.Unknown);
		googleAnalytics.SetUserIDOverride ("Dev-Admin");
		googleAnalytics.DispatchHits ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdatePlayedLevel(string LevelName){
		googleAnalytics.LogEvent ("LevelPlayed", "Start", LevelName, 0);
		googleAnalytics.DispatchHits ();
	}
}
