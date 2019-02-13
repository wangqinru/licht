using UnityEngine;
using System.Collections;

public class AutoRotation : MonoBehaviour {

	private Transform bCamera;

	private ShinBattleController target;

	private Transform cursor;

	private Transform HpGauge;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (target != null)
		{
			transform.position = new Vector3 (target.transform.position.x, target.transform.position.y + 0.02f, target.transform.position.z);
			
			cursor.eulerAngles = new Vector3 (360.0f-bCamera.eulerAngles.x, bCamera.eulerAngles.y-180, 0.0f);
		
			float offset = (float)target.nowHp / (float)target.status.hp;

			offset = (1 - offset)/2;

			offset = Mathf.Clamp (offset, 0.0f, 0.5f);

			HpGauge.GetComponent <Renderer> ().material.SetTextureOffset ("_MainTex", new Vector2(0, offset));
		}
	}

	public void InitPosition (float y, Transform target)
	{
		bCamera = GameObject.Find ("BattleCamera(Clone)").transform;

		this.target = target.GetComponent <ShinBattleController> ();

		cursor = transform.Find ("HpCursor");

		HpGauge = cursor.Find ("cursol");

		cursor.localPosition = new Vector3 (0.0f, y, 0.0f);
	}
}
