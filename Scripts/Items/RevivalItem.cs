using UnityEngine;
using System.Collections;

public class RevivalItem : ItemBase {

	private float amount;

	public RevivalItem (ItemData i, int n, float a) : base (i, n)
	{
		amount = a;
	}

	public override void ItemEvent (ShinBattleController s)
	{
		s.enabled = true;
		s.GetComponent<CharacterController> ().enabled = true;
		s.Revival ();
		s.Recovery (true, amount);
		GameObject ef = Resources.Load ("Prefabs/Effects/Recovery_HP", typeof(GameObject)) as GameObject;
		MonoBehaviour.Instantiate (ef, s.transform.position, Quaternion.Euler (270.0f, 0.0f, 0.0f));
	}

	public override void ItemEvent (ShinBattleController[] ss)
	{
		foreach (ShinBattleController s in ss)
		{
			s.enabled = true;
			s.GetComponent<CharacterController> ().enabled = true;
			s.Revival ();
			s.Recovery (true, amount);
			GameObject ef = Resources.Load ("Prefabs/Effects/Recovery_HP", typeof(GameObject)) as GameObject;
			MonoBehaviour.Instantiate (ef, s.transform.position, Quaternion.Euler (270.0f, 0.0f, 0.0f));
		}
	}
}
