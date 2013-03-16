using UnityEngine;
using System.Collections.Generic;

public class PAnimation {

	private int[] falling;
	private int[] standing;
	private int[] walking;
	private int[] running;
	private int[] jumping;	
	private int[] w_attacking;
	private int[] r_attacking;
	private int[] j_attacking;
	private int[] s_attacking;
	
	public Dictionary<PlayerStates, int[]> variables = new Dictionary<PlayerStates, int[]>();
	
	public Material spritesheet;
	
	public PAnimation(Material spriteMaterial)
	{
		spritesheet = spriteMaterial;
	}
	
}
