using UnityEngine;
using System.Collections;

public class PandaAnimation : PAnimation
{
	//spriteColumnSize,spriteRowSize,spriteColumnStart,spriteRowStart,spriteFrames,spriteFPS
	private int[] falling = {9,8,0,7,3,8};
	private int[] standing = {9,8,0,0,1,8};
	private int[] walking = {9,8,1,0,8,8};
	private int[] running = {9,8,1,2,8,8};
	private int[] jumping = {9,8,0,1,6,8};
	private int[] w_attacking = {9,8,0,4,6,10};
	private int[] r_attacking = {9,8,0,6,6,10};
	private int[] j_attacking = {9,8,0,5,6,10};
	private int[] s_attacking = {9,8,0,3,6,10};
	
	public PandaAnimation(Material spriteMaterial):base(spriteMaterial)
	{
		variables.Add(PlayerStates.FALLING, falling);
		variables.Add(PlayerStates.STANDING, standing);
		variables.Add(PlayerStates.WALKING, walking);
		variables.Add(PlayerStates.RUNNING, running);
		variables.Add(PlayerStates.JUMPING, jumping);
		variables.Add(PlayerStates.SERIOUSLY_FALLING,falling);
		variables.Add(PlayerStates.DOUBLE_JUMPING, jumping);
		variables.Add(PlayerStates.FALL_JUMP, jumping);
		variables.Add(PlayerStates.W_ATTACKING, w_attacking);
		variables.Add(PlayerStates.R_ATTACKING, r_attacking);
		variables.Add(PlayerStates.J_ATTACKING, j_attacking);
		variables.Add(PlayerStates.S_ATTACKING, s_attacking);
	}
}

