using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	public Player player;
	public bool routine = false;
	public GameObject bloodPrefab;
	public GameObject specialPrefab;
	public AttackType activeAttack;
	public bool attackReported = false;
	public PlayerStates currentAttackAnimation = PlayerStates.S_ATTACKING;
	public bool attackAnimationFinished = true;
	public float attackTime = 0.6f;
		
	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
		activeAttack = new NormalAttack(this);
	}
	
	void Update()
	{
		if(routine)
		{
			StartCoroutine("attackCounter");												//start the attack coroutine
			return;
		}
		StopCoroutine("attackCounter");
	}
	
	public void updateAttack ()  
	{
		activeAttack.updateAttack();
	}
		
	IEnumerator attackCounter() 															//coroutine used as a timer for attack animation
	{  				
		Vector3 pos = player.transform.position;
		pos.x += player.getDirection()*(player.transform.localScale.x/2+1.0f);
		activeAttack.checkAttack();															//check and report attacks
        yield return new WaitForSeconds(attackTime);												//wait time		
		routine = false;
		attackAnimationFinished = true;														//and reset the booleans
		attackReported = false;			
        player.setAttacking(false);
    }
	
	public void instantiateBloodPrefab(Vector3 pos, Quaternion rot)
	{
		Instantiate(bloodPrefab,pos,rot);													//particle test		
	}
	
	public void instantiateSpecialPrefab(Vector3 pos, Quaternion rot, Vector3 force)
	{
		GameObject special = Instantiate(specialPrefab,pos,rot) as GameObject;	
		special.rigidbody.AddForce(player.getDirection()*force.x,force.y,force.z,ForceMode.Impulse);
	}
	
	public void setActiveAttack(AttackTypes type)
	{
		switch(type)
		{
			case AttackTypes.SPECIAL_ATTACK:
				activeAttack = new SpecialAttack(this);
				break;
			case AttackTypes.NORMAL_ATTACK:
				activeAttack = new NormalAttack(this);
				break;
		}
	}
}
