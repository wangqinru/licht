using UnityEngine;
using System.Collections;

public class IconAnimation : MonoBehaviour {

	private GUITexture icon;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 targetPosition = new Vector3 (icon.transform.localPosition.x, 
		                                      0.12f + Mathf.PingPong (Time.time, 0.5f) / 100.0f,
		                                      icon.transform.localPosition.z);

		icon.transform.localPosition = targetPosition;
	}

	public void Prepare ()
	{
		icon = GetComponent<GUITexture> ();
	}
}
