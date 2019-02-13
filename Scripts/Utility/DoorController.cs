using UnityEngine;
using System.Collections;

public class DoorController : WarpController {

	private GameObject icon;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected override void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Leader")
		{
			if (icon == null)
				icon = Instantiate (Resources.Load ("Prefabs/UIPrefabs/icon_proceed") as GameObject) as GameObject;
		}
	}

	protected override void OnTriggerStay (Collider other)
	{
		if (other.tag == "Leader")
		{
			if (sceneManager.fade == null && sceneManager.inputManger.AttackButton == 1 && other.GetComponent<ShinFieldController> ().enabled)
			{
				sceneManager.CreateFadeAnimate ();
				sceneManager.inputManger.enabled = false;
				sceneManager.inputManger.ClearInput ();
				Destroy (icon);
				sceneManager.GetComponent<BattleManager> ().StopFieldUnit ();
			}
			else if (sceneManager.fade != null)
			{
				if (sceneManager.fade.isComplete)
				{
					int piont = nextStage > sceneManager.nowStage ? 0 : 1;
					sceneManager.DeleteStage ();
					sceneManager.CreateStage (nextStage, nextPosition);
					sceneManager.nowStage = nextStage;
					sceneManager.InitStage (piont);
					sceneManager.inputManger.enabled = true;
				}
			}
		}
	}

	protected void OnTriggerExit (Collider other)
	{
		if (other.tag == "Leader")
		{
			Destroy (icon);
		}
	}
}
