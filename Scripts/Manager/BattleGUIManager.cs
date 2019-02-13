using UnityEngine;
using System.Collections;

public class BattleGUIManager : MenuObject {

	private BattleManager battleManager;
	private InputManager inputManager;
	private BattleUIStatus[] memberStatus;

	private GUITexture pauseObject;
	public GUITexture pauseMenuObject { get; set;}
	public LimitGauge itemGauge { get; set;}
	private LimitGauge escapeGauge { get; set;}

	public Font KozMinPro_Bold { get; private set;}
	public Font KozMinPro_Medium { get; private set;}
	public Texture escapeGaugeImage { get; private set;}

	// Use this for initialization
	void Start () {

		GUITexture[] partyMember = GetComponentsInChildren <GUITexture> () as GUITexture[];
		memberStatus = new BattleUIStatus[battleManager.playerList.Count];

		for (int i=0; i<battleManager.playerList.Count; i++)
		{
			partyMember[i].enabled = true;
			memberStatus[i] = partyMember[i].gameObject.AddComponent <BattleUIStatus> ();
			memberStatus[i].Prepare (battleManager.playerList[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		CallPause (inputManager.StartButton == 1);
		CallPauseMenu (inputManager.MenuButton == 1 && inputManager.SkillButton == 0);
	}

	void CallPause (bool inputFlag)
	{
		if (inputFlag)
		{
			if (pauseObject == null)
			{
				pauseObject = CreateLayerMask ("Pause");

				GUITexture pause = CreateGUITexture ("pause_img", "UI/battle/ui_pause", 
				                                     new Rect (-590, -60, 1180, 120));
				pause.transform.parent = pauseObject.transform;
				pause.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.1f);

				battleManager.StopAnimation ();
			}
			else
			{
				Destroy (pauseObject.gameObject);
				battleManager.RestartAnimation ();
			}
		}
	}

	void CallPauseMenu (bool inputFlag)
	{
		if (inputFlag)
		{
			if (pauseMenuObject == null)
			{
				int w = Screen.width/2*-1;
				int h = Screen.height/2*-1;

				float ww = Screen.width / 1280.0f;
				float hw = Screen.height / 720.0f;

				pauseMenuObject = CreateGUITexture ("PauseMenu");
				pauseMenuObject.transform.position = new Vector3 (0.5f, 0.5f, 2.0f);

				GUITexture menuFrame = CreateGUITexture ("Menu_Frame", "UI/battle/battle_menu_frame", 
				                                         new Rect (w + 427*ww, h + 240*hw, 424*ww, 160*hw));
				menuFrame.transform.parent = pauseMenuObject.transform;
				menuFrame.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.1f);

				CursorObject co = CreateCursor (pauseMenuObject.transform);
				co.Prepare (inputManager, this, new int[2][] {new int[3] {2, (int)(192*ww), (int)(w+434*ww)}, new int[3] {1, 0, (int)(h+273*hw)}}, 2);

				string[][] iPath = new string[2][] {
					new string[2] {"UI/battle/bmenu_button_item_normal", "UI/battle/bmenu_button_item_select"},
					new string[2] {"UI/battle/bmenu_button_escape_normal", "UI/battle/bmenu_button_escape_select"},
				};

				bool[] enableFlag = new bool[2]{
					battleManager.playerList[0].nowHp > 0,
					!battleManager.eventBattleflag,
				};
				for (int i=0; i<2; i++)
				{
					GUITexture b = CreateGUITexture ("button00"+i, iPath[i][0], 
					                                 new Rect (w+452*ww+192*ww*i, h+291*hw, 182*ww, 86*hw));
					b.transform.parent = pauseMenuObject.transform;
					b.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);

					ButtonObject mo = b.gameObject.AddComponent<ButtonObject> ();
					mo.PrepareButton (co, i, iPath[i], b);
					if (!enableFlag[i])
						b.color = new Color (0.5f, 0.5f, 0.5f, 0.2f);
				}

				battleManager.StopAnimation ();
			}
		}
	}

	public void CallResult ()
	{
		ResultAnimation ra = gameObject.AddComponent<ResultAnimation> ();
		ra.Prepare (battleManager, inputManager);
	}

	public override void ButtonEvent (int bID)
	{
		switch (bID)
		{
		case	0:
			if (battleManager.playerList[0].nowHp > 0)
			{
				pauseMenuObject.gameObject.SetActive (false);
				GUITexture mask = CreateLayerMask ("ItemMenu");
				BattleUIItem uitem = mask.gameObject.AddComponent<BattleUIItem> ();
				uitem.Prepare (battleManager.status, inputManager, this);
			}
			break;
		case	1:
			if (!battleManager.eventBattleflag)
			{
				this.enabled = false;
				battleManager.RestartAnimation ();
				inputManager.ClearInput ();
				Destroy (pauseMenuObject.gameObject);
				escapeGauge = CreateLimitGauge ();
				escapeGauge.ChangeTexture (escapeGaugeImage);
				escapeGauge.Prepare (300.0f);
				battleManager.StartEscape ();
			}
			break;
		}
	}

	public override void ChangeCategoryEvent ()
	{
		battleManager.SortPartyMember ();
		for (int i=0; i<battleManager.playerList.Count; i++)
		{
			memberStatus[i].UpdateStatus (battleManager.playerList[i]);
			battleManager.playerList[i].InitIcon (memberStatus[i]);
			GUITexture item = pauseMenuObject.transform.Find ("button000").GetComponent<GUITexture> ();
			item.color = battleManager.playerList[0].nowHp > 0 ? new Color (0.5f, 0.5f, 0.5f, 0.5f) : new Color (0.5f, 0.5f, 0.5f, 0.2f);
		}
	}

//	public void CheckScreen ()
//	{
//		GameObject o = 
//	}

	public override void CancelEvent ()
	{
		if (pauseObject != null) Destroy (pauseObject.gameObject);

		if (pauseMenuObject != null) Destroy (pauseMenuObject.gameObject);

		battleManager.RestartAnimation ();
	}

	public void Prepare (BattleManager bm, InputManager im)
	{
		battleManager = bm;
		inputManager = im;

		KozMinPro_Bold = Resources.Load ("UI/font/KozMinPro-Bold", typeof (Font)) as Font;
		KozMinPro_Medium = Resources.Load ("UI/font/KozMinPro-Medium", typeof (Font)) as Font;
		escapeGaugeImage = Resources.Load ("UI/battle/ui_limit_gauge", typeof(Texture)) as Texture;
	}
}
