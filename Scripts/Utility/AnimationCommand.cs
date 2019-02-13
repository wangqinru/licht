using UnityEngine;
using System.Collections;

public class AnimationCommand : BaseCommand {

	public AnimationCommand()
	{
		isComplete = false;
	}
	
	public override void Run (EventManager eventManager)
	{
		base.Run (eventManager);
	}
	
	public override void Initialization ()
	{
		base.Initialization ();
	}
};

public class MoveAnimation : AnimationCommand {

	float[] speed;
	Vector3[] targetPosition;
	string[] clips;

	public MoveAnimation(float[] spd, Vector3[] tpos, string[] n)
	{
		isComplete = false;
		speed = spd;
		targetPosition = tpos;
		clips = n;
	}
	
	public override void Run (EventManager eventManager)
	{
		for (int i=0; i<2; i++)
		{
			float dis = (targetPosition[i] - eventManager.character[i].transform.position).magnitude;
			if (dis < 0.5f)
			{
				if (!eventManager.character[i].IsPlaying (eventManager.character[i].clip.name))
					eventManager.character[i].CrossFade (eventManager.character[i].clip.name);
				
				if (i==1) isComplete = true;
			}
			else
			{
				eventManager.character[i].transform.Translate (Vector3.forward*Time.deltaTime*speed[i]);
				if (!eventManager.character[i].IsPlaying (clips[i])) eventManager.character[i].CrossFade (clips[i]);
			}
		}
	}
	
	public override void Initialization ()
	{
		base.Initialization ();
	}
};

public class RotateAnimation : AnimationCommand {
	
	float angle;
	float speed;
	string clip;

	public RotateAnimation(float a, float s, string cs)
	{
		isComplete = false;
		angle = a;
		speed = s;
		clip = cs;
	}
	
	public override void Run (EventManager eventManager)
	{
		if (eventManager.character[0].transform.eulerAngles.y > angle)
		{
			eventManager.character[0].CrossFade (eventManager.character[0].clip.name);
			isComplete = true;
		}
		else
		{
			eventManager.character[0].transform.Rotate (Vector3.up*speed*Time.deltaTime);
			if (!eventManager.character[0].IsPlaying (clip)) eventManager.character[0].CrossFade (clip);
		}
	}
	
	public override void Initialization ()
	{
		base.Initialization ();
	}
};

