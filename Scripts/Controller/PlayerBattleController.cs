using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AnimationController))]

public class PlayerBattleController : ShinBattleController {

	private string Win = "@Win";
	private ParticleSystem waponParticle;
	private AutoRotation cursorObject;

	private GameObject[] effects;
	public bool isUser { get; set;}
	private ActionState nextState;

	// Use this for initialization
	void Awake () {

		_animation = GetComponent <AnimationController> ();
		actionState = ActionState.move;
	}
	
	void Start ()
	{
		SetCollider ();		
	//	AddAttackEvent ();		

		if (isUser)
		{	
			StartCoroutine ("ManualOperation");
		}
		else 
		{
			target = battleManager.enemyList[battleManager.GetNearUnit (transform.position, true)];
			StartCoroutine ("AutoOperation");
		}
	}
	
	
	protected override void SetCollider ()
	{
		base.SetCollider ();

		checkHit.targetTag = "Enemy";		
		GameObject o = weapon.gameObject;
		waponParticle = o.GetComponent <ParticleSystem> ();
		Rigidbody rb = o.AddComponent <Rigidbody> ();
		
		rb.useGravity = false; rb.isKinematic = true;		
		weapon.enabled = false;
		waponParticle.enableEmission = false;
	}
	
	
	// Update is called once per frame
	void Update () {
		
		//StateController ();	
		_animation.PlayMotion (StateProcessing ());
	}
	
	protected override void StateController ()
	{
		if (!CheckState ((int)ActionState.damage) && !CheckState ((int)ActionState.back_step) && breakNumber <= 0)
		{
			actionState = ActionState.damage;			
			_animation.SetMotionList (status.alphabet + Damage);
			waponParticle.enableEmission = false;
			iconStatus.ChangeFace (2);
			hit = 0;
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
			if (_animation.motionEnd && isUser)
			{
				_animation.SetMotionList (status.alphabet + Bidle);
				waponParticle.enableEmission = false;
				actionState = ActionState.move;
				checkHit.SetColliderEnable (false);
				hit = 0;
				iconStatus.ChangeFace (0);
			}
			
			break;
		case	ActionState.skill:

			if (_animation.motionEnd)
			{
				_animation.SetMotionList (status.alphabet + Bidle);
				waponParticle.enableEmission = false;
				actionState = ActionState.move;
				iconStatus.ChangeFace (0);
			}

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

			if (verticalSpeed == 0.0f)
			{
				//moveSpeed = 0.0f;
				gAcceleration = 1.0f;

				_animation.SetMotionList (status.alphabet + Bidle);

				inputManager.ClearInput ();
				actionState = ActionState.guard;
			}

			break;
		case	ActionState.item:

			if (_animation.motionEnd)
			{
				_animation.SetMotionList (status.alphabet + Bidle);
				actionState = ActionState.move;
			}
			break;
		case	ActionState.damage:
			if (_animation.motionEnd && isUser)
				iconStatus.ChangeFace (0);
			DamageState ();
			break;
		case	ActionState.dead:

			if (_animation.motionEnd)
			{
				this.enabled = false;
				iconStatus.ChangeFace (3);
				//battleManager.RemoveUnit (this);
				battleManager.CheckRest (this);
			}
			break;
		case	ActionState.win:	
			if (guardObject != null) Destroy (guardObject);
			if (_animation.motionEnd && isUser)
			{
				battleManager.ShowResult ();
				this.enabled = false;
			}
			break;
		}
		
		return loop;
	}
	
	protected override Vector3 GetMoveDirection ()
	{
		Vector3 forward = battleCamera.transform.TransformDirection (Vector3.forward);		
		forward.y = 0.0f;		
		forward = forward.normalized;	
		Vector3 right = new Vector3 (forward.z, 0.0f, -forward.x);	
		Vector3 targetDirection = inputManager.horizontal * right + inputManager.vertical * forward;
		
		return targetDirection.normalized;
	}

