using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	private FieldCameraFixation shinCamera;

	private SoundManager soundManager;
	public InputManager inputManger { get; private set;}

	private GameObject fieldMenuObject; 
	private GUITexture fieldMenu;

	private StatusData status;

	public int nowStage {get; set;}
	public FadeIn fade { get; private set;}

	// Use this for initialization
	void Start () {
		soundManager = GetComponent <SoundManager> ();
		inputManger = GetComponent <InputManager> ();
		shinCamera = GameObject.Find ("Main Camera").AddComponent <FieldCameraFixation> ();
		status = GetComponent <StatusData> ();
		nowStage = 0;
		soundManager.PlayBGM (GetComponent<SceneManager> ().nowStage == 0 ? 1 : 2);
		CreateStage (nowStage, Vector3.zero);
		//InitStage (0);
		this.enabled = false;
	}

	public void InitStage (int p)
	{	
		soundManager.PlayBGM (GetComponent<SceneManager> ().nowStage == 0 ? 1 : 2);
		shinCamera.Prepare (status.stageData[nowStage].cameraDistance, status.stageData[nowStage].cameraPosition, status.stageData[nowStage].rotateFlag);
		CreateUnits (nowStage, Vector3.zero, p);
		fieldMenuObject = Resources.Load ("Prefabs/UIPrefabs/Field_Menu", typeof(GameObject)) as GameObject;
	}

	public void DeleteStage ()
	{
		GameObject[] stages = GameObject.FindGameObjectsWithTag ("Stage");

		GameObject[] symbols = GameObject.FindGameObjectsWithTag ("Symbol");

		for (int i=0; i<stages.Length; i++)
			Destroy (stages[i]);

		for (int i=0; i<symbols.Length; i++)
			Destroy (symbols[i]);

		Destroy (GameObject.FindWithTag ("Leader"));
	}

	public void CreateStage (int stageNumber, Vector3 pos)
	{
		GameObject stage = Instantiate (Resources.Load (status.stageData[stageNumber].path, typeof (GameObject)) as GameObject, 
		                                status.stageData[stageNumber].appearPosition, 
		                                Quaternion.identity) as GameObject;

		for (int i=0; i<status.stageData[stageNumber].warpDestination.Length; i++)
		{
			string warpPoint = string.Format("Warp Point00{0}", i+1);

			GameObject wp = stage.transform.Find (warpPoint).gameObject;

			wp.GetComponent<WarpController> ().Prepare (status.stageData[stageNumber].warpDestination[i], status.stageData[stageNumber].startPosition[i], this);
		}
	}

	public void CreateUnits (int stageNumber, Vector3 pos, int p)
	{
		GameObject player = Instantiate (status.playersData[status.partyMember[0]].modle[0], 
		                                 status.stageData[stageNumber].startPosition[p],
		                                 Quaternion.Euler (status.stageData[stageNumber].startEuler[p])) as GameObject;
		
		player.tag = "Leader";
		
		ShinFieldController scon = player.AddComponent <ShinFieldController> ();
		scon.Prepare (0, status.playersData[status.partyMember[0]].status, shinCamera, this.gameObject);
		
		
		if (status.stageData[stageNumber].symbolNumber.Length < 2) return;
		
		Instantiate (Resources.Load (status.stageData[stageNumber].battlePath, typeof (GameObject)) as GameObject, status.stageData[stageNumber].battlePosition, Quaternion.identity);
		
		for (int i=0; i<status.stageData[stageNumber].symbolNumber.Length; i++)
		{
			int sNumber = status.stageData[stageNumber].symbolNumber[i];
			
			GameObject symbol = Instantiate (status.monstersData[sNumber].modle, 
			                                 status.stageData[stageNumber].symbolPosition[i],
			                                 Quaternion.Euler (status.stageData[stageNumber].symbolEuler[i])) as GameObject;
			symbol.tag = "Symbol";
			
			SphereCollider sc = symbol.AddComponent <SphereCollider> ();
			
			sc.center = new Vector3 (0.0f, 0.5f, 0.0f); sc.isTrigger = true;
			
			symbol.AddComponent <SymbolController> ().Prepare (status.monstersData[status.stageData[stageNumber].symbolNumber[i]].status.alphabet, status.stageData[stageNumber].symbolNumber[i]);
		}
	}

	// Update is called once per frame
	void Update () {
	
		CallFieldMenu (inputManger.MenuButton == 1);
		ChangeLeader (inputManger.ButtonR == 1);
	}

	void CallFieldMenu (bool button)
	{
		if (button)
		{
			if (fieldMenu == null)
			{
				GameObject o = Instantiate (fieldMenuObject) as GameObject;
				fieldMenu = o.GetComponent<GUITexture> ();
				fieldMenu.pixelInset = new Rect (0, 0, Screen.width, Screen.height);

				FieldUIManager fum = o.AddComponent<FieldUIManager> ();
				fum.Prepare (inputManger, status, o);

				GetComponent<BattleManager> ().StopFieldUnit ();
				this.enabled = false;
			}
		}
	}

	Vector3 GetGroundPosition (float x, float z)
	{
		RaycastHit hit;

		Vector3 start = new Vector3 (x, 2.0f, z);
		Vector3 end = new Vector3 (x, -100.0f, z);
		
		if (Physics.Linecast (end, start, out hit))
		{
			return hit.point;
		}
		
		return end;
	}

	public void ChangeLeader (bool button)
	{
		if (button)
		{
			status.SortParty ();
			GameObject nowLeader = GameObject.FindWithTag ("Leader");
			Vector3 nowPosition = nowLeader.transform.position;
			Quaternion nowRotation = nowLeader.transform.rotation;
			Destroy (nowLeader);

			GameObject player = Instantiate (status.playersData[status.partyMember[0]].modle[0], 
			                                 nowPosition,
			                                 nowRotation) as GameObject;
			
			player.tag = "Leader";

			ShinFieldController sfc = player.AddComponent<ShinFieldController> ();
			sfc.Prepare (0, status.playersData[status.partyMember[0]].status, shinCamera, this.gameObject);
		}
	}

	public void CreateFadeAnimate ()
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/FadeIn", typeof(GameObject))as GameObject) as GameObject;
		fade = o.GetComponent<FadeIn> ();
	}
}
