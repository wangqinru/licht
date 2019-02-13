using UnityEngine;
using System.Collections;

public class ComboNumber : MonoBehaviour {

	private int counter;
	private int combo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		counter --;
		if (counter < 0)
			Destroy (gameObject);
	}

	void OnGUI ()
	{
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (Screen.width/1280.0f, Screen.height/720.0f, 1));

		GUI.Label (new Rect (36.0f, 24.0f, 240.0f, 64.0f), Resources.Load ("UI/battle/battle_combo_bg", typeof(Texture)) as Texture);

		int temp = combo;
		for (int i=0; i<3; i++)
		{
			int digit = temp%10;

			string path = "UI/number/c_0"+digit.ToString ();
			GUI.Label (new Rect (250.0f-i*35.0f, 24.0f, 64.0f, 64.0f), Resources.Load (path, typeof (Texture)) as Texture);

			temp /= 10;

			if (temp==0)
				break;
		}
	}

	public void ShowComboNumber ()
	{
		counter = 100;
		combo ++;
	}
}
