using UnityEngine;
using System.Collections.Generic;

public class SizeFixer
{
	public float defWidth = 1920.0f; // Optimal resolution
	public float defHeight = 1080.0f;
	
	public Rect getRectFixResolution(float x, float y, float w, float h)
	{
		float widthScale = Screen.width/defWidth;
		float heightScale = Screen.height/defHeight;
		
		float left = (float)Screen.width - ( x*widthScale );
		float right = y*heightScale;
		
		return new Rect(left, right, w*widthScale , h*heightScale);
	}
}


