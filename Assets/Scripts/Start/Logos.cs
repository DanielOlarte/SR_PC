using UnityEngine;
using System.Collections;

public class Logos : MonoBehaviour {
	
	public Texture2D logo;
	public string nextScene;
	
	private float _alpha = 1F;
	private bool fading = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine(changeBackground());
		if(fading){
			_alpha = Mathf.Lerp(_alpha, 0F, Time.deltaTime*6);
		}
	}
	
	void OnGUI(){
		GUI.color = new Color(_alpha,_alpha,_alpha,1.0f);
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),logo);	
	}
	IEnumerator changeBackground(){	
		yield return new WaitForSeconds(3.0f);
		fading=true;
		yield return new WaitForSeconds(0.7f);
		Application.LoadLevel(nextScene);
	}
}
