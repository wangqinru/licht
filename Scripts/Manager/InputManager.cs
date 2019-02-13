using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	//決定
	public int DecisionButton { get; private set;} 
	//キャンセル
	public int CancelButton { get; private set;}
	public int AttackButton { get; private set;}
	public int SkillButton { get; private set;}
	public int GuardButton { get; private set;}
	public int MenuButton { get; private set;}
	public int[] sbList {get; private set;}
	public int ButtonA { get; private set;}
	public int ButtonB { get; private set;}
	public int ButtonX { get; private set;}
	public int ButtonY { get; private set;}
	public int ButtonL { get; private set;}
	public int ButtonR { get; private set;}
	public int StartButton { get; private set;}
	public int CameraResetButton { get; private set;}

	public float vertical { get; private set;}
	public float horizontal { get; private set;}
	public float cameraV { get; private set;}
	public float cameraH { get; private set;}

	public int StickLR { get; private set;}
	public int StickUD { get; private set;}

	public float DpadX { get; private set;}
	public float DpadY { get; private set;}

	public bool VHflag { get; set;}

	private int counter = 0;
	private int timer = 0;
	public int speed { get; set;}
	// Use this for initialization
	void Start () {

		VHflag = true;
		sbList = new int[4];
	}
	
	// Update is called once per frame
	void Update () {

		if (VHflag)
		{
			vertical = Input.GetAxisRaw("Vertical");
			horizontal = Input.GetAxisRaw("Horizontal");

			cameraV = Input.GetAxisRaw ("CameraV");
			cameraH = Input.GetAxisRaw ("CameraH");

			DpadX = Input.GetAxis ("DpadX");
			DpadY = Input.GetAxis ("DpadY");
		}

		CancelButton = ButtonB = sbList[0] = (Input.GetButton ("ButtonB")) ? ButtonB + 1 : 0;
		DecisionButton = AttackButton = sbList[3] =  ButtonA = (Input.GetButton ("ButtonA")) ? ButtonA + 1 : 0;
		MenuButton = ButtonY = sbList[2] = (Input.GetButton ("ButtonY")) ? ButtonY + 1 : 0;
		ButtonL = SkillButton = (Input.GetButton ("ButtonLB")) ? ButtonL + 1 : 0;
		ButtonR = CameraResetButton = (Input.GetButton ("ButtonRB")) ? ButtonR + 1 : 0;
		GuardButton = ButtonX = sbList[1] = (Input.GetButton ("ButtonX")) ? ButtonX + 1 : 0;

		StartButton = (Input.GetButton ("Start")) ? StartButton + 1 : 0;

		counter = ButtonR > 0 ? counter + 1 : counter;	

		if (Mathf.Abs (horizontal) > Mathf.Abs (vertical) || Mathf.Abs (DpadX) > Mathf.Abs (DpadY))
		{	
			if (horizontal > 0.01f || DpadX > 0.01f)
			{
				if (StickLR < 0) StickLR = 0;
				StickLR ++;
				if (StickLR > 15+speed) StickLR = 0;
			}
			
			if (horizontal < -0.01f || DpadX < -0.01f)
			{
				if (StickLR > 0) StickLR = 0;
				StickLR --;
				if (StickLR < -15-speed) StickLR = 0;
			}
		}

		if (Mathf.Abs (vertical) > Mathf.Abs (horizontal) || Mathf.Abs (DpadY) > Mathf.Abs (DpadX))
		{	
			if (vertical > 0.01f || DpadY > 0.01f)
			{
				if (StickUD > 0) StickUD = 0;
				StickUD --;
				if (StickUD < -15-speed) StickUD = 0;
			}

			if (vertical < -0.01f || DpadY < -0.01f)
			{
				if (StickUD < 0) StickUD = 0;
				StickUD ++;
				if (StickUD > 15+speed) StickUD = 0;
			}
		}

		if ((vertical + horizontal + cameraV + cameraH + ButtonA + ButtonB) == 0)
			timer ++;
		else
			timer = 0;

		if (timer > 3600*5) Application.LoadLevel ("TitleScene");
	}

	public int GetTargetChangeButton ()
	{
		if (counter > 0 && counter < 60 && ButtonR == 0.0f)
		{
			counter = 0;
			return 1;
		}

		if (counter > 60)
		{
			if (ButtonR == 0)
			{
				counter = 0;
				return 3;
			}
			else
				return 2;
		}

		return 0;
	}

	public void ClearInput ()
	{
		//Input.ResetInputAxes ();
		vertical = 0.0f;
		horizontal = 0.0f;
		cameraV = 0.0f;
		cameraH = 0.0f;
		DecisionButton = AttackButton = ButtonA = 0;
		CancelButton = ButtonB = sbList[0] = 0;
	}
}