	//---------------------------------------------------------------------------------------------------------------------------------------------------INITここ！！！！！---------//
	public override void InitBattle (GameObject manager, GameObject c, Vector3 pos, Quaternion q, string tName, int id, StatusBase s)
	{
		inputManager = manager.GetComponent <InputManager> ();		
		
		base.InitBattle (manager, c, pos, q, tName, id, s);
		if (isUser)
		{
			InitTarget ();
			battleCamera.InitBattle (GetHeadPosition(), target.GetComponent<ShinBattleController>().GetHeadPosition ());
		}
		battleCamera.targetPositon = new Vector3 (transform.position.x, transform.position.y + headOffest, transform.position.z);
		_animation.SetMotionList (status.alphabet + Bidle);

		_skill = gameObject.AddComponent <SkillController> ();		
		_skill.Prepar (this, battleManager);		
	}

	//--------------------------------------------------------------------------------------------------------------------attackEvent---------------------------------//
	protected override void MakeDamage (int a)
	{
		base.MakeDamage (a);

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
	//---------------------------------------------------------------------------------------------------------------------復活
	public override void Revival ()
	{
		this.enabled = true;
		this.GetComponent<CharacterController> ().enabled = true;
		_animation.SetMotionList (status.alphabet + Bidle);
		actionState = ActionState.move;

		if (isUser)
		{
			StartCoroutine ("ManualOperation");
		}
		else
		{
			StartCoroutine ("AutoOperation");
		}

		iconStatus.ChangeFace (0);
	}

	void SetCameraPosition ()
	{
		battleCamera.playerPosition = GetHeadPosition ();
		
		if (target != null)
			battleCamera.targetPositon = target.GetComponent <ShinBattleController> ().GetHeadPosition ();
	}
	
	public override void InitTarget ()
	{		
		target = battleManager.enemyList[battleManager.GetNearUnit (transform.position, true)];
		battleManager.playerTarget = target.GetInstanceID ();
		float height = target.GetComponent<CharacterController> ().bounds.max.y - target.transform.position.y + 0.2f;
		
		CreateCursor (height);
	}

	public override void ChangeTarget ()
	{
		int next = battleManager.GetNearUnit (transform.position, true);
		if (next == -1) 
		{
			_animation.SetMotionList (status.alphabet + Bidle);			
			moveDirection = Vector3.zero;		
			actionState = ActionState.move;
			return;
		}
		
		target = battleManager.enemyList[next];

		_animation.SetMotionList (status.alphabet + Bidle);
		//	waponParticle.enableEmission = false;
		moveDirection = Vector3.zero;
		actionState = ActionState.move;
		hit = 0;

		if (isUser)
		{
			battleManager.playerTarget = target.GetInstanceID ();
			float height = target.GetComponent<CharacterController> ().bounds.max.y - target.transform.position.y + 0.2f;
			ChangeCursorPosition (height);
		}
		else
			GetNextState ();
	}
	//ターゲットチェンジ
	protected void ChangeTargetManual ()
	{
		if (battleManager.enemyList.Count == 0) return;

		int changeKey = inputManager.GetTargetChangeButton ();

		if (changeKey == 1)
		{
			int next = battleManager.GetNearUnit (transform.position, true);
			if (next == -1) 
			{
				_animation.SetMotionList (status.alphabet + Bidle);			
				moveDirection = Vector3.zero;		
				actionState = ActionState.move;
				return;
			}
			
			target = battleManager.enemyList[next];
			battleManager.playerTarget = target.GetInstanceID ();
			float height = target.GetComponent<CharacterController> ().bounds.max.y - target.transform.position.y + 0.2f;

			ChangeCursorPosition (height);

		}else if (changeKey == 2)
		{
			//battleManager.StopAnimation ();
		}
		else if (changeKey == 3)
		{
			//battleManager.RestartAnimation ();
		}
	}

	public override void DeleteTarget ()
	{
		if (cursorObject != null)
			Destroy (cursorObject.gameObject);
	}

	protected void ChangeOperationChara ()
	{
		if (inputManager.ButtonL == 1)
		{
			battleManager.SortPartyMember ();
		}
	}

	private void ChangeCursorPosition (float y)
	{
		cursorObject.InitPosition (y, target.transform);
	}

	private void CreateCursor (float y)
	{
		GameObject cursor = Instantiate (Resources.Load (battleManager.cursorPath, typeof(Object)) as Object) as GameObject;		
		cursorObject = cursor.AddComponent<AutoRotation> ();		
		cursorObject.InitPosition (y, target.transform);
	}

	//--------------------------------------------------------------------------------------------------------------------------戦闘終了-----------
	public override void BattleEnd ()
	{
		_animation.SetMotionList (status.alphabet + Win);
		actionState = ActionState.win;

		if (!isUser) StopCoroutine ("AutoOperation");
	}

	/// <summary>
	/// チェンジ操作モード
	/// </summary>
	/// <param name="flag">If set to <c>true</c> flag.</param>
	public override void ChangeOperationMode (bool flag)
	{
		if (isUser != flag)
		{
			isUser = flag;
			if (isUser)
			{
				if (!CheckState ((int)ActionState.dead))
				{
					StopCoroutine ("AutoOperation");
					StartCoroutine ("ManualOperation");

					_animation.SetMotionList (status.alphabet + Bidle);
					moveDirection = Vector3.zero;
					actionState = ActionState.move;
				}
				SetCameraPosition ();

				battleManager.playerTarget = target.GetInstanceID ();
				float height = target.GetComponent<CharacterController> ().bounds.max.y - target.transform.position.y + 0.2f;
				CreateCursor (height);
			}
			else
			{
				if (!CheckState ((int)ActionState.dead))
				{
					StopCoroutine ("ManualOperation");
					StartCoroutine ("AutoOperation");

					_animation.SetMotionList (status.alphabet + Bidle);
					moveDirection = Vector3.zero;
					actionState = ActionState.move;
				}
				DeleteTarget ();
			}
		}
	}

	public void SetEffects (GameObject[] gos)
	{
		effects = gos;
	}
	//--------------------------------------------------------------------------------------------------------------------------操作処理-----------
	private IEnumerator ManualOperation ()
	{
		while (isUser)
		{
			switch (actionState)
			{
			case	ActionState.move:

				moveDirection = GetMoveDirection ();

				if (inputManager.AttackButton == 1 && inputManager.SkillButton == 0)
				{
					transform.LookAt (target.transform.position);
					actionState = ActionState.attack;
					_animation.SetMotionList (status.alphabet + Attack[0]);
					waponParticle.enableEmission = true;
					iconStatus.ChangeFace (1);
				}
				
				if (_skill.CheckSkillButton (inputManager))
				{
					transform.LookAt (target.transform.position);
					actionState = ActionState.skill;
					_animation.SetMotionList (_skill.nowSkill.motionName);
					waponParticle.enableEmission = true;
					iconStatus.ChangeFace (1);
				}
				
				if (inputManager.GuardButton > 0 && inputManager.SkillButton == 0)
				{
					transform.LookAt (target.transform.position);
					
					actionState = ActionState.guard;					
					breakNumber = 3;		
					_animation.SetMotionList (status.alphabet + Guard);
				}
				break;
			case	ActionState.attack:		
				string[] attackChips = new string[3] { 
					status.alphabet + Attack[0],
					status.alphabet + Attack[0], 
					status.alphabet + Attack[2]};
				
				_animation.SetAttackMotion (attackChips, inputManager);

				break;
			case	ActionState.skill:
				break;
			case	ActionState.guard:
				
				if (inputManager.GuardButton == 0)
				{
					acceleration = 0.0f;
					friction = 0.0f;
					moveSpeed = 0.0f;
					breakNumber = 0;
					_animation.SetMotionList (status.alphabet + Bidle);
					actionState = ActionState.move;
				}
				
				Vector3 forward = transform.TransformDirection (Vector3.forward);
				
				if (Mathf.Abs (inputManager.horizontal) > 0.5f && Vector3.Angle (GetMoveDirection (), forward) > 70.0f)
				{
					_animation.SetMotionList (status.alphabet + Bstep);
					
					verticalSpeed = 1.0f;
					gravity = -20.0f;
					moveSpeed = 6.0f;
					gAcceleration = 3.0f;
					actionState = ActionState.back_step;
				}

				break;
			case	ActionState.back_step:	
				break;
			case	ActionState.damage:		
				break;
			case	ActionState.dead:
				break;
			}

			battleCamera.rotateFlag = moveSpeed == 0.0f ? true : false;
			
			SetCameraPosition ();			
			ChangeTargetManual ();

			yield return 0;
		}
	}

	private IEnumerator AutoOperation ()
	{
		moveDirection = Vector3.zero;
		yield return new WaitForSeconds (1.5f);

		if (target == null) ChangeTarget ();
		GetNextState ();

		while (!isUser)
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
					yield return new WaitForSeconds (0.8f);
				break;
			case	ActionState.damage:		
				break;
			case	ActionState.dead:
				break;
			}
			yield return 0;
		}
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
		
