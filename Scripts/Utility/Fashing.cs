using UnityEngine;
using System.Collections;

public class Fashing : MonoBehaviour {

	private float counter = 0.0f;

	private GUITexture highLight;

	// Use this for initialization
	void Awake () {

		highLight = GetComponent<GUITexture> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		counter = Mathf.PingPong (Time.time, 1.0f);
		highLight.color = new Color (0.5f, 0.5f, 0.5f, counter);
	}
}
