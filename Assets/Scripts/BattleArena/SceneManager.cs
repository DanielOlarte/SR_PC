using UnityEngine;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour {
	
	private List<string> controllersList;
	private float levelLenght = 40.0f;
	public GameObject playerPrefab;
	
	public Vector3 positionPlayer1 = new Vector3(-2.0f,0.0f,-1.0f);
	public Vector3 positionPlayer2 = new Vector3(2.0f,0.0f,-1.0f);
	
	public string idController1;
	public string idController2;
	
	public List<KeyCode> keysController1 = new List<KeyCode>(){KeyCode.Keypad0, 
															   KeyCode.Keypad1, 
															   KeyCode.Keypad2};
	public List<KeyCode> keysController2 = new List<KeyCode>(){KeyCode.LeftShift, 
															   KeyCode.Space, 
															   KeyCode.G};
	public List<KeyCode> keysControllerGP1 = new List<KeyCode>(){KeyCode.Joystick1Button5, 
															  	 KeyCode.Joystick1Button0, 
															   	 KeyCode.Joystick1Button2};
	public List<KeyCode> keysControllerGP2 = new List<KeyCode>(){KeyCode.Joystick2Button5, 
															  	 KeyCode.Joystick2Button0, 
															   	 KeyCode.Joystick2Button2};
		
	private List<GameObject> playerList = new List<GameObject>();
	
	public float getLevelLenght()
	{
		return levelLenght;
	}
	
	// Use this for initialization
	void Start () 
	{
		controllersList = new List<string>();

		instantiatePlayer(0, positionPlayer1, 5.0f, idController1, getKeysBasedOnController(idController1) );	
		instantiatePlayer(1, positionPlayer2, 6.0f, idController2, getKeysBasedOnController(idController2) );	
	}
	
	private void instantiatePlayer(int ID,Vector3 position, float speed, string idController, List<KeyCode> listKeys)
	{
		GameObject player = (GameObject)Instantiate(playerPrefab,position,playerPrefab.transform.rotation);
		Player playerScript = player.GetComponent<Player>();
		playerScript.ID = ID;
		playerScript.playerSpeed = speed;
		playerScript.initializeInputManager(idController, listKeys);
		playerList.Add(player);
		
		controllersList.Add(idController);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnGUI() {
        GUI.Label(new Rect(10, 10, 50, 20), "Player 1");
		GUI.HorizontalScrollbar(new Rect (60,10,200,20), 0, playerList[0].GetComponent<Player>().health,0, 100);
		GUI.Label(new Rect(Screen.width-50, 10, 50, 20), "Player 2");
		GUI.HorizontalScrollbar(new Rect (Screen.width-260,10,200,20), 0, playerList[1].GetComponent<Player>().health,0, 100);
    }
	
	public List<GameObject> getPlayers()
	{
		return playerList;
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
