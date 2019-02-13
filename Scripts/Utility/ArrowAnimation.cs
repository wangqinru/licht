using UnityEngine;
using System.Collections;

public class ArrowAnimation : MonoBehaviour {

	private GUITexture arrow;
	private float offset;
	//private float maxOffset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//offset = Mathf.PingPong (Time.time, maxOffset);
		//arrow.pixelInset = new Rect (arrow.pixelInset.x+offset, arrow.pixelInset.y,  
		                           //  arrow.pixelInset.width, arrow.pixelInset.height);
	}

	public void Prepare (bool right, Vector2 pos)
	{
		arrow = GetComponent <GUITexture> ();

		if (right)
		{
			arrow.texture = Resources.Load ("UI/field/ui_change_right", typeof(Texture)) as Texture;
			//maxOffset = 3.0f;
		}
	//	else
			//maxOffset = -3.0f;

		arrow.pixelInset = new Rect (pos.x, pos.y, arrow.pixelInset.width, arrow.pixelInset.height);
	}
}
