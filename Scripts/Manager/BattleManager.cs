using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

	private InputManager inputManager;
	private BattleGUIManager buiManager;
	private SoundManager soundManager;
	private EventPoint eventBattle;
	private AutoDelect skillFrame;
	public StatusData status { get; private set;}
	private GameObject battleCamera;
	private ComboNumber comboNumber;
	public int playerTarget { get; set;}

	public List<ShinBattleController> playerList {get; set;}
	public List<ShinBattleController> enemyList {get; set;}

	public List<int> getItems { get; private set;}
	public int getMoney { get; set;}
	public int getExp { get; set;}
	public int encount { get; set;}

	public string cursorName {get; private set;}
	public string cursorPath {get; private set;}

	public bool eventBattleflag { get; set;}

	// Use this for initialization
	void Start () {
	
		inputManager = GetComponent <InputManager> ();
		soundManager = GetComponent <SoundManager> ();
		status = GetComponent <StatusData> ();

		playerList = new List<ShinBattleController> ();
		enemyList = new List<ShinBattleController> ();

		getItems = new List<int> ();
		getMoney = 0;
		getExp = 0;
		encount = 0;
		eventBattleflag = false;
		cursorName = "TargetCursor(Clone)";
		cursorPath = "Prefabs/UIPrefabs/TargetCursor";

		InitAttackEvent ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void InitBattle (int num)
	{
		inputManager.ClearInput ();
		inputManager.enabled = false;
		GetComponent<SceneManager> ().enabled = false;

		soundManager.StopSE ();
		soundManager.PlaySE (3);

		GameObject[] symbols = GameObject.FindGameObjectsWithTag ("Symbol");

		foreach (GameObject o in symbols)
		{
			o.GetComponent <SymbolController> ().enabled = false;
			o.GetComponent <Animation> ().enabled = false;
		}

		GameObject leader = GameObject.FindWithTag ("Leader");

		leader.GetComponent <ShinFieldController> ().enabled = false;

		leader.GetComponent <Animation> ().enabled = false;

		CreateUnits (num);
	}
	
	public void ShowResult ()
	{
		buiManager.CallResult ();
	}

	public void UnitBattle ()
	{
		for (int i=0; i<getItems.Count; i++)
		{
			bool have = false;
			for (int j=0; j<status.itemList.Count; j++)
			{
				if (getItems[i] == status.itemList[j].id)
				{
					status.itemList[j].number ++;
					have = true;
					break;
				}
			}
			if (!have)
			{
				status.itemList.Add (status.itemBaseData[getItems[i]]);
			}
		}

		getItems.Clear ();
		status.Money += getMoney;
		getMoney = 0;
		encount ++;

		RestartFieldUnit ();
		for (int i=0; i<playerList.Count; i++)
		{
			status.playersData[playerList[i].ID].status.hp = playerList[i].nowHp;
			status.playersData[playerList[i].ID].status.mp = playerList[i].nowMp;
			Destroy (playerList[i].gameObject);
		}

		playerList.Clear ();

		Destroy (battleCamera.gameObject);
		GameObject mainCamera = GameObject.Find ("Main Camera");
		GetComponent<SceneManager> ().enabled = true;
		mainCamera.GetComponent<Camera>().enabled = true;
		mainCamera.GetComponent <FieldCameraFixation> ().enabled = true;
		mainCamera.GetComponent <FieldCameraFixation> ().ChangeDistance (1.0f);

		Destroy (buiManager.gameObject);
		if (eventBattleflag)
		{	
			eventBattle.AfterBattle ();
			StopFieldUnit ();
		}
		else
			soundManager.PlayBGM (GetComponent<SceneManager> ().nowStage == 0 ? 1 : 2);
	}

	public void StopFieldUnit ()
	{
		GameObject[] symbols = GameObject.FindGameObjectsWithTag ("Symbol");
		foreach (GameObject o in symbols)
		{
			o.GetComponent <SymbolController> ().enabled = false;
			o.GetComponent <Animation> ().enabled = false;
		}

		GameObject leader = GameObject.FindWithTag ("Leader");
		
		leader.GetComponent <ShinFieldController> ().enabled = false;
		
		leader.GetComponent <Animation> ().enabled = false;
	}

	public void RestartFieldUnit ()
	{
		GameObject[] symbols = GameObject.FindGameObjectsWithTag ("Symbol");
		foreach (GameObject o in symbols)
		{
			o.GetComponent <SymbolController> ().enabled = true;
			o.GetComponent <Animation> ().enabled = true;
		}
		
		GameObject leader = GameObject.FindWithTag ("Leader");
		
		leader.GetComponent <ShinFieldController> ().enabled = true;
		
		leader.GetComponent <Animation> ().enabled = true;
	}

	void CreateUnits (int num)
	{
		battleCamera = CreateCamera ();

		for (int i=0; i<status.monsterNumbers[num].Length; i++)
		{
			int number = status.monsterNumbers[num][i];

			GameObject enemy = Instantiate (status.monstersData[number].modle) as GameObject;

			if (number == 3)
			{
				BossBattleController auto = enemy.AddComponent <BossBattleController> ();
				enemyList.Add (auto);
				auto.InitBattle (this.gameObject, battleCamera, status.monsterPositions[i], Quaternion.identity, "Enemy", number, status.playersData[number].status);	
				auto.SetEffects (status.playersData[number].attackEffect);
				auto.enabled = false;
			}
			else
			{
				EnemyBattleController auto = enemy.AddComponent <EnemyBattleController> ();
				enemyList.Add (auto);
				auto.InitBattle (this.gameObject, battleCamera, status.monsterPositions[i], Quaternion.identity, "Enemy", number, status.monstersData[number].status);				
				auto.enabled = false;
			}

		}

		for (int i=0; i<4; i++)
		{
			if (status.partyMember[i] >= 0)
			{
				GameObject player = Instantiate (status.playersData[status.partyMember[i]].modle[1]) as GameObject;

				PlayerBattleController mb = player.AddComponent <PlayerBattleController> ();
				playerList.Add (mb);

				if (i==0) mb.isUser = true;
				else mb.isUser = false;

				mb.InitBattle (this.gameObject, battleCamera, status.playerPositions[i], Quaternion.Euler (0.0f, 180.0f, 0.0f), "Player", status.partyMember[i], status.playersData[status.partyMember[i]].status);
				mb.SetEffects (status.playersData[status.partyMember[i]].attackEffect);
				mb.enabled = false;
			}
		}
	}

	GameObject CreateCamera ()
	{
		GameObject battleCamera = Instantiate (Resources.Load ("Prefabs/Cameras/BattleCamera", typeof (GameObject))as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
	
		battleCamera.GetComponent<Camera>().enabled = false;

		battleCamera.AddComponent <ShinBattleCamera> ();

		GameObject mainCamera = GameObject.Find ("Main Camera");
		
		mainCamera.GetComponent <FieldCameraFixation> ().ChangeDistance (-1.0f);

		StartCoroutine (EncountAnimation (mainCamera, battleCamera));

		return battleCamera;
	}

	//エンカントアニメーション
	IEnumerator EncountAnimation (GameObject mainCamera, GameObject battleCamera)
	{
		MotionBlur montionBule = mainCamera.GetComponent <MotionBlur> ();
		
		montionBule.enabled = true;
		
		montionBule.extraBlur = true;
		
		montionBule.blurAmount = 0.1f;
		
		float n = 0.0f;
		
		while (n < 1.0f)
		{
			n += 0.1f;
			
			montionBule.blurAmount = Mathf.Lerp (0.0f, 1.0f, n);
			
			yield return 0;
		}
		
		yield return new WaitForSeconds (1.0f);
		
		montionBule.enabled = false;
		
		mainCamera.GetComponent<Camera>().enabled = false;
		
		battleCamera.GetComponent<Camera>().enabled = true;
		
		mainCamera.GetComponent <FieldCameraFixation> ().enabled = false;

		GUITexture m;

		inputManager.enabled = true;
		if (encount == 0)
		{
			GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/Manual", typeof(GameObject)) as GameObject) as GameObject;
			m = o.GetComponent<GUITexture> ();
			m.texture = Resources.Load ("UI/Manual/gBattle", typeof(Texture)) as Texture;

			while (true)
			{
				if (inputManager.ButtonB == 1)
				{
					Destroy (m.gameObject);
					break;
				}
				yield return 0;
			}
		}
		inputManager.ClearInput ();
		inputManager.enabled = false;

		GameObject um = Instantiate (Resources.Load ("Prefabs/UIPrefabs/BattleUIObject", typeof(GameObject)) as GameObject) as GameObject;
		buiManager = um.AddComponent<BattleGUIManager> ();
		buiManager.Prepare (this, inputManager);

		foreach (ShinBattleController sp in playerList)
			sp.enabled = true;

		foreach (ShinBattleController se in enemyList)
			se.enabled = true;

		inputManager.enabled = true;
		soundManager.PlayBGM (3);
	}

	//ドロップアイテム
	void ItemDrop (int id)
	{
		MonsterData md = status.monstersData[id];
		for (int i=0; i<md.dropItem.Length; i++)
		{
			float rand = Random.Range (0.0f, 1.0f);
			if (rand < md.dropRate[i])
				getItems.Add (md.dropItem[i]);
		}

		getExp += md.experience;
		getMoney += md.dropMoney;
	}

	//一番近いユニットを探す
	public int GetNearUnit (Vector3 d, bool isPlayer)
	{
		if (isPlayer) //呼び出し元がプレイヤーでしたら
		{
			int index = -1;
			float distance = 100.0f;

			for (int i=0; i<enemyList.Count; i++)
			{
				if ((enemyList[i].transform.position - d).magnitude < distance && enemyList[i].nowHp > 0)
				{
					distance = (enemyList[i].transform.position - d).magnitude;
					index = i;
				}
			}
			//if (index == -1) index = 0;
			return index;
		}
		else //	敵でしたら
		{
			int index = -1;
			float distance = 100.0f;
			
			for (int i=0; i<playerList.Count; i++)
			{
				if ((playerList[i].transform.position - d).magnitude < distance && playerList[i].nowHp > 0)
				{
					distance = (playerList[i].transform.position - d).magnitude;
					index = i;
				}
			}
			//if (index == -1) index = 0;
			return index;
		}
	}

	public void StopAnimation ()
	{
		foreach (ShinBattleController sp in playerList)
		{
			if (!sp.CheckState (7))
			{
				sp.gameObject.GetComponent <Animation> ().enabled = false;
				sp.enabled = false;
			}
		}

		foreach (ShinBattleController se in enemyList)
		{
			se.gameObject.GetComponent <Animation> ().enabled = false;
			se.enabled = false;
		}
	}

	public void RestartAnimation ()
	{
		foreach (ShinBattleController sp in playerList)
		{
			if (!sp.CheckState (7))
			{
				sp.gameObject.GetComponent <Animation> ().enabled = true;
				sp.enabled = true;
			}
		}

		foreach (ShinBattleController se in enemyList)
		{
			se.gameObject.GetComponent <Animation> ().enabled = true;
			se.enabled = true;
		}
	}

	//----------------------------------------------------------------------------------------------------------戦闘終了-------//
	public void BattleOver (bool win)
	{
		buiManager.enabled = false;
		if (skillFrame != null) Destroy (skillFrame.gameObject); 
		if (win)
		{
			for (int i=0; i<playerList.Count; i++)
			{
				playerList[i].transform.position = status.playerPositions[i];
				playerList[i].transform.eulerAngles = new Vector3 (0.0f, 180.0f, 0.0f);
			}
			
			buiManager.transform.position = new Vector3 (-1.0f, 0.0f, 0.0f);
			battleCamera.GetComponent<ShinBattleCamera> ().enabled = false;
			battleCamera.transform.position = playerList[0].transform.position - new Vector3 (1.0f, -1.0f, 2.5f);
			battleCamera.transform.LookAt ((playerList[0].transform.position + playerList[1].transform.position)/2 + new Vector3 (0.0f, 1.0f, 0.0f));
		}
		else
		{
			if (eventBattleflag)
			{	
				eventBattle.AfterBattle ();
				buiManager.transform.position = new Vector3 (-1.0f, 0.0f, 0.0f);
				StopFieldUnit ();
			}
			else
			{
				StartCoroutine ("GameOver");
			}
		}
	}

	/// <summary>
	/// Checks the rest.
	/// </summary>
	public void CheckRest (ShinBattleController d)
	{
		if (d.tag == "Player")
		{
			if (GetDeadNumber () == playerList.Count)
			{
				foreach (ShinBattleController se in enemyList)
					se.enabled = false;
				
				BattleOver (false);
			}
		}
		else
		{
			enemyList.Remove (d);
			if (enemyList.Count <= 0)
			{
				foreach (ShinBattleController sp in playerList)
				{
					if (!sp.enabled) sp.enabled = true;
					sp.BattleEnd ();
				}
				playerList[0].DeleteTarget ();
				BattleOver (true);
			}
		}
	}

	//------------------------------------------------------------------------------------ユニットが死んだときの処理-------------
	public void RemoveUnit (ShinBattleController d)
	{
		//d.enabled = false;
		//d.GetComponent<CharacterController> ().enabled = false;
		if (d.tag == "Player")
		{
			//playerList.Remove (d);
			foreach (ShinBattleController se in enemyList)
				se.ChangeTarget ();

			if (GetDeadNumber () == 0)buiManager.enabled = false;
		}
		else
		{
			ItemDrop (d.ID);
		//	Destroy (d.gameObject);

			foreach (ShinBattleController sp in playerList)
				sp.ChangeTarget ();

			if (enemyList.Count == 0)  buiManager.enabled = false;
		}
	}

	/// <summary>
	/// Sorts the party member.
	/// </summary>
	/// ------------------------------------操作キャラ切り替え-------------------------------------------------------- ///
	public void SortPartyMember ()
	{
		ShinBattleController temp = playerList [0];
		for (int i=0; i<playerList.Count-1; i++)
		{
			playerList[i] = playerList[i+1];
		}
		playerList[playerList.Count-1] = temp;

		for (int i=0; i<playerList.Count; i++)
		{
			playerList[i].ChangeOperationMode (i==0);
		}
	}

	/// <summary>
	/// Adds the combo  ------------------------------------------------------コンボ数-------------------------------------.
	/// </summary>
	public void AddCombo (int id)
	{
		if (playerTarget == id)
		{
			if (comboNumber == null)
			{
				GameObject o = new GameObject ();
				comboNumber = o.AddComponent<ComboNumber> ();
			}

			comboNumber.ShowComboNumber ();
		}
	}
	
	public int ExamineAroundUnits (Vector3 pos, bool isPlayer)
	{
		int temp = 0;

		if (isPlayer)
		{
			foreach (ShinBattleController ec in enemyList)
			{
				if ((ec.transform.position - pos).magnitude < 5.0f)
						temp++;
			}
		}
		else 
		{
			foreach (ShinBattleController pc in playerList)
			{
				if ((pc.transform.position - pos).magnitude < 5.0f)
					temp++;
			}
		}

		return temp;
	}

	public int GetNextTarget (int n, bool flag)
	{
		int temp = -1;
		if (n == playerList.Count-1) n = -1;

		if (!flag)
		{
			for (int i=n+1; i<playerList.Count; i++)
			{
				if (playerList[i].nowHp > 0)
				{
					temp = i;
					break;
				}
			}
		}
		else
		{
			for (int i=n+1; i<playerList.Count; i++)
			{
				if (playerList[i].nowHp == 0)
				{
					temp = i;
					break;
				}
			}
		}
		if (temp == -1) temp = n;
		return temp;
	}

	public int GetBeforeTarget (int n, bool flag)
	{
		int temp = -1;
		if (n == 0) n = playerList.Count;
		
		if (!flag)
		{
			for (int i=n-1; i>=0; i--)
			{
				if (playerList[i].nowHp > 0)
				{
					temp = i;
					break;
				}
			}
		}
		else
		{
			for (int i=n-1; i>=0; i--)
			{
				if (playerList[i].nowHp == 0)
				{
					temp = i;
					break;
				}
			}
		}
		if (temp == -1) temp = n;
		return temp;
	}

	public int GetDeadNumber ()
	{
		int temp = 0;
		for (int i=0; i<playerList.Count; i++)
		{
			if (playerList[i].nowHp <= 0)
				temp++;
		}

		return temp;
	}

	public void SetEventFlag (bool flag, EventPoint ep)
	{
		eventBattleflag = flag;
		eventBattle = ep;
	}

	private void InitAttackEvent ()
	{
		GameObject battleCamera = Instantiate (Resources.Load ("Prefabs/Cameras/BattleCamera", typeof (GameObject))as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
		battleCamera.GetComponent<Camera>().enabled = false;	
		battleCamera.AddComponent <ShinBattleCamera> ();

		for (int i=0; i<2; i++)
		{
			GameObject player = Instantiate (status.playersData[i].modle[1]) as GameObject;
				
			PlayerBattleController mb = player.AddComponent <PlayerBattleController> ();
			mb.InitBattle (this.gameObject, battleCamera, status.playerPositions[i], Quaternion.Euler (0.0f, 180.0f, 0.0f), "Player", i, status.playersData[status.partyMember[i]].status);
			mb.AddAttackEvent ();
			Destroy (player);
		}

		for (int i=0; i<4; i++)
		{
			GameObject enemy = Instantiate (status.monstersData[i].modle) as GameObject;
			
			if (i == 3)
			{
				BossBattleController auto = enemy.AddComponent <BossBattleController> ();
				auto.InitBattle (this.gameObject, battleCamera, status.monsterPositions[i], Quaternion.identity, "Enemy", 0, status.playersData[i].status);	
				auto.AddAttackEvent ();
			}
			else
			{
				EnemyBattleController auto = enemy.AddComponent <EnemyBattleController> ();
				auto.InitBattle (this.gameObject, battleCamera, status.monsterPositions[i], Quaternion.identity, "Enemy", 0, status.monstersData[i].status);				
				auto.AddAttackEvent ();
			}
			Destroy (enemy);
		}

		Destroy (battleCamera);
	}

	public void ShowSkill (Texture t, string n)
	{
		if (skillFrame == null)
		{
			GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/Skill_frame", typeof (GameObject))) as GameObject;
			skillFrame = o.GetComponent<AutoDelect> ();
		}

		skillFrame.InitCounter (120, t, n);
	}

	public void StartEscape ()
	{
		if (!eventBattleflag)
			StartCoroutine ("escape");
	}

	IEnumerator GameOver()
	{
		int count = 300;

		while (count-- > 0)
		{
			battleCamera.transform.position -= new Vector3 (0.01f, 0.0f, 0.01f);
			yield return 0;
		}

		this.GetComponent<SceneManager> ().CreateFadeAnimate ();
		Application.LoadLevel ("TitleScene");
	}

	IEnumerator escape ()
	{
		yield return new WaitForSeconds (5.0f);

		//GetComponent<SceneManager> ().CreateFadeAnimate ();
		if (skillFrame != null) Destroy (skillFrame.gameObject); 
		RestartFieldUnit ();
		GetComponent<SceneManager> ().enabled = true;
		playerList [0].DeleteTarget ();
		for (int i=0; i<playerList.Count; i++)
		{
			status.playersData[playerList[i].ID].status.hp = playerList[i].nowHp;
			status.playersData[playerList[i].ID].status.mp = playerList[i].nowMp;
			Destroy (playerList[i].gameObject);
		}
		playerList.Clear ();
		for (int i=0; i<enemyList.Count; i++)
		{
			Destroy (enemyList[i].gameObject);
		}
		enemyList.Clear ();
		
		Destroy (battleCamera.gameObject);
		GameObject mainCamera = GameObject.Find ("Main Camera");
		mainCamera.GetComponent<Camera>().enabled = true;
		mainCamera.GetComponent <FieldCameraFixation> ().enabled = true;
		mainCamera.GetComponent <FieldCameraFixation> ().ChangeDistance (1.0f);	
		Destroy (buiManager.gameObject);
		soundManager.PlayBGM (GetComponent<SceneManager> ().nowStage == 0 ? 1 : 2);
	}
}
