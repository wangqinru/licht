using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AnimationController))]

public class EnemyBattleController : ShinBattleController {

	public MonsterData monsterData { get; private set;}

	void Awake ()
	{
		_animation = GetComponent <AnimationController> ();
		Attack[1] = "@AttackTwo";
		actionState = ActionState.move;
	}

	// Use this for initialization
	void Start ()
	{
		SetCollider ();
		//AddAttackEvent ();
		StartCoroutine ("AIManager");
	}
	
	// Update is called once per frame
	void Update () {

		_animation.PlayMotion (StateProcessing ());
	}

	protected override void SetCollider ()
	{	

		base.SetCollider ();

		checkHit.targetTag = this.tag == "Player" ? "Enemy" : "Player";

		//weapon = GetComponentInChildren <SphereCollider> ();
		
		//weapon.center = new Vector3 (0.0f, 0.0f, 0.95f);
		
		//weapon.radius = 1.2f;

		GameObject o = weapon.gameObject;

		Rigidbody rb = o.AddComponent <Rigidbody> ();
		
		rb.useGravity = false; rb.isKinematic = true;
		
		weapon.enabled = false;
	}

	public override void ChangeTarget ()
	{
		int next = battleManager.GetNearUnit (transform.position, false);
		if (next == -1) 
		{
			_animation.SetMotionList (status.alphabet + Bidle);			
			moveDirection = Vector3.zero;		
			actionState = ActionState.move;
			return;
		}

		target = battleManager.playerList[next];
	}

	protected override void StateController ()
	{
		if (!CheckState ((int)ActionState.damage) && breakNumber <= 0)
		{
			actionState = ActionState.damage;
			
			_animation.SetMotionList (status.alphabet + Damage);

			StopCoroutine ("AIManager");
		}
	}
	
	protected override bool StateProcessing ()
	{
		bool loop = false;
		
		switch (actionState)
		{
		case	ActionState.move:
			
			if (moveDirection == Vector3.zero)
			{
				moveSpeed = 0.0f;
				
				if (!_animation.CheckClip (status.alphabet + Bidle))
				{
					_animation.SetMotionList (status.alphabet + Bidle);
				}
			}
			else
			{
				moveSpeed = runSpeed;
				
				if (!_animation.CheckClip (status.alphabet + Brun))
				{
					_animation.SetMotionList (status.alphabet + Brun);
				}
			}

			MoveController (true);

			loop = true;

			break;

		case	ActionState.attack:
			
			if (_animation.motionEnd)
			{
				_animation.SetMotionList (status.alphabet + Bidle);			
				moveDirection = Vector3.zero;		
				actionState = ActionState.move;
				checkHit.SetColliderEnable (false);
				StartCoroutine ("AIManager");
			}
			
			break;

		case	ActionState.guard:
			
			if (target.CheckState ((int)ActionState.move))
			{
				acceleration = 0.0f;
				friction = 0.0f;
				moveSpeed = 0.0f;

				moveDirection = Vector3.zero;

				_animation.SetMotionList (new string[1]{status.alphabet + Bidle});
				actionState = ActionState.move;
				StartCoroutine ("AIManager");
			}

			Recession ();

			loop = false;
			break;
		case	ActionState.damage:
			
			if (_animation.motionEnd)
			{
				int rand = Random.Range (0, 10);
				if (rand > 7) ChangeTarget ();
				
				if (nowHp == 0)
				{
					_animation.SetMotionList (status.alphabet + Bdead);
					actionState = ActionState.dead;

					battleManager.RemoveUnit (this);
				}
				else
				{
					_animation.SetMotionList (status.alphabet + Bidle);
					actionState = ActionState.move;
					StartCoroutine ("AIManager");
				}
			}
			
			break;
		case	ActionState.dead:
			if (_animation.motionEnd)
			{
				battleManager.CheckRest (this);
				Destroy (gameObject);
			}
			break;
		}
		
		return loop;
	}

	protected override void MakeDamage (int a)
	{
		base.MakeDamage (a);
	}

	public override void AddAttackEvent ()
	{
		for (int i=0; i<2; i++)
		{
			AnimationEvent attackEvent = new AnimationEvent ();
			
			attackEvent.time = status.attackTimes[i];
			
			attackEvent.functionName = "MakeDamage";
			
			attackEvent.intParameter = status.attack;
			
			GetComponent<Animation>().GetClip (status.alphabet + Attack[i]).AddEvent (attackEvent);
		}
		//_skill.AddAttackEvent ();
	}

	//---------------------------------------------------------------------------------------------------------------------------------------------------INITここ！！！！！---------//
	public override void InitBattle (GameObject manager, GameObject c, Vector3 pos, Quaternion q, string tName, int id, StatusBase s)
	{
		base.InitBattle (manager, c, pos, q, tName, id, s);

		_animation.SetMotionList (status.alphabet + Bidle);

		//_skill = gameObject.AddComponent <SkillController> ();	
		//_skill.Prepar (this);

	}

	IEnumerator AIManager ()
	{
		moveDirection = Vector3.zero;

		yield return new WaitForSeconds (1.0f);

		bool longF = true;

		if (target == null)
		{
			int r = Random.Range (0, 10);

			if (r > 7)
				target = battleManager.playerList[battleManager.GetNearUnit (transform.position, false)];
			else
				target = battleManager.playerList[0];
		}

		while (true)
		{
			moveDirection = (target.transform.position - transform.position).normalized;
			moveDirection.y = 0.0f;

			float dis = (target.transform.position - transform.position).magnitude;
			if (dis > 8.0f)
			{
				int rand = Random.Range (0, 10);

				if (rand > 5)
				{
					target = battleManager.playerList[battleManager.GetNearUnit (transform.position, false)];
				}
			}

			if (AttackAI ((target.transform.position - transform.position).magnitude, ref longF)) break;

			yield return 0;
		}
	}

	bool AttackAI (float distance, ref bool flag)
	{
		if (distance < status.attackRanges[1] && distance > status.attackRanges[1] - 0.2f && flag)
		{
			int rand = Random.Range (0, 10);

			if (rand > 6)
			{
				actionState = ActionState.attack;
			
				_animation.SetMotionList (status.alphabet + Attack[1]);

				return true;
			}

			flag = false;
		}

		if (distance < status.attackRanges[0])
		{
			int rand = Random.Range (0, 10);
			
			if (rand > 4)
			{
				actionState = ActionState.attack;
				
				_animation.SetMotionList (new string[2] {status.alphabet + Attack[0], status.alphabet + Attack[1]});
				
				rand = Random.Range (0, 10);
				
				_animation.nextClip = rand > 6 ? 1 : 0;
			}
			else
			{
				
				if (target.CheckState ((int)ActionState.attack))
				{
					_animation.SetMotionList (status.alphabet + Guard);
					breakNumber = 3;
					actionState = ActionState.guard;
				}
				else
				{
					actionState = ActionState.attack;
					
					_animation.SetMotionList (new string[2] {status.alphabet + Attack[0], status.alphabet + Attack[1]});
					
					rand = Random.Range (0, 10);
					
					_animation.nextClip = rand > 6 ? 1 : 0;
				}
			}
			
			moveSpeed = 0.0f;

			return true;
		}

		return false;
	}
}
