using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//ステータスデータ
public struct StatusBase {
	public string name;
	public string fullName;
	public string alphabet;
	public int level;
	public int experience;
	public int hp;
	public int maxhp;
	public int mp;
	public int maxmp;
	public int attack;
	public int defense;
	public int magicAttack;
	public int magicDefense;
	public int speed;
	public int lucky;
	public float[] attackTimes;
	public float[] attackRanges;
	public Texture[] iconFace;
	public List<SkillsData> skillList;
	public SkillsData[] signSkill;
	public AudioClip[] soundEffects;
	
	public StatusBase (string n, string fn, string al, int lev, int exe, int h, int m, int atk, int def, int mat, int mdf, int spd, int luc, float[] at, Texture[] icf, float[] rs, AudioClip[] ses)
	{
		name = n;
		fullName = fn;
		alphabet = al;
		level = lev;
		experience = exe;
		hp = h;
		maxhp = h;
		mp = m;
		maxmp = m;
		attack = atk;
		defense = def;
		magicAttack = mat;
		magicDefense = mdf;
		speed = spd;
		lucky = luc;
		attackTimes = at;
		attackRanges = rs;
		iconFace = icf;
		skillList = new List<SkillsData>();
		signSkill = new SkillsData[4];
		soundEffects = ses;
	}
};

public struct GrowthData {

	public int hp;
	public int mp;
	public int attack;
	public int defense;
	public int magicAttack;
	public int magicDefense;
	public int speed;
	public int lucky;

	public GrowthData (int h, int m, int atk, int def, int mat, int mde, int spd, int luc)
	{
		hp = h;
		mp = m;
		attack = atk;
		defense = def;
		magicAttack = mat;
		magicDefense = mde;
		speed = spd;
		lucky = luc;
	}
};

public struct CharacterData {

	public StatusBase status;
	public EquipData[] equips;
	public GameObject[] modle;
	public Texture[] charaImage;
	public Texture nameFolder;
	public string personality;
	public int totalExp;
	public GameObject[] attackEffect;
	public GrowthData growthData;

	public CharacterData (StatusBase s, EquipData[] qd, GameObject[] go, Texture[] c, Texture nf, string str, int tot, GameObject[] ae, GrowthData gt)
	{
		status = s;
		equips = qd;
		modle = go;
		charaImage = c;
		nameFolder = nf;
		personality = str;
		totalExp = tot;
		attackEffect = ae;
		growthData = gt;
	}
};

public struct MonsterData {

	public StatusBase status;
	public GameObject modle;
	public int[] dropItem;
	public float[] dropRate;
	public int experience;
	public int dropMoney;

	public MonsterData (StatusBase s, GameObject m, int[] di, float[] dt, int exp, int mon)
	{
		status = s;
		modle = m;
		dropItem = di;
		dropRate = dt;
		experience = exp;
		dropMoney = mon;
	}
};

//ステージデータ
public struct Stage {
	
	public string name;
	public string path;
	public Vector3 appearPosition;
	public string battlePath;
	public Vector3 battlePosition;
	public Vector3[] startPosition;
	public Vector3[] startEuler;
	public Vector3 cameraPosition;
	public Vector3 cameraDistance;
	public bool rotateFlag;
	public int[] warpDestination;
	public int[] symbolNumber;
	public Vector3[] symbolPosition;
	public Vector3[] symbolEuler;
	
	public Stage (string n, string p, Vector3 ap, string b, Vector3 bp, Vector3[] sp, Vector3[] sea, Vector3 cp, Vector3 cd, bool rf, int[] wd, int[] sn, Vector3[] spos, Vector3[] se)
	{
		name = n;
		path = p;
		appearPosition = ap;
		battlePath = b;
		battlePosition = bp;
		startPosition = sp;
		startEuler = sea;
		cameraPosition = cp;
		cameraDistance = cd;
		rotateFlag = rf;
		warpDestination = wd;
		symbolNumber = sn;
		symbolPosition = spos;
		symbolEuler = se;
	}
};

public struct ItemData {

	public int id;
	public string name;
	public string work;
	public Texture image; 

	public ItemData (int i, string n, string w, Texture img)
	{
		id = i;
		name = n;
		work = w;
		image = img;
	}
};

public struct EquipData {
	
	public int id;
	public string name;
	public string work;
	public int attribute;
	public Texture image; 
	
	public EquipData (int i, string n, string w, int at, Texture img)
	{
		id = i;
		name = n;
		work = w;
		attribute = at;
		image = img;
	}
};

