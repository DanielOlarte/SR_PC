using UnityEngine;
using System;
using System.Collections;

public class CameraCustom : MonoBehaviour {
	
	public float margin = 1.0f; // Speed of zoom out (Higher = slower zoom out)
	
	private float initialDistance;
	public float maxDistance = 20.0f;
	
	private Transform tCharacter1; // Transform player 1
	private Transform tCharacter2; // Transform player 2
		
	private SceneManager sceneManager;
	
	private float xLeft; // Coordinate X of left screen
	private float xRight; // Coordinate X of right screen

	// Use this for initialization
	void Start () 
	{
		sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		
		tCharacter1 = GameObject.Find("Player").transform;    
	    tCharacter2 = GameObject.Find("Player2").transform;
		
		calculateScreen(tCharacter1, tCharacter2);
		initialDistance = (2*this.camera.orthographicSize*this.camera.aspect) - margin;
	}
	
	void Update ()
	{
		Vector3 tempPosition = transform.position;
		
		float cameraHalfWidth = Camera.main.orthographicSize*Camera.main.aspect;
		Vector3 posC1 = tCharacter1.position;
		posC1 = new Vector3(Mathf.Clamp(posC1.x,transform.position.x-cameraHalfWidth+0.5f,transform.position.x+cameraHalfWidth-0.5f),
							posC1.y,
							posC1.z);
		tCharacter1.position = posC1;
		
		Vector3 posC2 = tCharacter2.position;
		posC2 = new Vector3(Mathf.Clamp(posC2.x,transform.position.x-cameraHalfWidth+0.5f,transform.position.x+cameraHalfWidth-0.5f),
							posC2.y,
							posC2.z);
		tCharacter2.position = posC2;
		
		calculateScreen (tCharacter1, tCharacter2);
		float distancePlayers = xRight - xLeft;
		
		if (distancePlayers > initialDistance
			&& distancePlayers < maxDistance){ 	// If the distance between players is greater than the initial, 
							 					// adjust zoom of the camera.			
			this.camera.orthographicSize = (distancePlayers + margin) / (2*this.camera.aspect);
	    }
		cameraHalfWidth = Camera.main.orthographicSize*Camera.main.aspect;
		// Center the camera
		float 	leftLimit = -sceneManager.getLevelLenght()/2+cameraHalfWidth,
				rightLimit = sceneManager.getLevelLenght()/2-cameraHalfWidth;
		
		tempPosition.x = Mathf.Clamp(	(xRight + xLeft)/2,
										leftLimit,
										rightLimit);
		
		transform.position = tempPosition;
	}
	
	void calculateScreen(Transform p1, Transform p2)
	{
		 // Calculates the xLeft and xRight screen coordinates based on
		 // position of the left and right player and the margin.
		
	    if (p1.position.x < p2.position.x){
	       xLeft = p1.position.x ;
	       xRight = p2.position.x ;
	    } else {
	       xLeft = p2.position.x ;
	       xRight = p1.position.x ;
	    }
	}

	
}
