using UnityEngine;
using System.Collections;

public class TargetSelect : MonoBehaviour {

	private BattleManager battleManager;
	private InputManager inputManager;
	private BattleUIItem itemMenu;
	private int target;
	private bool deadTarget;

	private float counter = 0.0f;
	private GUITexture targetLight;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//int old = target;

		counter = Mathf.PingPong (Time.time, 0.5f);
		targetLight.color = new Color (0.5f, 0.5f, 0.5f, counter);

		if (inputManager.StickLR == 1) 
		{
			target = battleManager.GetNextTarget (target, deadTarget);
			targetLight.transform.position = new Vector3 (0.15f+target*0.24f, targetLight.transform.position.y, 2.5f);
		}
		if (inputManager.StickLR == -1) 
		{
			target = battleManager.GetBeforeTarget (target, deadTarget);
			targetLight.transform.position = new Vector3 (0.15f+target*0.24f, targetLight.transform.position.y, 2.5f);
		}

		if (inputManager.DecisionButton == 1 && target != -1)
		{
			itemMenu.UseItem (target);
			inputManager.speed = 0;
			Destroy (this.gameObject);
		}
		
		if (inputManager.CancelButton == 1)
		{
			itemMenu.gameObject.SetActive (true);
			inputManager.speed = 0;
			Destroy (this.gameObject);
		}
	}

	public void Prepare (StatusData sd, BattleUIItem mo, bool flag)
	{
		float ww = Screen.width / 1280.0f;
		float hw = Screen.height / 720.0f;

		targetLight = GetComponent<GUITexture> ();
		targetLight.pixelInset = new Rect (targetLight.pixelInset.x, targetLight.pixelInset.y, 
		                                   targetLight.pixelInset.width*ww, targetLight.pixelInset.height*hw);
		itemMenu = mo;
		inputManager = sd.GetComponent<InputManager> ();
		inputManager.speed = 20;
		battleManager = sd.GetComponent<BattleManager> ();
		deadTarget = flag;

		target = battleManager.GetNextTarget (-1, deadTarget);
		targetLight.transform.position = new Vector3 (0.15f+target*0.24f, targetLight.transform.position.y+0.01f, 2.5f);
		if (target==-1) targetLight.texture = null;
	}

}
