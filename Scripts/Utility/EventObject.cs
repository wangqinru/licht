using UnityEngine;
using System.Collections;

public class EventObject : MonoBehaviour {

	private InputManager inputManager;

	private string openAnimation = "Door_open";
	private GUITexture icon;
	// Use this for initialization
	void Awake () {

		inputManager = GameObject.Find ("GameManager").GetComponent<InputManager> ();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Leader")
		{
			if (icon == null)
			{	
				GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/icon_proceed") as GameObject) as GameObject;
				icon = o.GetComponent<GUITexture> ();
				icon.texture = Resources.Load ("UI/field/icon_open", typeof (Texture)) as Texture;
			}
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.tag == "Leader")
		{
			if (inputManager.DecisionButton == 1)
			{
				GetComponent<Animation>().Play (openAnimation);
				this.GetComponent<BoxCollider> ().enabled = false;
				GameObject o = Instantiate (Resources.Load ("Prefabs/Event/Event_point", typeof (GameObject)) as GameObject,
				                            new Vector3 (1.0f, 1.21f, -8.5f),
				                            Quaternion.identity) as GameObject;
				EventPoint ep = o.AddComponent<EventPoint> ();
				ep.Prepare (inputManager);
				Destroy (icon.gameObject);
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Leader")
		{
			Destroy (icon.gameObject);
		}
	}
}
