using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillController : MonoBehaviour{
	
	private ShinBattleController battle;
	private BattleManager battleManager;
	private SoundManager soundManager;

	public SkillsData nowSkill {get; protected set;}

	protected List <SkillsData> skillList;

	public void AddAttackEvent ()
	{
		foreach (SkillsData sd in battle.status.skillList)
		{
			for (int i=0; i<sd.attackTime.Length; i++)
			{
				AnimationEvent attackEvent = new AnimationEvent ();	

				attackEvent.time = sd.attackTime[i];

				attackEvent.intParameter = battle.status.attack;

				nowSkill = sd;

				attackEvent.functionName = "SkillEffect";

				GetComponent<Animation>().GetClip (sd.motionName).AddEvent (attackEvent);
			}
		}
	}

	public bool CheckSkillButton (InputManager inputManager)
	{
		for (int i=0; i<1; i++)
		{
			if (inputManager.sbList[i] == 1 && battle.nowMp > skillList[i].cost && skillList[i].signUp)
			{
				nowSkill = skillList[i];
				nowSkill.effectNumber = 0;
				battle.nowMp -= skillList[i].cost;
				battleManager.ShowSkill (battle.status.iconFace[4], nowSkill.skillName);
				return true;
			}
		}

		return false;
	}

	public void SetNowSkill (int n)
	{
		nowSkill = skillList[n];
		nowSkill.effectNumber = 0;
		battleManager.ShowSkill (battle.status.iconFace[4], nowSkill.skillName);
		battle.nowMp -= nowSkill.cost;
	}

	protected void SkillEffect (int a)
	{
		nowSkill.SkillEffect (battle, battle.target, a);
		soundManager.PlaySE (nowSkill.soundEffect);
	}

	public void Prepar (ShinBattleController b, BattleManager bm)
	{
		battle = b;
		battleManager = bm;
		skillList = b.status.skillList;
		soundManager = battleManager.GetComponent<SoundManager> ();
	}

	public bool CheckMp (int n)
	{
		if (battle.nowMp > skillList[n].cost)
			return true;

		return false;
	}
}
