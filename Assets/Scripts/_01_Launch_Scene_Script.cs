using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _01_Launch_Scene_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// First launch
		if (PlayerPrefs.GetInt ("FirstLaunch") == 0) {
			PlayerPrefs.SetInt ("FirstLaunch", 1);
			SceneManager.LoadScene ("02_Initial_Scene");
		} 
		// Normal launch
		else {
			SceneManager.LoadScene ("03_Main_Scene");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
