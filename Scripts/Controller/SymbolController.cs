using UnityEngine;
using System.Collections;

public class SymbolController : MonoBehaviour {

	private string unitName;
	private int unitID;

	private string idle = "@Idle";
	private string walk = "@Walk";
	private string run = "@Run";

	private Animation _animation;

	private float startTime = 0.0f;
	private float waitTime = 5.0f;

	private float moveSpeed = 0.0f;
	private float walkSpeed = 2.0f * 60.0f;
	private float runSpeed = 4.5f * 60.0f;

	private Vector3 moveDirection = Vector3.zero;
	private Vector3 originPosition = Vector3.zero;

	private Vector3 target;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		CharacterController _controller = GetComponent<CharacterController> ();

		moveDirection = _animation.IsPlaying (unitName+run) ? Pursuit () : Wander ();

		moveDirection *= Time.deltaTime;

		_controller.SimpleMove (moveDirection * moveSpeed);
	}

	Vector3 Wander ()
	{
		if (Time.time - startTime > waitTime) 
		{
			startTime = Time.time;
			if (_animation.IsPlaying (unitName + idle))
			{
				if ((originPosition - transform.position).magnitude > 11.0f )
				{
					startTime += ((originPosition - transform.position).magnitude - 10.0f)/(walkSpeed/60.0f);
					Vector3 relative = transform.InverseTransformPoint (originPosition);
					float angle = Mathf.Atan2 (relative.x, relative.z) * Mathf.Rad2Deg;
					transform.Rotate (0.0f, angle, 0.0f);
				}
				else
					transform.Rotate (new Vector3 (0.0f, 180.0f, 0.0f));

				moveSpeed = walkSpeed;

				_animation.CrossFade (unitName + walk);
			}
			else if (_animation.IsPlaying (unitName + walk))
			{
				moveSpeed = 0.0f;
				_animation.CrossFade (unitName + idle);
			}
		}

		return transform.TransformDirection (Vector3.forward);
	}

	void OnTriggerStay (Collider other)
	{
		Vector3 forward = transform.TransformDirection (Vector3.forward);

		if (other.tag == "Leader" & Vector3.Angle (forward, (other.transform.position - transform.position).normalized) < 120.0f &
		    Mathf.Abs(other.transform.position.y - transform.position.y) < 0.6f)
		{
			moveSpeed = runSpeed;
			_animation.CrossFade (unitName + run);
			target = other.GetComponent<ShinFieldController> ().scamera.targetPosition;
		}

		if (other.tag == "Leader" && (other.transform.position - transform.position).magnitude < 1.5f)
		{
			GameObject.Find ("GameManager").GetComponent <BattleManager> ().InitBattle (unitID);
			StartCoroutine ("DelectSymbol");
			GetComponent<SphereCollider> ().enabled = false;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Leader")
		{
			startTime = Time.time;
			moveSpeed = 0.0f;
			_animation.CrossFade (unitName +idle);
		}
	}

	Vector3 Pursuit ()
	{
		if (moveDirection != Vector3.zero) transform.rotation = Quaternion.LookRotation (moveDirection);

		Vector3 muki = target - transform.position;
		muki.y = 0.0f;

		return muki.normalized; 
	}

	public void Prepare (string n, int id)
	{
		unitName =  n;

		unitID = id;

		_animation = GetComponent<Animation> ();
		
		startTime = Time.time + Random.Range (0, 10);
		
		_animation.CrossFade (unitName +idle);
		
		GetComponent <SphereCollider> ().radius *= 10.0f;
		
		originPosition = transform.position;

	}

	IEnumerator DelectSymbol ()
	{
		yield return new WaitForSeconds (3.0f);
		Destroy (gameObject);
	}
}
