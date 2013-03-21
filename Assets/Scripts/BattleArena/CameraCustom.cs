using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CameraCustom : MonoBehaviour {
	
	public float margin = 1.5f; 
	
	private float initialHorizontalDistance;
	private float initialVerticalDistance;
	public float maxDistance = 40.0f;
	
	private Transform tCharacter1; // Transform player 1
	private Transform tCharacter2; // Transform player 2
		
	private SceneManager sceneManager;
	
	private float initialYPos;
	private Vector2 velocity;
	private float verticalOrtSize = 0.0f, horizOrtSize = 0.0f;
	// Use this for initialization
	void Start () 
	{
		sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		
		List<GameObject> players = sceneManager.getPlayers();
		
		GameObject 	p1 = (GameObject)players[0],
					p2 = (GameObject)players[1];
		
		tCharacter1 = p1.transform;
		tCharacter2 = p2.transform;
		
		initialHorizontalDistance = 2*this.camera.orthographicSize*this.camera.aspect;
		initialVerticalDistance = 2*this.camera.orthographicSize;
		initialYPos = transform.position.y;
	}
	
	void Update ()
	{
		restrictPlayers2Level();
		restrictPlayers2Distance();
		
		float midX = (tCharacter1.position.x+tCharacter2.position.x)/2;
		float midY = (tCharacter1.position.y+tCharacter2.position.y)/2;
		float 	leftLimit = -sceneManager.getLevelLenght()/2+this.camera.orthographicSize*this.camera.aspect,
				rightLimit = sceneManager.getLevelLenght()/2-this.camera.orthographicSize*this.camera.aspect;
		Vector3 tempPosition = transform.position;
		tempPosition.x = Mathf.Clamp(	midX,
										leftLimit,
										rightLimit);
		tempPosition.y = Mathf.Clamp(	midY,
										initialYPos,
										2*this.camera.orthographicSize);
		
		float playerHDistance = Mathf.Abs(tCharacter1.position.x-tCharacter2.position.x)+margin;
		float playerVDistance = Mathf.Abs(tCharacter1.position.y-tCharacter2.position.y)+margin;		
		if(playerHDistance>initialHorizontalDistance)
		{
			horizOrtSize = playerHDistance/(2*this.camera.aspect);
		}
		if(playerVDistance > initialVerticalDistance 
			&& (playerVDistance)*this.camera.aspect < maxDistance)
		{
			verticalOrtSize = playerVDistance/2;
		}
		if(verticalOrtSize>horizOrtSize)
		{
			this.camera.orthographicSize = verticalOrtSize;
		}
		else if(verticalOrtSize<horizOrtSize){
			this.camera.orthographicSize = horizOrtSize;
		}
		transform.Translate(tempPosition-transform.position);
	}
	
	private void restrictPlayers2Distance()
	{
		Vector3 posC1 = tCharacter1.position;
		posC1.x = Mathf.Clamp(posC1.x,transform.position.x-maxDistance/2,transform.position.x+maxDistance/2);
		tCharacter1.position = posC1;
		
		Vector3 posC2 = tCharacter2.position;
		posC2.x = Mathf.Clamp(posC2.x,transform.position.x-maxDistance/2,transform.position.x+maxDistance/2);
		tCharacter2.position = posC2;
	}
	
	private void restrictPlayers2Level()
	{
		float levelLength = sceneManager.getLevelLenght() - margin;
		Vector3 posC1 = tCharacter1.position;
		posC1.x = Mathf.Clamp(posC1.x,-levelLength/2,levelLength/2);
		tCharacter1.position = posC1;
		
		Vector3 posC2 = tCharacter2.position;
		posC2.x = Mathf.Clamp(posC2.x,-levelLength/2,levelLength/2);
		tCharacter2.position = posC2;
	}
}
