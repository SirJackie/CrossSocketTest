using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour {

	Vector3 position;
	float maxX = 3.0f;
	float speed = 0.02f;
	bool direction = true;

	// Use this for initialization
	void Start () {
		position = transform.position;

	}
	
	// Update is called once per frame
	void Update () {

		// Step in
		if (direction) {
			position.x += speed;
		} else {
			position.x -= speed;
		}

		// Clamp
		if (position.x >= maxX || position.x <= -maxX) {
			direction = !direction;
		}

		// Apply
		transform.position = position;
	}
}
