using UnityEngine;
using System.Collections;

public class CursorObject : MenuObject {

	private InputManager inputManger;
	private MenuObject menu;
	public GUITexture cursorTexture { get; private set;}

	//[0]横縦のボタン数 [1]横縦ボタンの間隔 [2]横縦初期位置
	private int[][] Standard;
	private int total;
	private Vector2 nowPosition;

	public int nowNumber { get; private set;}
	public int nowCategory { get; private set;}
	private int maxCategory;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		int cx = nowNumber%Standard[0][0];
		int cy = nowNumber/Standard[0][0];

		int old = nowNumber;

		float tx = cursorTexture.pixelInset.x;
		float ty = cursorTexture.pixelInset.y;

		if (inputManger.StickLR == 1 && cx < Standard[0][0] - 1 && nowNumber+1 < total) 
		{
			nowNumber ++;
			tx += Standard[0][1];
		}

		if (inputManger.StickLR == -1 && cx > 0)
		{
			nowNumber --;
			tx -= Standard[0][1];
		}

		if (inputManger.StickUD == 1 && cy < Standard[1][0]-1 && nowNumber+Standard[0][0] < total)
		{
			nowNumber += Standard[0][0];
			ty += Standard[1][1];
		}

		if (inputManger.StickUD == -1 && cy > 0)
		{
			nowNumber -= Standard[0][0];
			ty -= Standard[1][1];
		}

		if (inputManger.DecisionButton == 1)
		{
			CallButtonEvent callButtonEvent = new CallButtonEvent (menu.ButtonEvent);
			callButtonEvent (nowNumber);
		}

		if (inputManger.CancelButton == 1)
		{
			menu.CancelEvent ();
		}

		ChangeCategory ();

		if (old != nowNumber)
		{
			cursorTexture.pixelInset = new Rect (tx, ty, cursorTexture.pixelInset.width, cursorTexture.pixelInset.height);
			menu.ButtonOver ();
		}
	}

	void ChangeCategory ()
	{
		if (inputManger.ButtonL == 1)
		{
			nowCategory --;
			if (nowCategory < 0) nowCategory = maxCategory-1;
			
			menu.ChangeCategoryEvent ();
		}
		
		if (inputManger.ButtonR == 1)
		{
			nowCategory ++;
			if (nowCategory > maxCategory-1) nowCategory = 0;
			
			menu.ChangeCategoryEvent ();
		}
	}

	public void Prepare (InputManager im, MenuObject m, int[][] s, int t)
	{
		inputManger = im;
		menu = m;
		Standard = s;
		cursorTexture = GetComponent<GUITexture> ();
		cursorTexture.pixelInset = new Rect (Standard[0][2], Standard[1][2], cursorTexture.pixelInset.width, cursorTexture.pixelInset.height);
		total = t;
		nowNumber = 0;
	}

	public void Reset (int[][] s, int t)
	{
		Standard = s;
		cursorTexture.pixelInset = new Rect (Standard[0][2], Standard[1][2], cursorTexture.pixelInset.width, cursorTexture.pixelInset.height);
		total = t;
		nowNumber = 0;
	}

	public void InitCategory (int max)
	{
		maxCategory = max;
	}
}
