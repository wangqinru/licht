using UnityEngine;
using System.Collections;
using System.Collections.Generic;

delegate void CallButtonEvent (int number);

public class MenuObject : MonoBehaviour {

	protected void CreateArrow (Transform t, Vector2 l, Vector2 r)
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/Arrow_Object", typeof(GameObject)) as GameObject) as GameObject;
		o.name = "arrow_left";
		o.transform.parent = t;
		o.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.5f);
		ArrowAnimation aa = o.GetComponent<ArrowAnimation> ();
		aa.Prepare (false, l);

    	o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/Arrow_Object", typeof(GameObject)) as GameObject) as GameObject;
		o.name = "arrow_right";
		o.transform.parent = t;
		o.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.5f);
		aa = o.GetComponent<ArrowAnimation> ();
		aa.Prepare (true, r);
	}

	protected GUITexture CreateGUITexture (string name, string texturePath, Rect r)
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/GUItextureObject", typeof(GameObject)) as GameObject) as GameObject;
		o.name = name;
		GUITexture gt = o.GetComponent <GUITexture> ();
		gt.texture = Resources.Load (texturePath, typeof (Texture)) as Texture;
		gt.pixelInset = r;
		
		return gt;
	}

	protected void CreateGUITexture (string name, string texturePath, Rect r, Transform t, float z)
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/GUItextureObject", typeof(GameObject)) as GameObject) as GameObject;
		o.name = name;
		GUITexture gt = o.GetComponent <GUITexture> ();
		gt.texture = Resources.Load (texturePath, typeof (Texture)) as Texture;
		gt.pixelInset = r;

		gt.transform.parent = t;
		gt.transform.localPosition = new Vector3 (0.0f, 0.0f, z);
	}

	protected GUITexture CreateGUITexture (string name, Texture t, Rect r)
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/GUItextureObject", typeof(GameObject)) as GameObject) as GameObject;
		o.name = name;
		GUITexture gt = o.GetComponent <GUITexture> ();
		gt.texture = t;
		gt.pixelInset = r;
		
		return gt;
	}

	protected void CreateGUITexture (string name, Texture t, Rect r, Transform tf, float z)
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/GUItextureObject", typeof(GameObject)) as GameObject) as GameObject;
		o.name = name;
		GUITexture gt = o.GetComponent <GUITexture> ();
		gt.texture = t;
		gt.pixelInset = r;
		
		gt.transform.parent = tf;
		gt.transform.localPosition = new Vector3 (0.0f, 0.0f, z);
	}

	protected GUITexture CreateGUITexture (string name)
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/GUItextureObject", typeof(GameObject)) as GameObject) as GameObject;
		o.name = name;
		GUITexture gt = o.GetComponent <GUITexture> ();
		
		return gt;
	}

	protected GUIText CreateGUIText (string name, string t, int fSize, Vector2 v)
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/GUItextObject", typeof (GameObject)) as GameObject) as GameObject;
		o.name = name;
		GUIText gt = o.GetComponent <GUIText> ();
		gt.text = t;
		gt.fontSize = fSize;
		gt.pixelOffset = v;
		
		return gt;
	}
	
	protected GUIText CreateGUIText (string name, string t, int fSize, Vector2 v, Font f)
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/GUItextObject", typeof (GameObject)) as GameObject) as GameObject;
		o.name = name;
		GUIText gt = o.GetComponent <GUIText> ();
		gt.text = t;
		gt.fontSize = fSize;
		gt.pixelOffset = v;
		gt.font = f;
		
		return gt;
	}

	protected GUIText CreateGUITextRerurn (string name, string str, int fSize, Vector2 v, Font f, Transform tf, float z, TextAnchor ta)
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/GUItextObject", typeof (GameObject)) as GameObject) as GameObject;
		o.name = name;
		GUIText gt = o.GetComponent <GUIText> ();
		gt.text = str;
		gt.fontSize = fSize;
		gt.pixelOffset = v;
		gt.font = f;
		gt.anchor = ta;
		
		gt.transform.parent = tf; 
		gt.transform.localPosition = new Vector3 (0.0f, 0.0f, z);

		return gt;
	}

	protected void CreateGUIText (string name, string t, int fSize, Vector2 v, Font f, Transform tf, float z, TextAnchor ta)
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/GUItextObject", typeof (GameObject)) as GameObject) as GameObject;
		o.name = name;
		GUIText gt = o.GetComponent <GUIText> ();
		gt.text = t;
		gt.fontSize = fSize;
		gt.pixelOffset = v;
		gt.font = f;
		gt.anchor = ta;

		gt.transform.parent = tf; 
		gt.transform.localPosition = new Vector3 (0.0f, 0.0f, z);
	}

	protected GUITexture CreateLayerMask (string n)
	{
		GUITexture mask = CreateGUITexture (n, "UI/battle/ui_mask", 
		                                new Rect (Screen.width/2*-1, Screen.height/2*-1, Screen.width, Screen.height));
		mask.transform.position = new Vector3 (0.5f, 0.5f, 2.0f);

		return mask;
	}

	protected CursorObject CreateCursor (Transform p)
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/UI_Cursor", typeof(GameObject)) as GameObject) as GameObject;
		o.transform.parent = p;
		o.transform.localPosition = new Vector3 (0.0f, 0.0f, 3.0f);

		CursorObject co = o.GetComponent<CursorObject> ();
		return co;
	}

	protected GUITexture CreateHighLight (Transform p, Vector2 pos)
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/GUItextureObject", typeof(GameObject)) as GameObject) as GameObject;
		o.AddComponent <Fashing> ();
		o.name = "hightlight";
		GUITexture gt = o.GetComponent<GUITexture> ();
		gt.texture = Resources.Load ("UI/common/ui_highlight", typeof(Texture)) as Texture;
		gt.transform.parent = p;
		gt.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.1f);
		gt.pixelInset = new Rect (pos.x, pos.y, 450, 32);

		return gt;
	}

	//[0]横縦のボタン数 [1]横縦ボタンの間隔 [2]横縦初期位置
	protected void CreateItemList (List<ItemBase> ib, Transform p, int[][] s, Font f)
	{
		float ww = Screen.width / 1280.0f;
		//float hw = Screen.height / 720.0f;
		
		for (int i=0; i<ib.Count; i++)
		{
			float tx = s[0][2] + (i%s[0][0])*s[0][1];
			float ty = s[1][2] + (i/s[0][0])*s[1][1];
			
			GUIText gtext = CreateGUIText ("item", ib[i].name, (int)(20*ww), new Vector2 (tx, ty), f);
			gtext.transform.parent = p;
			gtext.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);
			
			GUIText number = CreateGUIText ("itemNumber", ib[i].number.ToString (), (int)(20*ww), new Vector2 (tx+435*ww, ty), f);
			number.transform.parent = p;
			number.transform.localPosition = new Vector3 (0.0f, 0.0f, 2.2f);
		}
	}

	protected void CreateStatusGauge (Transform t, Vector2 v2, Font f, Rect r, float wh, StatusBase s, int size, float z, float gw)
	{
		int[] now = new int[3] {s.hp, s.mp, s.experience};
		int[] next = new int[3] {s.maxhp, s.maxmp, ReturnNextExe (s.level)};

		for (int i=0; i<3; i++)
		{
			string str = now[i].ToString ()+"/"+next[i].ToString ();
			float weight = (float)(next[i] - now[i])/(float)next[i] * gw;

			GUIText g = CreateGUIText ("numeric", str, size, new Vector2 (v2.x, v2.y+i*wh), f);
			g.transform.parent = t;
			g.transform.localPosition = new Vector3 (0.0f, 0.0f, z);
			g.anchor = TextAnchor.UpperRight;

			GUITexture gau = CreateGUITexture ("gauge", "UI/battle/battle_gauge_mask",
			                                   new Rect (r.x, r.y+i*wh, weight, r.height));
			gau.transform.parent = t;
			gau.transform.localPosition = new Vector3 (0.0f, 0.0f, z);
		}
	}

	protected void CreateStatusGauge (Transform p, ref GUITexture[] gau, ref GUIText[] num, StatusBase s, Vector2 textPos, Rect texturePos, float h, float z, int size, Font f)
	{
		int[] now = new int[3] {s.hp, s.mp, s.experience};
		int[] next = new int[3] {s.maxhp, s.maxmp, ReturnNextExe (s.level)};

		for (int i=0; i<3; i++)
		{
			string str = now[i].ToString ()+"/"+next[i].ToString ();
			float wight = (float)(next[i] - now[i])/(float)next[i] * texturePos.width;

			num[i] = CreateGUIText ("numeric", str, size, new Vector2 (textPos.x, textPos.y+i*h), f);
			num[i].transform.parent = p;
			num[i].transform.localPosition = new Vector3 (0.0f, 0.0f, z);
			num[i].anchor = TextAnchor.UpperRight;
			num[i].color = new Color (1.0f, 0.95f, 0.79f);

			gau[i] = CreateGUITexture ("gauge", "UI/battle/battle_gauge_mask",
			                           new Rect (texturePos.x, texturePos.y+i*h, wight, texturePos.height));
			gau[i].transform.parent = p;
			gau[i].transform.localPosition = new Vector3 (0.0f, 0.0f, z);
		}
	}

	protected void CreateStatusGaugeTwo (Transform p, ref GUITexture[] gau, ref GUIText[] num, StatusBase s, Vector2 textPos, Rect texturePos, float h, float z, int size, Font f)
	{
		int[] now = new int[2] {s.hp, s.mp};
		int[] next = new int[2] {s.maxhp, s.maxmp};
		
		for (int i=0; i<2; i++)
		{
			string str = now[i].ToString ()+"/"+next[i].ToString ();
			float wight = (float)(next[i] - now[i])/(float)next[i] * texturePos.width;
			
			num[i] = CreateGUIText ("numeric", str, size, new Vector2 (textPos.x, textPos.y+i*h), f);
			num[i].transform.parent = p;
			num[i].transform.localPosition = new Vector3 (0.0f, 0.0f, z);
			num[i].anchor = TextAnchor.UpperRight;
			num[i].color = new Color (1.0f, 0.95f, 0.79f);
			
			gau[i] = CreateGUITexture ("gauge", "UI/battle/battle_gauge_mask",
			                           new Rect (texturePos.x, texturePos.y+i*h, wight, texturePos.height));
			gau[i].transform.parent = p;
			gau[i].transform.localPosition = new Vector3 (0.0f, 0.0f, z);
		}
	}

	protected void ChangeStatusGauge (ref GUITexture[] gau, ref GUIText[] num, StatusBase s, float gw)
	{
		int[] now = new int[3] {s.hp, s.mp, s.experience};
		int[] next = new int[3] {s.maxhp, s.maxmp, ReturnNextExe (s.level)};

		for (int i=0; i<3; i++)
		{
			string str = now[i].ToString ()+"/"+next[i].ToString ();
			float wight = (next[i] - now[i])/next[i] * gw;

			num[i].text = str;
			gau[i].pixelInset = new Rect (gau[i].pixelInset.x, gau[i].pixelInset.y, wight, gau[i].pixelInset.height);
		}
	}

	protected void ChangeStatusGaugeTwo (ref GUITexture[] gau, ref GUIText[] num, StatusBase s, float gw)
	{
		int[] now = new int[2] {s.hp, s.mp};
		int[] next = new int[2] {s.maxhp, s.maxmp};
		
		for (int i=0; i<2; i++)
		{
			string str = now[i].ToString ()+"/"+next[i].ToString ();
			float wight = (next[i] - now[i])/next[i] * gw;
			
			num[i].text = str;
			gau[i].pixelInset = new Rect (gau[i].pixelInset.x, gau[i].pixelInset.y, wight, gau[i].pixelInset.height);
		}
	}
	
	protected LimitGauge CreateLimitGauge ()
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/Limit_Gauge", typeof(GameObject)) as GameObject) as GameObject;
		LimitGauge lg = o.GetComponent<LimitGauge> ();

		return lg;
	}

	public virtual void ButtonOver ()
	{

	}

	public virtual void ButtonEvent (int bID)
	{

	}

	public virtual void CancelEvent ()
	{

	}

	public virtual void ChangeCategoryEvent ()
	{
	}

	protected int ReturnNextExe (int level)
	{
	 	return 10+ 4*level*(level*level-1);
	}
}
