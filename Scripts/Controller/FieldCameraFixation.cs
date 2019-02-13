using UnityEngine;
using System.Collections;

public class FieldCameraFixation : MonoBehaviour {
	
	public Vector3 targetPosition {get; set;}
	private Vector3 distance;	
	private float angel = 120.0f;
	private bool rotateFlag = true;

	private LayerMask lineofsightMask = 0;	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if (rotateFlag)
		{
			transform.position = targetPosition - new Vector3 (Mathf.Sin(Mathf.Deg2Rad * angel)*3.0f, 0.0f, Mathf.Cos(Mathf.Deg2Rad * angel)*3.0f);
			angel = 90 + Mathf.Repeat (targetPosition.y*24, 360);
		}
		else
			transform.position = AdjustLineOfSight (targetPosition - distance, targetPosition);

		transform.LookAt (targetPosition);
	}

	Vector3 AdjustLineOfSight (Vector3 cameraPosition, Vector3 targetPosition)
	{
		RaycastHit hit;
		
		if (Physics.Linecast (targetPosition, cameraPosition, out hit, lineofsightMask.value))
		{
			return new Vector3 (hit.point.x, hit.point.y + 0.5f, hit.point.z);
		}
		
		return cameraPosition;
	}
	
	public void ChangeDistance (float var, Vector3 tp)
	{
		StartCoroutine (SnapingPosition (distance.z + var, tp));
	}
	
	public void ChangeDistance (float var)
	{
		StartCoroutine (SnapingPosition (distance.z + var));
	}
	
	IEnumerator SnapingPosition (float td)
	{	
		float nTime = 0.0f;
		float t = 0.05f;
		
		float nd = distance.z;
		
		while (nTime != 1.0f)
		{
			nTime += t;
			
			nTime += t; t -= (t>= 0.02f) ? 0.001f : 0;
			
			float z = Mathf.Lerp (nd, td, nTime);
			distance = new Vector3 (distance.x, distance.y, z);

			if (nTime >= 1) {nTime = 1; break;}
			
			yield return 0;
		}
		
	}
	
	IEnumerator SnapingPosition (float td, Vector3 tpos)
	{
		Vector3 npos = targetPosition;
		
		float nTime = 0.0f;
		float t = 0.05f;
		
		float nd = distance.z;
		
		while (nTime != 1.0f)
		{
			nTime += t;
			
			float z = Mathf.Lerp (nd, td, nTime);
			distance = new Vector3 (distance.x, distance.y, z);

			float nposx = Mathf.Lerp (npos.x, tpos.x, nTime);
			float nposy = Mathf.Lerp (npos.y, tpos.y, nTime);
			float nposz = Mathf.Lerp (npos.z, tpos.z, nTime);
			
			targetPosition = new Vector3 (nposx, nposy, nposz);
			
			if (nTime >= 1) {nTime = 1; break;}
			
			yield return 0;
		}		
	}

	public void Prepare (Vector3 dis, Vector3 pos, bool flag)
	{
		distance = dis;
		rotateFlag = flag;
		transform.position = pos;
		transform.position = targetPosition - distance;
		lineofsightMask = ~(1 << LayerMask.NameToLayer ("nonPlayer"));
	}
}
