using UnityEngine;
using System.Collections;

public class BaseCommand {

	public bool isComplete { get; set;}
	protected static bool skip = false;
	protected int count = 0;

	public BaseCommand()
	{
		isComplete = false;
	}
	
	public virtual void Run (EventManager eventManager)
	{
		if (eventManager.inputManager.ButtonA == 1) skip = true;
	}
	
	public virtual void Initialization ()
	{
		count = 0;
		
		isComplete = false;
	}
};

public class CreateTalkWindow : BaseCommand {

	public CreateTalkWindow ()
	{
		isComplete = false;
	}

	public override void Run (EventManager eventManager)
	{
		GameObject tw = MonoBehaviour.Instantiate (Resources.Load (eventManager.windowPath, typeof(GameObject)) as GameObject) as GameObject;

		eventManager.talkWindow = tw.GetComponent<TalkWindow> ();
		isComplete = true;
	}

	public override void Initialization ()
	{

	}
};

public class DeleteTalkWindow : BaseCommand {
	
	public DeleteTalkWindow ()
	{
		isComplete = false;
	}
	
	public override void Run (EventManager eventManager)
	{
		MonoBehaviour.Destroy (eventManager.talkWindow.gameObject);
		isComplete = true;
	}
	
	public override void Initialization ()
	{
		
	}
};

public class ScenarioCommand : BaseCommand {

	protected static int currentLine = 1;
	protected string text;
	protected string setText;

	public ScenarioCommand()
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

public class NameSettingCommand : ScenarioCommand {
	
	public string name { get; private set;}
	
	public NameSettingCommand (string n)
	{
		text = n;
		
		name = string.Copy (text);
		
		currentLine = 0;
	}
	
	public override void Run (EventManager eventManager)
	{
		eventManager.talkWindow.guiTexts[currentLine].text = name;
		
		isComplete = true;
		
		skip = false;
	}
	
	public override void Initialization ()
	{
		base.Initialization ();
	}
	
};
//顔command
public class FaceSettingCommand : ScenarioCommand {
	
	public FaceSettingCommand (string f)
	{
		text = f;
		
		currentLine = 0;
	}
	
	public override void Run (EventManager eventManager)
	{
		eventManager.talkWindow.faceTexture.texture = Resources.Load ("UI/face/"+text, typeof(Texture)) as Texture;
		
		isComplete = true;
	}
	
	public override void Initialization ()
	{
		base.Initialization ();
	}
};
//TEXTcommand
public class TextSettingCommand : ScenarioCommand {
	
	public TextSettingCommand (string t)
	{
		text = t; 
		setText = string.Copy (text);
	}
	
	public override void Run (EventManager eventManager)
	{
		base.Run (eventManager);
		
		if (count == 0)
		{
			setText = setText.Insert (0, eventManager.talkWindow.guiTexts[currentLine].text);
			count += eventManager.talkWindow.guiTexts[currentLine].text.Length*2;
		}
		
		if (skip)
		{
			eventManager.talkWindow.guiTexts[currentLine].text = setText.Substring (0, setText.Length);
			
			isComplete = true;
		}
		else
		{
			if (count/2 < setText.Length)
				eventManager.talkWindow.guiTexts[currentLine].text = setText.Substring (0, count/2+1);
			
			count ++;
			
			if (count/2 == setText.Length) isComplete = true;
		}
	}
	
	public override void Initialization ()
	{
		setText = text;
		base.Initialization ();
	}
};
//赤色command
public class ColorRedTextSettingCommand : ScenarioCommand {
	
	string colorstart = "<color=red>";
	string colorend = "</color>";
	
	int number = 0;
	
	public ColorRedTextSettingCommand (string t)
	{
		text = t;
		
		setText = string.Copy (text);
		
		setText = setText.Insert (0, colorstart);
		
		for (;;)
		{
			int end = setText.LastIndexOf (">");
			
			if (end != -1) setText = setText.Insert (end+2, colorend);
			
			end = setText.LastIndexOf (">");
			
			if (end+1 < setText.Length) setText = setText.Insert (end+1, colorstart);
			else break;
		}
	}
	
	public override void Run (EventManager eventManager)
	{
		base.Run (eventManager);
		
		if (count == 0)
		{
			setText = setText.Insert (0, eventManager.talkWindow.guiTexts[currentLine].text);
			
			number = eventManager.talkWindow.guiTexts[currentLine].text.Length;
		}
		
		if (skip)
		{
			eventManager.talkWindow.guiTexts[currentLine].text = setText.Substring (0, setText.Length);
			
			isComplete = true;
		}
		else
		{
			if (count%2 == 0)
			{
				number = setText.IndexOf ("</", number+1);
				int n = setText.IndexOf (">", number+1);
				eventManager.talkWindow.guiTexts[currentLine].text = setText.Substring (0, n+1);
				
				if (n+1 == setText.Length) isComplete = true;
			}
			
			count ++;
		}
	}
	
