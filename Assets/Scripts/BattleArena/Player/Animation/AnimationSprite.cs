using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationSprite : MonoBehaviour {
	
	private Dictionary<PlayerStates, PlayerStates> animationChooser = new Dictionary<PlayerStates, PlayerStates>()
	{
		{PlayerStates.DOUBLE_JUMPING,PlayerStates.J_ATTACKING},	
		{PlayerStates.JUMPING,PlayerStates.J_ATTACKING},
		{PlayerStates.FALL_JUMP,PlayerStates.J_ATTACKING},
		{PlayerStates.FALLING,PlayerStates.J_ATTACKING},
		{PlayerStates.SERIOUSLY_FALLING,PlayerStates.J_ATTACKING},
		{PlayerStates.WALKING,PlayerStates.W_ATTACKING},
		{PlayerStates.RUNNING,PlayerStates.R_ATTACKING},
		{PlayerStates.STANDING,PlayerStates.S_ATTACKING},
	};
	
	public void animateAttack(PlayerCharacter pCharacter, PlayerStates pCurrentState)
	{
		animateSprite(pCharacter,animationChooser[pCurrentState]);		
	}
	
	public void animateSprite (PlayerCharacter pCharacter, PlayerStates pState)					// function for animating sprites
	{
		int[] aniVars = GameObject.Find("SceneManager").GetComponent<SceneManager>().variables.getVariables(pCharacter,pState);
		int columnSize = aniVars[0], 
			rowSize = aniVars[1], 
			colFrameStart = aniVars[2], 
			rowFrameStart = aniVars[3], 
			totalFrames = aniVars[4], 
			framesPerSecond = aniVars[5];
		
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
