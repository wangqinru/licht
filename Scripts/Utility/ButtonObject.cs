using UnityEngine;
using System.Collections;

public class ButtonObject : MenuObject {

	private CursorObject _cursor;
	private Texture[] textures;

	private GUITexture button;

	private int buttonNumber;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (_cursor.nowNumber == buttonNumber)
		{
			button.texture = textures[1];
		}
		else
		{
			if (button.texture != textures[0])
				button.texture = textures[0];
		}
	}

	public void PrepareButton (CursorObject co, int n, string[] path, GUITexture gt)
	{
		_cursor = co;
		buttonNumber = n;

		textures = new Texture[2] {
			Resources.Load (path [0], typeof(Texture)) as Texture,
			Resources.Load (path [1], typeof(Texture)) as Texture,
		};

		button = gt;
	}
}
