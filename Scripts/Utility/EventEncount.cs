using UnityEngine;
using System.Collections;

public class EventEncount : MonoBehaviour {

	private BattleManager battleManager;
	private int eventID;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay (Collider other)
	{
		if (other.tag == "Leader")
		{
			this.GetComponent<SphereCollider> ().enabled = false;
			battleManager.InitBattle (eventID);
		}
	}

	public void Prepare (BattleManager bm, int ei)
	{
		battleManager = bm;
		eventID = ei;
	}
}
