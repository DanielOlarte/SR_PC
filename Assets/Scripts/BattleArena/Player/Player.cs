using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	public PlayerCharacter playerCharacter;
	public int sceneID;																					      //(needs to be initialized)
	public float playerSpeed = 3.0f;		//walking speed														(needs to be initialized)
	public float jumpForce = 5.5f;			//jump force used to impulse the player up							(needs to be initialized)
	public float jumpXMovPerc = 0.7f;		//percentage of movility based on walking speed during air time		(needs to be initialized)
	public float runXMovPerc = 1.4f;		//percentage of movility based on walking speed during run			(needs to be initialized)
	public float strength = 7.0f;			//amount of dmg done by the player									(needs to be initialized)
	public float stamina = 0.0f;			//not sure how to do this yet
	public float attackReach = 0.6f;		//how far the attack will reach
	
	public float health = 100.0f;
	public float staminaMultiplier = 2.0f;	//multiplies the strength to add stamina on each hit				(needs to be initialized)
	
	public float tempInputChange = 1.0f;
	
	public GameObject jumpPrefab;
	public GameObject bloodPrefab;
	
	private float speedMultiplier= 1.0f;	//changes the speed based on the state
		
	private AnimationSprite aniManager;		//animation class
	private bool inputHandled = true;		
	private InputManager inputManager;
	private bool attacking = false;
	private bool attackReported = false;
	private PlayerFSM fsm;
	private float direction = 1.0f;
	private SceneManager sceneManager;
	private bool attackAnimationFinished = true;
	private PlayerStates currentAttackAnimation = PlayerStates.S_ATTACKING;
		
	void Start () 
	{
		sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		aniManager= GetComponent<AnimationSprite>();
		fsm = GetComponent<PlayerFSM>();
	}
	
	void Update()
	{
		changeState();															//detect state changes
		if(attacking)															//if it is attacking
		{
			if(attackAnimationFinished)											//check if the animation ended
			{
				currentAttackAnimation = fsm.getCurrentState().getID();			//if it did set a new state to lock on
				attackAnimationFinished = false;								//and report it is now animating
			}
			aniManager.animateAttack(playerCharacter,currentAttackAnimation);	//animate with the animator class
			StartCoroutine("attackCounter");									//start the attack coroutine
		}
		else 																	//if not attacking
		{
			StopCoroutine("attackCounter");										//stop coroutine if changing state
			fsm.getCurrentState().animateState(aniManager,playerCharacter);		//and animate new state
		}
		changePlayerDirection();												//detect and change direction
	}
	
	private RaycastHit attackRaycast(float yOffset)
	{
		//generate a raycast to check if there is someone inside striking distance
		RaycastHit 	hit;
		Vector3 rayDirection = new Vector3(direction,0,0);
		Vector3 rayPosition = new Vector3(transform.position.x,transform.position.y+yOffset,transform.position.z);
		Ray ray = new Ray(rayPosition,rayDirection);
		Physics.Raycast(ray,out hit, attackReach);								//check if there is collision within the specified reach
		return hit;																//return information
	}
	
	private void checkAttack()
	{
		float[] offsets = {transform.localScale.y/3,0,-transform.localScale.y/3};  //y positions of the 3 raycast 
		RaycastHit 	hit;
		bool found = false;															//initialize found bool
		for(int i=0;i<3;i++)														//for the 3 rays
		{
			if(attackRaycast(offsets[i]).collider!=null)							//if a collision was founf
			{
				if(attackRaycast(offsets[i]).collider.gameObject.tag.Equals("Player"))//and it is a player
				{
					hit = attackRaycast(offsets[i]);								//asign the information
					found=true;														//report it was found
					break;															//break for we dont need more information
				}
			}
		}
		if(!attackReported&&found)
		{//if attack not reported and someone within distance
			
			if(hit.collider.gameObject.tag.Equals("Player"))
			{//and if is a player (did 2 if's just to make it more understandable))
				int id = hit.collider.gameObject.GetComponent<Player>().sceneID;			//check player id in the scene
				sceneManager.reportHit(id,strength);										//report the hit to the scene manager
				
				attackReported = true;														//inform self that is not necessary to report again
				
				stamina += strength * staminaMultiplier;									//set new stamina value
				if(stamina>100.0f){stamina=100.0f;}											//set stamina limit
				
				Instantiate(bloodPrefab,hit.transform.position,hit.transform.rotation);		//particle test		
			}
		}
	}
	
	IEnumerator attackCounter() 															//coroutine used as a timer for attack animation
	{  				
		checkAttack();																		//check and report attacks
        yield return new WaitForSeconds(0.6f);												//wait time		
		attackAnimationFinished = true;														//and reset the booleans
		attackReported = false;			
        attacking=false;
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
			attackReported = false;
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
	void OnCollisionEnter(Collision collisionInfo)
	{
		//reset double jump when you land on the floor
		if(collisionInfo.gameObject.tag.Equals("Floor"))
		{
			if(Mathf.RoundToInt( collisionInfo.contacts[0].normal.y)==1)
			{	
				fsm.validateNewAction(PlayerActions.LAND);
			}
			else if(Mathf.CeilToInt( Mathf.Abs(collisionInfo.contacts[0].normal.x))==1)
			{
				rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,rigidbody.velocity.z);
				fsm.validateNewAction(PlayerActions.FALL);
			}
		}
	}
	void OnCollisionStay(Collision collisionInfo)
	{
		if(collisionInfo.gameObject.tag.Equals("Floor"))
		{
			if(Mathf.CeilToInt( Mathf.Abs(collisionInfo.contacts[0].normal.x))==1)
			{
				rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,rigidbody.velocity.z);
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
