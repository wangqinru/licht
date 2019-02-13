using UnityEngine;
using System.Collections;

public class ItemMenu : FieldUIManager {

	private GameObject itemPreview;
	private GameObject mainMenu;
	
	private int nowCategory;

	private GUITexture category;
	private GUITexture itemImage;
	private GUIText itemName;
	private GUIText itemText;

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
		switch (cursor.nowCategory)
		{
		case	0:
			itemImage.texture = status.itemList[cursor.nowNumber].image;
			itemName.text = status.itemList[cursor.nowNumber].name;
			itemText.text = status.itemList[cursor.nowNumber].description;

			highLight.pixelInset = new Rect (
				cursor.cursorTexture.pixelInset.x+55, cursor.cursorTexture.pixelInset.y+34,
				highLight.pixelInset.width, highLight.pixelInset.height);
			break;
		}
	}
	
	public override void CancelEvent ()
	{
		Destroy (itemPreview);
		mainMenu.SetActive (true);
		GetComponent<SkillMenu> ().enabled = false;
	}
	
	public override void ChangeCategoryEvent ()
	{
		float ww = Screen.width / 1280.0f;
		float hw = Screen.height / 720.0f;
		
		nowCategory = cursor.nowCategory;

		GUIText[] os = itemPreview.GetComponentsInChildren<GUIText> ();
		for (int i=0; i<os.Length; i++)
		{
			Destroy (os[i].gameObject);
		}
		if (itemImage != null) Destroy (itemImage.gameObject);
		if (highLight != null) Destroy (highLight.gameObject);

		if (nowCategory == 0)
		{
			itemImage = CreateGUITexture ("item_image", status.itemList[0].image, new Rect (136*ww, 90*hw, 72*ww, 72*hw));
			itemImage.transform.parent = itemPreview.transform;
			itemImage.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.3f);
			
			itemName = CreateGUITextRerurn ("item_name", status.itemList[0].name, 
			                                (int)(20*ww), new Vector2 (220*ww, 160*hw), KozMinPro_Heavy, 
			                                itemPreview.transform, 2.3f, TextAnchor.UpperLeft);
			itemName.color = new Color (1.0f, 0.95f, 0.79f);
			
			itemText = CreateGUITextRerurn ("item_text", status.itemList[0].description, 
			                                (int)(16*ww), new Vector2 (220*ww, 130*hw), KozMinPro_Medium, 
			                                itemPreview.transform, 2.3f, TextAnchor.UpperLeft);

			//[0]横縦のボタン数 [1]横縦ボタンの間隔 [2]横縦初期位置
			int[][] lp = new int[2][]{
				new int[3]{2, (int)(505*ww), (int)(ww*150)},
				new int[3]{status.itemList.Count/2+1, (int)(-37*hw), (int)(hw*515)}
			};
			CreateItemList (status.itemList, itemPreview.transform, lp, KozMinPro_Bold);

			int[][] sp = new int[2][]{
				new int[3]{2, (int)(505*ww), (int)(ww*100)},
				new int[3]{status.itemList.Count/2+1, (int)(-37*hw), (int)(hw*450)}
			};
			cursor.Reset (sp, status.itemList.Count);

			highLight = CreateHighLight (itemPreview.transform, new Vector2 ((int)(ww*100)+55, (int)(hw*450)+34));
			highLight.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);
		}
		else
		{
			int[][] st = new int[2][]{
				new int[3]{1, 0, (int)(Screen.width*2)},
				new int[3]{1, 0, (int)(Screen.height*2)},
			};
			cursor.Reset (st, 1);
		}

		category.pixelInset = new Rect (131*ww+170*ww*nowCategory, 573*hw, 169*ww, 62*hw);
	}
	
	void CreateItemPreview (Transform t)
	{
		float ww = Screen.width / 1280.0f;
		float hw = Screen.height / 720.0f;
		
		CreateGUITexture ("skill_frame", fieldPath+"menu_item_frame", new Rect (102*ww, 68*hw, 1077*ww, 580*hw), t, 2.1f);
		CreateGUITexture ("status_text", fieldPath+"ui_menu_item", new Rect (200*ww, 672*hw, 546*ww, 42*hw), t, 2.2f);

		category = CreateGUITexture ("highlight", fieldPath+"item_select", new Rect (131*ww, 573*hw, 169*ww, 62*hw));
		category.transform.parent = t;
		category.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);

		itemImage = CreateGUITexture ("item_image", status.itemList[0].image, new Rect (136*ww, 90*hw, 72*ww, 72*hw));
		itemImage.transform.parent = t;
		itemImage.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.3f);

		itemName = CreateGUITextRerurn ("item_name", status.itemList[0].name, 
		                                 (int)(20*ww), new Vector2 (220*ww, 160*hw), KozMinPro_Heavy, 
		                                 t, 2.3f, TextAnchor.UpperLeft);
		itemName.color = new Color (1.0f, 0.95f, 0.79f);

		itemText = CreateGUITextRerurn ("item_text", status.itemList[0].description, 
		                                (int)(16*ww), new Vector2 (220*ww, 130*hw), KozMinPro_Medium, 
		                                 t, 2.3f, TextAnchor.UpperLeft);

		//[0]横縦のボタン数 [1]横縦ボタンの間隔 [2]横縦初期位置
		int[][] lp = new int[2][]{
			new int[3]{2, (int)(505*ww), (int)(ww*150)},
			new int[3]{status.itemList.Count/2+1, (int)(-37*hw), (int)(hw*515)}
		};
		CreateItemList (status.itemList, t, lp, KozMinPro_Bold);
		
		int[][] sp = new int[2][]{
			new int[3]{2, (int)(505*ww), (int)(ww*100)},
			new int[3]{status.itemList.Count/2+1, (int)(-37*hw), (int)(hw*450)}
		};
		cursor = CreateCursor (t);
		cursor.Prepare (inputManager, this, sp, status.itemList.Count);
		cursor.InitCategory (6);

		highLight = CreateHighLight (t, new Vector2 ((int)(ww*100)+55, (int)(hw*450)+34));
		highLight.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);

		CreateGUITexture ("skill_category", fieldPath+"item_category", new Rect (100*ww, 560*hw, 1078*ww, 88*hw), t, 2.3f);
		
		CreateGUITexture ("Indication", fieldPath+"ui_Indication_3", new Rect (415*ww, 3*hw, 450*ww, 52*hw), t, 2.2f);
	}
	
	public override void Prepare (InputManager im, StatusData s, GameObject mm)
	{
		inputManager = im;
		sceneManager = inputManager.GetComponent<SceneManager> ();
		status = s;
		mainMenu = mm;
		
		nowCategory = 0;
		InitFont ();
		
		itemPreview = CreateGUITexture ("item").gameObject;
		CreateItemPreview (itemPreview.transform);
	}
}
