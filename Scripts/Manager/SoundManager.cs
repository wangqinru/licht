using UnityEngine;
using System.Collections;
using System;

public class SoundManager : MonoBehaviour {

	private AudioSource BGMsource;
	private AudioSource[] SEsources;

	private AudioClip[] BGM;
	private AudioClip[] SE;

	// Use this for initialization
	void Awake () {

		BGMsource = gameObject.AddComponent <AudioSource> ();
		BGMsource.loop = true;	

		SEsources = new AudioSource[8];
		for (int i=0; i<SEsources.Length; i++)
		{
			SEsources[i] = gameObject.AddComponent <AudioSource> ();
		}

		BGM = new AudioClip[4]{Resources.Load ("Sound/BGM/title", typeof (AudioClip)) as AudioClip, 
			Resources.Load ("Sound/BGM/kataribe", typeof (AudioClip)) as AudioClip,
			Resources.Load ("Sound/BGM/idou", typeof (AudioClip)) as AudioClip,
			Resources.Load ("Sound/BGM/battle", typeof (AudioClip)) as AudioClip};

		SE = new AudioClip[4]{Resources.Load ("Sound/SE/asiato1", typeof (AudioClip)) as AudioClip,
			Resources.Load ("Sound/SE/sword", typeof (AudioClip)) as AudioClip,
			Resources.Load ("Sound/SE/punch", typeof (AudioClip)) as AudioClip,
			Resources.Load ("Sound/SE/totunyu", typeof (AudioClip)) as AudioClip,};

		//PlayBGM (1);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayBGM (int index)
	{
		if( 0 > index || BGM.Length <= index ){
			return;
		}
		// 同じBGMの場合は何もしない
		if( BGMsource.clip == BGM[index] ){
			return;
		}
		BGMsource.Stop();

		BGMsource.clip = BGM[index];

		BGMsource.Play();
	}

	public void StopBGM()
	{
		BGMsource.Stop();

		BGMsource.clip = null;
	}

	public void PlaySE (int index)
	{
		if( 0 > index || SE.Length <= index )
			return;
		
		// 再生中で無いAudioSouceで鳴らす
		foreach (AudioSource source in SEsources)
		{
			if(!source.isPlaying)
			{
				source.clip = SE[index];
				source.Play();
				break;
			} 
		}
	}

	public void PlaySEOnce (int index)
	{
		if( 0 > index || SE.Length <= index )
			return;

		if (!SEsources[7].isPlaying)
		{
			SEsources[7].clip = SE[index];
			SEsources[7].Play();
		}
	}

	public void PlaySE (AudioClip clip)
	{		
		// 再生中で無いAudioSouceで鳴らす
		foreach (AudioSource source in SEsources)
		{
			if(!source.isPlaying)
			{
				source.clip = clip;
				source.Play();
				break;
			} 
		}
	}

	public void StopSE(){
		// 全てのSE用のAudioSouceを停止する
		foreach (AudioSource source in SEsources)
		{
			source.Stop ();
			source.clip = null;
		}
	}
}
