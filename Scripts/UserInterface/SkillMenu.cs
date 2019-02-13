using UnityEngine;
using System.Collections;

public class SkillMenu : FieldUIManager {

	private GameObject skillPreview;
	private GameObject mainMenu;

	private int nowChara;

	private GUITexture charaName;
	private GUITexture charaImage;
	private GUIText[] charaNumeric;
	private GUITexture[] charaGauge;

	private GUIText[] skill;
	private GUIText[] skillList;
	private GUIText skillName;
	private GUIText skillAttack;
	private GUIText skillCost;
	private GUIText skillText;

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
		float y = (398-cursor.nowNumber*35) * Screen.height / 720.0f;
		highLight.pixelInset = new Rect (highLight.pixelInset.x, y, highLight.pixelInset.width, highLight.pixelInset.height);


		SkillsData selectSkill = status.playersData[nowChara].status.signSkill[cursor.nowNumber];
		if (selectSkill != null)
		{
			skillName.text = selectSkill.skillName;
			skillAttack.text = (selectSkill.correction*100).ToString ();
			skillCost.text = selectSkill.cost.ToString ();
			skillText.text = selectSkill.skillText;
		}
		else
		{
			skillName.text = "";
			skillAttack.text = "";
			skillCost.text = "";
			skillText.text = "";
		}
	}

	public override void CancelEvent ()
	{
		Destroy (skillPreview);
		mainMenu.SetActive (true);
		GetComponent<SkillMenu> ().enabled = false;
	}

	public override void ChangeCategoryEvent ()
	{
		float ww = Screen.width / 1280.0f;
		float hw = Screen.height / 720.0f;

		nowChara = status.partyMember[cursor.nowCategory];

		charaName.texture = status.playersData[nowChara].nameFolder;
		charaImage.texture = status.playersData[nowChara].status.iconFace[0];

		ChangeStatusGaugeTwo (ref charaGauge, ref charaNumeric, status.playersData[nowChara].status, -215*ww);
		for (int i=0; i<4; i++)
		{
			SkillsData sd = status.playersData[nowChara].status.signSkill[i];
			
			if (sd != null)
				skill[i].text = sd.skillName;
			else
				skill[i].text = "";
		}

		for (int i=0; i<skillList.Length; i++)
		{
			if (skillList[i] != null)
				Destroy (skillList[i].gameObject);
		}

		skillList = new GUIText[status.playersData[nowChara].status.skillList.Count];

		float x = 672*ww;
		float y = 565*hw;

		for (int i=0; i<status.playersData[nowChara].status.skillList.Count; i++)
		{		
			SkillsData sd = status.playersData[nowChara].status.skillList[i];
			if (!sd.signUp)
			{
				skillList[i] =CreateGUITextRerurn ("skill", sd.skillName, (int)(20*hw),
				                                   new Vector2 (x, y), KozMinPro_Heavy,
				                                   skillPreview.transform, 2.3f, TextAnchor.UpperLeft);
				skillList[i].color = new Color (1.0f, 0.95f, 0.79f);
				
				y -= 35*hw;
			}
		}

		SkillsData selectSkill = status.playersData[nowChara].status.signSkill[cursor.nowNumber];
		if (selectSkill != null)
		{
			skillName.text = selectSkill.skillName;
			skillAttack.text = (selectSkill.correction*100).ToString ();
			skillCost.text = selectSkill.cost.ToString ();
			skillText.text = selectSkill.skillText;
		}
		else
		{
			skillName.text = "";
			skillAttack.text = "";
			skillCost.text = "";
			skillText.text = "";
		}
	}
	
	void CreateSkillPreview (Transform t)
	{
		float ww = Screen.width / 1280.0f;
		float hw = Screen.height / 720.0f;

		CreateGUITexture ("skill_frame", fieldPath+"menu_skill_frame", new Rect (102*ww, 68*hw, 1077*ww, 580*hw), t, 2.1f);
		CreateGUITexture ("status_text", fieldPath+"ui_menu_skill", new Rect (200*ww, 672*hw, 546*ww, 42*hw), t, 2.2f);

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

		skill = new GUIText[4];
		for (int i=0; i<4; i++)
		{
			SkillsData sd = status.playersData[nowChara].status.signSkill[i];

			if (sd != null)
			{
				skill[i] = CreateGUITextRerurn ("skill", sd.skillName, (int)(20*hw),
				                                new Vector2 (609*ww, 418*hw-35*hw*i),
				                                KozMinPro_Heavy, t, 2.3f, TextAnchor.UpperRight);
			}
			else
			{
				skill[i] = CreateGUITextRerurn ("skill", "", (int)(20*hw),
				                                new Vector2 (609*ww, 418*hw-35*hw*i),
				                                KozMinPro_Heavy, t, 2.3f, TextAnchor.UpperRight);
			}
			skill[i].color = new Color (1.0f, 0.95f, 0.79f);
		}

		skillList = new GUIText[status.playersData[nowChara].status.skillList.Count];

		float x = 672*ww;
		float y = 565*hw;

		for (int i=0; i<status.playersData[nowChara].status.skillList.Count; i++)
		{
			SkillsData sd = status.playersData[nowChara].status.skillList[i];
			if (!sd.signUp)
			{
				skillList[i] =CreateGUITextRerurn ("skill", sd.skillName, (int)(20*hw),
				                                   new Vector2 (x, y), KozMinPro_Heavy,
				                                   t, 2.3f, TextAnchor.UpperLeft);
				skillList[i].color = new Color (1.0f, 0.95f, 0.79f);

				y -= 35*hw;
			}
		}

		int[][] sand = new int[2][]{
			new int[3] {1, 0, (int)(110*ww)},
			new int[3] {4, (int)(-35*hw), (int)(360*hw)},
		};

		cursor = CreateCursor (t);
		cursor.Prepare (inputManager, this, sand, 4);
		cursor.InitCategory (status.GetPartyNumber ());

		SkillsData selectSkill = status.playersData[nowChara].status.signSkill[cursor.nowNumber];
		if (selectSkill != null)
		{
			skillName = CreateGUITextRerurn ("skill_name", selectSkill.skillName, (int)(24*hw),
			                                 new Vector2 (152*ww, 160*hw), KozMinPro_Heavy, t,
			                                 2.3f, TextAnchor.UpperLeft);
			skillAttack = CreateGUITextRerurn ("skill_attack", (selectSkill.correction*100).ToString (), (int)(20*hw),
			                                   new Vector2 (880*ww, 160*hw), KozMinPro_Bold, t,
			                                   2.3f, TextAnchor.UpperLeft);
			skillCost = CreateGUITextRerurn ("skill_cost", selectSkill.cost.ToString (), (int)(20*hw),
			                                   new Vector2 (1048*ww, 160*hw), KozMinPro_Bold, t,
			                                   2.3f, TextAnchor.UpperLeft);
			skillText = CreateGUITextRerurn ("skill_text", selectSkill.skillText, (int)(20*hw),
			                                 new Vector2 (150*ww, 125*hw), KozMinPro_Bold, t,
			                                 2.3f, TextAnchor.UpperLeft);
		}

		highLight = CreateGUITexture ("high_light", "UI/common/ui_highlight", new Rect (157*ww, 398*hw, 460*ww, 25*hw));
		highLight.transform.parent = t;
		highLight.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);

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

		skillPreview = CreateGUITexture ("Status").gameObject;
		CreateSkillPreview (skillPreview.transform);
	}
}
