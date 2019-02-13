using UnityEngine;
using System.Collections;

public class CheckCollision : MonoBehaviour {

	public int attack {get; set;}

	public string targetTag {get; set;}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerStay (Collider other)
	{
		if (other.tag == targetTag)
		{
			ShinBattleController sc = other.GetComponent <ShinBattleController>();
			int damage = (int)((attack - sc.status.defense/2)*Random.Range (0.9f, 1.10f));
			if (damage<0) damage = 0;

			other.GetComponent <ShinBattleController>().GiveDamage (damage);
			this.GetComponent<BoxCollider> ().enabled = false;
		//	if (targetTag == "Enemy") print ("attack : "+damage);

			GameObject o = Resources.Load ("Prefabs/Effects/hit_effect", typeof(GameObject)) as GameObject;
			Vector3 head = (sc.GetHeadPosition () - sc.transform.position)/2;
			Instantiate (o, sc.transform.position + head, Quaternion.identity);
		} 
	}

	public void SetColliderEnable (bool flag)
	{
		if (this.GetComponent<BoxCollider> ().enabled != flag)
			this.GetComponent<BoxCollider> ().enabled = flag;
	}
}
