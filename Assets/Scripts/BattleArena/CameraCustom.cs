using UnityEngine;
using System;
using System.Collections;

public class CameraCustom : MonoBehaviour {
	
	public float margin = 2.0f; // Speed of zoom out (Higher = slower zoom out)
	
	private float initialDistance = 0.0f;
	
	private Transform tCharacter1; // Transform player 1
	private Transform tCharacter2; // Transform player 2

	private float xLeft; // Coordinate X of left screen
	private float xRight; // Coordinate X of right screen

	// Use this for initialization
	void Start () 
	{
		tCharacter1 = GameObject.Find("Player").transform;    
	    tCharacter2 = GameObject.Find("Player2").transform;

		calculateScreen(tCharacter1, tCharacter2);
		initialDistance = (2*this.camera.orthographicSize*this.camera.aspect) - margin;
	}
	
	void Update ()
	{
		Vector3 tempPosition = transform.position;

		calculateScreen (tCharacter1, tCharacter2);
		float distancePlayers = xRight - xLeft;
		
		if (distancePlayers > initialDistance){ // If the distance between players is greater than the initial, 
							 					// adjust zoom of the camera.
			this.camera.orthographicSize = (distancePlayers + margin) / (2*this.camera.aspect);
	    }
	    
		// Center the camera
	    tempPosition.x = (xRight + xLeft)/2;
		
		transform.position = tempPosition;
	}
	
	
	void calculateScreen(Transform p1, Transform p2)
	{
		 // Calculates the xLeft and xRight screen coordinates based on
		 // position of the left and right player and the margin.
		
	    if (p1.position.x < p2.position.x){
	       xLeft = p1.position.x - margin;
	       xRight = p2.position.x + margin;
	    } else {
	       xLeft = p2.position.x - margin;
	       xRight = p1.position.x + margin;
	    }
	}

	
}
