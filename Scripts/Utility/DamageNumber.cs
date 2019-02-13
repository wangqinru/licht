using UnityEngine;
using System.Collections;

public class DamageNumber : MonoBehaviour {

	private GUIText[] damageText;
	private int counter;

	// Use this for initialization
	void Awake () {

		damageText = GetComponentsInChildren<GUIText> ();
		counter = 60;
	}
	
	// Update is called once per frame
	void Update () {
	
		counter --;

		if (counter < 30)
		{
			foreach (GUIText gt in damageText)
			{
				gt.color = new Color (gt.color.a, gt.color.g, gt.color.b, (float)counter/60.0f); 
			}

			transform.position -= new Vector3 (0.0f, 0.01f, 0.0f);
		}

		if (counter <= 0)
		{
			Destroy (gameObject);
		}
	}

	public void ShowDamageNumber (int number, Camera c, Vector3 pos, bool red)
	{
		if (red) damageText[0].color = new Color (0.7f, 0.4f, 0.4f, damageText[0].color.a);
		Vector3 screenPos = c.WorldToViewportPoint (pos);
		float randx = Random.Range (-0.1f, 0.1f);
		float randy = Random.Range (0.1f, 0.2f);
		transform.position = screenPos + new Vector3 (randx, randy, 0.0f);

		foreach (GUIText gt in damageText)
			gt.text = number.ToString ();
	}
}
