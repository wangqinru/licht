using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AnimationController))]

public class ShinBattleController : MonoBehaviour {
	
	protected InputManager inputManager;
	protected BattleManager battleManager;
	protected ShinBattleCamera battleCamera;
	protected SoundManager soundManager;
	protected BattleUIStatus iconStatus;
	//protected DamageNumber damageNumber;

	protected AnimationController _animation;
	protected SkillController _skill;
	
	protected float moveSpeed = 0.0f;
	protected float runSpeed = 6.0f;

	protected int breakNumber = 3;

	protected string Bidle = "@Bidle";
	protected string Brun = "@Brun";
	protected string Damage = "@Damage";
	protected string Guard = "@Guard";
	protected string[] Attack = new string[3]{"@AttackOne", "@AttackOne", "@AttackThree"};
	protected string Bdead = "@Dead";
	protected string Bstep = "@Step";
	protected string Item = "@Item";

	protected Vector3 moveDirection = Vector3.zero;

	protected float headOffest = 0.0f;

	public float damageTime {get; set;}

	protected BoxCollider weapon;
	public CheckCollision checkHit { get; private set;} 

	public ShinBattleController target {get; set;}

	public StatusBase status { get; protected set;}

	protected GameObject guardObject;
	protected string guardPath = "Prefabs/Effects/guard_effect";

	public int nowHp { get; set;}
	public int nowMp { get; set;}

	public int ID { get; private set;}

	//--------------加速度----------------摩擦力------------重力-------------垂直スピード------------重力加速度-----------//
	protected float acceleration = 0.0f, friction = 0.0f, gravity = 10.0f, verticalSpeed = 0.0f, gAcceleration = 1.0f;
	protected int hit = 0;

	protected enum ActionState
	{
		move,
		guard,
		attack,
		skill,
		back_step,
		item,
		damage,
		dead,
		win,
	};
	
	protected ActionState actionState;
	
	// Use this for initialization
	void Awake () {

	}

	protected virtual void SetCollider ()
	{
		weapon = GetComponentInChildren <BoxCollider> ();

		checkHit = GetComponentInChildren <CheckCollision> ();
	}

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	protected virtual void StateController ()
	{

	}

	protected virtual bool StateProcessing ()
	{
		return false;
	}

	protected virtual Vector3 GetMoveDirection ()
	{
		return Vector3.zero;
	}

	protected void MoveController (bool flag)
	{	
		Vector3 movement = moveDirection * moveSpeed + new Vector3 (0.0f, verticalSpeed, 0.0f);

		movement *= Time.deltaTime;
		
		CharacterController _characterController = GetComponent<CharacterController> ();
		
		_characterController.Move (movement);

		if (moveDirection != Vector3.zero && flag) transform.rotation = Quaternion.LookRotation (moveDirection);

		if (_characterController.isGrounded)
		{
			if (verticalSpeed != 0.0f)
			{
				verticalSpeed = 0.0f;

				gravity = 10.0f;
			}
		}
		else
		{
			verticalSpeed -= gravity*Time.deltaTime;

			gravity += gAcceleration;
		}
	}

	protected void SlipController (Vector3 muki)
	{
		Vector3 movement = muki * moveSpeed + new Vector3 (0.0f, verticalSpeed, 0.0f);

		movement *= Time.deltaTime;

		CharacterController _characterController = GetComponent<CharacterController> ();
		
		_characterController.Move (movement);
		
		if (_characterController.isGrounded)
		{
			if (verticalSpeed != 0.0f)
			{
				verticalSpeed = 0.0f;
				
				gravity = 10.0f;
			}
		}
		else
		{
			verticalSpeed -= gravity*Time.deltaTime;
			
			gravity += gAcceleration;
		}
	}

	//---------------------------------------------------------------------------------------------------------------------------------------------------INITここ！！！！！---------//
	public virtual void InitBattle (GameObject manager, GameObject c, Vector3 pos, Quaternion q, string tName, int id, StatusBase s)
	{		
		battleManager = manager.GetComponent <BattleManager> ();
		soundManager = manager.GetComponent <SoundManager> ();
		transform.position = pos;
		this.tag = tName;	
		transform.rotation = q;

		GameObject cameraObject = c;		
		battleCamera = cameraObject.GetComponent<ShinBattleCamera> ();		

		headOffest = GetComponent <CharacterController> ().bounds.max.y - pos.y;

		ID = id;
		breakNumber = 0;
		status = s;

		runSpeed = status.speed/7;

		nowHp = s.hp;
		nowMp = s.mp;
	}

