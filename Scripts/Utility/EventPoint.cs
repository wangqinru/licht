using UnityEngine;
using System.Collections;

public class EventPoint : MonoBehaviour {
	
	private InputManager inputManager;
	private BattleManager batterManager;
	private SceneManager sceneManager;
	
	private GameObject mainCamera;
	private GameObject subCamera;
	private GameObject moviePlayer;
	
	private MovieTexture mtex;
	
	private enum StoryState {
		fadein,
		playing,
		battle,
		end,
	};
	
	private StoryState story;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		switch (story)
		{
		case	StoryState.fadein:
			if (sceneManager.fade != null)
			{
				if (sceneManager.fade.isComplete && !mtex.isPlaying)
				{
					batterManager.GetComponent<SoundManager> ().StopBGM ();
					mtex.Play ();
					GetComponent<AudioSource>().Play ();
					sceneManager.enabled = false;
					inputManager.enabled = true;
					story = StoryState.playing;
				}
			}
			break;
		case	StoryState.playing:
			if (inputManager.CancelButton == 1)
			{
				mtex.Stop ();
			}
			if (!mtex.isPlaying)
			{
				InitBattle ();
				story = StoryState.battle;
			}
			break;
		case	StoryState.battle:
			break;
		case	StoryState.end:
			if (inputManager.CancelButton == 1)
			{
				mtex.Stop ();
			}
			if (!mtex.isPlaying)
			{
				Application.LoadLevel ("TitleScene");
			}
			break;
		}
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Leader")
		{
			batterManager.StopFieldUnit ();
			sceneManager.CreateFadeAnimate ();
			inputManager.enabled = false;
			inputManager.ClearInput ();
			this.GetComponent<BoxCollider> ().enabled = false;
			BeforeBattle ();
		}
	}
	
	public void Prepare (InputManager im)
	{
		inputManager = im;
		batterManager = inputManager.GetComponent<BattleManager> ();
		sceneManager = inputManager.GetComponent<SceneManager> ();
		mainCamera = GameObject.Find ("Main Camera");
		story = StoryState.fadein;
	}
	
	void BeforeBattle ()
	{
		moviePlayer = Instantiate (Resources.Load ("Prefabs/Event/Movie_player", typeof (GameObject)),
		                           new Vector3 (0.0f, 80.0f, 8.0f),
		                           Quaternion.Euler (new Vector3 (90.0f, 0.0f, 0.0f))) as GameObject;
		moviePlayer.transform.localScale = new Vector3 (1.28f*Screen.width/1280.0f, 1.0f, 0.72f*Screen.height/720.0f);
		mtex = moviePlayer.GetComponent<Renderer>().material.mainTexture as MovieTexture;
		mainCamera.GetComponent<Camera>().enabled = false;
		subCamera = Instantiate (Resources.Load ("Prefabs/Cameras/BattleCamera", typeof (GameObject))as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
		subCamera.name = "SubCamera";
		subCamera.transform.position = new Vector3 (0.0f, 80.0f, 14.0f);
		subCamera.transform.LookAt (moviePlayer.transform);
		GetComponent<AudioSource>().clip = mtex.audioClip;
		
		Instantiate (Resources.Load ("Prefabs/BattleFields/yFloor_battleField", typeof (GameObject))as GameObject, 
		             new Vector3 (50.0f, 26.5f, 6.0f), Quaternion.identity);
	}
	
	public void AfterBattle ()
	{
		sceneManager.enabled = false;
		mainCamera.GetComponent<Camera>().enabled = false;
		subCamera.GetComponent<Camera>().enabled = true;
		moviePlayer.GetComponent<Renderer> ().material.mainTexture = Resources.Load ("Movies/movie_03", typeof(MovieTexture)) as MovieTexture;
		mtex = moviePlayer.GetComponent<Renderer>().material.mainTexture as MovieTexture;
		GetComponent<AudioSource>().clip = mtex.audioClip;
		batterManager.GetComponent<SoundManager> ().StopBGM ();
		mtex.Play ();
		GetComponent<AudioSource>().Play ();
		
		story = StoryState.end;
	}
	
	void InitBattle ()
	{
		subCamera.GetComponent<Camera>().enabled = false;
		mainCamera.GetComponent<Camera>().enabled = true;
		GameObject player = GameObject.FindWithTag ("Leader");
		player.transform.position -= new Vector3 (0.0f, 0.0f, 8.0f);
		mainCamera.transform.position -= new Vector3 (0.0f, 0.0f, 8.0f);
		
		GameObject yri = Instantiate (Resources.Load ("Prefabs/Characters/yuriusu_field", typeof(GameObject)) as GameObject, 
		                              new Vector3 (1.4f, -0.3f, -17.5f),
		                              Quaternion.identity) as GameObject;
		//yri.tag = "Symbol";
		batterManager.RestartFieldUnit ();
		SphereCollider ys = yri.AddComponent<SphereCollider> ();
		ys.isTrigger = true;
		ys.radius = 5.0f;
		EventEncount ee = yri.AddComponent<EventEncount> ();
		ee.Prepare (batterManager, 3);
		batterManager.SetEventFlag (true, this);
	}
}

