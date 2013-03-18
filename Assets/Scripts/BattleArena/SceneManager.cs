using UnityEngine;
using System.Collections.Generic;


public class SceneManager : MonoBehaviour {
	
	private List<string> controllersList;
	private float levelLenght = 40.0f;
	public GameObject playerPrefab;
	public GameObject controllerAndroidPrefab;
	
	public Vector3 positionPlayer1 = new Vector3(-2.0f,0.0f,-1.0f);
	public Vector3 positionPlayer2 = new Vector3(2.0f,0.0f,-1.0f);
	
	public string idController1 = "Keyboard1";
	public string idController2 = "Keyboard2";
	
	public AnimationVars variables;
	
	public List<KeyCode> keysController1 = new List<KeyCode>(){KeyCode.Keypad0, 
															   KeyCode.Keypad1, 
															   KeyCode.Keypad2};
	public List<KeyCode> keysController2 = new List<KeyCode>(){KeyCode.LeftShift, 
															   KeyCode.Space, 
															   KeyCode.F};
	public List<KeyCode> keysControllerGP1 = new List<KeyCode>(){KeyCode.Joystick1Button5, 
															  	 KeyCode.Joystick1Button0, 
															   	 KeyCode.Joystick1Button2};
	public List<KeyCode> keysControllerGP2 = new List<KeyCode>(){KeyCode.Joystick2Button5, 
															  	 KeyCode.Joystick2Button0, 
															   	 KeyCode.Joystick2Button2};
	
	private List<GameObject> playerList = new List<GameObject>();
	private GameObject androidController;
	
	public float getLevelLenght()
	{
		return levelLenght;
	}
	
	// Use this for initialization
	void Start () 
	{
		controllersList = new List<string>();
		
		#if UNITY_ANDROID
			idController1 = "Android";
			instantiateControllerAndroid();
			instantiatePlayer(0,PlayerCharacter.SURICATTA , positionPlayer1, 5.0f, new Vector3(1.82f,1.0f,1.0f),idController1, getKeysBasedOnController(idController1) );	
			instantiatePlayer(1,PlayerCharacter.PANDA , positionPlayer2, 6.0f, new Vector3(1.93f,1.5f,1.0f),idController1, getKeysBasedOnController(idController1) );	
		
			CustomGUI gui = GameObject.Find ("CustomGUI").GetComponent<CustomGUI>();
			gui.initializeButtonsAndroid();
		#endif
		
		#if UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
			instantiatePlayer(0,PlayerCharacter.SURICATTA , positionPlayer1, 5.0f, new Vector3(1.82f,1.0f,1.0f),idController1, getKeysBasedOnController(idController1) );	
			instantiatePlayer(1,PlayerCharacter.PANDA , positionPlayer2, 6.0f, new Vector3(1.93f,1.5f,1.0f),idController2, getKeysBasedOnController(idController2) );		
		#endif

	}
	
	private void instantiatePlayer(int sceneID,PlayerCharacter playerCharacter,Vector3 position, float speed, Vector3 scale, string idController, List<KeyCode> listKeys)
	{
		GameObject player = (GameObject)Instantiate(playerPrefab,position,playerPrefab.transform.rotation);
		player.renderer.material = variables.characters[playerCharacter].spritesheet;		
		player.transform.localScale = scale;		
		Player playerScript = player.GetComponent<Player>();
		playerScript.sceneID = sceneID;
		playerScript.playerCharacter = playerCharacter;
		playerScript.playerSpeed = speed;
		playerScript.initializeInputManager(idController, listKeys);
		playerList.Add(player);
		
		controllersList.Add(idController);
	}
	
	private void instantiateControllerAndroid()
	{
		androidController = (GameObject)Instantiate (controllerAndroidPrefab, new Vector3(0, 0, 0), controllerAndroidPrefab.transform.rotation);
	}
		
	void OnGUI() {
		//Health
        GUI.Label(new Rect(10, 10, 50, 20), "Player 1");
		GUI.HorizontalScrollbar(new Rect (60,10,200,20), 0, playerList[0].GetComponent<Player>().health,0, 100);
		GUI.Label(new Rect(Screen.width-50, 10, 50, 20), "Player 2");
		GUI.HorizontalScrollbar(new Rect (Screen.width-260,10,200,20), 0, playerList[1].GetComponent<Player>().health,0, 100);
		//Stamina
		GUI.Label(new Rect(10, Screen.height-20, 50, 20), "Stamina");
		GUI.HorizontalScrollbar(new Rect (60,Screen.height-20,200,20), 0, playerList[0].GetComponent<Player>().stamina,0, 100);
		GUI.Label(new Rect(Screen.width-50, Screen.height-20, 50, 20), "Stamina");
		GUI.HorizontalScrollbar(new Rect (Screen.width-260,Screen.height-20,200,20), 0, playerList[1].GetComponent<Player>().stamina,0, 100);
    }
	
	public List<GameObject> getPlayers()
	{
		return playerList;
	}
	
	public GameObject getAndroidController()
	{
		return androidController;
	}
	
	public void reportHit(int playerHittedID,float hitStrength)
	{
		playerList[playerHittedID].GetComponent<Player>().health-=hitStrength;
	}
	
	public List<KeyCode> getKeysBasedOnController(string id)
	{
		switch ( id )
		{
			case "Keyboard1":
			{
				return keysController1;
			}
			case "Keyboard2":
			{
				return keysController2;
			}
			case "Controller1":
			{
				return keysControllerGP1;
			}
			case "Controller2":
			{
				return keysControllerGP2;
			}
		}
		
		return new List<KeyCode>();
	}
}