	public virtual void InitIcon (BattleUIStatus bus)
	{
		iconStatus = bus;

		int temp = 0;
		if (actionState == ActionState.attack)
			temp = 1;

		if (actionState == ActionState.damage)
			temp = 2;

		if (actionState == ActionState.dead)
			temp = 3;

		iconStatus.ChangeFace (temp);
	}

	public void GiveDamage (int a)
	{
		if (nowHp == 0)
		{
			if (!CheckState ((int)ActionState.dead))
			{
				_animation.SetMotionList (status.alphabet + Bdead);
				actionState = ActionState.dead;
			}
			return;
		}

		if (CheckState ((int)ActionState.guard))
		{
			a /= 3;
			nowHp -= a;

			acceleration = 2.0f;
			gAcceleration = 1.0f;
			friction = 0.1f;
			moveSpeed = 2.0f;
		}
		else if (CheckState ((int)ActionState.back_step))
		{
			a /= 3;
			nowHp -= a;
		}
		else 
		{
			nowHp -= a;

			StateController ();
			_animation.ReSetAnimation (status.alphabet+Damage);
		}

		if (nowHp <= 0)
		{
			nowHp = 0;
			_animation.SetMotionList (status.alphabet + Bdead);
			actionState = ActionState.dead;
			battleManager.RemoveUnit (this);
		}

		CreateDamageNumber (a);
		battleManager.AddCombo (GetInstanceID ());
		soundManager.PlaySE (status.soundEffects [1]);
		breakNumber --;
	}

	protected void CreateDamageNumber (int damage)
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/Damage_Number", typeof (GameObject))) as GameObject;
		DamageNumber dn = o.AddComponent<DamageNumber> ();
		dn.ShowDamageNumber (damage, battleCamera.GetComponent<Camera>(), transform.position, tag=="Player");
	}

	protected virtual void MakeDamage (int a)
	{
		weapon.enabled = true;
		soundManager.PlaySE (status.soundEffects [0]);
		checkHit.attack = a;
	}

	protected virtual void MakeImpact (int a)
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/Effects/majinken", typeof(GameObject)) as GameObject, 
		               transform.TransformPoint (Vector3.forward), Quaternion.identity) as GameObject;

		EffectsController ec = o.AddComponent <EffectsController> ();

		ec.Prepar (target.transform.position, "Enemy", a);
	}

	protected void Recession ()
	{
		SlipController (transform.TransformDirection (Vector3.back).normalized * acceleration);

		if (acceleration > 0.0f)
		{
			acceleration -= friction;
			friction += 0.1f;
		}
		else moveSpeed = 0.0f; 
	}

	public virtual void AddAttackEvent ()
	{

	}

	public virtual void Revival ()
	{

	}

	/// <summary>
	/// ----------------------------------------ダメージ---------------------------------///
	/// </summary>
	protected void DamageState ()
	{		
		if (_animation.motionEnd)
		{
			_animation.SetMotionList (status.alphabet + Bidle);
			actionState = ActionState.move;
		}
	}

	public void Recovery (bool hp, float amount)
	{
		if (hp)
		{
			nowHp += (int)(amount*status.maxhp);
			if (nowHp > status.maxhp) nowHp = status.maxhp;
		}
		else
		{
			nowMp += (int)(amount*status.maxmp);
			if (nowMp > status.maxmp) nowMp = status.maxmp;
		}
	}

	//-------------------------------------------------------------------------------------------------------アイテムを使う
	public void UseItem ()
	{
		_animation.SetMotionList (status.alphabet + Item);
		actionState = ActionState.item;
	}

	//-------------------------------------------------------------------------------------------------------戦闘終了
	public virtual void BattleEnd ()
	{

	}

	/// <summary>
	/// チェンジ操作モード
	/// </summary>
	public virtual void ChangeOperationMode (bool flag)
	{
	}

	public virtual void InitTarget ()
	{

	}

	public virtual void ChangeTarget ()
	{

	}

	public virtual void DeleteTarget ()
	{
	}

	public Vector3 GetHeadPosition ()
	{
		return new Vector3 (transform.position.x, transform.position.y + headOffest, transform.position.z);
	}

	public bool CheckState (int index)
	{
		return actionState == (ActionState)index;
	}
}
