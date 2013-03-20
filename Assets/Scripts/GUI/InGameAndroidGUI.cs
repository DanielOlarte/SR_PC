using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InGameAndroidGUI : InGameGUI
{
	public override void createButtons(GameObject buttonPrefab, Texture jump, Texture attack, Texture attack1)
	{
		sizeFixer = new SizeFixer();
		
		float x = 380.0f; 
		float y = 80.0f;
		float width = 140.0f;
		float height = 140.0f; 
		
		GameObject button = (GameObject)Instantiate(buttonPrefab, buttonPrefab.transform.position, buttonPrefab.transform.rotation);
		Button buttonScript = button.GetComponent<Button>();
		buttonScript.initializeVariables("JUMP", sizeFixer.getRectFixResolution(x, y, width, height), jump);
		listButtons.Add(buttonScript);
		
		x = 200.0f; 
		y = 80.0f;
		width = 140.0f;
		height = 140.0f; 

		button = (GameObject)Instantiate(buttonPrefab, buttonPrefab.transform.position, buttonPrefab.transform.rotation);
		buttonScript = button.GetComponent<Button>();
		buttonScript.initializeVariables("ATTACK", sizeFixer.getRectFixResolution(x, y, width, height), attack);
		listButtons.Add(buttonScript);		
		
		x = 200.0f; 
		y = 240.0f;
		width = 140.0f;
		height = 140.0f; 
		
		button = (GameObject)Instantiate(buttonPrefab, buttonPrefab.transform.position, buttonPrefab.transform.rotation);
		buttonScript = button.GetComponent<Button>();
		buttonScript.initializeVariables("ATTACK1", sizeFixer.getRectFixResolution(x, y, width, height), attack1);
		listButtons.Add(buttonScript);
	}
	
	public override void checkEvents()
	{
		foreach ( Touch touch in Input.touches )
		{
			foreach( Button b in listButtons )
			{
				if ( b.containsPoint(touch.position) )
				{
					b.checkPressedState(touch);
				}
				else
				{
					b.updateTouchState();
				}
			}
		}
	}
}

