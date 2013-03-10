using UnityEngine;
using System.Collections;

public class AnimationSprite : MonoBehaviour {

	
	public void animateSprite (	int columnSize, int rowSize, int colFrameStart, 
							int rowFrameStart, int totalFrames, int framesPerSecond)					// function for animating sprites
	{
		int index = Mathf.FloorToInt(Time.time*framesPerSecond);										// time control fps
		index = index % totalFrames;																	// modulate to total number of frames
		
		Vector2 size = new Vector2 ( 1.0f / columnSize, 1.0f / rowSize);								// scale for column and row size
		
		int u = index % columnSize;																		// u gets current x coordinate from column size
		int v = index / columnSize;																		// v gets current y coordinate by dividing by column size
		
		Vector2 offset = new Vector2 (	(u + colFrameStart) * size.x,
									(1.0f - size.y) - (v + rowFrameStart) * size.y);					// offset equals column and row
		
		renderer.material.mainTextureOffset = offset;													// texture offset for diffuse map
		renderer.material.mainTextureScale  = size;														// texture scale  for diffuse map
	}
}
