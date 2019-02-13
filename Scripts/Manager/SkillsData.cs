using UnityEngine;
using System.Collections;

public class SkillsData {

	public string skillName {get; protected set;} 
	public string clipName {get; protected set;}
	public string skillText {get; protected set;}
	public bool signUp { get; set;}

	public float correction {get; protected set;}
	public int cost { get; protected set;}
	public int continuationTime { get; protected set;}

	public float[] attackTime {get; protected set;}
	public string motionName {get; protected set;}
	
	public float range {get; protected set;}
	public AudioClip soundEffect {get; protected set;}
	public int effectNumber {get; set;}
	protected string[] effectPath;

	public SkillsData (string chara, string sn, string cn, string text, float cor, int co, float r, float[] at, string[] path, AudioClip se)
	{
		range = r;	
		skillName = sn;
		clipName = cn;	
		motionName = chara + clipName;
		correction = cor;
		cost = co;
		attackTime = at;
		effectPath = path;
		skillText = text;
		signUp = true;
		effectNumber = 0;
		continuationTime = 0;
		soundEffect = se;
	}

	public SkillsData (string chara, string sn, string cn, string text, float cor, int co, float r, float[] at, string[] path, int ct, AudioClip se)
	{
		range = r;	
		skillName = sn;
		clipName = cn;	
		motionName = chara + clipName;
		correction = cor;
		cost = co;
		attackTime = at;
		effectPath = path;
		skillText = text;
		signUp = true;
		effectNumber = 0;
		continuationTime = ct;
		soundEffect = se;
	}

	public virtual void SkillEffect (ShinBattleController s, ShinBattleController ts, int atk)
	{

	}

}
