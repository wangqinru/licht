using UnityEngine;
using System.Collections;

public class LongEffects : MonoBehaviour {
	
	protected string targetTag;
	protected int attack;
	protected float starttime;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate (Vector3.forward * 0.5f * Time.deltaTime);
		starttime = Mathf.Repeat (Time.time, 0.6f);
		if (starttime > 0.55f)
		{
			if (!this.GetComponent <BoxCollider> ().enabled) 
				this.GetComponent <BoxCollider> ().enabled = true;
		}
		
		if (!GetComponent<Animation>().isPlaying)
			Destroy (gameObject);
	}
	
	public void Prepar (Vector3 targetPos, string tag, int atk)
	{
		targetTag = tag;
		attack = atk;
		
		Rigidbody rb = gameObject.AddComponent <Rigidbody> ();
		rb.useGravity = false;
		rb.isKinematic = true;
	}
	
	void OnTriggerStay (Collider other)
	{
		if (other.tag == targetTag)
		{
			ShinBattleController sc = other.GetComponent <ShinBattleController>();

			sc.GiveDamage (attack);
			
			this.GetComponent <BoxCollider> ().enabled = false;
		}
	}
}