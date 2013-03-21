using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour {
	
	public const int MAX_WINNER_ROUNDS = 2;
	
	private List<string> controllersList;
	private float levelLenght = 80.0f;
	public GameObject playerPrefab;
	public GameObject controllerAndroidPrefab;
	
	public Vector3 positionPlayer1 = new Vector3(-2.0f,0.0f,-1.0f);
	public Vector3 positionPlayer2 = new Vector3(2.0f,0.0f,-1.0f);
	
	public string idController1 = "Keyboard1";
	public string idController2 = "Keyboard2";
	
	public AnimationVars variables;
	
	public List<KeyCode> keysController1 = new List<KeyCode>(){KeyCode.Keypad0, 
															   KeyCode.Keypad1, 
															   KeyCode.Keypad2,
															   KeyCode.Keypad3};
	public List<KeyCode> keysController2 = new List<KeyCode>(){KeyCode.LeftShift, 
															   KeyCode.Space, 
															   KeyCode.F,
															   KeyCode.E};
	public List<KeyCode> keysControllerGP1 = new List<KeyCode>(){KeyCode.Joystick1Button5, 
															  	 KeyCode.Joystick1Button0, 
															   	 KeyCode.Joystick1Button2};
	public List<KeyCode> keysControllerGP2 = new List<KeyCode>(){KeyCode.Joystick2Button5, 
															  	 KeyCode.Joystick2Button0, 
															   	 KeyCode.Joystick2Button2};
	
	private List<GameObject> playerList = new List<GameObject>();
	private GameObject androidController;
	
	private string winnerFight, winnerRound;
	private static int roundWinCharacter1 = 0;
	private static int roundWinCharacter2 = 0;
	
	public float getLevelLenght()
	{
		return levelLenght;
	}
	
	// Use this for initialization
	void Start () 
	{
		controllersList = new List<string>();
		variables = GetComponent<AnimationVars>();
		
		#if UNITY_ANDROID
			idController1 = "Android";
			instantiateControllerAndroid();
		
			instantiatePlayer(0,PlayerCharacter.SURICATTA , positionPlayer1,idController1, getKeysBasedOnController(idController1) );	
			instantiatePlayer(1,PlayerCharacter.PANDA , positionPlayer2, idController1, getKeysBasedOnController(idController1) );	
		
			CustomGUI gui = GameObject.Find ("CustomGUI").GetComponent<CustomGUI>();
			gui.initializeButtonsAndroid();
		#endif
		
		#if UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
			instantiatePlayer(0,PlayerCharacter.SURICATTA , positionPlayer1,idController1, getKeysBasedOnController(idController1) );	
			instantiatePlayer(1,PlayerCharacter.PANDA , positionPlayer2,  idController2, getKeysBasedOnController(idController2) );		
		#endif
		
		winnerFight = "No Winner Yet";
		winnerRound = "No Winner Yet";
	}
	
	void Update()
	{
		checkEndRound();
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
		// Rounds
		GUI.Label (new Rect(300, 10, 50, 20), roundWinCharacter1.ToString());
		GUI.Label(new Rect(Screen.width-300, 10, 50, 20), roundWinCharacter2.ToString());
		// Winner
		if ( winnerFight != "No Winner Yet")
		{
			GUI.Label ( new Rect(350, 200, 50, 20), winnerFight);
		}
		if ( winnerRound != "No Winner Yet")
		{
			GUI.Label ( new Rect(350, 200, 50, 20), winnerRound);
		}
    }
	
	private void instantiatePlayer(int sceneID,PlayerCharacter playerCharacter,Vector3 position,string idController, List<KeyCode> listKeys)
	{
		GameObject player = (GameObject)Instantiate(playerPrefab,position,playerPrefab.transform.rotation);
		player.renderer.material = variables.characters[playerCharacter].spritesheet;		
		player.transform.localScale = variables.characters[playerCharacter].playerScale;		
		Player playerScript = player.GetComponent<Player>();
		playerScript.sceneID = sceneID;
		playerScript.strength = variables.characters[playerCharacter].strength;
		playerScript.jumpXMovPerc = variables.characters[playerCharacter].jumpXMovPerc;
		playerScript.runXMovPerc = variables.characters[playerCharacter].runXMovPerc;
		playerScript.playerCharacter = playerCharacter;
		playerScript.attackReach=variables.characters[playerCharacter].attackReach;
		playerScript.playerSpeed = variables.characters[playerCharacter].playerSpeed;
		playerScript.jumpForce=variables.characters[playerCharacter].jumpForce;
		playerScript.initializeInputManager(idController, listKeys);
		playerList.Add(player);
		
		controllersList.Add(idController);
	}
	
	private void instantiateControllerAndroid()
	{
		androidController = (GameObject)Instantiate (controllerAndroidPrefab, new Vector3(0, 0, 0), controllerAndroidPrefab.transform.rotation);
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

	private void checkEndRound()
	{
		if ( winnerRound == "No Winner Yet" )
		{
			if ( playerList[0].GetComponent<Player>().health <= 0.0f )
			{
				roundWinCharacter2 += 1;
				winnerRound = "Player2";
				if ( roundWinCharacter2 == MAX_WINNER_ROUNDS)
				{
					winnerFight = "Player2";
				}
			}
			else if ( playerList[1].GetComponent<Player>().health <= 0.0f )
			{
				roundWinCharacter1 += 1;
				winnerRound = "Player1";
				if ( roundWinCharacter1 == MAX_WINNER_ROUNDS)
				{
					winnerFight = "Player1";
				}
			}
		}
		
		if ( winnerFight != "No Winner Yet" ) 
		{
			StartCoroutine( startWait(4.0f, "Fight") );
			playerList[0].GetComponent<Player>().enabled = false;
			playerList[1].GetComponent<Player>().enabled = false;
		}
		else if ( winnerRound != "No Winner Yet" ) 
		{
			StartCoroutine( startWait(2.0f, "Round") );
			playerList[0].GetComponent<Player>().enabled = false;
			playerList[1].GetComponent<Player>().enabled = false;
		}
	}
	
	private IEnumerator startWait(float seconds, string winnerState)
	{
		yield return StartCoroutine( waitSeconds(seconds) );
		
		if ( winnerState == "Fight" )
		{
			roundWinCharacter1 = 0;
			roundWinCharacter2 = 0;
		}
	}
	
	private IEnumerator waitSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Application.LoadLevel(Application.loadedLevelName);
	}
}
