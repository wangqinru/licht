using UnityEngine;
using System.Collections;

public class BattleUIStatus : MenuObject {
	
	private ShinBattleController player;
	private GUITexture faceIcon;
	private GUITexture[] gauge;
	private GUIText[] status;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		float ww = Screen.width / 1280.0f;
		//float hw = Screen.height / 720.0f;

		int[][] g = new int[2][] {
			new int[2] {player.nowHp, player.status.maxhp},
			new int[2] {player.nowMp, player.status.maxmp},
		};
		
		for (int i=0; i<2; i++)
		{
			status[i].text = g[i][0].ToString ();
			
			float widht = (float)(g[i][1] - g[i][0])/(float)g[i][1] * -143.0f*ww;
			gauge[i].pixelInset = new Rect (gauge[i].pixelInset.x, gauge[i].pixelInset.y, widht, gauge[i].pixelInset.height);
		}
	}

	public void ChangeFace (int i)
	{
		faceIcon.texture = player.status.iconFace[i];
	}

	public void Prepare (ShinBattleController p)
	{
		player = p;
		
		float ww = Screen.width / 1280.0f;
		float hw = Screen.height / 720.0f;
		
		GUITexture frame = GetComponent<GUITexture> ();
		frame.pixelInset = new Rect (frame.pixelInset.x*ww, frame.pixelInset.y*hw, 274*ww, 118*hw);
		
		faceIcon = CreateGUITexture ("PlayerStatus", player.status.iconFace [0], 
		                             new Rect (-134*ww, -56*hw, 114*ww, 114*hw));
		faceIcon.transform.parent = transform;
		faceIcon.transform.localPosition = new Vector3 (0.0f, 0.0f, 1.0f);
		
		gauge = new GUITexture[2];
		status = new GUIText[2];
		
		for (int i=0; i<2; i++)
		{
			gauge[i] = CreateGUITexture ("gauge", "UI/battle/battle_gauge_mask",
			                             new Rect (130*ww, -15*hw - 37*hw*i, 0, 10*hw));
			gauge[i].transform.parent = transform;
			gauge[i].transform.localPosition = new Vector3 (0.0f, 0.0f, 1.0f);
			
			status[i] = CreateGUIText ("Status", player.nowHp.ToString (), (int)(20*ww), 
			                           new Vector2 (128*ww, 17*hw - 37*hw*i));
			status[i].anchor = TextAnchor.UpperRight;
			status[i].transform.parent = transform;
			status[i].transform.localPosition = new Vector3 (0.0f, 0.0f, 1.0f);
		}

		p.InitIcon (this);
	}

	public void UpdateStatus (ShinBattleController p)
	{
		player = p;

		faceIcon.texture = player.status.iconFace [0];

		float ww = Screen.width / 1280.0f;
		//float hw = Screen.height / 720.0f;
		
		int[][] g = new int[2][] {
			new int[2] {player.nowHp, player.status.maxhp},
			new int[2] {player.nowMp, player.status.maxmp},
		};

		for (int i=0; i<2; i++)
		{
			status[i].text = g[i][0].ToString ();
			
			float widht = (float)(g[i][1] - g[i][0])/(float)g[i][1] * -143.0f*ww;
			gauge[i].pixelInset = new Rect (gauge[i].pixelInset.x, gauge[i].pixelInset.y, widht, gauge[i].pixelInset.height);
		}
	}
}
