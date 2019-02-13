using UnityEngine;
using System.Collections;

public class BattleUIItem : MenuObject {
	
	protected StatusData status;

	protected GUIText itemName;
	protected GUIText itemDescription;
	protected GUITexture itemImage;

	protected GUITexture highLight;
	protected CursorObject cursor;
	protected CursorObject cursor_2;

	protected TargetSelect selected;
	protected BattleGUIManager home;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void ButtonOver ()
	{
		itemImage.texture = status.itemList [cursor.nowNumber].image;
		itemName.text = status.itemList[cursor.nowNumber].name;
		itemDescription.text = status.itemList[cursor.nowNumber].description;

		highLight.pixelInset = new Rect (
			cursor.cursorTexture.pixelInset.x+45, cursor.cursorTexture.pixelInset.y+26,
			highLight.pixelInset.width, highLight.pixelInset.height);
	}

	//---------------------------アイテム使うときの処理-------------------------------------------------------
	public override void ButtonEvent (int bID)
	{
		this.gameObject.SetActive (false);
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/Target_select", typeof (GameObject)) as GameObject) as GameObject;
		selected = o.GetComponent<TargetSelect> ();
		selected.Prepare (status, this, status.itemList[cursor.nowNumber].id == 4);
	}

	public override void CancelEvent ()
	{
		home.pauseMenuObject.gameObject.SetActive (true);
		Destroy (gameObject);
	}

	protected void CreateItemPreview (float ix, float iy)
	{
		float ww = Screen.width / 1280.0f;
		float hw = Screen.height / 720.0f;

		itemImage = CreateGUITexture ("itemImage", status.itemList[cursor.nowNumber].image, new Rect (ix, iy, 72*ww, 72*hw));
		itemImage.transform.parent = transform;
		itemImage.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);

		itemName = CreateGUIText ("itemName", status.itemList[cursor.nowNumber].name, (int)(20*ww), new Vector2 (ix+82*ww, iy+63*hw), home.KozMinPro_Bold);
		itemName.transform.parent = transform;
		itemName.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);

		itemDescription = CreateGUIText ("itemDescription", status.itemList[cursor.nowNumber].description, (int)(16*ww), new Vector2 (ix+82*ww, iy+32*hw), home.KozMinPro_Medium);
		itemDescription.transform.parent = transform;
		itemDescription.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);
	}

	public void Prepare (StatusData s, InputManager im, BattleGUIManager ho)
	{
		home = ho;

		float w = GetComponent<GUITexture> ().pixelInset.x;
		float h = GetComponent<GUITexture> ().pixelInset.y;

		float ww = Screen.width / 1280.0f;
		float hw = Screen.height / 720.0f;

		GUITexture itemFrame = CreateGUITexture ("Item_Frame", "UI/battle/bmenu_item_frame", new Rect (w+84*ww, h+15*hw, 1072*ww, 536*hw));
		itemFrame.transform.parent = transform;
		itemFrame.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.1f);
		status = s;

		int[][] lp = new int[2][]{
			new int[3]{2, (int)(505*ww), (int)(w+134*ww)},
			new int[3]{status.itemList.Count/2+1, (int)(-37*hw), (int)(h+450*hw)}
		};
		CreateItemList (status.itemList, transform, lp, home.KozMinPro_Bold);

		int[][] sp = new int[2][]{
			new int[3]{2, (int)(505*ww), (int)(w+89*ww)},
			new int[3]{status.itemList.Count/2+1, (int)(-37*hw), (int)(h+397*hw)}
		};

		cursor = CreateCursor (transform);
		cursor.Prepare (im, this, sp, status.itemList.Count);

		highLight = CreateHighLight (transform, new Vector2 (cursor.cursorTexture.pixelInset.x + 45, cursor.cursorTexture.pixelInset.y + 26));

		CreateItemPreview (w+119*ww, h+63*hw);
	}

	public void UseItem (int target)
	{
		Destroy (home.pauseMenuObject.gameObject);
		Destroy (gameObject);
		home.itemGauge = CreateLimitGauge ();
		home.itemGauge.Prepare (180.0f);
		status.GetComponent<BattleManager> ().RestartAnimation ();
		status.GetComponent<BattleManager> ().playerList[0].UseItem ();

		status.itemList[cursor.nowNumber].ItemEvent (status.GetComponent<BattleManager> ().playerList[target]);
		if (--status.itemList[cursor.nowNumber].number == 0)
			status.itemList.Remove (status.itemList[cursor.nowNumber]);
	}
}
