using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using System.Reflection;

public class EventManager : MonoBehaviour {

	public string windowPath {get; private set;} 

	public InputManager inputManager {get; private set;}
	public Animation[] character {get; set;}
	public TalkWindow talkWindow {get; set;}

	private SceneManager sceneManager;
	private StatusData status;
	private string fileName = "Data/openning";

	private StreamReader streamReader;	
	private List<BaseCommand> commands = new List<BaseCommand> ();	
	private Dictionary<string, string> dcommand = new Dictionary<string, string> ();

	private BaseCommand nowcommand;
	private int nowIndex = 0;
	// Use this for initialization
	void Awake () {

		windowPath = "Prefabs/UIPrefabs/TalkWindow";
		inputManager = GetComponent<InputManager> ();
		sceneManager = GetComponent<SceneManager> ();
		status = GetComponent<StatusData> ();

		InitEvent ();
	}

	void InitEvent ()
	{
		GameObject.Find ("Main Camera").GetComponent<Camera>().enabled = false;
		GameObject sub = Instantiate (Resources.Load ("Prefabs/Cameras/BattleCamera", typeof (GameObject)) as GameObject, 
		                              new Vector3 (-4.47f, 1.65f, -1.37f),
		                              Quaternion.Euler (0.0f, 48.0f, 0.0f)) as GameObject;
		sub.name = "SubCamera";
		character = new Animation[2];
		for (int i=0; i<2; i++)
		{
			GameObject player = Instantiate (status.playersData[i].modle[0], 
			                                 new Vector3 (-1.4f+0.6f*i, 0.0f, -4.5f-0.6f*i),
			                                 Quaternion.identity) as GameObject;

			character[i] = player.GetComponent<Animation> ();
		}

		commands.Add (new MoveAnimation (new float[2] {4.0f, 1.4f}, 
										new Vector3[2]{new Vector3 (-1.6f, 0.0f, 2.8f), new Vector3 (-0.8f, 0.0f, 1.6f)}, 
										new string[2]{"rihito@Run", "guren@Walk"}) );
		commands.Add (new RotateAnimation (150.0f, 300.0f, "rihito@Walk"));
		commands.Add (new CreateTalkWindow ());

		TextAsset text = Resources.Load (fileName, typeof(TextAsset)) as TextAsset;
		MemoryStream memoryStream = new MemoryStream (text.bytes);

		streamReader = new StreamReader(memoryStream, Encoding.GetEncoding("utf-8"));
		CommandAnalysis ();
		streamReader.Close ();

		commands.Add (new DeleteTalkWindow ());

		nowcommand = commands[nowIndex];
	}

	// Update is called once per frame
	void Update () {

		if (nowIndex < commands.Count -1)
		{
			if (nowcommand.isComplete)
			{
				nowIndex++;
				nowcommand = commands[nowIndex];
			}
			nowcommand.Run (this);
			if (nowIndex == commands.Count-1) sceneManager.CreateFadeAnimate ();
		}
		else
		{
			if (sceneManager.fade.isComplete)
			{
				GameObject.Find ("Main Camera").GetComponent<Camera>().enabled = true;
				Destroy (GameObject.Find ("SubCamera"));

				for (int i=0; i<character.Length; i++)
					Destroy (character[i].gameObject);

				sceneManager.InitStage (0);
				sceneManager.GetComponent<BattleManager> ().StopFieldUnit ();
				ManualManager mm = gameObject.AddComponent<ManualManager> ();
				mm.Prepare (true, inputManager);
				this.enabled = false;
			}
		}
	}

	void CommandAnalysis ()
	{
		dcommand.Add ("name", "NameSettingCommand");
		dcommand.Add ("face", "FaceSettingCommand");	
		dcommand.Add ("red", "ColorRedTextSettingCommand");		
		dcommand.Add ("yellow", "ColorYellowTextSettingCommand");		
		dcommand.Add ("orange", "ColorOrangeTextSettingCommand");		
		dcommand.Add ("bold", "BoldTextSettingCommand");
		dcommand.Add ("text", "TextSettingCommand");	
		dcommand.Add ("br", "IndentionSettingCommand");
		
		for (;;)
		{
			string tempText = streamReader.ReadLine ();
			if (tempText == null) break;
			
			bool waitFlag = false;
			
			if (tempText.EndsWith ("#")) tempText = tempText.Substring (0, tempText.Length-1);
			
			int endT = tempText.LastIndexOf (">");
			int endT2 = tempText.LastIndexOf ("<");
			
			if (endT != -1 && endT2 != -1 && tempText.Substring (endT2+1, endT-endT2-1) == "wait")
			{
				tempText = tempText.Substring (0, endT2);
				waitFlag = true;
			}
			
			for (int i=0; i<tempText.Length; i+=0)
			{
				int[] index = new int[4] {-1, -1, -1, -1};
				
				if ((index[0] = tempText.IndexOf ("[", i)) != -1)
				{
					if (index[0] > 0)
					{
						int t = tempText.LastIndexOf ("]", index[0]-1);
						
						SetCommand (tempText, "text", new int[2]{t+1, index[0]-t-1});
					}
					
					index[1] = tempText.IndexOf ("]", index[0]+1);
					
					index[2] = tempText.IndexOf ("[/", index[1]+1);
					
					index[3] = tempText.IndexOf ("]", index[2]+1);
					
					string cs = tempText.Substring (index[0]+1, index[1]-index[0]-1);
					string ce = tempText.Substring (index[2]+2, index[3]-index[2]-2);
					
					if (cs == ce)
						SetCommand (tempText, cs, new int[2]{index[1]+1, index[2]-index[1]-1});
					
					i = index[3]+1;
				}
				else
				{
					SetCommand (tempText, "text", new int[2]{i, tempText.Length-i});
					
					i = tempText.Length;
				}
			}
			if (waitFlag) commands.Add (new InputWaitCommand ());
			else SetCommand ("richtext", "br", new int[2]);
		}
	}
	
	void SetCommand (string tex, string com, int[] ind)
	{
		Type tag = Type.GetType (dcommand[com]);
		
		BaseCommand o = (BaseCommand)Activator.CreateInstance (tag, new object[] {tex.Substring (ind[0], ind[1])});
		
		commands.Add (o);
	}
}
