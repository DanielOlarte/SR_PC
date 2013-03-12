using UnityEngine;
using System.Collections;

public class ParticleDestroyer : MonoBehaviour {
	
	public float lifeTime;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine(counter());
	}
	IEnumerator counter(){
		yield return new WaitForSeconds(lifeTime);
		Destroy(gameObject);
	}
}
