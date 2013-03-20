using System;

using UnityEngine;
using System.Collections.Generic;

public class CustomGUI : MonoBehaviour
{
	public GameObject buttonPrefab;
	public InGameGUI inGameGUI;
	
	public Texture jump;
	public Texture attack;
	public Texture attack1;
	
	private SceneManager sceneManager;
	
	void Start()
	{
		sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		
		#if UNITY_ANDROID
			inGameGUI = GetComponent<InGameAndroidGUI>();
			inGameGUI.createButtons(buttonPrefab, jump, attack, attack1);
			
		#endif
		
		#if UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
			inGameGUI = GetComponent<InGameGUI>();
		#endif

	}
	
	void Update()
	{
		inGameGUI.checkEvents();
	}
	
	#if UNITY_ANDROID
		public void initializeButtonsAndroid()
		{
			sceneManager.getPlayers()[0].GetComponent<Player>().initializeButtonsAndroid(inGameGUI.getButtons());
			sceneManager.getPlayers()[1].GetComponent<Player>().initializeButtonsAndroid(inGameGUI.getButtons());
		}
	#endif

}

