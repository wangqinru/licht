using UnityEngine;
using System.Collections;

public class GameTitle : MonoBehaviour {

	private GameObject[] titleObject = new GameObject[2];

	private GUITexture backGround, titleLogo, start;

	private float nativeHorizontalResolution = 1280.0f;

	private float nativeVerticalResolution = 720.0f;

	private InputManager inputManager;

	// Use this for initialization
	void Start () {

		titleObject[0] = Resources.Load ("Prefabs/UIPrefabs/TitleBg", typeof(GameObject)) as GameObject;

		titleObject[1] = Resources.Load ("Prefabs/UIPrefabs/TitleMenu", typeof(GameObject)) as GameObject;

		Instantiate (titleObject[0], Vector3.zero, Quaternion.identity);

		backGround = GameObject.Find ("titlebackground").GetComponent <GUITexture> ();

		titleLogo = GameObject.Find ("titlelogo").GetComponent <GUITexture> ();

		start = GameObject.Find ("pressstart").GetComponent <GUITexture> ();

		backGround.color = new Color (0, 0, 0);

		titleLogo.color = new Color (0.5f, 0.5f, 0.5f, 0);

		start.color = new Color (0.5f, 0.5f, 0.5f, 0);

		inputManager = GetComponent <InputManager> ();

		StartCoroutine (FadeIn ());
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI () 
	{		
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (Screen.width/nativeHorizontalResolution, Screen.height/nativeVerticalResolution, 1));

		backGround.pixelInset = new Rect (0.0f, 0.0f, Screen.width, Screen.height);
	}

	IEnumerator FadeIn ()
	{
		int counter = 0;

		while (counter < 300)
		{
			counter ++; 

			float col = (counter / 300.0f);
			col = Mathf.Clamp (col, 0.0f, 0.5f);
			
			backGround.color = new Color (col, col, col);
			
			float coll = (counter - 150) / 200.0f;
			coll = Mathf.Clamp (coll, 0.0f, 1.0f);
			
			titleLogo.color = new Color (0.5f, 0.5f, 0.5f, coll);

			if (inputManager.StartButton == 1)
			{
				backGround.color = new Color (0.5f, 0.5f, 0.5f);

				titleLogo.color = new Color (0.5f, 0.5f, 0.5f, 1.0f);

				break;
			}

			yield return 0;
		}

		while (true)
		{
			start.color = new Color (0.5f, 0.5f, 0.5f, Mathf.PingPong (Time.time, 1.5f)/3 + 0.1f);

			if (inputManager.StartButton == 1)
			{
				//Instantiate (titleObject[1], Vector3.zero, Quaternion.identity);

				//start.color = new Color (0.5f, 0.5f, 0.5f, 0.0f);
				Application.LoadLevel ("GameScene");
				break;
			}

			yield return 0;
		}

	}
}
