using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class StatusData : MonoBehaviour {

	private SceneManager sceneManager;
	public CharacterData[] playersData {get; set;}
	public MonsterData[] monstersData {get; set;}
	public int Money { get; set;}
	
	public Stage[] stageData {get; private set;}
	
	public Vector3[] playerPositions {get; private set;}
	public Vector3[] monsterPositions {get; private set;}
	
	public int[][] monsterNumbers {get; private set;}
	
	public int[] partyMember {get; set;}
	
	public ItemData[] itemData { get; private set;}
	public ItemBase[] itemBaseData { get; private set;}
	public List<ItemBase> itemList { get; set;}
	public EquipData[] equipData { get; private set;}
	// Use this for initialization
	void Awake () {
		
		AnalysisXMLItem ();
		AnalysisXML ();
		AnalysisXMLSkill ();
		AnalysisXMLStage ();
		
		InitPath ();
		InitItem ();
		InitSkill ();
		
		partyMember = new int[4] {0, 1, -1, -1};
	}

	/*void Start ()
	{
		GameObject o = Instantiate (Resources.Load ("Prefabs/UIPrefabs/GUItextureObject", typeof(GameObject)) as GameObject) as GameObject;
		GUITexture gt = o.GetComponent <GUITexture> ();
		gt.texture = Resources.Load ("UI/battle/loading", typeof(Texture)) as Texture;
		o.transform.position = Vector3.zero;
		gt.pixelInset = new Rect (0.0f, 0.0f, Screen.width, Screen.height);
		
		GetComponent<SceneManager> ().enabled = true;
		GetComponent<SoundManager> ().enabled = true;
		GetComponent<InputManager> ().enabled = true;
		GetComponent<BattleManager> ().enabled = true;
		GetComponent<EventManager> ().enabled = true;

		Destroy (o);
	}*/

	// Update is called once per frame
	void Update () {
	}
	
	void AnalysisXML ()
	{
		TextAsset xmlTextAsset = Resources.Load ("Data/StatusData", typeof(TextAsset)) as TextAsset;
		XmlDocument doc = new XmlDocument();
		doc.LoadXml (xmlTextAsset.text);
		
		XmlElement root = doc.DocumentElement;
		//----------------------------------------------------------------------------------------------プレイヤーdata
		XmlNodeList list = root.SelectSingleNode ("character").ChildNodes;
		
		playersData = new CharacterData[list.Count];
		
		int i = 0;
		
		foreach (XmlNode node in list)
		{
			StatusBase s = new StatusBase ();
			s.name = node.Attributes["name"].Value;
			s.fullName = node.Attributes["fullname"].Value;
			s.alphabet = node.Attributes["alphabet"].Value;
			s.level = int.Parse (node.Attributes["level"].Value);
			s.experience = int.Parse (node.Attributes["exe"].Value);
			s.hp = int.Parse (node.Attributes["hp"].Value);
			s.maxhp = s.hp;
			s.mp = int.Parse (node.Attributes["mp"].Value);
			s.maxmp = s.mp;
			s.attack = int.Parse (node.Attributes["attack"].Value);
			s.defense = int.Parse (node.Attributes["defense"].Value);
			s.magicAttack = int.Parse (node.Attributes["mat"].Value);
			s.magicDefense = int.Parse (node.Attributes["mdf"].Value);
			s.speed = int.Parse (node.Attributes["speed"].Value);
			s.lucky = int.Parse (node.Attributes["luc"].Value);
			
			XmlNode attackInfo = node.SelectSingleNode ("attackInfo");
			s.attackTimes = new float[3]{
				float.Parse (attackInfo.Attributes["attackTime1"].Value),
				float.Parse (attackInfo.Attributes["attackTime2"].Value),
				float.Parse (attackInfo.Attributes["attackTime3"].Value),
			};
			
			s.attackRanges = new float[2] {
				float.Parse (attackInfo.Attributes["attackRange1"].Value),
				float.Parse (attackInfo.Attributes["attackRange2"].Value),
			};
			s.skillList = new List<SkillsData> ();
			s.signSkill = new SkillsData[4];
			
			XmlNode icons = node.SelectSingleNode ("Icon");
			s.iconFace = new Texture[5]{
				Resources.Load ("UI/battle/"+icons.Attributes["normal"].Value, typeof (Texture)) as Texture,
				Resources.Load ("UI/battle/"+icons.Attributes["attack"].Value, typeof (Texture)) as Texture,
				Resources.Load ("UI/battle/"+icons.Attributes["damage"].Value, typeof (Texture)) as Texture,
				Resources.Load ("UI/battle/"+icons.Attributes["dead"].Value, typeof (Texture)) as Texture,
				Resources.Load ("UI/battle/"+icons.Attributes["skill"].Value, typeof (Texture)) as Texture,
			};

			icons = node.SelectSingleNode ("Sound");
			s.soundEffects = new AudioClip[2]{
				Resources.Load ("Sound/SE/"+icons.Attributes["attack"].Value, typeof (AudioClip)) as AudioClip,
				Resources.Load ("Sound/SE/"+icons.Attributes["hit"].Value, typeof (AudioClip)) as AudioClip,
			};
			playersData[i].status = s;
			
			icons = node.SelectSingleNode ("Modle");
			playersData[i].modle = new GameObject[2]{
				Resources.Load ("Prefabs/Characters/"+icons.Attributes["fpath"].Value, typeof (GameObject)) as GameObject,
				Resources.Load ("Prefabs/Characters/"+icons.Attributes["bpath"].Value, typeof (GameObject)) as GameObject,
			};
			
			icons = node.SelectSingleNode ("Image");
			playersData[i].nameFolder = Resources.Load ("UI/field/"+icons.Attributes["name"].Value, typeof (Texture)) as Texture;
			playersData[i].charaImage = new Texture[2]{
				Resources.Load ("UI/field/"+icons.Attributes["imageA"].Value, typeof (Texture)) as Texture,
				Resources.Load ("UI/field/"+icons.Attributes["imageB"].Value, typeof (Texture)) as Texture,
			};
			
			icons = node.SelectSingleNode ("Other");
			playersData[i].personality = icons.Attributes["Int"].Value;
			playersData[i].totalExp = int.Parse (icons.Attributes["totalExp"].Value);
			
			icons = node.SelectSingleNode ("attackEffect");
			playersData[i].attackEffect = new GameObject[2];
			playersData[i].attackEffect[0] = Resources.Load (icons.Attributes["path1"].Value) as GameObject;
			playersData[i].attackEffect[1] = Resources.Load (icons.Attributes["path2"].Value) as GameObject;
			
			playersData[i].equips = equipData;
			playersData[i].status.attack += equipData[0].attribute;
			playersData[i].status.defense += equipData[1].attribute + equipData[2].attribute + equipData[1].attribute;
			
			icons = node.SelectSingleNode ("GrowthData");
			playersData[i].growthData.hp = int.Parse (icons.Attributes["hp"].Value);
			playersData[i].growthData.mp = int.Parse (icons.Attributes["mp"].Value);
			playersData[i].growthData.attack = int.Parse (icons.Attributes["attack"].Value);
			playersData[i].growthData.defense = int.Parse (icons.Attributes["defense"].Value);
			playersData[i].growthData.magicAttack = int.Parse (icons.Attributes["mat"].Value);
			playersData[i].growthData.magicDefense = int.Parse (icons.Attributes["mde"].Value);
			playersData[i].growthData.speed = int.Parse (icons.Attributes["spd"].Value);
			playersData[i].growthData.lucky = int.Parse (icons.Attributes["luc"].Value);
			
			i++;
		}
		
		i = 0;
		
		//----------------------------------------------------------------------------------------------モンスターdata
		list = root.SelectSingleNode ("monster").ChildNodes;
		
		monstersData = new MonsterData[list.Count];
		
		foreach (XmlNode node in list)
		{
			StatusBase s = new StatusBase ();
			s.name = node.Attributes["name"].Value;
			s.fullName = node.Attributes["fullname"].Value;
			s.alphabet = node.Attributes["alphabet"].Value;
			s.level = int.Parse (node.Attributes["level"].Value);
			s.hp = int.Parse (node.Attributes["hp"].Value);
			s.mp = int.Parse (node.Attributes["mp"].Value);
			s.attack = int.Parse (node.Attributes["attack"].Value);
			s.defense = int.Parse (node.Attributes["defense"].Value);
			s.magicAttack = int.Parse (node.Attributes["mat"].Value);
			s.magicDefense = int.Parse (node.Attributes["mdf"].Value);
			s.speed = int.Parse (node.Attributes["speed"].Value);
			s.lucky = int.Parse (node.Attributes["luc"].Value);
			
			XmlNode attackInfo = node.SelectSingleNode ("attackInfo");
			s.attackTimes = new float[2]{
				float.Parse (attackInfo.Attributes["attackTime1"].Value),
				float.Parse (attackInfo.Attributes["attackTime2"].Value),
			};
			
			s.attackRanges = new float[2] {
				float.Parse (attackInfo.Attributes["attackRange1"].Value),
				float.Parse (attackInfo.Attributes["attackRange2"].Value),
			};
			
			s.skillList = new List<SkillsData> ();
			s.signSkill = new SkillsData[4];

			XmlNode sound = node.SelectSingleNode ("Sound");
			s.soundEffects = new AudioClip[2]{
				Resources.Load ("Sound/SE/"+sound.Attributes["attack"].Value, typeof (AudioClip)) as AudioClip,
				Resources.Load ("Sound/SE/"+sound.Attributes["hit"].Value, typeof (AudioClip)) as AudioClip,
			};

			monstersData[i].status = s;
			
			XmlNode icons = node.SelectSingleNode ("Modle");
			monstersData[i].modle = Resources.Load ("Prefabs/Monsters/"+icons.Attributes["path"].Value, typeof (GameObject)) as GameObject;
			monstersData[i].experience = int.Parse (icons.Attributes["exp"].Value);
			monstersData[i].dropMoney = int.Parse (icons.Attributes["money"].Value);
			
			icons = node.SelectSingleNode ("drop");
			XmlNodeList its = icons.ChildNodes;
			monstersData[i].dropItem = new int[its.Count];
			monstersData[i].dropRate = new float[its.Count];
			for (int j=0; j<its.Count; j++)
			{
				monstersData[i].dropItem[j] = int.Parse (its[j].Attributes["id"].Value);
				monstersData[i].dropRate[j] = float.Parse (its[j].Attributes["rate"].Value);
			}
			i++;
		}
		
	}
	
	void AnalysisXMLSkill ()
	{
		TextAsset xmlTextAsset = Resources.Load ("Data/SkillData", typeof(TextAsset)) as TextAsset;
		XmlDocument doc = new XmlDocument();
		doc.LoadXml (xmlTextAsset.text);
		
		XmlElement root = doc.DocumentElement;
		
		foreach (CharacterData cd in playersData)
		{
			XmlNodeList list = root.SelectSingleNode (cd.status.alphabet).ChildNodes;
			
			if (list.Count > 0)
			{
				foreach (XmlNode node in list)
				{
					XmlNode cNode = node.SelectSingleNode ("description");
					XmlNodeList ats = cNode.ChildNodes;
					float[] atimes = new float[ats.Count];
					
					for (int j=0; j<ats.Count; j++)
					{
						atimes[j] = float.Parse (ats[j].Attributes["time"].Value);
					}
					
					XmlNodeList efs = node.SelectNodes ("path");
					string[] ps = new string[efs.Count];
					
					for (int j=0; j<efs.Count; j++)
					{
						ps[j] = efs[j].Attributes["path"].Value;
					}
					XmlNode sound = node.SelectSingleNode ("Sound");
					switch (node.Attributes["type"].Value)
					{
					case	"遠距離攻撃":
						cd.status.skillList.Add (new LongSkill (cd.status.alphabet, 
						                                        node.Attributes["skillName"].Value, 
						                                        node.Attributes["clipName"].Value, 
						                                        cNode.Attributes["text"].Value,
						                                        float.Parse (cNode.Attributes["attack"].Value),
						                                        int.Parse (cNode.Attributes["cons"].Value),
						                                        float.Parse (cNode.Attributes["range"].Value),
						                                        atimes,
						                                        ps,
						                                        Resources.Load ("Sound/SE/"+sound.Attributes["effect"].Value, typeof (AudioClip)) as AudioClip));
						break;
					case	"近距離攻撃":
						cd.status.skillList.Add (new ShortSkill (cd.status.alphabet, 
						                                         node.Attributes["skillName"].Value, 
						                                         node.Attributes["clipName"].Value, 
						                                         cNode.Attributes["text"].Value,
						                                         float.Parse (cNode.Attributes["attack"].Value),
						                                         int.Parse (cNode.Attributes["cons"].Value),
						                                         float.Parse (cNode.Attributes["range"].Value),
						                                         atimes,
						                                         ps,
						                                         Resources.Load ("Sound/SE/"+sound.Attributes["effect"].Value, typeof (AudioClip)) as AudioClip));
						break;
					case	"サポート":
						cd.status.skillList.Add (new SupportSkill (cd.status.alphabet, 
						                                           node.Attributes["skillName"].Value, 
						                                           node.Attributes["clipName"].Value, 
						                                           cNode.Attributes["text"].Value,
						                                           float.Parse (cNode.Attributes["attack"].Value),
						                                           int.Parse (cNode.Attributes["cons"].Value),
						                                           float.Parse (cNode.Attributes["range"].Value),
						                                           atimes,
						                                           ps,
						                                           int.Parse (cNode.Attributes["time"].Value),
						                                           Resources.Load ("Sound/SE/"+sound.Attributes["effect"].Value, typeof (AudioClip)) as AudioClip));
						break;
					case	"遠距離多段攻撃":
						cd.status.skillList.Add (new HighLongSkill (cd.status.alphabet, 
						                                            node.Attributes["skillName"].Value, 
						                                            node.Attributes["clipName"].Value, 
						                                            cNode.Attributes["text"].Value,
						                                            float.Parse (cNode.Attributes["attack"].Value),
						                                            int.Parse (cNode.Attributes["cons"].Value),
						                                            float.Parse (cNode.Attributes["range"].Value),
						                                            atimes,
						                                            ps,
						                                            Resources.Load ("Sound/SE/"+sound.Attributes["effect"].Value, typeof (AudioClip)) as AudioClip));
						break;
					case	"中距離攻撃":
						cd.status.skillList.Add (new MiddleSkill (cd.status.alphabet, 
						                                          node.Attributes["skillName"].Value, 
						                                          node.Attributes["clipName"].Value, 
						                                          cNode.Attributes["text"].Value,
						                                          float.Parse (cNode.Attributes["attack"].Value),
						                                          int.Parse (cNode.Attributes["cons"].Value),
						                                          float.Parse (cNode.Attributes["range"].Value),
						                                          atimes,
						                                          ps,
						                                          Resources.Load ("Sound/SE/"+sound.Attributes["effect"].Value, typeof (AudioClip)) as AudioClip));
						break;
					}
				}
			}
		}
	}
	
	void AnalysisXMLStage ()
	{
		TextAsset xmlTextAsset = Resources.Load ("Data/StageData", typeof(TextAsset)) as TextAsset;
		XmlDocument doc = new XmlDocument();
		doc.LoadXml (xmlTextAsset.text);
		
		XmlElement root = doc.DocumentElement;
		XmlNodeList slist = root.SelectNodes ("stage");
		
		stageData = new Stage[slist.Count];
		
		int i = 0;
		foreach (XmlNode list in slist)
		{
			stageData[i].name = list.Attributes["name"].Value;
			
			XmlNode cl = list.SelectSingleNode ("dungeon");
			stageData[i].path = cl.Attributes["path"].Value;
			stageData[i].appearPosition = new Vector3 (
				float.Parse (cl.Attributes["px"].Value),
				float.Parse (cl.Attributes["py"].Value),
				float.Parse (cl.Attributes["pz"].Value));
			
			cl = list.SelectSingleNode ("battlefield");
			stageData[i].battlePath = cl.Attributes["path"].Value;
			stageData[i].battlePosition = new Vector3 (
				float.Parse (cl.Attributes["px"].Value),
				float.Parse (cl.Attributes["py"].Value),
				float.Parse (cl.Attributes["pz"].Value));
			
			cl = list.SelectSingleNode ("startposition");
			XmlNodeList clist = cl.ChildNodes;
			stageData[i].startPosition = new Vector3[clist.Count];
			stageData[i].startEuler = new Vector3[clist.Count];
			for (int j=0; j<clist.Count; j++)
			{
				stageData[i].startPosition[j] = new Vector3 (
					float.Parse (clist[j].Attributes["px"].Value),
					float.Parse (clist[j].Attributes["py"].Value),
					float.Parse (clist[j].Attributes["pz"].Value));
				stageData[i].startEuler[j] = new Vector3 (
					float.Parse (clist[j].Attributes["ex"].Value),
					float.Parse (clist[j].Attributes["ey"].Value),
					float.Parse (clist[j].Attributes["ez"].Value));
			}
			
			cl = list.SelectSingleNode ("camera");
			stageData[i].cameraPosition = new Vector3 (
				float.Parse (cl.Attributes["px"].Value),
				float.Parse (cl.Attributes["py"].Value),
				float.Parse (cl.Attributes["pz"].Value));
			stageData[i].cameraDistance = new Vector3 (
				float.Parse (cl.Attributes["dx"].Value),
				float.Parse (cl.Attributes["dy"].Value),
				float.Parse (cl.Attributes["dz"].Value));
			stageData[i].rotateFlag = bool.Parse (cl.Attributes["rotate"].Value);
			
			cl = list.SelectSingleNode ("warpDestination");
			clist = cl.ChildNodes;
			stageData[i].warpDestination = new int[clist.Count];
			for (int j=0; j<clist.Count; j++)
			{
				stageData[i].warpDestination[j] = int.Parse (clist[j].Attributes["point"].Value);
			}
			
			cl = list.SelectSingleNode ("symbolNumber");
			clist = cl.ChildNodes;
			stageData[i].symbolNumber = new int[clist.Count];
			stageData[i].symbolPosition = new Vector3[clist.Count];
			stageData[i].symbolEuler = new Vector3[clist.Count];
			for (int j=0; j<clist.Count; j++)
			{
				stageData[i].symbolNumber[j] = int.Parse (clist[j].Attributes["id"].Value);
				
				stageData[i].symbolPosition[j] = new Vector3 (
					float.Parse (clist[j].Attributes["px"].Value),
					float.Parse (clist[j].Attributes["py"].Value),
					float.Parse (clist[j].Attributes["pz"].Value));
				
				stageData[i].symbolEuler[j] = new Vector3 (
					float.Parse (clist[j].Attributes["ex"].Value),
					float.Parse (clist[j].Attributes["ey"].Value),
					float.Parse (clist[j].Attributes["ez"].Value));
			}
			
			i++;
		}
	}
	
	void AnalysisXMLItem ()
	{
		TextAsset xmlTextAsset = Resources.Load ("Data/ItemData", typeof(TextAsset)) as TextAsset;
		XmlDocument doc = new XmlDocument();
		doc.LoadXml (xmlTextAsset.text);
		
		XmlElement root = doc.DocumentElement;
		XmlNodeList xlist = root.SelectSingleNode ("items").ChildNodes;
		itemData = new ItemData[xlist.Count];
		
		int i = 0;
		foreach (XmlNode n in xlist)
		{
			itemData[i].id = int.Parse (n.Attributes["id"].Value);
			itemData[i].name = n.Attributes["name"].Value;
			itemData[i].work = n.Attributes["work"].Value;
			itemData[i].image = Resources.Load ("UI/items/"+n.Attributes["image"].Value, typeof (Texture)) as Texture;
			
			i++;
		}
		
		xlist = root.SelectSingleNode ("equipments").ChildNodes;
		equipData = new EquipData[xlist.Count];
		
		i = 0;
		foreach (XmlNode n in xlist)
		{
			equipData[i].id = int.Parse (n.Attributes["id"].Value);
			equipData[i].name = n.Attributes["name"].Value;
			equipData[i].work = n.Attributes["work"].Value;
			equipData[i].attribute = int.Parse (n.Attributes["attribute"].Value);
			equipData[i].image = Resources.Load ("UI/equipments/"+n.Attributes["image"].Value, typeof (Texture)) as Texture;
			i++;
		}
	}
	
	void InitPath ()
	{		
		playerPositions = new Vector3[4]{
			
			new Vector3 (50.0f, 0.1f, 10.0f),
			new Vector3 (48.0f, 0.1f, 10.5f),
			new Vector3 (50.0f, 0.1f, 10.5f),
			new Vector3 (0.0f, 0.1f, 0.0f),
		};
		
		monsterPositions = new Vector3[6]{
			
			new Vector3 (50.0f, 0.1f, 0.0f),
			new Vector3 (52.0f, 0.1f, -1.5f),
			new Vector3 (48.0f, 0.1f, -1.5f),
			new Vector3 (50.0f, 0.1f, -3.0f),
			new Vector3 (53.0f, 0.1f, -4.0f),
			new Vector3 (47.0f, 0.1f, -4.0f),
		};
		
		//各シンボルとエンカウントした後現れるモンスター
		//new int[3] {0, 0, 1},
		//-----------------------------------------------------------------------------------------------------------------------------------------------------------------//
		
		// [0]ケロピョン [1]ウルフ [2]スケルトン [3]ユリウス
		monsterNumbers = new int[4][]{
			new int[3] {0, 0, 1},
			new int[3] {1, 1, 0},
			new int[3] {2, 1, 2},
			new int[1] {3},
		};
		//-----------------------------------------------------------------------------------------------------------------------------------------------------------------//
	}
	
	void InitItem ()
	{
		itemBaseData = new ItemBase[5];
		itemBaseData[0] = new HPRecoveryItem (itemData[0], 1, 0.3f);
		itemBaseData[1] = new HPRecoveryItem (itemData[1], 1, 1.0f);
		itemBaseData[2] = new MPRecoveryItem (itemData[2], 1, 0.3f);
		itemBaseData[3] = new MPRecoveryItem (itemData[3], 1, 1.0f);
		itemBaseData[4] = new MPRecoveryItem (itemData[4], 1, 0.3f);
		
		itemList = new List<ItemBase> ();
		itemList.Add (new HPRecoveryItem (itemData[0], 5, 0.3f));
		itemList.Add (new MPRecoveryItem (itemData[2], 5, 0.3f));
		itemList.Add (new RevivalItem (itemData[4], 2, 0.3f));
		
		Money = 1000;
	}
	
	void InitSkill ()
	{
		foreach (CharacterData cd in playersData)
		{
			cd.status.signSkill[0] = cd.status.skillList[0];
			cd.status.signSkill[0].signUp = true;
		}
	}
	
	public void SortParty ()
	{
		int number = 0;
		for (int i=0; i<4; i++)
		{
			if (partyMember[i] > -1)
				number++;
		}

		int temp = partyMember[0];
		
		for (int i=0; i<3; i++)
		{
			partyMember[i] = partyMember[i+1];
		}
		
		partyMember[number-1] = temp;
		
	}
	
	public int ReturnNextExe (int level)
	{
		return 10+ 4*level*(level*level-1);
	}
	
	public void LevelUp (int id)
	{
		playersData[id].status.level ++;
		playersData[id].status.experience = 0;
		
		playersData[id].status.maxhp += playersData[id].growthData.hp;
		playersData[id].status.hp = playersData[id].status.maxhp;
		
		playersData[id].status.maxmp += playersData[id].growthData.mp;
		playersData[id].status.mp = playersData[id].status.maxmp;
		
		playersData[id].status.attack += playersData[id].growthData.attack;
		playersData[id].status.defense += playersData[id].growthData.defense;
		playersData[id].status.magicAttack += playersData[id].growthData.magicAttack;
		playersData[id].status.magicDefense += playersData[id].growthData.magicDefense;
		playersData[id].status.speed += playersData[id].growthData.speed;
		playersData[id].status.lucky += playersData[id].growthData.lucky;
	}
	
	public int GetPartyNumber ()
	{
		int temp = 0;
		for (int i=0; i<partyMember.Length; i++)
		{
			if (partyMember[i] > -1)
				temp ++;
		}
		
		return temp;
	}
}


