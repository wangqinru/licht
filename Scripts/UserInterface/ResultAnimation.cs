using UnityEngine;
using System.Collections;

public class ResultAnimation : MenuObject {

	private GUITexture resultObject;
	private BattleManager battleManager;
	private StatusData status;
	private InputManager inputManager;
	private GUITexture[] expGauge;
	private GUIText[] exp;
	private GUIText[] items;

	private Font KozMinPro_Bold;
	private Font KozMinPro_Medium; 

	private int speed = 1;
	private int expVar = 0;
	private float ww;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (battleManager.getExp > 0)
		{
			battleManager.getExp -= speed;

			if (inputManager.CancelButton == 1)
			{
				speed += battleManager.getExp;
				battleManager.getExp = 0;
				inputManager.ClearInput ();
			}
			expVar += speed;
			for (int i=0; i<battleManager.playerList.Count; i++)
			{
				PlayerBattleController player = (PlayerBattleController)battleManager.playerList[i];
				int nextExp = ReturnNextExe(player.status.level);

				status.playersData[player.ID].status.experience += speed;
				if (status.playersData[player.ID].status.experience >= nextExp)
				{
					status.LevelUp (player.ID);
					player.nowHp = status.playersData[player.ID].status.maxhp;
					player.nowMp = status.playersData[player.ID].status.maxmp;
					CreateGUITexture ("level_up", "UI/battle/level_up", 
					                  new Rect (expGauge[i].pixelInset.x-300, expGauge[i].pixelInset.y-30, 160, 20),
					                  expGauge[i].transform.parent,2.3f);
				}

				float widht = (float)(nextExp - status.playersData[player.ID].status.experience)/(float)nextExp * -145.0f*ww;
				expGauge[i].pixelInset = new Rect (expGauge[i].pixelInset.x, expGauge[i].pixelInset.y, widht, expGauge[i].pixelInset.height);
				exp[i].text = "+"+ expVar.ToString ();
			}
		}

		if (battleManager.getExp <= 0 && inputManager.DecisionButton == 1)
		{
			Destroy (resultObject.gameObject);
			battleManager.UnitBattle ();
		}

	}

	public void Prepare (BattleManager bm, InputManager im)
	{
		battleManager = bm;
		inputManager = im;
		status = battleManager.GetComponent<StatusData> ();

		KozMinPro_Bold = Resources.Load ("UI/font/KozMinPro-Bold", typeof (Font)) as Font;
		KozMinPro_Medium = Resources.Load ("UI/font/KozMinPro-Medium", typeof (Font)) as Font;

		ww = Screen.width/1280.0f;
		float hw = Screen.height/720.0f;
		
		resultObject = CreateGUITexture ("result", "UI/battle/result", new Rect (0, 0, Screen.width, Screen.height));
		
		expGauge = new GUITexture[battleManager.playerList.Count];
		exp = new GUIText[battleManager.playerList.Count];
		for (int i=0; i<battleManager.playerList.Count; i++)
		{
			int id = battleManager.playerList[i].ID;
			ShinBattleController player = battleManager.playerList[i];
			int nextExp = ReturnNextExe(player.status.level);
			float widht = (float)(nextExp - player.status.experience)/(float)nextExp * -145.0f*ww;
			
			CreateGUITexture ("name", status.playersData[id].nameFolder, new Rect (960*ww, 520*hw - 54*i*hw, 150*ww, 30*hw), resultObject.transform, 2.1f);
			CreateGUITexture ("exp_frame", "UI/battle/exp_gauge", new Rect (1110*ww, 510*hw - 54*i*hw, 145*ww, 30*hw), resultObject.transform, 2.1f);
			expGauge[i] = CreateGUITexture ("exp_gauge", "UI/battle/battle_gauge_mask", new Rect (1255*ww, 533*hw - 54*i*hw, widht, 7*hw));
			expGauge[i].transform.parent = resultObject.transform;
			expGauge[i].transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);

			exp[i] = CreateGUITextRerurn ("exp", "+", (int)(15*ww), new Vector2 (1250*ww, 530*hw - 54*i*hw), KozMinPro_Medium, resultObject.transform, 2.2f, TextAnchor.UpperRight);
			exp[i].color = new Color (0.9f, 1.0f, 0.75f);
		}

		items = new GUIText[battleManager.getItems.Count];
		for (int i=0; i<battleManager.getItems.Count; i++)
		{
			items[i] = CreateGUITextRerurn ("item", status.itemData[battleManager.getItems[i]].name + "     x1", (int)(20*ww), 
			                                new Vector2 (25*ww, 550*hw - 35*i*hw), KozMinPro_Bold, resultObject.transform, 2.2f, TextAnchor.UpperLeft );
		}
		//speed = battleManager.getExp / 2;
	}
}
