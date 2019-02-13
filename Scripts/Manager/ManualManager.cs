using UnityEngine;
using System.Collections;

public class ManualManager : MonoBehaviour {

	private InputManager inputManager;
	private SceneManager sceneManager;
	private BattleManager battleManager;
	private GUITexture manual;
	private bool isField;

	private string[] manualImage = new string[2]{
		"UI/Manual/gField",
		"UI/Manual/gBattle",
	};
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (inputManager.ButtonB == 1)
		{
			if (isField)
			{
				sceneManager.enabled = true;
				battleManager.RestartFieldUnit ();
			}
			else
			{
				battleManager.enabled = true;
				battleManager.RestartAnimation ();
			}

			Destroy (manual.gameObject);
			this.enabled = false;
		}
	}

	public void Prepare (bool flag, InputManager im)
	{
		isField = flag;
		inputManager = im;
		sceneManager = inputManager.GetComponent<SceneManager> ();
		battleManager = inputManager.GetComponent<BattleManager> ();
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/Manual", typeof(GameObject)) as GameObject) as GameObject;
		manual = o.GetComponent<GUITexture> ();
		if (!flag)
			manual.texture = Resources.Load (manualImage[1], typeof(Texture)) as Texture;
	}
}
