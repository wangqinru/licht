using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animation))]

public class ShinFieldController : MonoBehaviour {

	private InputManager inputManager;
	private SoundManager soundManager;
	public FieldCameraFixation scamera { get; private set;}

	//private int ID;
	private StatusBase status;

	private string idle = "@Idle";
	private string walk = "@Walk";
	private string run = "@Run";
	//private string relax = "@Relax";

	//アニメーションスピード
	private float idleAnimationSpeed = 1.0f;
	private float walkAnimationSpeed = 1.0f;
	private float runAnimationSpeed = 1.0f;
	//private float relaxAnimationSpeed = 1.0f;

	private float moveSpeed = 0.0f;
	private float walkSpeed = 2.0f * 60.0f;
	private float runSpeed = 6.0f * 60.0f;
	private float count = 0;

	private Vector3 moveDirection = Vector3.zero;

	public float headOffset { get; private set;}

	private Animation _animation;

	private Material face;

	// Use this for initialization

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//カメラの前の向きをとる
		Transform cameraTransform = Camera.main.transform;

		Vector3 forward = cameraTransform.TransformDirection (Vector3.forward);

		forward.y = 0;

		forward = forward.normalized;

		Vector3 right = new Vector3 (forward.z, 0, -forward.x);

		Vector3 targetDirection = inputManager.horizontal * right + inputManager.vertical * forward;

		AnimationController (inputManager.vertical, inputManager.horizontal);

		moveDirection = targetDirection.normalized;

		moveDirection *= Time.deltaTime;

		CharacterController _characterController = GetComponent<CharacterController> ();

		_characterController.SimpleMove (moveDirection * moveSpeed);

		if (moveDirection != Vector3.zero) transform.rotation = Quaternion.LookRotation (moveDirection);

	}

	void AnimationController (float v, float h)
	{
		if (Mathf.Abs (v) + Mathf.Abs (h) == 0.0f)
		{
			_animation[status.alphabet+idle].speed = idleAnimationSpeed;
			_animation.CrossFade(status.alphabet+idle);
			FaceOffset ();
		}
		else if (Mathf.Abs (v) + Mathf.Abs (h) > 0.0f && Mathf.Abs (v) + Mathf.Abs (h) <= 0.3f)
		{
			moveSpeed = walkSpeed;
			_animation[status.alphabet+walk].speed = walkAnimationSpeed;
			_animation.CrossFade (status.alphabet+walk);
			soundManager.PlaySEOnce (0);
		}
		else if (Mathf.Abs (v) + Mathf.Abs (h) > 0.3f)
		{
			moveSpeed = runSpeed;
			_animation[status.alphabet+run].speed = runAnimationSpeed;
			_animation.CrossFade (status.alphabet+run);
			soundManager.PlaySEOnce (0);
		}

		SetCameraPosition ();
	}

	void SetCameraPosition ()
	{
		scamera.targetPosition = new Vector3 (transform.position.x, transform.position.y + headOffset, transform.position.z);
	}

	void FaceOffset ()
	{
		count = Mathf.PingPong (Time.time, 3);

		face.SetTextureOffset ("_MainTex", new Vector2 (count > 2.9f ? 0.5f : 0.0f, 0.0f));
	}

	public void Prepare (int id, StatusBase s, FieldCameraFixation sc, GameObject manager)
	{
	//	ID = id;

		status = s;

		scamera = sc;
		
		inputManager = manager.GetComponent <InputManager> ();

		soundManager = manager.GetComponent <SoundManager> ();
		
		_animation = GetComponent <Animation> ();
		
		headOffset = (GetComponent<CharacterController> ().bounds.max.y - transform.position.y)/3 * 2;
		
		scamera.targetPosition = new Vector3 (transform.position.x, transform.position.y + headOffset, transform.position.z);

		face = GetComponentInChildren<Renderer> ().materials[0];
	}
}
