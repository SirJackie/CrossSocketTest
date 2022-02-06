using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFPS : MonoBehaviour {

	public float deltaTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;
		string fpsString = "FPS: " + fps.ToString ();

		GUILayout.BeginArea (new Rect (0, 0, 130, 130));
		GUILayout.TextField (fpsString);
		GUILayout.EndArea ();
	}
}
