using UnityEngine;
using System;
using System.Collections.Generic;

public class InGameGUI : MonoBehaviour
{
	public List<Button> listButtons;
	public SizeFixer sizeFixer;
	
	public List<Button> getButtons()
	{
		return listButtons;
	}
	
	virtual public void createButtons(GameObject buttonPrefab, Texture jump, Texture attack, Texture attack1)
	{
	}
	
	virtual public void checkEvents()
	{
	}
}

