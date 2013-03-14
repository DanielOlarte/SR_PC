using UnityEngine;
using System;
using System.Collections.Generic;


public class ControllerAndroid : InputManager
{
	private Joystick joystick;

	public override void setController(string id)
	{
		SceneManager sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		joystick = (Joystick)sceneManager.getAndroidController().GetComponent<Joystick>();
		idController = "Android";
	}
	
	public override void setKeys(List<KeyCode> list)
	{
		// TO-DO
	}
	
	public override float getHorizontalAxis()
	{
		return joystick.getHorizontalAxis();
	}
	
	public override float getVerticallAxis()
	{
		return joystick.getVerticalAxis();
	}
	
	public override bool getKey(PlayerKeys key)
	{
		return false;
		//return Input.GetKey(listKeys[(int)key]);
	}
	
	public override bool getKeyDown(PlayerKeys key)
	{
		return false;
		//return Input.GetKeyDown(listKeys[(int)key]);
	}
	
	
}


