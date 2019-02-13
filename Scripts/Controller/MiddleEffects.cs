using UnityEngine;
using System.Collections;

public class MiddleEffects : EffectsController {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		transform.Translate (Vector3.forward * 10.0f * Time.deltaTime);
		
		if (!GetComponent<Animation>().isPlaying)
			Destroy (gameObject);
	}
	
	public override void Prepar (Vector3 targetPos, string tag, int atk)
	{
		base.Prepar (targetPos, tag, atk);
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == targetTag)
		{
			ShinBattleController sc = other.GetComponent <ShinBattleController>();
		
			sc.GiveDamage (attack);
			
			this.GetComponent <SphereCollider> ().enabled = false;
		}
	}
};
