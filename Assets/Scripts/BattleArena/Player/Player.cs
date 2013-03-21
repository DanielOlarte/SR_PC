using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	public PlayerCharacter playerCharacter;
	public int sceneID;																					      //(needs to be initialized)
	public float playerSpeed;				//walking speed														(needs to be initialized)
	public float jumpForce;					//jump force used to impulse the player up							(needs to be initialized)
	public float jumpXMovPerc;				//percentage of movility based on walking speed during air time		(needs to be initialized)
	public float runXMovPerc;				//percentage of movility based on walking speed during run			(needs to be initialized)
	public float strength;					//amount of dmg done by the player									(needs to be initialized)
	public float attackReach;				//how far the attack will reach
	public float stamina = 0.0f;			//not sure how to do this yet
	
	public float health = 100.0f;
	public float staminaMultiplier = 2.0f;	//multiplies the strength to add stamina on each hit				(needs to be initialized)
	
	public float tempInputChange = 1.0f;
	
	public GameObject jumpPrefab;
	
	private float speedMultiplier= 1.0f;	//changes the speed based on the state
		
	private AnimationSprite aniManager;		//animation class
	private bool inputHandled = true;		
	private InputManager inputManager;
	private bool attacking = false;
	private PlayerFSM fsm;
	private float direction = 1.0f;
	private SceneManager sceneManager;
	private PlayerAttack playerAttack;
	
	public AnimationSprite getAnimation(){return aniManager;}
	public PlayerFSM getFSM(){return fsm;}
	public float getDirection(){return direction;}
	public float getAttackReach(){return attackReach;}
	public SceneManager getSceneManager(){return sceneManager;}
	public void setAttacking(bool newAtValue){attacking=newAtValue;}
	
	
	void Start () 
	{
		sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		aniManager= GetComponent<AnimationSprite>();
		fsm = GetComponent<PlayerFSM>();
		playerAttack = GetComponent<PlayerAttack>();
	}
	
	void Update()
	{
		changeState();															//detect state changes
		if(attacking)															//if it is attacking
		{
			playerAttack.updateAttack();
		}
		else 																	//if not attacking
		{
			fsm.getCurrentState().animateState(aniManager,playerCharacter);		//and animate new state
		}
		changePlayerDirection();												//detect and change direction
	}
	
	
	
	void changePlayerDirection()
	{
		Vector3 scale =  transform.localScale;
		float lastDirection = direction;
		//turn left
		if(tempInputChange*inputManager.getHorizontalAxis()<0.0f)
		{	
			direction=-1.0f;				
		}
		//turn right
		else if(tempInputChange*inputManager.getHorizontalAxis() > 0.0f)
		{
			direction=1.0f;
		}	
		scale.x = direction*Mathf.Abs(scale.x);
		transform.localScale = scale;			
		if(direction!=lastDirection)
		{
			attacking=false;
		}	
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
			playerAttack.setActiveAttack(AttackTypes.NORMAL_ATTACK);
		}
		else if(inputManager.getKeyDown(PlayerKeys.SPECIAL_ATTACK))
		{
			attacking = true;
			playerAttack.setActiveAttack(AttackTypes.SPECIAL_ATTACK);
		}
		//detect jump, set speed multiplier and tell program we have to handle input
		else if(inputManager.getKeyDown(PlayerKeys.JUMP))
		{
			if(fsm.validateNewAction(PlayerActions.JUMP_INPUT))
			{
				inputHandled=false;
				speedMultiplier*=jumpXMovPerc;
				//particle test
				Vector3 pos = new Vector3(	transform.position.x+0.1f,
											transform.position.y-transform.localScale.y/3.0f,
											transform.position.z);
				Instantiate(jumpPrefab,pos,transform.rotation);
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
				(!fsm.getCurrentState().getID().Equals(PlayerStates.JUMPING)&&!fsm.getCurrentState().getID().Equals(PlayerStates.FALLING)))
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
		rigidbody.MovePosition( new Vector3(transform.position.x+movementX*Time.fixedDeltaTime,transform.position.y,transform.position.z));
		//Handle Double jump
		if( fsm.getCurrentState().getID().Equals(PlayerStates.JUMPING) || fsm.getCurrentState().getID().Equals(PlayerStates.DOUBLE_JUMPING) ||
			fsm.getCurrentState().getID().Equals(PlayerStates.FALL_JUMP))
		{	
			if( !inputHandled )
			{	
				rigidbody.velocity = new Vector3(rigidbody.velocity.x,0,rigidbody.velocity.z);
				rigidbody.AddForce(new Vector3(0,jumpForce,0),ForceMode.VelocityChange);
			}			
			inputHandled=true;
		}
	}
	void OnCollisionStay(Collision collisionInfo)
	{
		//reset double jump when you land on the floor
		if(collisionInfo.gameObject.tag.Equals("Floor")&&collisionInfo.contacts.Length!=0)
		{
			if(Mathf.RoundToInt( collisionInfo.contacts[0].normal.y)==1)
			{	
				fsm.validateNewAction(PlayerActions.LAND);
			}
			if(Mathf.RoundToInt( Mathf.Abs(collisionInfo.contacts[0].normal.x))==1)
			{
				rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,rigidbody.velocity.z);
			}
		}
	}
	void OnCollisionEnter(Collision collisionInfo)
	{		
		if(collisionInfo.gameObject.tag.Equals("Floor")&&collisionInfo.contacts.Length!=0)
		{
			if(Mathf.RoundToInt( Mathf.Abs(collisionInfo.contacts[0].normal.x))==1)
			{
				rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,rigidbody.velocity.z);
				fsm.validateNewAction(PlayerActions.FALL);
			}
			if(Mathf.RoundToInt( collisionInfo.contacts[0].normal.y)==1)
			{	
				fsm.setCurrentState(PlayerStates.STANDING);
			}
		}
	}
	
	public void initializeInputManager(string id, List<KeyCode> keys)
	{
		switch ( id )
		{
			case "Android":
			{
				inputManager = GetComponent<ControllerAndroid>();
				GetComponent<ControllerAndroid>().enabled = true;
				break;
			}
			default:
			{
				inputManager = GetComponent<InputManager>();
				break;
			}
		}
		inputManager.setController(id);
		inputManager.setKeys(keys);
	}
	
	#if UNITY_ANDROID
		public void initializeButtonsAndroid(List<Button> buttons)
		{
			inputManager.addButtons(buttons);
		}
	#endif
}
