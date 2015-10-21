﻿using UnityEngine;
using UnityEditor;

public class CreateAssetBundles
{
	[MenuItem ("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles ()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundles");

	}
	[MenuItem ("Assets/Get AssetBundle names")]
	static void GetNames ()
	{
		var names = AssetDatabase.GetAllAssetBundleNames();
		foreach (var name in names)
			Debug.Log ("AssetBundle: " + name);
	}
}