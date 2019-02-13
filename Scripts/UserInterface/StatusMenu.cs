using UnityEngine;
using System.Collections;

public class StatusMenu : FieldUIManager {

	private GameObject statusPreview;

	private int nowChara;
	private GUITexture charaImage;
	private GUITexture charaName;
	private GUIText charaLevel;
	private GUIText[] charaNumeric;
	private GUITexture[] charaGauge;
	private GUIText charaTotalExp;
	private GUIText charaIntroduction;
	private GUIText[] charaParameters;

	private GameObject mainMenu;
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
		Destroy (statusPreview);
		mainMenu.SetActive (true);
		GetComponent<StatusMenu> ().enabled = false;
	}

	public override void ChangeCategoryEvent ()
	{
		nowChara = status.partyMember[cursor.nowCategory];

		charaImage.texture = status.playersData[nowChara].charaImage[1];
		charaName.texture = status.playersData[nowChara].nameFolder;
		charaLevel.text = status.playersData[nowChara].status.level.ToString ();

		ChangeStatusGauge (ref charaGauge, ref charaNumeric, status.playersData[nowChara].status,Screen. width/1280.0f*-274);

		int[] parameters = new int[6]{
			status.playersData[nowChara].status.attack,
			status.playersData[nowChara].status.defense,
			status.playersData[nowChara].status.magicAttack,
			status.playersData[nowChara].status.magicDefense,
			status.playersData[nowChara].status.speed,
			status.playersData[nowChara].status.lucky,
		};

		for (int i=0; i<parameters.Length; i++)
		{
			charaParameters[i].text = parameters[i].ToString ();
		}

		charaIntroduction.text = status.playersData[nowChara].personality;
	}

	public override void Prepare (InputManager im, StatusData s, GameObject mm)
	{
		inputManager = im;
		sceneManager = inputManager.GetComponent<SceneManager> ();
		status = s;
		mainMenu = mm;
		InitFont ();

		statusPreview = CreateGUITexture ("Status").gameObject;
		nowChara = 0;

		CreateStatusPreview (statusPreview.transform);
	}
		
	void CreateStatusPreview (Transform t)
	{
		float ww = Screen.width / 1280.0f;
		float hw = Screen.height / 720.0f;

		nowChara = status.partyMember[0];
		CreateGUITexture ("status_text", fieldPath+"ui_menu_status", new Rect (200*ww, 672*hw, 546*ww, 42*hw), t, 2.2f);

		charaImage = CreateGUITexture ("chara_img", status.playersData[nowChara].charaImage[1], new Rect (480*ww, 57*hw, 800*ww, 606*hw));
		charaImage.transform.parent = t;
		charaImage.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.1f);

		CreateGUITexture ("status_frame", fieldPath+"ui_status_frame", new Rect (102*ww, 70*hw, 695*ww, 580*hw), t, 2.2f); 

		charaName = CreateGUITexture ("chara_name", status.playersData[nowChara].nameFolder, new Rect (235*ww, 545*hw, 420*ww, 64*hw));
		charaName.transform.parent = t;
		charaName.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.3f);

		charaLevel = CreateGUITextRerurn ("chara_level", status.playersData[nowChara].status.level.ToString (), 
		                           		 (int)(38*hw), new Vector2 (420*ww, 482*hw), KozMinPro_Bold, t, 2.3f, TextAnchor.UpperRight); 
		charaLevel.color = new Color (1.0f, 0.95f, 0.79f);

		charaNumeric = new GUIText[3];
		charaGauge = new GUITexture[3];

		CreateStatusGauge (t, ref charaGauge, ref charaNumeric, status.playersData[nowChara].status, 
		                   new Vector2 (432*ww, 418*hw), new Rect (432*ww, 383*hw, -274*ww, 13*hw), 
		                   -54*hw, 2.3f, (int)(20*hw), KozMinPro_Medium);  

		charaTotalExp = CreateGUITextRerurn ("chara_totalExp", status.playersData[nowChara].totalExp.ToString (),
		                                     (int)(20*hw), new Vector2 (432*ww, 270*hw), KozMinPro_Bold, t, 2.3f, TextAnchor.UpperRight);
		charaTotalExp.color = new Color (1.0f, 0.95f, 0.79f);

		charaIntroduction = CreateGUITextRerurn ("chara_introduction", status.playersData[nowChara].personality, (int)(16*hw),
		                                         new Vector2 (168*ww, 203*hw), KozMinPro_Medium, t, 2.3f, TextAnchor.UpperLeft);
		charaIntroduction.color = new Color (0.0f, 0.0f, 0.0f);
		charaIntroduction.lineSpacing = 0.7f;

		int[] parameters = new int[6]{
			status.playersData[nowChara].status.attack,
			status.playersData[nowChara].status.defense,
			status.playersData[nowChara].status.magicAttack,
			status.playersData[nowChara].status.magicDefense,
			status.playersData[nowChara].status.speed,
			status.playersData[nowChara].status.lucky,
		};

		charaParameters = new GUIText[parameters.Length];

		for (int i=0; i<parameters.Length; i++)
		{
			charaParameters[i] = CreateGUITextRerurn ("chara_parameters", parameters[i].ToString (), (int)(20*hw), 
			                                          new Vector2 (750*ww, 480*hw-34*hw*i),
			                                          KozMinPro_Medium, t, 2.3f, TextAnchor.UpperRight);
			charaParameters[i].color = new Color (1.0f, 0.95f, 0.79f);
		}

		int[][] st = new int[2][]{
			new int[3]{1, 0, (int)(Screen.width*2)},
			new int[3]{1, 0, (int)(Screen.height*2)},
		};
		cursor = CreateCursor (t);
		cursor.Prepare (inputManager, this, st, 5);
		cursor.InitCategory (status.GetPartyNumber ());

		CreateGUITexture ("Indication", fieldPath+"ui_Indication_2", new Rect (415*ww, 3*hw, 450*ww, 52*hw), t, 2.2f);
		CreateArrow (t, new Vector2 (5*ww, 333*hw), new Vector2 (1226*ww, 333*hw));
	}
}
