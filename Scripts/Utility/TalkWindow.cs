using UnityEngine;
using System.Collections;

public class TalkWindow : MonoBehaviour {

	public GUIText[] guiTexts {get; set;}
	public GUITexture faceTexture {get; set;}
	public GUITexture proceed {get; set;}

	void Awake () {
	
		float ww = Screen.width / 1280.0f;
		float hw = Screen.height / 720.0f;

		guiTexts = GetComponentsInChildren <GUIText> ();
		for (int i=0; i<guiTexts.Length; i++) {
			guiTexts[i].fontSize = (int)(guiTexts[i].fontSize*ww);
		}

		GUITexture[] textures = GetComponentsInChildren<GUITexture> ();
		for (int i=0; i<textures.Length; i++)
		{
			textures[i].pixelInset = new Rect (0, 0,
			                                   textures[i].pixelInset.width*ww, textures[i].pixelInset.height*hw);
		}
		faceTexture = textures[0];
		proceed = textures[1];
		proceed.GetComponent<IconAnimation> ().Prepare ();

		proceed.enabled = false;
	}

	void Update () {
	
	}
}
