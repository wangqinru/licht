using UnityEngine;
using System.Collections;

public class EquipMenu : FieldUIManager {

	private GameObject equipPreview;
	private GameObject mainMenu;
	
	private int nowChara;
	
	private GUITexture charaName;
	private GUITexture charaImage;
	private GUIText[] charaNumeric;
	private GUITexture[] charaGauge;
	
	private GUIText[] charaEquip;
	private GUIText[] charaParameters;
	private GUIText equips;
	private GUITexture equipImage;
	private GUIText equipName;
	private GUIText equipText;
	
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
		float y = (400-cursor.nowNumber*37) * Screen.height / 720.0f;
		highLight.pixelInset = new Rect (highLight.pixelInset.x, y, highLight.pixelInset.width, highLight.pixelInset.height);
		equipImage.texture = status.playersData[nowChara].equips[cursor.nowNumber].image;
		equipName.text = status.playersData[nowChara].equips[cursor.nowNumber].name;
		equipText.text = status.playersData[nowChara].equips[cursor.nowNumber].work;
	}
	
	public override void CancelEvent ()
	{
		Destroy (equipPreview);
		mainMenu.SetActive (true);
		GetComponent<SkillMenu> ().enabled = false;
	}
	
	public override void ChangeCategoryEvent ()
	{
		float ww = Screen.width / 1280.0f;
		//float hw = Screen.height / 720.0f;
		
		nowChara = status.partyMember[cursor.nowCategory];
		
		charaName.texture = status.playersData[nowChara].nameFolder;
		charaImage.texture = status.playersData[nowChara].status.iconFace[0];
		
		ChangeStatusGaugeTwo (ref charaGauge, ref charaNumeric, status.playersData[nowChara].status, -215*ww);

		int[] parameters = new int[6]{
			status.playersData[nowChara].status.attack,
			status.playersData[nowChara].status.defense,
			status.playersData[nowChara].status.magicAttack,
			status.playersData[nowChara].status.magicDefense,
			status.playersData[nowChara].status.speed,
			status.playersData[nowChara].status.lucky,			
		};

		for (int i=0; i<6; i++)
		{
			charaParameters[i].text = parameters[i].ToString ();
		}
	}
	
	void CreateEquipPreview (Transform t)
	{
		float ww = Screen.width / 1280.0f;
		float hw = Screen.height / 720.0f;
		
		CreateGUITexture ("skill_frame", fieldPath+"menu_equip_frame", new Rect (102*ww, 68*hw, 1077*ww, 580*hw), t, 2.1f);
		CreateGUITexture ("status_text", fieldPath+"ui_menu_equip", new Rect (200*ww, 672*hw, 546*ww, 42*hw), t, 2.2f);
		
		charaName = CreateGUITexture ("chara_name", status.playersData[nowChara].nameFolder, new Rect (248*ww, 582*hw, 263*ww, 40*hw));
		charaName.transform.parent = t;
		charaName.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.3f);
		
		charaImage = CreateGUITexture ("chara_img", status.playersData[nowChara].status.iconFace[0], new Rect (137*ww, 514*hw, 114*ww, 114*hw));
		charaImage.transform.parent = t;
		charaImage.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.3f);
		
		charaGauge = new GUITexture[2];
		charaNumeric = new GUIText[2];
		CreateStatusGaugeTwo (t, ref charaGauge, ref charaNumeric,
		                      status.playersData[nowChara].status,
		                      new Vector2 (486*ww, 561*hw), new Rect (486*ww, 540*hw, -215*ww, 5*hw),
		                      -27*hw, 2.3f, (int)(14*hw), KozMinPro_Medium);
		
		int[][] sand = new int[2][]{
			new int[3] {1, 0, (int)(105*ww)},
			new int[3] {4, (int)(-35*hw), (int)(365*hw)},
		};
		
		cursor = CreateCursor (t);
		cursor.Prepare (inputManager, this, sand, 4);
		cursor.InitCategory (status.GetPartyNumber ());

		highLight = CreateHighLight (t, new Vector2 (152*ww, 400*hw));
		highLight.pixelInset = new Rect (152*ww, 400*hw, 455*ww, 30*hw);
		highLight.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);

		int[] parameters = new int[6]{
			status.playersData[nowChara].status.attack,
			status.playersData[nowChara].status.defense,
			status.playersData[nowChara].status.magicAttack,
			status.playersData[nowChara].status.magicDefense,
			status.playersData[nowChara].status.speed,
			status.playersData[nowChara].status.lucky,			
		};

		charaParameters = new GUIText[6];
		for (int i=0; i<6; i++)
		{
			charaParameters[i] = CreateGUITextRerurn ("parameters", parameters[i].ToString (), (int)(20*hw),
			                                          new Vector2 (265*ww + (i/3)*245*ww, 232*hw - (i%3)*35*hw), 
			                                          KozMinPro_Bold, t,
			                                          2.3f, TextAnchor.UpperLeft);
			charaParameters[i].color = new Color (1.0f, 0.95f, 0.79f);
		}

		charaEquip = new GUIText[4];
		for (int i=0; i<4; i++)
		{
			charaEquip[i] = CreateGUITextRerurn ("equip", status.playersData[nowChara].equips[i].name, 
			                                     (int)(20*ww), new Vector2 (610*ww, 425*hw-36*i*hw), KozMinPro_Bold, 
			                                     t, 2.3f, TextAnchor.UpperRight);
			charaEquip[i].color = new Color (71.0f/255.0f, 61.0f/255.0f, 43.0f/255.0f);
		}

		equipImage = CreateGUITexture ("equip_image", status.playersData[nowChara].equips[cursor.nowNumber].image, 
		                               new Rect (650*ww, 90*hw, 72*ww, 72*hw));
		equipImage.transform.parent = t; 
		equipImage.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.3f);

		equipName = CreateGUITextRerurn ("equip_name", status.playersData[nowChara].equips[cursor.nowNumber].name, 
		                                 (int)(20*ww), new Vector2 (722*ww, 160*hw), KozMinPro_Heavy, 
		                                 t, 2.3f, TextAnchor.UpperLeft);
		equipName.color = new Color (1.0f, 0.95f, 0.79f);

		equipText = CreateGUITextRerurn ("equip_text", status.playersData[nowChara].equips[cursor.nowNumber].work, 
		                                 (int)(16*ww), new Vector2 (722*ww, 130*hw), KozMinPro_Medium, 
		                                 t, 2.3f, TextAnchor.UpperLeft);
		equipText.lineSpacing = 0.72f;
		CreateGUITexture ("Indication", fieldPath+"ui_Indication_2", new Rect (415*ww, 3*hw, 450*ww, 52*hw), t, 2.2f);
		CreateArrow (t, new Vector2 (5*ww, 333*hw), new Vector2 (1226*ww, 333*hw));
	}
	
	public override void Prepare (InputManager im, StatusData s, GameObject mm)
	{
		inputManager = im;
		sceneManager = inputManager.GetComponent<SceneManager> ();
		status = s;
		mainMenu = mm;
		
		nowChara = status.partyMember[0];
		InitFont ();
		
		equipPreview = CreateGUITexture ("equip").gameObject;
		CreateEquipPreview (equipPreview.transform);
	}
}
