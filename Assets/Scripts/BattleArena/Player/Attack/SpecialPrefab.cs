using UnityEngine;
using System.Collections;

public class SpecialPrefab : MonoBehaviour {
	
	public float attackStrenght = 20.0f;
	public GameObject explosionPrefab;
	
	private bool destroy = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(destroy)
		{
			StartCoroutine("destroyer");
		}
	}
	
	void OnTriggerEnter(Collider collisionInfo)
	{
		if(collisionInfo.gameObject.tag.Equals("Player"))
		{
			Player player = collisionInfo.gameObject.GetComponent<Player>();
			player.health-=attackStrenght;
			destroy=true;
		}
	}
	
	IEnumerator destroyer()
	{
		Vector3 pos = transform.position;
		pos.z = 0.2f;
		Instantiate(explosionPrefab,pos,transform.rotation);
		yield return new WaitForSeconds(0.1f);
		Destroy(gameObject);
	}
	
}
