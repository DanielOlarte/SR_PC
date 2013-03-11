using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	public int ID;
	public float playerSpeed = 3.0f;		//walking speed
	public float jumpForce = 5.5f;			//jump force used to impulse the player up
	public float jumpXMovPerc = 0.7f;		//percentage of movility based on walking speed during air time
	public float runXMovPerc = 1.4f;		//percentage of movility based on walking speed during run
	public float strength = 7.0f;			//amount of dmg done by the player
	public float stamina = 0.0f;			//not sure how to do this yet
	
	public float tempInputChange = 1.0f;
	
	private float speedMultiplier= 1.0f;
	
	public float health = 100;
	
	private InputManager inputManager;
	private AnimationSprite aniManager;
	private int jumpCount=0;
	private bool jumpXApplied = false;
	private bool inputHandled = true;
	private bool attacking = false;
	private bool attackReported = false;
	private PlayerFSM fsm;
	
	private SceneManager sceneManager;
	
	void Start () 
	{
		sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		aniManager= GetComponent<AnimationSprite>();
		fsm = new PlayerFSM();
	}
	
	void Update()
	{
		changeState();
		if(attacking)
		{
			int[] variables = AnimationVars.variables[PlayerStates.ATTACKING];
			aniManager.animateSprite(variables[0],variables[1],variables[2],variables[3],variables[4],variables[5]);
			StartCoroutine("attackCounter");
		}
		else
		{
			StopCoroutine("attackCounter");
			fsm.getCurrentState().animateState(aniManager);
		}
		changePlayerDirection();
	}
	
	IEnumerator attackCounter() {  
		
		RaycastHit hit;
		Vector3 direction = new Vector3(transform.localScale.x/Mathf.Abs(transform.localScale.x),0,0);
		Ray ray = new Ray(transform.position,direction);
		if(!attackReported && Physics.Raycast(ray,out hit, Mathf.Abs(transform.localScale.x/2)))
		{
			if(hit.collider.gameObject.tag.Equals("Player"))
			{
				int id = hit.collider.gameObject.GetComponent<Player>().ID;
				sceneManager.reportHit(id,strength);
				attackReported = true;
			}
		}
		//wait time
        yield return new WaitForSeconds(0.5f);
		attackReported = false;
        attacking=false;
    }
	
	void changePlayerDirection()
	{
		Vector3 scale =  transform.localScale;
		//turn left
		if(tempInputChange*inputManager.getHorizontalAxis()<0.0f)
		{	
			scale.x = -Mathf.Abs(scale.x);	
		}
		//turn right
		else if(tempInputChange*inputManager.getHorizontalAxis() > 0.0f)
		{
			scale.x = Mathf.Abs(scale.x);				
		}	
		transform.localScale = scale;
	}
	
	void changeState()
	{//Is there another way to detect the input instead all this if's?
		if(!inputHandled)
		{
			return;
		}
		if(inputManager.getKeyDown(PlayerKeys.ATTACK))
		{
			attacking = true;
		}
		//detect jump, set speed multiplier and tell program we have to handle input
		if(inputManager.getKeyDown(PlayerKeys.JUMP))
		{
			if(fsm.validateNewAction(PlayerActions.JUMP_INPUT))
			{
				inputHandled=false;
				speedMultiplier*=jumpXMovPerc;
			}
		}  
		//detect walking and reset speed multiplier
		else if(Mathf.RoundToInt(inputManager.getHorizontalAxis()*10)!=0 &&
				Mathf.RoundToInt(rigidbody.velocity.y)==0 && 
				!fsm.getCurrentState().getID().Equals(PlayerStates.JUMPING))
		{	
			
			speedMultiplier=1.0f;
			fsm.validateNewAction(PlayerActions.WALK_INPUT);
			//detect run and set speed multiplier
			if(inputManager.getKey(PlayerKeys.RUN))
			{
				fsm.validateNewAction(PlayerActions.RUN_INPUT);
				speedMultiplier*=runXMovPerc;
			}
		}
		//detect fall and set speedmultiplier
		else if(Mathf.RoundToInt(rigidbody.velocity.y)<0)
		{
			fsm.validateNewAction(PlayerActions.FALL);
			speedMultiplier=jumpXMovPerc;
		}
		//detect player stoping and reset speed multiplier
		else if(Mathf.RoundToInt(inputManager.getHorizontalAxis()*10)==0 && 
				!fsm.getCurrentState().getID().Equals(PlayerStates.JUMPING))
		{
			fsm.validateNewAction(PlayerActions.STOP);
			speedMultiplier=1.0f;
		}
	}
	
	void FixedUpdate () 
	{
		//get input with default keys
		float movementX = inputManager.getHorizontalAxis()*playerSpeed*speedMultiplier*tempInputChange; 	
		//move horizontally		
		rigidbody.velocity = new Vector3(movementX,rigidbody.velocity.y,rigidbody.velocity.z);
		//Handle Double jump
		if(fsm.getCurrentState().getID().Equals(PlayerStates.JUMPING))
		{	
			if(jumpCount<2 && !inputHandled)
			{					
				rigidbody.velocity = Vector3.zero;
				jumpCount+=1;
				rigidbody.AddForce(new Vector3(0,jumpForce,0),ForceMode.Impulse);
			}			
			inputHandled=true;
		}
	}
	void OnCollisionStay(Collision collisionInfo)
	{
		//reset double jump when you land on the floor
		if(collisionInfo.gameObject.tag.Equals("Floor"))
		{
			if(Mathf.RoundToInt( collisionInfo.contacts[0].normal.y)==1)
			{				
				if(fsm.validateNewAction(PlayerActions.LAND))
				{
					jumpCount=0;
				}
			}
			else if(Mathf.RoundToInt( collisionInfo.contacts[0].normal.x)==1)
			{
				fsm.validateNewAction(PlayerActions.FALL);
			}
		}
	}
	
	public void initializeInputManager(string id, List<KeyCode> keys)
	{
		inputManager = GetComponent<InputManager>();
		inputManager.setController(id);
		inputManager.setKeys(keys);
	}
}
