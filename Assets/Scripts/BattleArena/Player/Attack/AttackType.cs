using UnityEngine;
using System.Collections;

public abstract class AttackType{
		
	protected PlayerAttack playerAttack;

	abstract public void updateAttack ();
	abstract public void checkAttack();
	
}
