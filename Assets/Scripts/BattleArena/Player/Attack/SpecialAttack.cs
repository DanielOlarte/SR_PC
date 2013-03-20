using UnityEngine;
using System.Collections;

public class SpecialAttack : AttackType {
		
	public SpecialAttack(PlayerAttack newPAttack)  
	{
		playerAttack = newPAttack;
	}
	public override void updateAttack ()  
	{
		if(playerAttack.attackAnimationFinished)																		//check if the animation ended
		{
			playerAttack.currentAttackAnimation = playerAttack.player.getFSM().getCurrentState().getID();				//if it did set a new state to lock on
			playerAttack.attackAnimationFinished = false;																//and report it is now animating
			Vector3 pos = playerAttack.player.transform.position;
			pos.x += playerAttack.player.getDirection()*(1.0f);
			playerAttack.instantiateSpecialPrefab(pos,Quaternion.Euler(0,0,0),new Vector3(15.0f,0,0));
		}
		playerAttack.player.getAnimation().animateAttack(playerAttack.player.playerCharacter,playerAttack.currentAttackAnimation);	//animate with the animator class
		playerAttack.routine = true;
	}
	
	public override void checkAttack()
	{
		
	}
	
}
