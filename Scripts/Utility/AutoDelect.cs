using UnityEngine;
using System.Collections;

public class AutoDelect : MonoBehaviour {

	private int counter = 0;
	private GUITexture[] face;
	private GUIText[] skillName;

	// Use this for initialization
	void Awake () {
	
		float ww = Screen.width / 1280.0f;
		float hw = Screen.height / 720.0f;

		face = GetComponentsInChildren<GUITexture> ();
		skillName = GetComponentsInChildren<GUIText> ();

		for (int i=0; i<face.Length; i++)
		{
			face[i].pixelInset = new Rect (face[i].pixelInset.x*ww, face[i].pixelInset.y*hw, 
			                               face[i].pixelInset.width*ww, face[i].pixelInset.height*hw);
			skillName[i].fontSize = (int)(skillName[i].fontSize*ww);
		}
	}
	// Update is called once per frame
	void Update () {
	
		if (--counter < 0)
			Destroy (gameObject);
	}

	public void InitCounter (int c, Texture t, string n)
	{
		counter = c;

		face[1].texture = t;
		skillName[0].text = n;
		skillName[1].text = n;
	}
}
