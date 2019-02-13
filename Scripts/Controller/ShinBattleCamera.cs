using UnityEngine;
using System.Collections;

public class ShinBattleCamera : MonoBehaviour {

	public Vector3 playerPosition { get; set;}
	public Vector3 targetPositon { get; set;}

	public float originAngle { get; set;}
	public bool rotateFlag { get; set;}

	private Vector3 centerPosition = Vector3.zero;
	private float distance = 4.5f, height = 0.0f;

	private Vector3 moveDirection = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void LateUpdate ()
	{
		Vector3 distancePT = targetPositon - playerPosition;
		
		distancePT.y = 0;
		
		distance = distancePT.magnitude/1.5f;
		
		distance = Mathf.Clamp (distance, 4.0f, 10.0f);
		
		height = distance/2.5f;
		height = Mathf.Clamp (height, 0.5f, 6.0f); 

		Vector3 centerPosition = GetCenterPosition ();
		Vector3 nextPosition = centerPosition + CameraPosition ();

		/*float nextX = Mathf.MoveTowards (transform.position.x, nextPosition.x, 0.05f);
		float nextY = Mathf.MoveTowards (transform.position.y, nextPosition.y, 0.05f);
		float nextZ = Mathf.MoveTowards (transform.position.z, nextPosition.z, 0.05f);

		transform.position = new Vector3 (nextX, nextY, nextZ);*/

		transform.position = Vector3.Slerp (transform.position, nextPosition, 0.05f);

		transform.LookAt (centerPosition);

		moveDirection = (transform.position - centerPosition).normalized;
	}

	public Vector3 CameraPosition ()
	{
		if (rotateFlag)
		{
			Vector3 cameraCross = Vector3.Cross (targetPositon - playerPosition, transform.position - playerPosition);  //外積
			
				/* 外積の中身の処理
			 * 
			 * Vector3 A = targetPositon - playerPosition;
			 * 
			 * Vector3 B = transform.position - playerPosition;
			 * 
			 * Vector3 AxB = new Vector3 (A.y*B.z - A.z*B.y, A.z*B.x - A.x*B.z, A.x*B.y - A.y*B.x);
			 * 
			 * */

			return CameraPosition ((cameraCross.y > 0) ? Quaternion.Euler (0.0f, 90.0f, 0.0f) : Quaternion.Euler (0.0f, 90.0f + 180.0f, 0.0f));
		}
		else
			return new Vector3 (moveDirection.x * distance, height, moveDirection.z * distance);
	}
	
	Vector3 CameraPosition (Quaternion rot)
	{
		Vector3 vectorPT = targetPositon - playerPosition;

		Matrix4x4 m = Matrix4x4.TRS (Vector3.zero, rot, Vector3.one);

		/*     中身の処理
		 *float x1 = Mathf.Cos (60.0f * Mathf.Deg2Rad) * vectorPT.x + Mathf.Sin (60.0f * Mathf.Deg2Rad) * vectorPT.z;
		
		 *float y1 = vectorPT.y;
		
		 *float z1 = Mathf.Sin (60.0f * Mathf.Deg2Rad) + vectorPT.x * -1 + Mathf.Cos (60.0f * Mathf.Deg2Rad) * vectorPT.z;
		*/

		Vector3 newPosition = m.MultiplyPoint3x4 (vectorPT).normalized;

		return new Vector3 (newPosition.x * distance, height, newPosition.z * distance);
	}

	//行列
	Vector3 CameraPosition (Vector3 pos)
	{
		Vector3 vectorPT = targetPositon - playerPosition;
		
		Matrix4x4 m = Matrix4x4.TRS (pos, Quaternion.identity, Vector3.one);

		Vector3 newPosition = m.MultiplyPoint3x4 (vectorPT).normalized;

		return new Vector3 (newPosition.x * distance, vectorPT.y, newPosition.z * distance);
	}

	Vector3 GetCenterPosition ()
	{
		Vector3 centerPE = (targetPositon + playerPosition) / 2;

		//Vector3 centerPosition = new Vector3 (centerPE.x, 0.6f, centerPE.z);

		float x = Mathf.MoveTowards (centerPosition.x, centerPE.x, 5.0f*Time.deltaTime);
		float z = Mathf.MoveTowards (centerPosition.z, centerPE.z, 5.0f*Time.deltaTime);

		centerPosition = new Vector3 (x, 0.3f, z);

		return centerPosition;
	}

	public void InitBattle (Vector3 p, Vector3 e)
	{
		centerPosition = (e + p) / 2;

		rotateFlag = false;
		targetPositon = e;

		playerPosition = p;

		transform.position = centerPosition + CameraPosition (Quaternion.Euler (0.0f, 90.0f, 0.0f));

		transform.LookAt (centerPosition);
	}
}
