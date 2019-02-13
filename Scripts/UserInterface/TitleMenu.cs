using UnityEngine;
using System.Collections;

public class TitleMenu : MonoBehaviour {

	private GUITexture newGame, continueGame, cursor; 

	private int select = 0;

	private string[] fileNames = new string[4]{
												"UI/new_normal",
												"UI/new_selected",
												"UI/continue_normal",
												"UI/continue_select"};

	private InputManager inputManager;

	// Use this for initialization
	void Start () {

		newGame = GameObject.Find ("new game").GetComponent <GUITexture> ();
	
		continueGame = GameObject.Find ("continue").GetComponent <GUITexture> ();

		cursor = GameObject.Find ("curosr").GetComponent <GUITexture> ();

		cursor.transform.position = new Vector3 (0.5f, 0.28f, 0.0f);

		inputManager = GameObject.Find ("Main Camera").GetComponent <InputManager> ();

		newGame.texture = Resources.Load (select == 0 ? fileNames[1] : fileNames[0], typeof(Texture)) as Texture;
		continueGame.texture = Resources.Load (select == 1 ? fileNames[3] :  fileNames[2], typeof(Texture)) as Texture;
	}
	
	// Update is called once per frame
	void Update () {
			
		if (Mathf.Abs (inputManager.DpadY) > 0.5f || Mathf.Abs (inputManager.vertical) > 0.2f)
		{
			select += (int)inputManager.DpadY - Mathf.FloorToInt (inputManager.vertical);
			select = (int)Mathf.Clamp01 (select);

			newGame.texture = Resources.Load (select == 0 ? fileNames[1] : fileNames[0], typeof(Texture)) as Texture;
			continueGame.texture = Resources.Load (select == 1 ? fileNames[3] : fileNames[2], typeof(Texture)) as Texture;

			cursor.transform.position = new Vector3 (0.5f, 0.28f - select/10.0f, 0.0f);
		}

		cursor.color = new Color (0.5f, 0.5f, 0.5f, Mathf.PingPong (Time.time, 1.0f)/1.5f + 0.2f);

		if (select == 0 && inputManager.DecisionButton == 1)
			Application.LoadLevel ("GameScene");
	
	}
}
