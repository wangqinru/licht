using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AnimationController))]

public class BossBattleController : ShinBattleController {

	private ActionState nextState;
	private GameObject[] effects;

	void Awake ()
	{
		_animation = GetComponent <AnimationController> ();
		
		actionState = ActionState.move;
	}
	
	// Use this for initialization
	void Start ()
	{
		SetCollider ();
		breakNumber = 8;
	//	AddAttackEvent ();
		
		StartCoroutine ("BossAIManager");
	}
	
	// Update is called once per frame
	void Update () {

		_animation.PlayMotion (StateProcessing ());
	}
	
	protected override void SetCollider ()
	{	

		base.SetCollider ();
		checkHit.targetTag = "Player";
	
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

		_animation.SetMotionList (status.alphabet + Bidle);
		//	waponParticle.enableEmission = false;
		moveDirection = Vector3.zero;
		actionState = ActionState.move;
		hit = 0;
		
		GetNextState ();
	}
	
	protected override void StateController ()
	{
		if (!CheckState ((int)ActionState.damage) && !CheckState ((int)ActionState.back_step) && breakNumber <= 0)
		{
			actionState = ActionState.damage;
			hit = 0;
			_animation.SetMotionList (status.alphabet + Damage);
		}
	}
	
	protected override bool StateProcessing ()
	{
		bool loop = false;
		
		switch (actionState)
		{
		case	ActionState.move:

			if (guardObject != null) Destroy (guardObject);		
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
			
			break;
		case	ActionState.skill:
			
			break;
		case	ActionState.guard:
			if (guardObject == null)
				guardObject = Instantiate (Resources.Load (guardPath, typeof (GameObject)) as GameObject, transform.position, transform.rotation) as GameObject;
			else
				guardObject.transform.position = transform.position;
			
			Recession ();
			loop = true;
			break;
		case	ActionState.back_step:
			if (guardObject != null) Destroy (guardObject);
			
			SlipController (transform.TransformDirection (Vector3.back).normalized);
			
			break;
		case	ActionState.item:

			break;
		case	ActionState.damage:
			
			break;
		case	ActionState.dead:
			
			if (_animation.motionEnd)
			{
				Destroy (gameObject);
				battleManager.CheckRest (this);
			}
			break;
		}
		
		return loop;
	}
	
	protected override void MakeDamage (int a)
	{
		base.MakeDamage (a);
		
		soundManager.PlaySE (2);

		if (_animation.CheckPlaying (status.alphabet + Attack[0]))
		{
			if (hit == 0 || hit == 2)
				Instantiate (effects[0], transform.position, transform.rotation);
		}
		else
			Instantiate (effects[1], transform.position, transform.rotation);
		
		hit++;
	}
	
	public override void AddAttackEvent ()
	{
		for (int i=0; i<3; i++)
		{
			AnimationEvent attackEvent = new AnimationEvent ();
			attackEvent.time = status.attackTimes[i];
			attackEvent.functionName = "MakeDamage";
			attackEvent.intParameter = status.attack;
			GetComponent<Animation>().GetClip (status.alphabet + Attack[i]).AddEvent (attackEvent);
		}

		_skill.AddAttackEvent ();
	}
	
	//---------------------------------------------------------------------------------------------------------------------------------------------------INITここ！！！！！---------//
	public override void InitBattle (GameObject manager, GameObject c, Vector3 pos, Quaternion q, string tName, int id, StatusBase s)
	{
		base.InitBattle (manager, c, pos, q, tName, id, s);	
		_animation.SetMotionList (status.alphabet + Bidle);	
		_skill = gameObject.AddComponent <SkillController> ();	
		_skill.Prepar (this, battleManager);

	}

	public void SetEffects (GameObject[] gos)
	{
		effects = gos;
	}
	