	public override void Initialization ()
	{
		setText = string.Copy (text);
		
		setText = setText.Insert (0, colorstart);
		
		for (;;)
		{
			int end = setText.LastIndexOf (">");
			
			if (end != -1) setText = setText.Insert (end+2, colorend);
			
			end = setText.LastIndexOf (">");
			
			if (end+1 < setText.Length) setText = setText.Insert (end+1, colorstart);
			else break;
		}
		
		number = 0;
		
		base.Initialization ();
	}
};
//黄色command
public class ColorYellowTextSettingCommand : ScenarioCommand {
	
	string colorstart = "<color=yellow>";
	string colorend = "</color>";
	
	int number = 0;
	
	public ColorYellowTextSettingCommand (string t)
	{
		text = t;
		
		setText = string.Copy (text);
		
		setText = setText.Insert (0, colorstart);
		
		for (;;)
		{
			int end = setText.LastIndexOf (">");
			
			if (end != -1) setText = setText.Insert (end+2, colorend);
			
			end = setText.LastIndexOf (">");
			
			if (end+1 < setText.Length) setText = setText.Insert (end+1, colorstart);
			else break;
		}
	}
	
	public override void Run (EventManager eventManager)
	{
		base.Run (eventManager);
		
		if (count == 0)
		{
			setText = setText.Insert (0, eventManager.talkWindow.guiTexts[currentLine].text);
			
			number = eventManager.talkWindow.guiTexts[currentLine].text.Length;
		}
		
		if (skip)
		{
			eventManager.talkWindow.guiTexts[currentLine].text = setText.Substring (0, setText.Length);
			
			isComplete = true;
		}
		else
		{
			if (count%2 == 0)
			{
				number = setText.IndexOf ("</", number+1);
				int n = setText.IndexOf (">", number+1);
				eventManager.talkWindow.guiTexts[currentLine].text = setText.Substring (0, n+1);
				
				if (n+1 == setText.Length) isComplete = true;
			}
			
			count ++;
		}
	}
	
	public override void Initialization ()
	{
		setText = string.Copy (text);
		
		setText = setText.Insert (0, colorstart);
		
		for (;;)
		{
			int end = setText.LastIndexOf (">");
			
			if (end != -1) setText = setText.Insert (end+2, colorend);
			
			end = setText.LastIndexOf (">");
			
			if (end+1 < setText.Length) setText = setText.Insert (end+1, colorstart);
			else break;
		}
		
		number = 0;
		
		base.Initialization ();
	}
};
//青色command
public class ColorBlueTextSettingCommand : ScenarioCommand {
	
	string colorstart = "<color=blue>";
	string colorend = "</color>";
	
	int number = 0;
	
	public ColorBlueTextSettingCommand (string t)
	{
		text = t;
		
		setText = string.Copy (text);
		
		setText = setText.Insert (0, colorstart);
		
		for (;;)
		{
			int end = setText.LastIndexOf (">");
			
			if (end != -1) setText = setText.Insert (end+2, colorend);
			
			end = setText.LastIndexOf (">");
			
			if (end+1 < setText.Length) setText = setText.Insert (end+1, colorstart);
			else break;
		}
	}
	
	public override void Run (EventManager eventManager)
	{
		base.Run (eventManager);
		
		if (count == 0)
		{
			setText = setText.Insert (0, eventManager.talkWindow.guiTexts[currentLine].text);
			
			number = eventManager.talkWindow.guiTexts[currentLine].text.Length;
		}
		
		if (skip)
		{
			eventManager.talkWindow.guiTexts[currentLine].text = setText.Substring (0, setText.Length);
			
			isComplete = true;
		}
		else
		{
			if (count%2 == 0)
			{
				number = setText.IndexOf ("</", number+1);
				int n = setText.IndexOf (">", number+1);
				eventManager.talkWindow.guiTexts[currentLine].text = setText.Substring (0, n+1);
				
				if (n+1 == setText.Length) isComplete = true;
			}
			
			count ++;
		}
	}
	
	public override void Initialization ()
	{
		setText = string.Copy (text);
		
		setText = setText.Insert (0, colorstart);
		
		for (;;)
		{
			int end = setText.LastIndexOf (">");
			
			if (end != -1) setText = setText.Insert (end+2, colorend);
			
			end = setText.LastIndexOf (">");
			
			if (end+1 < setText.Length) setText = setText.Insert (end+1, colorstart);
			else break;
		}
		
		number = 0;
		
		base.Initialization ();
	}
};
//orange色command
public class ColorOrangeTextSettingCommand : ScenarioCommand {
	
	string colorstart = "<color=orange>";
	string colorend = "</color>";
	
	int number = 0;
	
