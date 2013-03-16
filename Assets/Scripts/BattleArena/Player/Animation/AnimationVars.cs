using UnityEngine;
using System;
using System.Collections.Generic;

public class AnimationVars : MonoBehaviour
{	
	public Dictionary<PlayerCharacter, PAnimation> characters = new Dictionary<PlayerCharacter, PAnimation>();
	
	public Material suricattaSpritesheet;
	public Material pandaSpritesheet;
	
	private SuriAnimation suricatta;
	private PandaAnimation panda;
	
	
	void Start()
	{
		suricatta = new SuriAnimation(suricattaSpritesheet);
		panda = new PandaAnimation(pandaSpritesheet);
		
		characters.Add(PlayerCharacter.SURICATTA,suricatta);
		characters.Add(PlayerCharacter.PANDA,panda);
	}
	
	public int[] getVariables(PlayerCharacter playerCharacter, PlayerStates playerState)
	{
		Dictionary<PlayerStates, int[]> vars = characters[playerCharacter].variables;
		return vars[playerState];
	}
}