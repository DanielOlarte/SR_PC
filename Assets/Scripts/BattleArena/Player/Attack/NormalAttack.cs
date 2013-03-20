using UnityEngine;
using System.Collections;

public class NormalAttack : AttackType {
			
	public NormalAttack(PlayerAttack newPAttack)  
	{
		playerAttack = newPAttack;
	}
	
	public override void updateAttack ()  
	{
		if(playerAttack.attackAnimationFinished)																		//check if the animation ended
		{
			playerAttack.currentAttackAnimation = playerAttack.player.getFSM().getCurrentState().getID();				//if it did set a new state to lock on
			playerAttack.attackAnimationFinished = false;																//and report it is now animating
		}
		playerAttack.player.getAnimation().animateAttack(playerAttack.player.playerCharacter,playerAttack.currentAttackAnimation);	//animate with the animator class
		playerAttack.routine = true;
	}
	
	public override void checkAttack()
	{
		float[] offsets = {playerAttack.player.transform.localScale.y/3,0,-playerAttack.player.transform.localScale.y/3};  //y positions of the 3 raycast 
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
		if(!playerAttack.attackReported&&found)
		{//if attack not reported and someone within distance
			
			if(hit.collider.gameObject.tag.Equals("Player"))
			{//and if is a player (did 2 if's just to make it more understandable))
				int id = hit.collider.gameObject.GetComponent<Player>().sceneID;			//check player id in the scene
				playerAttack.player.getSceneManager().reportHit(id,playerAttack.player.strength);	//report the hit to the scene manager
				
				playerAttack.attackReported = true;														//inform self that is not necessary to report again
				
				playerAttack.player.stamina += playerAttack.player.strength * playerAttack.player.staminaMultiplier;//set new stamina value
				if(playerAttack.player.stamina>100.0f){playerAttack.player.stamina=100.0f;}							//set stamina limit
				Vector3 pos = hit.transform.position;
				pos.z-=1.0f;
				playerAttack.instantiateBloodPrefab(pos,Quaternion.Euler(0,-90.0f*playerAttack.player.getDirection(),0));
			}
		}
	}
	
	public RaycastHit attackRaycast(float yOffset)
	{
		//generate a raycast to check if there is someone inside striking distance
		RaycastHit 	hit;
		Vector3 rayDirection = new Vector3(playerAttack.player.getDirection(),0,0);
		Vector3 rayPosition = new Vector3(	playerAttack.player.transform.position.x,
											playerAttack.player.transform.position.y+yOffset,
											playerAttack.player.transform.position.z);
		Ray ray = new Ray(rayPosition,rayDirection);
		Physics.Raycast(ray,out hit, playerAttack.player.getAttackReach());								//check if there is collision within the specified reach
		return hit;																			//return information
	}
	
}
