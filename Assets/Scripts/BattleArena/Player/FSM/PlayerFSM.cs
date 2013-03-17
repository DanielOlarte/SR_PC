using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerFSM
{
	private List<FSMState> fsmStates;
	private PlayerStates currentState = PlayerStates.FALLING;
	
	public PlayerFSM ()
	{
		fsmStates = new List<FSMState>();
		initializeDefStates();
	}
	public bool validateNewAction(PlayerActions newAction)
	{
		
		FSMState result = getCurrentState();
		
		PlayerStates newState = result.validateNewAction(newAction);
		
		if(newState.Equals(PlayerStates.NULL))
		{
			return false;	
		}
		else
		{
			if ( newState == PlayerStates.DOUBLE_JUMPING )
			{
				Debug.Log("dobl");
			}
			currentState=newState;
			return true;	
		}
	}
	
	public FSMState getCurrentState()
	{
		return fsmStates.Find(
            delegate(FSMState st)
            {
                return st.getID() == currentState;
            }
        );
	}
	
	public void initializeDefStates()
	{//should this be hardcoded?
		FSMState fall = new FSMState(PlayerStates.FALLING);
		fall.addTransition(PlayerActions.LAND,PlayerStates.STANDING);
		fall.addTransition(PlayerActions.JUMP_INPUT,PlayerStates.FALL_JUMP);
		fsmStates.Add(fall);
		
		FSMState stand = new FSMState(PlayerStates.STANDING);
		stand.addTransition(PlayerActions.WALK_INPUT,PlayerStates.WALKING);
		stand.addTransition(PlayerActions.RUN_INPUT,PlayerStates.RUNNING);
		stand.addTransition(PlayerActions.JUMP_INPUT,PlayerStates.JUMPING);
		fsmStates.Add(stand);
		
		FSMState walk = new FSMState(PlayerStates.WALKING);
		walk.addTransition(PlayerActions.RUN_INPUT,PlayerStates.RUNNING);
		walk.addTransition(PlayerActions.JUMP_INPUT,PlayerStates.JUMPING);
		walk.addTransition(PlayerActions.STOP,PlayerStates.STANDING);
		walk.addTransition(PlayerActions.FALL,PlayerStates.FALLING);
		fsmStates.Add(walk);
		
		FSMState run = new FSMState(PlayerStates.RUNNING);
		run.addTransition(PlayerActions.WALK_INPUT,PlayerStates.WALKING);
		run.addTransition(PlayerActions.JUMP_INPUT,PlayerStates.JUMPING);
		run.addTransition(PlayerActions.STOP,PlayerStates.STANDING);
		run.addTransition(PlayerActions.FALL,PlayerStates.FALLING);
		fsmStates.Add(run);
		
		FSMState jump = new FSMState(PlayerStates.JUMPING);
		jump.addTransition(PlayerActions.FALL,PlayerStates.FALLING);		
		jump.addTransition(PlayerActions.JUMP_INPUT,PlayerStates.DOUBLE_JUMPING);	
		fsmStates.Add(jump);
		
		FSMState double_jump = new FSMState(PlayerStates.DOUBLE_JUMPING);
		double_jump.addTransition(PlayerActions.FALL,PlayerStates.SERIOUSLY_FALLING);		
		fsmStates.Add(double_jump);
		
		FSMState fall_jump = new FSMState(PlayerStates.FALL_JUMP);
		fall_jump.addTransition(PlayerActions.FALL,PlayerStates.SERIOUSLY_FALLING);			
		fsmStates.Add(fall_jump);
		
		FSMState serious_fall = new FSMState(PlayerStates.SERIOUSLY_FALLING);
		serious_fall.addTransition(PlayerActions.LAND,PlayerStates.STANDING);			
		fsmStates.Add(serious_fall);
		
	}
}

