using UnityEngine;
using System.Collections;

public class RecoveryEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!GetComponent<Animation>().isPlaying)
		{
			Destroy (gameObject);
		}
	}
}
