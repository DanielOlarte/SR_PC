using UnityEngine;
using System;
using System.Collections.Generic;

public class Button : MonoBehaviour
{
	private string name;
	private Rect position;
	private bool isPressed;
	private bool isActive;
	private GUITexture guiTexture;
	
	private int fingerId = -1;
	private TouchPhase currentPhase;

	void Start()
	{
		guiTexture = GetComponent<GUITexture>();
	}
	
	public void initializeVariables(string id, Rect pos, Texture jump)
	{
		name = id;
		guiTexture = GetComponent<GUITexture>();
		guiTexture.texture = jump;
		guiTexture.pixelInset = pos;
		
		position = guiTexture.pixelInset;
		
		currentPhase = TouchPhase.Canceled;
	}
	
	public Rect getPosition()
	{
		return position;
	}
	
	public bool containsPoint(Vector2 point)
	{
		if ( point.x >= position.x &&
			 point.x <= position.x + position.width && 
			 point.y >= position.y &&
			 point.y <= position.y + position.height )
		{
			return true;
		}
		
		return false;
	}
	
	public bool wasPressed()
	{
		return isPressed;
	}
	
	public void setIsPressed(bool pressed)
	{
		isPressed = pressed;
	}
	
	public void checkPressedState(Touch touch)
	{	
		if ( fingerId == touch.fingerId )
		{
			if ( touch.phase == TouchPhase.Began )
			{
				isPressed = true;
				currentPhase = touch.phase;
			}
			else if ( touch.phase == TouchPhase.Ended )
			{
				updateTouchState();
				isPressed = false;
			}
			else
			{
				isPressed = false;
				currentPhase = touch.phase;
			}
			
		}
		else if ( currentPhase == TouchPhase.Canceled && touch.phase == TouchPhase.Began )
		{
			fingerId = touch.fingerId;
			currentPhase = touch.phase;
			isPressed = true;
		}
	}
	
	public void updateTouchState()
	{
		fingerId = -1;
		currentPhase = TouchPhase.Canceled;
		isPressed = false;
	}
	
}


