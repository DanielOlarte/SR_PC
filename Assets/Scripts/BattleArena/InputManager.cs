using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum PlayerKeys {RUN, JUMP, ATTACK}

public class InputManager : MonoBehaviour {
	
	public string idController;
	private string horizontalAxis;
	private string verticalAxis;
	public List<KeyCode> listKeys;

	public void setController(string type)
	{
		idController = type;
		horizontalAxis = "Horizontal" + idController;
		verticalAxis = "Vertical" + idController;
	}
	
	public void setKeys(List<KeyCode> list)
	{
		listKeys = list;
	}
	
	public float getHorizontalAxis()
	{
		return Input.GetAxis(horizontalAxis);
	}
	
	public float getVerticallAxis()
	{
		return Input.GetAxis(verticalAxis);
	}
	
	public bool getKey(PlayerKeys key)
	{
		return Input.GetKey(listKeys[(int)key]);
	}
	
	public bool getKeyDown(PlayerKeys key)
	{
		return Input.GetKeyDown(listKeys[(int)key]);
	}
}
