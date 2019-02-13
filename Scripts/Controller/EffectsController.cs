using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]

public class EffectsController : MonoBehaviour {

	protected string targetTag;

	protected int attack;

	protected float starttime;

	protected const float endtime = 2.0f;

	// Use this for initialization
	void Start () {
		starttime = Time.time;
	}
	
	// Update is called once per frame
	void LateUpdate () {

		transform.Translate (Vector3.forward * 10.0f * Time.deltaTime);

		if (Time.time - starttime > endtime)
			Destroy (gameObject);
	}

	public virtual void Prepar (Vector3 targetPos, string tag, int atk)
	{
		transform.LookAt (targetPos);

		targetTag = tag;

		attack = atk;

		Rigidbody rb = gameObject.AddComponent <Rigidbody> ();
		rb.useGravity = false;
		rb.isKinematic = true;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == targetTag)
		{
			ShinBattleController sc = other.GetComponent <ShinBattleController>();
			int damage = (int)((attack - sc.status.defense/2)*Random.Range (0.9f, 1.10f));
			if (damage < 0) damage = 0;
			other.GetComponent <ShinBattleController>().GiveDamage (damage);

			Destroy (gameObject);
		}
	}
}