	IEnumerator BossAIManager ()
	{
		moveDirection = Vector3.zero;
		yield return new WaitForSeconds (2.0f);

		int aiRand = Random.Range (0, 10);

		if (target == null)
		{	
			if (aiRand > 7)
				target = battleManager.playerList[battleManager.GetNearUnit (transform.position, false)];
			else
				target = battleManager.playerList[0];
		}

		GetNextState ();

		while (true)
		{
			switch (actionState)
			{
			case	ActionState.move:
				IdleArtificialIntelligence ();
				break;
			case	ActionState.attack:		
				if (AttackArtificialIntelligence ())
					yield return new WaitForSeconds (0.5f);
				break;
			case	ActionState.skill:
				if (SkillArtificialIntelligence ())
					yield return new WaitForSeconds (0.5f);
				break;
			case	ActionState.guard:

				if (GuardArtificialIntelligence ())
					yield return new WaitForSeconds (0.5f);

				break;
			case	ActionState.back_step:

				if (BackStepArtificialIntelligence ())
					yield return new WaitForSeconds (0.5f);
				break;
			case	ActionState.damage:	
				if (breakNumber < -10)
				{
					breakNumber = 8;
					transform.LookAt (target.transform.position);
					_skill.SetNowSkill (1);
					actionState = ActionState.skill;
					_animation.SetMotionList (_skill.nowSkill.motionName);
				}
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

						moveDirection = Vector3.zero;

						GetNextState ();
						breakNumber = 8;
						yield return new WaitForSeconds (0.3f);
					}
				}
				break;
			case	ActionState.dead:
				break;
			}
			yield return 0;
		}
	}

	//--------------------------------------------------------------------------------------------------------------------待機状況のAI-----Start----//
	private void IdleArtificialIntelligence ()
	{
		if (nextState == ActionState.attack)
		{
			moveDirection = (target.transform.position - transform.position).normalized;

			if ((target.transform.position - transform.position).magnitude < status.attackRanges[0])
			{
				transform.LookAt (target.transform.position);
				actionState = ActionState.attack;
				
				string[] attackChips = new string[3] { 
					status.alphabet + Attack[0],
					status.alphabet + Attack[0], 
					status.alphabet + Attack[2]};
				
				_animation.SetMotionList (attackChips);
				//	waponParticle.enableEmission = true;
				_animation.nextClip = 1;
			}
		}
		else if (nextState == ActionState.guard)
		{
			transform.LookAt (target.transform.position);
			
			actionState = ActionState.guard;				
			breakNumber = 8;		
			_animation.SetMotionList (status.alphabet + Guard);
		}
		else if (nextState == ActionState.skill)
		{
			moveDirection = (target.transform.position - transform.position).normalized;

			if ((target.transform.position - transform.position).magnitude < _skill.nowSkill.range)
			{
				transform.LookAt (target.transform.position);
				actionState = ActionState.skill;
				_animation.SetMotionList (_skill.nowSkill.motionName);
			}
		}

	}
	//--------------------------------------------------------------------------------------------------------------------待機状況のAI-----End----//

	private bool AttackArtificialIntelligence ()
	{
		if (_animation.motionEnd)
		{
			if ((target.transform.position - transform.position).magnitude < status.attackRanges[0] && _animation.nextClip < 2 && target.nowHp > 0)
			{
				_animation.nextClip ++;
			}
			else
			{
				int ran = Random.Range (0, 10);
				
				if (ran < 3)
				{
					_animation.SetMotionList (status.alphabet + Bidle);
					//	waponParticle.enableEmission = false;
					moveDirection = Vector3.zero;
					actionState = ActionState.move;

					GetNextState ();
					return true;
				}
				else if (ran > 3 && ran < 7)
				{
					transform.LookAt (target.transform.position);
					
					actionState = ActionState.guard;	
					breakNumber = 8;		
					_animation.SetMotionList (status.alphabet + Guard);
				}
				else
				{
					transform.LookAt (target.transform.position);
					_skill.SetNowSkill (2);
					actionState = ActionState.skill;
					_animation.SetMotionList (_skill.nowSkill.motionName);
				}
				hit = 0;
				return true;
			}
		}

		return false;
	}

	private bool GuardArtificialIntelligence ()
	{
		int ran = Random.Range (0, 10);

		if (ran > 5)
		{
			_animation.SetMotionList (status.alphabet + Bstep);
			
			verticalSpeed = 1.0f;
			gravity = -20.0f;
			moveSpeed = 6.0f;
			gAcceleration = 3.0f;
			actionState = ActionState.back_step;

			return true;
		}

		if (target.CheckState ((int)ActionState.attack) || target.CheckState ((int)ActionState.skill))
		{
			if (ran > 4)
			{
				_animation.SetMotionList (status.alphabet + Bstep);
				
				verticalSpeed = 1.0f;
				gravity = -20.0f;
				moveSpeed = 6.0f;
				gAcceleration = 3.0f;
				actionState = ActionState.back_step;
				
				return true;
			}
			_animation.SetMotionList (status.alphabet + Bidle);
			actionState = ActionState.move;

			return true;
		}

		return false;
	}

	private bool SkillArtificialIntelligence ()
	{
		int rand = Random.Range (0, 10);

		if (_animation.motionEnd)
		{
			if (rand > 3)
			{
				_animation.SetMotionList (status.alphabet + Bidle);
				actionState = ActionState.move;
				moveDirection = Vector3.zero;

				GetNextState ();
				return true;
			}
			else
			{
				for (int i=1; i<3; i++)
				{
					if ((target.transform.position - transform.position).magnitude < status.skillList[i].range && _skill.CheckMp (i))
					{
						if (rand > 4)
						{
							transform.LookAt (target.transform.position);
							actionState = ActionState.skill;
							_skill.SetNowSkill (i);
							_animation.SetMotionList (_skill.nowSkill.motionName);
							break;
						}
					}

					actionState = ActionState.guard;	
					breakNumber = 8;		
					_animation.SetMotionList (status.alphabet + Guard);
				}
			}
		}

		return false;
	}

	private bool BackStepArtificialIntelligence ()
	{
		if (verticalSpeed == 0.0f)
		{
			//moveSpeed = 0.0f;
			gAcceleration = 1.0f;			
			_animation.SetMotionList (status.alphabet + Bidle);			

			int rand = Random.Range (0, 10);
			if (rand > 7)
			{
				_skill.SetNowSkill (0);
				actionState = ActionState.skill;
				_animation.SetMotionList (_skill.nowSkill.motionName);

				return true;
			}
			else
			{
				for (int i=1; i<3; i++)
				{
					if ((target.transform.position - transform.position).magnitude < status.skillList[i].range && _skill.CheckMp (i))
					{
						_skill.SetNowSkill (i);
						actionState = ActionState.skill;
						_animation.SetMotionList (_skill.nowSkill.motionName);

						return true;
					}
				}
			}

			actionState = ActionState.move;
			moveDirection = Vector3.zero;
			GetNextState ();

			return true;
		}

		return false;
	}
	
	void GetNextState ()
	{
		int rand = Random.Range (0, 10);

		if (rand > 5 && (target.transform.position - transform.position).magnitude < 2.0f && 
		    target.CheckState ((int)ActionState.attack) || target.CheckState ((int)ActionState.skill))
		{
			nextState = ActionState.guard;			
			return;
		}
		
		if ((target.transform.position - transform.position).magnitude > 3.0f && rand > 6 && _skill.CheckMp (0))
		{
			nextState = ActionState.skill;
			_skill.SetNowSkill (0);
			
			return;
		}
		
		for (int i=1; i<3; i++)
		{
			if (rand > (4-i*2) && (target.transform.position - transform.position).magnitude < status.skillList[i].range && _skill.CheckMp (i))
			{
				nextState = ActionState.skill;
				_skill.SetNowSkill (i);

				return;
			}
		}

		nextState = ActionState.attack;
	}
}
