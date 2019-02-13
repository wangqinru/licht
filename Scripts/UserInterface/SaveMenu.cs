using UnityEngine;
using System.Collections;

public class SaveMenu : FieldUIManager {

	private GameObject savePreview;
	private GameObject mainMenu;
	
	private int nowCategory;

	private GUITexture[] saveData;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override void ButtonEvent (int bID)
	{
		
	}
	
	public override void ButtonOver ()
	{
		
	}
	
	public override void CancelEvent ()
	{
		Destroy (savePreview);
		mainMenu.SetActive (true);
		GetComponent<SkillMenu> ().enabled = false;
	}
	
	public override void ChangeCategoryEvent ()
	{
		float ww = Screen.width / 1280.0f;
		float hw = Screen.height / 720.0f;
		
		nowCategory = cursor.nowCategory;
		
		highLight.pixelInset = new Rect (109*ww, 548*hw-86*hw*nowCategory, 313*ww, 81*hw);
	}
	
	void CreateSavePreview (Transform t)
	{
		float ww = Screen.width / 1280.0f;
		float hw = Screen.height / 720.0f;
		
		CreateGUITexture ("skill_frame", fieldPath+"menu_save_frame", new Rect (88*ww, 68*hw, 1107*ww, 580*hw), t, 2.1f);
		CreateGUITexture ("status_text", fieldPath+"ui_menu_save", new Rect (200*ww, 672*hw, 546*ww, 42*hw), t, 2.2f);
		
		highLight = CreateGUITexture ("highlight", fieldPath+"save_select", new Rect (109*ww, 548*hw, 313*ww, 81*hw));
		highLight.transform.parent = t;
		highLight.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);
		
		int[][] st = new int[2][]{
			new int[3]{1, 0, (int)(448*ww)},
			new int[3]{4, (int)(-120*hw), (int)(511*hw)},
		};
		cursor = CreateCursor (t);
		cursor.Prepare (inputManager, this, st, 4);
		cursor.InitCategory (2);

		saveData = new GUITexture[4];
		for (int i=0; i<4; i++)
		{
			CreateGUITexture ("savedata_frame", fieldPath+"savedata_frame", new Rect (448*ww, 511*hw-118*hw*i, 714*ww, 122*hw), t, 2.2f);
			saveData[i] = CreateGUITexture ("savedata", fieldPath+"savedata_nodata", new Rect (448*ww, 511*hw-118*hw*i, 714*ww, 122*hw));
			saveData[i].transform.parent = t;
			saveData[i].transform.localPosition = new Vector3 (0.0f, 0.0f, 2.3f);
		}

		CreateGUITexture ("save_category", fieldPath+"save_category", new Rect (190*ww, 443*hw, 150*ww, 205*hw), t, 2.3f);
		
		CreateGUITexture ("Indication", fieldPath+"ui_Indication_1", new Rect (415*ww, 3*hw, 450*ww, 52*hw), t, 2.2f);
	}
	
	public override void Prepare (InputManager im, StatusData s, GameObject mm)
	{
		inputManager = im;
		sceneManager = inputManager.GetComponent<SceneManager> ();
		status = s;
		mainMenu = mm;
		
		nowCategory = 0;
		InitFont ();
		
		savePreview = CreateGUITexture ("equip").gameObject;
		CreateSavePreview (savePreview.transform);
	}
}
