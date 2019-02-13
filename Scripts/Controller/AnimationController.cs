using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationController : MonoBehaviour {

	private Animation _animation;

	private List<string> animationClipList;

	public bool motionEnd {get; private set;}

	public int currentClip {get; set;}
	public int nextClip {get; set;}
	
	void Awake () {
	
		_animation = GetComponent <Animation> ();

		animationClipList = new List<string> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetMotionList (string name)
	{
		currentClip = 0;

		nextClip = 0;

		animationClipList.Clear ();
		animationClipList.Add (name);

		motionEnd = false;

		_animation.Play (animationClipList[0]);
	}

	public void SetMotionList (string[] names)
	{
		currentClip = 0;
		
		nextClip = 0;
		
		animationClipList.Clear ();

		foreach (string s in names)
			animationClipList.Add (s);
		
		motionEnd = false;
		
		_animation.Play (animationClipList[0]);
	}

	public void PlayMotion (bool loop)
	{
		if (loop)
		{
			if (!_animation.IsPlaying (animationClipList[0]))
				_animation.CrossFade (animationClipList[0]);
		}
		else
		{
			if (!_animation.IsPlaying (animationClipList[currentClip]))
			{	
				if (nextClip > currentClip)
				{
					currentClip = nextClip;
					_animation.CrossFade (animationClipList[currentClip]);
					motionEnd = false;
				}
				else
					motionEnd = true;
			}
		}
	}

	public void SetAttackMotion (string[] attackClips, InputManager inputManager)
	{
		if (currentClip == nextClip && nextClip < attackClips.Length - 1 && inputManager.AttackButton == 1)
		{
			nextClip ++;
			animationClipList.Add (attackClips[nextClip]);
		}
	}

	public void ReSetAnimation (string clip)
	{
		_animation.Rewind (clip);
	}

	public bool CheckClip (string name)
	{
		return animationClipList[0] == name;
	}

	public bool CheckPlaying (string name)
	{
		return _animation.IsPlaying (name);
	}
}

