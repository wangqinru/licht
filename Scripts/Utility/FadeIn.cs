using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {

	public bool isComplete { get; private set;}

	private GUITexture fadeMask;
	private float speed = 0.0f;
	// Use this for initialization

	void Awake () {
		fadeMask = GetComponent<GUITexture> ();	
		isComplete = false;
		fadeMask.pixelInset = new Rect (Screen.width / 2 * -1, Screen.height / 2 * -1,
		                                Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update () {

		speed = isComplete ? speed - 0.05f : speed + 0.05f;

		if (speed > 3.0f)
		{
			isComplete = true;
			speed = 1.0f;
		}

		if (speed < 0.1f && isComplete)
			Destroy (gameObject);

		float a = Mathf.Clamp (speed, 0.0f, 1.0f);
		fadeMask.color = new Color (0.0f, 0.0f, 0.0f, a);
	}
}
