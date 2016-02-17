using UnityEngine;
using System.Collections;

public class InputScript : MonoBehaviour
{
	public bool getInput = true;
	public bool getControllerInput = true;

	public KeyCode leftInput = KeyCode.A;
	public KeyCode rightInput = KeyCode.D;
	public KeyCode upInput = KeyCode.W;
	public KeyCode downInput = KeyCode.S;
	public KeyCode leftInputAlt = KeyCode.LeftArrow;
	public KeyCode rightInputAlt = KeyCode.RightArrow;
	public KeyCode upInputAlt = KeyCode.UpArrow;
	public KeyCode downInputAlt = KeyCode.DownArrow;
	public KeyCode jumpInput = KeyCode.Space;
	public KeyCode runInput = KeyCode.LeftShift;

	public bool leftInputPressed;
	public bool rightInputPressed;
	public bool upInputPressed;
	public bool downInputPressed;
	public bool jumpInputPressed;
	public bool runInputPressed;

	public bool leftInputDown;
	public bool rightInputDown;
	public bool upInputDown;
	public bool downInputDown;
	public bool jumpInputDown;
	public bool runInputDown;

	public bool leftInputUp;
	public bool rightInputUp;
	public bool upInputUp;
	public bool downInputUp;
	public bool jumpInputUp;
	public bool runInputUp;

	// Update is called once per frame
	void Update ()
	{
		leftInputPressed = false;
		rightInputPressed = false;
		upInputPressed = false;
		downInputPressed = false;
		jumpInputPressed = false;
		runInputPressed = false;
		leftInputDown = false;
		rightInputDown = false;
		upInputDown = false;
		downInputDown = false;
		jumpInputDown = false;
		runInputDown = false;
		leftInputUp = false;
		rightInputUp = false;
		upInputUp = false;
		downInputUp = false;
		jumpInputUp = false;
		runInputUp = false;

		if (getInput)
		{
			//Left input section
			if (Input.GetKey(leftInput)||Input.GetKey(leftInputAlt))
			{
				leftInputPressed = true;
			}
			if (Input.GetKeyDown(leftInput))
			{
				leftInputDown = true;
			}
			else if (Input.GetKeyUp(leftInput))
			{
				leftInputUp = true;
			}

			//Right input section
			if (Input.GetKey(rightInput)||Input.GetKey(rightInputAlt))
			{
				rightInputPressed = true;
			}
			if (Input.GetKeyDown(rightInput))
			{
				rightInputDown = true;
			}
			else if (Input.GetKeyUp(leftInput))
			{
				rightInputUp = true;
			}

			//Up input section
			if (Input.GetKey(upInput)||Input.GetKey(upInputAlt))
			{
				upInputPressed = true;
			}
			if (Input.GetKeyDown(upInput))
			{
				upInputDown = true;
			}
			else if (Input.GetKeyUp(upInput))
			{
				upInputUp = true;
			}

			//Down input section
			if (Input.GetKey(downInput)||Input.GetKey(downInputAlt))
			{
				downInputPressed = true;
			}
			if (Input.GetKeyDown(downInput))
			{
				downInputDown = true;
			}
			else if (Input.GetKeyUp(downInput))
			{
				downInputUp = true;
			}

			//Jump input section
			if (Input.GetKey(jumpInput))
			{
				jumpInputPressed = true;
			}
			if (Input.GetKeyDown(jumpInput))
			{
				jumpInputDown = true;
			}
			else if (Input.GetKeyUp(jumpInput))
			{
				jumpInputUp = true;
			}

			//Run input section
			if (Input.GetKey(runInput))
			{
				runInputPressed = true;
			}
			if (Input.GetKeyDown(runInput))
			{
				runInputDown = true;
			}
			else if (Input.GetKeyUp(runInput))
			{
				runInputUp = true;
			}
		}
	}
}
