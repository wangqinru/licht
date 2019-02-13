using UnityEngine;
using System.Collections;

public class WarpController : MonoBehaviour {

	protected int nextStage;

	protected Vector3 nextPosition;

	protected SceneManager sceneManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Prepare (int next, Vector3 nextPos, SceneManager s)
	{
		nextStage = next;
		nextPosition = nextPos;
		sceneManager = s;
	}
	
	protected virtual void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Leader" && other.GetComponent<ShinFieldController> ().enabled)
		{
			sceneManager.CreateFadeAnimate ();
			sceneManager.inputManger.enabled = false;
			sceneManager.GetComponent<BattleManager> ().StopFieldUnit ();
		}
	}

	protected virtual void OnTriggerStay (Collider other)
	{
		if (other.tag == "Leader")
		{
			if (sceneManager.fade.isComplete)
			{
				int point = nextStage > sceneManager.nowStage ? 0 : 1;
				sceneManager.DeleteStage ();
				sceneManager.CreateStage (nextStage, nextPosition);
				sceneManager.nowStage = nextStage;
				sceneManager.InitStage (point);
				sceneManager.inputManger.enabled = true;
			}
		}
	}
}
