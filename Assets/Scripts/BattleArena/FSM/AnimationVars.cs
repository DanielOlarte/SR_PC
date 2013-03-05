using System;
using System.Collections.Generic;

public class AnimationVars
{
	//spriteColumnSize,spriteRowSize,spriteColumnStart,spriteRowStart,spriteFrames,spriteFPS
	private static int[] falling = {9,8,0,7,3,8};
	private static int[] standing = {9,8,0,0,1,8};
	private static int[] walking = {9,8,1,0,8,8};
	private static int[] running = {9,8,1,2,6,8};
	private static int[] jumping = {9,8,0,1,6,8};
	private static int[] attacking = {9,8,0,5,5,8};
	
	public static Dictionary<PlayerStates, int[]> variables = new Dictionary<PlayerStates, int[]>()
	{
	    {PlayerStates.FALLING, falling},
	    {PlayerStates.STANDING, standing},
	    {PlayerStates.WALKING, walking},
		{PlayerStates.RUNNING, running},
		{PlayerStates.JUMPING, jumping},
	    {PlayerStates.ATTACKING, attacking}
	};
}

