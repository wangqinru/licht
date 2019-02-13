using UnityEngine;
using System.Collections;

public class LimitGauge : MenuObject {

	private GUITexture gaugueMask;
	private float limitTime;

	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
	
		gaugueMask.pixelInset = new Rect (
			gaugueMask.pixelInset.x, 
			gaugueMask.pixelInset.y,
			gaugueMask.pixelInset.width - limitTime,
			gaugueMask.pixelInset.height);

		if (gaugueMask.pixelInset.width <= -174.0f)
			Destroy (gameObject);
	}

	public void Prepare (float l)
	{
		limitTime = 174 / l;
		gaugueMask = transform.GetChild (0).GetComponent<GUITexture> ();
	}

	public void ChangeTexture (Texture t)
	{
		GetComponent<GUITexture> ().texture = t;
	}
}
