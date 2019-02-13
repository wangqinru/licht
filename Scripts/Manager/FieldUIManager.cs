using UnityEngine;
using System.Collections;

public class FieldUIManager : MenuObject {

	protected InputManager inputManager;
	protected SceneManager sceneManager;
	protected StatusData status;

	protected CursorObject cursor;
	protected GUITexture highLight;
	private GUITexture nowSelect;

	private GameObject mainMenu;

	protected const string fieldPath = "UI/field/";
	private Texture[] nowSelectImage;

	protected Font KozMinPro_Bold { get; private set;}
	protected Font KozMinPro_Medium { get; private set;}
	protected Font KozMinPro_Heavy { get; private set;}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void ButtonEvent (int bID)
	{
		mainMenu.SetActive (false);

		switch (bID)
		{
			//	ステータス
		case	0:
			GetComponent<StatusMenu> ().enabled = true;
			GetComponent<StatusMenu> ().Prepare (inputManager, status, mainMenu);
			break;
			//スキル
		case	1:
			GetComponent<SkillMenu> ().enabled = true;
			GetComponent<SkillMenu> ().Prepare (inputManager, status, mainMenu);
			break;
			//装備
		case	2:
			GetComponent<EquipMenu> ().enabled = true;
			GetComponent<EquipMenu> ().Prepare (inputManager, status, mainMenu);
			break;
			//アイテム
		case	3:
			GetComponent<ItemMenu> ().enabled = true;
			GetComponent<ItemMenu> ().Prepare (inputManager, status, mainMenu);
			break;
			//セーブ
		case	4:
			Application.LoadLevel ("TitleScene");
			break;
		}
	}

	public override void ButtonOver ()
	{
		float wh = Screen.height / 720.0f;

		highLight.pixelInset = new Rect (highLight.pixelInset.x, 567*wh-92.0f*wh*cursor.nowNumber,
		                                 highLight.pixelInset.width, highLight.pixelInset.height);

		nowSelect.texture = nowSelectImage[cursor.nowNumber];
	}

	public override void CancelEvent ()
	{
		Destroy (gameObject);
		Destroy (mainMenu.gameObject);
		inputManager.GetComponent<BattleManager> ().RestartFieldUnit ();
		sceneManager.enabled = true;
	}

	void CreateMainMenu (Transform t)
	{
		float wm = Screen.width / 1280.0f;
		float wh = Screen.height / 720.0f;
		
		GUITexture gt = CreateGUITexture ("frame_under", fieldPath+"main_menu", new Rect(102*wm, 68*wh, 1078.0f*wm, 580.0f*wh));
		gt.transform.parent = t;
		gt.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.1f);
		
