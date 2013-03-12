using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public int ID; 																						      //(needs to be initialized)
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
	private AnimationSprite aniManager;
	private int jumpCount=0;
	private bool jumpXApplied = false;
	private bool inputHandled = true;
	private bool attacking = false;
	private bool attackReported = true;
	private PlayerFSM fsm;
	private float direction = 1.0f;
	
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
		//generate a raycast to check if there is someone inside striking distance
		RaycastHit hit;		
		Vector3 rayDirection = new Vector3(direction,0,0);
		Vector3 rayPosition = new Vector3(	transform.position.x,
											transform.position.y,
											transform.position.z);
		Ray ray = new Ray(rayPosition,rayDirection);
		//i decided the distance is half of the player cube size
		if(!attackReported && Physics.Raycast(ray,out hit, attackReach))
		{//if attack not reported and someone within distance
			if(hit.collider.gameObject.tag.Equals("Player"))
			{//and if is a player (did 2 if's just to make it more understandable))
				int id = hit.collider.gameObject.GetComponent<Player>().ID;		//check player id
				sceneManager.reportHit(id,strength);							//report the hit to the scene manager
				attackReported = true;											//inform self that is not necessary to report again
				stamina += strength * staminaMultiplier;						//set new stamina value
				if(stamina>100.0f){stamina=100.0f;}								//set stamina limit
				Instantiate(bloodPrefab,hit.transform.position,hit.transform.rotation);
				
				if(ID==1){
					print (attacking);
				}
			}
		}
		//wait time
        yield return new WaitForSeconds(0.5f);
		//and reset the booleans
		attackReported = true;
        attacking=false;
    }
	
	void changePlayerDirection()
	{
		Vector3 scale =  transform.localScale;
		float lastDirection = direction;
		//turn left
		if(tempInputChange*inputManager.getHorizontalAxis()<0.0f)
		{	direction=-1.0f;
				
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
			attackReported = true;
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
			if(ID==1)
			{
				print ("now!");
			}
			attacking = true;
			attackReported = false;
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
		else if(Mathf.RoundToInt(Input.GetAxis("Horizontal")*10)==0 && 
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
		rigidbody.velocity = new Vector3(movementX,rigidbody.velocity.y,rigidbody.velocity.z);
		//Handle Double jump
		if( fsm.getCurrentState().getID().Equals(PlayerStates.JUMPING) || fsm.getCurrentState().getID().Equals(PlayerStates.DOUBLE_JUMPING) ||
			fsm.getCurrentState().getID().Equals(PlayerStates.FALL_JUMP))
		{	
			if( !inputHandled )
			{					
				Vector3 pos = new Vector3(transform.position.x,transform.position.y-transform.localScale.y/2,transform.position.z);
				Instantiate(jumpPrefab,pos,transform.rotation);
				rigidbody.velocity = Vector3.zero;
				rigidbody.AddForce(new Vector3(0,jumpForce,0),ForceMode.Impulse);
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