	public ColorOrangeTextSettingCommand (string t)
	{
		text = t;
		
		setText = string.Copy (text);
		
		setText = setText.Insert (0, colorstart);
		
		for (;;)
		{
			int end = setText.LastIndexOf (">");
			
			if (end != -1) setText = setText.Insert (end+2, colorend);
			
			end = setText.LastIndexOf (">");
			
			if (end+1 < setText.Length) setText = setText.Insert (end+1, colorstart);
			else break;
		}
	}
	
	public override void Run (EventManager eventManager)
	{
		base.Run (eventManager);
		
		if (count == 0)
		{
			setText = setText.Insert (0, eventManager.talkWindow.guiTexts[currentLine].text);
			
			number = eventManager.talkWindow.guiTexts[currentLine].text.Length;
		}
		
		if (skip)
		{
			eventManager.talkWindow.guiTexts[currentLine].text = setText.Substring (0, setText.Length);
			
			isComplete = true;
		}
		else
		{
			if (count%2 == 0)
			{
				number = setText.IndexOf ("</", number+1);
				int n = setText.IndexOf (">", number+1);
				eventManager.talkWindow.guiTexts[currentLine].text = setText.Substring (0, n+1);
				
				if (n+1 == setText.Length) isComplete = true;
			}
			
			count ++;
		}
	}
	
	public override void Initialization ()
	{
		setText = string.Copy (text);
		
		setText = setText.Insert (0, colorstart);
		
		for (;;)
		{
			int end = setText.LastIndexOf (">");
			
			if (end != -1) setText = setText.Insert (end+2, colorend);
			
			end = setText.LastIndexOf (">");
			
			if (end+1 < setText.Length) setText = setText.Insert (end+1, colorstart);
			else break;
		}
		
		number = 0;
		
		base.Initialization ();
	}
};

//BOLDcommand
public class BoldTextSettingCommand : ScenarioCommand {
	
	string boldstart = "<b>";
	string boldend = "</b>";
	
	int number = 0;
	
	public BoldTextSettingCommand (string t)
	{
		text = t;
		
		setText = string.Copy (text);;
		
		setText = setText.Insert (0, boldstart);
		
		for (;;)
		{
			int end = setText.LastIndexOf (">");
			
			if (end != -1) setText = setText.Insert (end+2, boldend);
			
			end = setText.LastIndexOf (">");
			
			if (end+1 < setText.Length) setText = setText.Insert (end+1, boldstart);
			else break;
		}
	}
	
	public override void Run (EventManager eventManager)
	{
		base.Run (eventManager);
		
		if (count == 0)
		{
			setText = setText.Insert (0, eventManager.talkWindow.guiTexts[currentLine].text);
			number = eventManager.talkWindow.guiTexts[currentLine].text.Length;
		}
		
		if (skip)
		{
			eventManager.talkWindow.guiTexts[currentLine].text = setText.Substring (0, setText.Length);
			
			isComplete = true;
		}
		else
		{
			if (count%2 == 0)
			{
				number = setText.IndexOf ("</", number+1);
				int n = setText.IndexOf (">", number+1);
				eventManager.talkWindow.guiTexts[currentLine].text = setText.Substring (0, n+1);
				if (n+1 == setText.Length) isComplete = true;
			}
			
			count ++;
		}
	}
	
	public override void Initialization ()
	{
		setText = string.Copy (text);
		
		setText = setText.Insert (0, boldstart);
		
		for (;;)
		{
			int end = setText.LastIndexOf (">");
			
			if (end != -1) setText = setText.Insert (end+2, boldend);
			
			end = setText.LastIndexOf (">");
			
			if (end+1 < setText.Length) setText = setText.Insert (end+1, boldstart);
			else break;
		}
		
		number = 0;
		
		base.Initialization ();
	}
};
//改行command
public class IndentionSettingCommand : ScenarioCommand {
	
	public IndentionSettingCommand (string t)
	{
	}
	
	public override void Run (EventManager eventManager)
	{
		currentLine ++;
		
		isComplete = true;
	}
	
	public override void Initialization ()
	{
		base.Initialization ();
	}
};
//ボタン待ちcommand
public class InputWaitCommand : ScenarioCommand {
	
	public InputWaitCommand ()
	{
	}
	
	public override void Run (EventManager eventManager)
	{
		if (!eventManager.talkWindow.proceed.enabled) eventManager.talkWindow.proceed.enabled = true;

		if (eventManager.inputManager.ButtonA == 1)
		{
			for (int i=1; i<eventManager.talkWindow.guiTexts.Length; i++)
				eventManager.talkWindow.guiTexts[i].text = "";
			
			currentLine = 0;
			skip = false;
			eventManager.talkWindow.proceed.enabled = false;

			isComplete = true;
		}
	}
	
	public override void Initialization ()
	{
		base.Initialization ();
	}
};