		for (int i=0; i<4; i++)
		{
			int mem = status.partyMember[i];
			if (mem > -1)
			{
				gt = CreateGUITexture ("chara_img", status.playersData[mem].charaImage[0], new Rect (385*wm+200*wm*i, 75*wh, 188.0f*wm, 572.0f*wh));
				gt.transform.parent = t;
				gt.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);
			}
		}

		highLight = CreateGUITexture ("high_light", fieldPath+"main_button_select", new Rect (105*wm, 567*wh, 266.0f*wm, 80.0f*wh));
		highLight.transform.parent = t;
		highLight.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);

		gt = CreateGUITexture ("frame_front", fieldPath+"main_menu_front", new Rect(102*wm, 68*wh, 1078.0f*wm, 580.0f*wh));
		gt.transform.parent = t;
		gt.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.3f);

		for (int i=0; i<4; i++)
		{
			int mem = status.partyMember[i];
			if (mem > -1)
			{
				gt = CreateGUITexture ("chara_name", status.playersData[mem].nameFolder, new Rect (400*wm+200.0f*wm*i, 620*wh, 420.0f*wm*0.36f, 64.0f*wh*0.36f));
				gt.transform.parent = t;
				gt.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.4f);

				GUIText text = CreateGUIText ("level", status.playersData[mem].status.level.ToString (), (int)(64.0f*wm),
				                              new Vector2 (550*wm+200.0f*wm*i, 236*wh), KozMinPro_Bold);
				text.transform.parent = t;
				text.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.4f);
				text.anchor = TextAnchor.UpperRight;
				text.fontStyle = FontStyle.Italic;
				text.color = new Color (1.0f, 0.98f, 0.8f);

				CreateStatusGauge (t, new Vector2 (565*wm+200*wm*i, 155*wh), KozMinPro_Medium, 
				                   new Rect(567*wm+200.0f*wm*i, 134*wh, -176.0f*wm, 5.0f*wh), -27*wh, status.playersData[mem].status,
				                   (int)(14.0f*wm), 2.4f, -176.0f*wm);
			}
		}

		CreateGUIText ("money", status.Money.ToString (), (int)(17.0f * wm), new Vector2 (346*wm, 172*wh),
		               KozMinPro_Heavy, t, 2.4f, TextAnchor.UpperRight);

		CreateGUIText ("time", Time.realtimeSinceStartup.ToString (), (int)(17.0f*wm), new Vector2 (346*wm, 140*wh),
		               KozMinPro_Heavy, t, 2.4f, TextAnchor.UpperRight);

		CreateGUIText ("stage", status.stageData[sceneManager.nowStage].name, (int)(17.0f*wm), new Vector2 (346*wm, 108*wh),
		               KozMinPro_Heavy, t, 2.4f, TextAnchor.UpperRight);

		int[][] st = new int[2][]{
			new int[3]{1, 0, (int)(105*wm)},
			new int[3]{5, (int)(-92*wh), (int)(567*wh)},
		};
		cursor = CreateCursor (t);
		cursor.Prepare (inputManager, this, st, 5);

		nowSelect = CreateGUITexture ("nowSelect", nowSelectImage[0], new Rect(200*wm, 672*wh, 546*wm, 42*wh));
		nowSelect.transform.parent = t;
		nowSelect.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);

		CreateGUITexture ("Indication", fieldPath+"ui_Indication_1", new Rect (415*wm, 3*wh, 450*wm, 52*wh),  t, 2.2f);
	}

	public virtual void Prepare (InputManager im, StatusData s, GameObject mm )
	{
		inputManager = im;
		sceneManager = inputManager.GetComponent<SceneManager> ();
		status = s;

		AddSubMenuComponent ();
		InitFont ();
		nowSelectImage = new Texture[5]{

			Resources.Load (fieldPath+"ui_menu_status", typeof(Texture)) as Texture,
			Resources.Load (fieldPath+"ui_menu_skill", typeof(Texture)) as Texture,
			Resources.Load (fieldPath+"ui_menu_equip", typeof(Texture)) as Texture,
			Resources.Load (fieldPath+"ui_menu_item", typeof(Texture)) as Texture,
			Resources.Load (fieldPath+"ui_menu_save", typeof(Texture)) as Texture,
		};

		mainMenu = CreateGUITexture ("MainMenu").gameObject;

		CreateMainMenu (mainMenu.transform);
	}	

	protected void InitFont ()
	{
		KozMinPro_Bold = Resources.Load ("UI/font/KozMinPro-Bold", typeof (Font)) as Font;
		KozMinPro_Medium = Resources.Load ("UI/font/KozMinPro-Medium", typeof (Font)) as Font;
		KozMinPro_Heavy = Resources.Load ("UI/font/KozMinPro-Heavy", typeof (Font)) as Font;
	}

	void AddSubMenuComponent ()
	{
		StatusMenu sm = gameObject.AddComponent<StatusMenu> ();
		sm.enabled = false;

		SkillMenu skm = gameObject.AddComponent<SkillMenu> ();
		skm.enabled = false;

		EquipMenu em = gameObject.AddComponent<EquipMenu> ();
		em.enabled = false;

		ItemMenu im = gameObject.AddComponent<ItemMenu> ();
		im.enabled = false;

		SaveMenu sam = gameObject.AddComponent<SaveMenu> ();
		sam.enabled = false;
	}
}
