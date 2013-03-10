using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CameraCustom : MonoBehaviour {
	
	public float margin = 1.5f; // Speed of zoom out (Higher = slower zoom out)
	
	private float initialHorizontalDistance;
	private float initialVerticalDistance;
	public float maxDistance = 20.0f;
	
	private Transform tCharacter1; // Transform player 1
	private Transform tCharacter2; // Transform player 2
		
	private SceneManager sceneManager;
	
	private float xLeft; // Coordinate X of left screen
	private float xRight; // Coordinate X of right screen
	private float yBottom;
	private float yTop;
	private Vector2 velocity;
	// Use this for initialization
	void Start () 
	{
		sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		
		List<GameObject> players = sceneManager.getPlayers();
		
		GameObject 	p1 = (GameObject)players[0],
					p2 = (GameObject)players[1];
		
		tCharacter1 = p1.transform;
		tCharacter2 = p2.transform;
		
		calculateScreen(tCharacter1, tCharacter2);
		initialHorizontalDistance = (2*this.camera.orthographicSize*this.camera.aspect) - margin;
		initialVerticalDistance = this.camera.orthographicSize;
	}
	
	void Update ()
	{
		Vector3 tempPosition = transform.position;
		
		float cameraHalfWidth = Camera.main.orthographicSize*Camera.main.aspect;
		
		restrictPlayersHorizontal(cameraHalfWidth);
		
		calculateScreen (tCharacter1, tCharacter2);
		float 	horizontalDistancePlayers = xRight - xLeft,
				verticalDistancePlayers = yTop - yBottom;
		
		if( horizontalDistancePlayers > initialHorizontalDistance 
			&& horizontalDistancePlayers < maxDistance)
		{ 			// If the distance between players is greater than the initial, 
					// adjust zoom of the camera.			
			this.camera.orthographicSize = Mathf.Lerp((horizontalDistancePlayers + margin) / (2*this.camera.aspect),
														this.camera.orthographicSize,
														0.1f);
	    }
		if( verticalDistancePlayers > initialVerticalDistance 
			&& 2*verticalDistancePlayers*this.camera.aspect - margin < maxDistance)
		{
			this.camera.orthographicSize = Mathf.Lerp(	verticalDistancePlayers,
														this.camera.orthographicSize,
														0.1f);
		}
		cameraHalfWidth = Camera.main.orthographicSize*Camera.main.aspect;
		// Center the camera
		float 	leftLimit = -sceneManager.getLevelLenght()/2+cameraHalfWidth,
				rightLimit = sceneManager.getLevelLenght()/2-cameraHalfWidth;
						
		tempPosition.x = Mathf.Clamp(	(xRight + xLeft)/2,
										leftLimit,
										rightLimit);
		
		tempPosition.y = Mathf.Clamp(	(yTop+yBottom)/2,
										float.MinValue,
										yBottom+this.camera.orthographicSize-1.0f);
		
		transform.Translate(tempPosition-transform.position);
		
	}
	
	private void restrictPlayersHorizontal(float halfMaxWidth)
	{
		Vector3 posC1 = tCharacter1.position;
		posC1 = new Vector3(Mathf.Clamp(posC1.x,transform.position.x-halfMaxWidth+0.5f,transform.position.x+halfMaxWidth-0.5f),
							posC1.y,
							posC1.z);
		tCharacter1.position = posC1;
		
		Vector3 posC2 = tCharacter2.position;
		posC2 = new Vector3(Mathf.Clamp(posC2.x,transform.position.x-halfMaxWidth+0.5f,transform.position.x+halfMaxWidth-0.5f),
							posC2.y,
							posC2.z);
		tCharacter2.position = posC2;
	}
	
	void calculateScreen(Transform p1, Transform p2)
	{
		 // Calculates the xLeft and xRight screen coordinates based on
		 // position of the left and right player and the margin.
		
	    if (p1.position.x < p2.position.x)
		{
	       xLeft = p1.position.x ;
	       xRight = p2.position.x ;
	    } else 
		{
	       xLeft = p2.position.x ;
	       xRight = p1.position.x ;
	    }
		
		if (p1.position.y < p2.position.y)
		{
	       yBottom = p1.position.y ;
	       yTop = p2.position.y ;
	    } else 
		{
	       yBottom = p2.position.y ;
	       yTop = p1.position.y ;
	    }
	}

	
}
