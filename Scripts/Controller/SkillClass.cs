using UnityEngine;
using System.Collections;

public class LongSkill : SkillsData {
	
	public LongSkill (string chara, string sn, string cn, string text, 
	                  float cor, int co, float r, float[] at, string[] path, AudioClip se) : base (chara, sn, cn, text, cor, co, r, at, path, se)
	{
	}
	
	public override void SkillEffect (ShinBattleController s,ShinBattleController ts, int atk)
	{
		Vector3 pos = s.transform.TransformPoint (Vector3.forward);
		GameObject o = MonoBehaviour.Instantiate (Resources.Load(effectPath[effectNumber], typeof(GameObject)) as GameObject, 
		                                          pos+new Vector3 
		                                          (0.0f, 0.2f, 0.0f), Quaternion.identity) as GameObject;
		
		if (effectNumber == 1)
		{
			EffectsController ec = o.AddComponent<EffectsController> ();
			int damage = (int)((atk - ts.status.defense/2)*correction*Random.Range (0.9f, 1.10f));
			if (damage < 0) damage = 0;
			ec.Prepar (ts.transform.position, ts.tag, damage);
		}
		
		effectNumber ++; if (effectNumber>=2) effectNumber = 0;
	}
};

public class MiddleSkill : SkillsData {
	
	public MiddleSkill (string chara, string sn, string cn, string text, float cor, int co, float r, float[] at, 
	                    string[] path, AudioClip se) : base (chara, sn, cn, text, cor, co, r, at, path, se)
	{
	}
	
	public override void SkillEffect (ShinBattleController s, 
	                                  ShinBattleController ts, int atk)
	{
		Vector3 pos = s.transform.TransformPoint (Vector3.back);
		GameObject o = MonoBehaviour.Instantiate (Resources.Load(effectPath[effectNumber], typeof(GameObject)) as GameObject, 
		                                          pos+new Vector3(0.0f, 0.2f, 0.0f), 
		                                          Quaternion.Euler (270.0f, 0.0f, 0.0f)) as GameObject;
		
		MiddleEffects ec = o.AddComponent <MiddleEffects> ();
		int damage = (int)((atk - ts.status.defense/2)*correction*Random.Range (0.9f, 1.10f));
		if (damage < 0) damage = 0;
		ec.Prepar (ts.transform.position, ts.tag, damage);
	}
};

public class HighLongSkill : SkillsData {
	
	public HighLongSkill (string chara, string sn, string cn, string text, float cor, int co, float r, 
	                      float[] at, string[] path, AudioClip se) : base (chara, sn, cn, text, cor, co, r, at, path, se)
	{
	}
	
	public override void SkillEffect (ShinBattleController s, ShinBattleController ts, int atk)
	{
		Vector3 pos = s.transform.TransformPoint(Vector3.forward);
		GameObject o = MonoBehaviour.Instantiate (Resources.Load 
		                                          (effectPath[effectNumber], typeof(GameObject)) as GameObject, 
		                                          pos, s.transform.rotation) as GameObject;
		
		LongEffects le = o.AddComponent<LongEffects> ();
		int damage = (int)((atk - ts.status.defense/2)*correction*Random.Range (0.9f, 1.10f));
		if (damage < 0) damage = 0;
		le.Prepar (ts.transform.position, ts.tag, damage);
	}
};

public class ShortSkill : SkillsData {
	
	public ShortSkill (string chara, string sn, string cn, string text, float cor, int co, float r, 
	                   float[] at, string[] path, AudioClip se) : base (chara, sn, cn, text, cor, co, r, at, path, se)
	{
	}
	
	public override void SkillEffect (ShinBattleController s, ShinBattleController ts, int atk)
	{
		if (effectNumber == 0)
		{
			Vector3 pos = s.transform.TransformPoint(Vector3.forward/2);
			MonoBehaviour.Instantiate(Resources.Load (effectPath[effectNumber], typeof(GameObject)) as GameObject, 
									  pos+new Vector3 (0.0f, 0.2f, 0.0f), 
									  s.transform.rotation);
		}
		
		
		int damage = (int)((atk - ts.status.defense/2)*correction*Random.Range (0.9f, 1.10f));
		if (damage < 0) damage = 0;
		s.checkHit.SetColliderEnable (true);
		s.checkHit.attack = damage;
		effectNumber ++;
	}
	
};

public class SupportSkill : SkillsData {
	
	public SupportSkill (string chara, string sn, string cn, string text, float cor, int co, float r, 
	                     float[] at, string[] path, int ct, AudioClip se) : base (chara, sn, cn, text, cor, co, r, at, path, ct, se)
	{
	}
	
	public override void SkillEffect (ShinBattleController s, ShinBattleController ts, int atk)
	{
		MonoBehaviour.Instantiate (Resources.Load (effectPath[effectNumber], typeof(GameObject)) as GameObject, 
		                           s.transform.position, 
		                           Quaternion.Euler (270.0f, 0.0f, 0.0f));
	}
};
