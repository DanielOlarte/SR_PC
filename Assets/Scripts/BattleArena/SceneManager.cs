using UnityEngine;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour {
	
	private float levelLenght = 40.0f;
	public GameObject playerPrefab;
	
	public Vector3 positionPlayer1 = new Vector3(-2.0f,0.0f,-1.0f);
	public Vector3 positionPlayer2 = new Vector3(2.0f,0.0f,-1.0f);
		
	private List<GameObject> playerList = new List<GameObject>();
	
	public float getLevelLenght()
	{
		return levelLenght;
	}
	
	// Use this for initialization
	void Start () 
	{
		instantiatePlayer(0,positionPlayer1,3.0f);	
		instantiatePlayer(1,positionPlayer2,6.0f);	
	}
	
	private void instantiatePlayer(int ID,Vector3 position,float speed)
	{
		GameObject player = (GameObject)Instantiate(playerPrefab,position,playerPrefab.transform.rotation);
		Player playerScript = player.GetComponent<Player>();
		playerScript.ID = ID;
		playerScript.playerSpeed = speed;
		playerList.Add(player);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnGUI() {
        GUI.Label(new Rect(10, 10, 50, 20), playerList[0].GetComponent<Player>().health.ToString());
		GUI.Label(new Rect(Screen.width-50, 10, 50, 20), playerList[1].GetComponent<Player>().health.ToString());
    }
	
	public List<GameObject> getPlayers()
	{
		return playerList;
	}
	
	public void reportHit(int playerID)
	{
		playerList[playerID].GetComponent<Player>().health-=5;
	}
}
