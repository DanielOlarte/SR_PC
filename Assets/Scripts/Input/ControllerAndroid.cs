using UnityEngine;
using System;
using System.Collections.Generic;


public class ControllerAndroid : InputManager
{
	public float changeToRun = 0.95f;
	public float accelerometerUpdateInterval = 1.0f / 60.0f;
	public float lowPassKernelWidthInSeconds = 1.0f;
	public float shakeDetectionThreshold = 1.5f;
	public float sensitivityShake = 1.3f;
	
	private Joystick joystick;
	private List<Button> listButtons;

	private float lowPassFilterFactor; 
	private Vector3 lowPassValue = Vector3.zero;
	private Vector3 acceleration;
	private Vector3 deltaAcceleration;	
	
	void Start()
	{
		lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds; 
		shakeDetectionThreshold *= shakeDetectionThreshold;
		lowPassValue = Input.acceleration;
	}
	
	void Update()
	{
		acceleration = Input.acceleration*sensitivityShake;
		
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        deltaAcceleration = acceleration - lowPassValue;
		
		if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
		{
			listButtons[(int)PlayerKeys.SPECIAL_ATTACK - 1].setIsPressed(true);
			Debug.Log("Shake event detected at time "+Time.time);
		}
		else
		{
			listButtons[(int)PlayerKeys.SPECIAL_ATTACK - 1].setIsPressed(false);
		}
	}
	
	public override void setController(string id)
	{
		SceneManager sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		joystick = (Joystick)sceneManager.getAndroidController().GetComponent<Joystick>();
		idController = "Android";
		
		listButtons = new List<Button>();
	}
	
	public override void setKeys(List<KeyCode> list)
	{
		listKeys = list;
	}
	
	public override void addButtons(List<Button> buttons)
	{
		listButtons = buttons;
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
		if ( key == PlayerKeys.RUN )
		{
			if ( Mathf.Abs(joystick.getHorizontalAxis()) >= Mathf.Abs(changeToRun) )
			{
				return true;
			}
		}

		return false;
	}
	
	public override bool getKeyDown(PlayerKeys key)
	{
		if ( key == PlayerKeys.JUMP )
		{
			return listButtons[((int)key) - 1].wasPressed();
		}
		
		if ( key == PlayerKeys.ATTACK )
		{
			return listButtons[((int)key) - 1].wasPressed();
		}
		
		if ( key == PlayerKeys.SPECIAL_ATTACK )
		{
			return listButtons[((int)key) - 1].wasPressed();
		}
		
		return false;
	}
}


