using UnityEngine;
using System.Collections;

//HP回復アイテム
public class HPRecoveryItem : ItemBase {

	private float amount;

	public HPRecoveryItem (ItemData i, int n, float a) : base (i, n)
	{
		amount = a;
	}

	public override void ItemEvent (ShinBattleController s)
	{
		s.Recovery (true,amount);
		GameObject ef = Resources.Load ("Prefabs/Effects/Recovery_HP", typeof(GameObject)) as GameObject;
		MonoBehaviour.Instantiate (ef, s.transform.position, Quaternion.Euler (270.0f, 0.0f, 0.0f));
	}

	public override void ItemEvent (ShinBattleController[] ss)
	{
		foreach (ShinBattleController s in ss)
		{
			s.Recovery (true, amount);
			GameObject ef = Resources.Load ("Prefabs/Effects/Recovery_HP", typeof(GameObject)) as GameObject;
			MonoBehaviour.Instantiate (ef, s.transform.position, Quaternion.Euler (270.0f, 0.0f, 0.0f));
		}
	}
}

//MP回復アイテム
public class MPRecoveryItem : ItemBase {

	private float amount;

	public MPRecoveryItem (ItemData i, int n, float a) : base (i, n)
	{
		amount = a;
	}
	
	public override void ItemEvent (ShinBattleController s)
	{
		s.Recovery (false,amount);
		GameObject ef = Resources.Load ("Prefabs/Effects/Recovery_MP", typeof(GameObject)) as GameObject;
		MonoBehaviour.Instantiate (ef, s.transform.position, Quaternion.Euler (270.0f, 0.0f, 0.0f));
	}
	
	public override void ItemEvent (ShinBattleController[] ss)
	{
		foreach (ShinBattleController s in ss)
		{
			s.Recovery (false, amount);
			GameObject ef = Resources.Load ("Prefabs/Effects/Recovery_MP", typeof(GameObject)) as GameObject;
			MonoBehaviour.Instantiate (ef, s.transform.position, Quaternion.Euler (270.0f, 0.0f, 0.0f));
		}
	}
}