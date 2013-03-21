using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum PlayerKeys {RUN, JUMP, ATTACK, SPECIAL_ATTACK, QUIT}

public class InputManager : MonoBehaviour {
	
	public string idController;
	private string horizontalAxis;
	private string verticalAxis;
	public List<KeyCode> listKeys;

	virtual public void setController(string type)
	{
		idController = type;
		horizontalAxis = "Horizontal" + idController;
		verticalAxis = "Vertical" + idController;
	}
	
	virtual public void setKeys(List<KeyCode> list)
	{
		listKeys = list;
	}
	
	virtual public void addButtons(List<Button> buttons)
	{
	}
	
	virtual public float getHorizontalAxis()
	{
		return Input.GetAxis(horizontalAxis);
	}
	
	virtual public float getVerticallAxis()
	{
		return Input.GetAxis(verticalAxis);
	}
	
	virtual public bool getKey(PlayerKeys key)
	{
		return Input.GetKey(listKeys[(int)key]);
	}
	
	virtual public bool getKeyDown(PlayerKeys key)
	{
		return Input.GetKeyDown(listKeys[(int)key]);
	}
}