		if (rand > 6 && _skill.CheckMp (0))
		{
			nextState = ActionState.skill;	
			return;
		}

		nextState = ActionState.attack;
	}

	private void IdleArtificialIntelligence ()
	{
		if (nextState == ActionState.attack)
		{
			moveDirection = (target.transform.position - transform.position).normalized;
			
			if ((target.transform.position - transform.position).magnitude < status.attackRanges[0])
			{
				transform.LookAt (target.transform.position);
				actionState = ActionState.attack;
				iconStatus.ChangeFace (1);
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
			breakNumber = 3;		
			_animation.SetMotionList (status.alphabet + Guard);
		}
		else if (nextState == ActionState.skill)
		{
			moveDirection = (target.transform.position - transform.position).normalized;

			if ((target.transform.position - transform.position).magnitude < status.skillList[0].range)
			{
				_skill.SetNowSkill (0);
				transform.LookAt (target.transform.position);
				actionState = ActionState.skill;
				iconStatus.ChangeFace (1);
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
					breakNumber = 3;		
					_animation.SetMotionList (status.alphabet + Guard);

					return true;
				}
				else if (_skill.CheckMp (0))
				{
					transform.LookAt (target.transform.position);
					_skill.SetNowSkill (0);
					actionState = ActionState.skill;
					_animation.SetMotionList (_skill.nowSkill.motionName);
				}
				hit = 0;
			}

			iconStatus.ChangeFace (0);
		}
		
		return false;
	}
	
	private bool GuardArtificialIntelligence ()
	{
		int ran = Random.Range (0, 10);
		
		if (target.CheckState ((int)ActionState.attack) || target.CheckState ((int)ActionState.skill) && ran > 6)
		{
			_animation.SetMotionList (status.alphabet + Bstep);
				
			verticalSpeed = 1.0f;
			gravity = -20.0f;
			moveSpeed = 6.0f;
			gAcceleration = 3.0f;
			breakNumber = 0;
			actionState = ActionState.back_step;
				
			return true;
		}

		if (ran > 7)
		{
			_animation.SetMotionList (status.alphabet + Bstep);
			
			verticalSpeed = 1.0f;
			gravity = -20.0f;
			moveSpeed = 6.0f;
			gAcceleration = 3.0f;
			breakNumber = 0;
			actionState = ActionState.back_step;
			
			return true;
		}
		else
		{
			_animation.SetMotionList (status.alphabet + Bidle);
			actionState = ActionState.move;
			moveDirection = Vector3.zero;
			breakNumber = 0;
			GetNextState ();

			return true;
		}
	}
	
	private bool SkillArtificialIntelligence ()
	{
		if (_animation.motionEnd)
		{
			int rand = Random.Range (0, 10);

			if (rand > 3)
			{
				_animation.SetMotionList (status.alphabet + Bidle);
				actionState = ActionState.move;
				moveDirection = Vector3.zero;
				iconStatus.ChangeFace (0);
				GetNextState ();
				return true;
			}
			else
			{
				transform.LookAt (target.transform.position);
				
				actionState = ActionState.guard;		
				breakNumber = 3;		
				_animation.SetMotionList (status.alphabet + Guard);
				iconStatus.ChangeFace (0);
				return true;
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

			if ((target.transform.position - transform.position).magnitude > 6.0f)
			{
				target = battleManager.enemyList[battleManager.GetNearUnit (transform.position, true)];

				actionState = ActionState.move;
				moveDirection = Vector3.zero;
				GetNextState ();

				return true;
			}

			if ((target.transform.position - transform.position).magnitude < status.skillList[0].range &&_skill.CheckMp (0))
			{
				_skill.SetNowSkill (0);
				transform.LookAt (target.transform.position);
				actionState = ActionState.skill;
				_animation.SetMotionList (_skill.nowSkill.motionName);
				iconStatus.ChangeFace (1);
				return true;

			}
			else
			{
				actionState = ActionState.move;
				moveDirection = Vector3.zero;
				GetNextState ();

				return true;
			}
		}
		
		return false;
	}
}
