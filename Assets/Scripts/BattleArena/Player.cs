using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float playerSpeed = 3.0f;
	public float jumpForce = 5.5f;
	public float jumpXMovPerc = 0.7f;
	public float runXMovPerc = 1.4f;
	
	public float tempInputChange = 1.0f;
	
	private float speedMultiplier= 1.0f;
	
	private AnimationSprite animation;
	private int jumpCount=0;
	private bool inputHandled=true;
	private PlayerFSM fsm;
	
	public float getEffectiveSpeed(){
		return playerSpeed*speedMultiplier;
	}
	
	void Start () {
		animation= GetComponent<AnimationSprite>();
		fsm = new PlayerFSM();
	}
	
	void Update(){
		changeState();
		fsm.getCurrentState().animateState(animation);
		changePlayerDirection();
	}
	
	void changePlayerDirection(){
		//turn left
		if(tempInputChange*Input.GetAxis("Horizontal")<0.0f){	
			Vector3 scale =  transform.localScale;
			scale.x = -Mathf.Abs(scale.x);	
			transform.localScale = scale;
		}
		//turn right
		else if(tempInputChange*Input.GetAxis("Horizontal")>0.0f){
			Vector3 scale =  transform.localScale;
			scale.x = Mathf.Abs(scale.x);	
			transform.localScale = scale;				
		}
	}
	
	void changeState(){
		if(!inputHandled){
			return;
		}
		//detect jump, set speed multiplier and tell program we have to handle input
		if(Input.GetKeyDown(KeyCode.Space)){
			if(fsm.validateNewAction(PlayerActions.JUMP_INPUT)){
				inputHandled=false;
				speedMultiplier*=jumpXMovPerc;
			}
		}  
		//detect walking and reset speed multiplier
		else if(Mathf.RoundToInt(Input.GetAxis("Horizontal")*10)!=0 &&
				Mathf.RoundToInt(rigidbody.velocity.y)==0 && 
				!fsm.getCurrentState().getID().Equals(PlayerStates.JUMPING)){	
			
			speedMultiplier=1.0f;
			fsm.validateNewAction(PlayerActions.WALK_INPUT);
			//detect run and set speed multiplier
			if(Input.GetKey(KeyCode.LeftShift)){
				fsm.validateNewAction(PlayerActions.RUN_INPUT);
				speedMultiplier*=runXMovPerc;
			}

		}
		//detect fall
		else if(Mathf.RoundToInt(rigidbody.velocity.y)<0){
			fsm.validateNewAction(PlayerActions.FALL);
		}
		//detect player stoping and reset speed multiplier
		else if(Mathf.RoundToInt(Input.GetAxis("Horizontal")*10)==0 && 
				!fsm.getCurrentState().getID().Equals(PlayerStates.JUMPING)){
			fsm.validateNewAction(PlayerActions.STOP);
			speedMultiplier=1.0f;
		}
	}
	
	void FixedUpdate () {
		float movementX = Input.GetAxis("Horizontal")*playerSpeed*speedMultiplier*tempInputChange; 	//get input with default keys
		//move horizontally
		rigidbody.velocity = new Vector3(movementX,rigidbody.velocity.y,rigidbody.velocity.z);
		//Handle Double jump
		if(fsm.getCurrentState().getID().Equals(PlayerStates.JUMPING)){	
			if(jumpCount<2 && !inputHandled){					
				rigidbody.velocity = Vector3.zero;
				jumpCount+=1;
				rigidbody.AddForce(new Vector3(0,jumpForce,0),ForceMode.Impulse);
			}			
			inputHandled=true;
		}
	}
	void OnCollisionStay(Collision collisionInfo){
		//reset double jump when you land on the floor
		if(collisionInfo.gameObject.tag.Equals("Floor")){
			if(Mathf.RoundToInt( collisionInfo.contacts[0].normal.y)==1){				
				if(fsm.validateNewAction(PlayerActions.LAND)){
					jumpCount=0;
				}
			}
			else if(Mathf.RoundToInt( collisionInfo.contacts[0].normal.x)==1){
				fsm.validateNewAction(PlayerActions.FALL);
			}
		}
	}
}
