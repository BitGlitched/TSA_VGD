using UnityEngine;
using System.Collections;

public class DualCharacterControl : MonoBehaviour
{
	public int activePlayer = 1;

	private bool canSwitch = true;

	public KeyCode switchInput = KeyCode.Q;
	
	public GameObject camera;
	public GameObject player1;
	public GameObject player2;

	private PlayerMovement pm1;
	private PlayerMovement pm2;
	private CameraScript cameraScript;

	// Use this for initialization
	void Awake ()
	{
		if (!camera)
		{
			camera = GameObject.FindGameObjectWithTag("MainCamera");
		}

		cameraScript = camera.GetComponent<CameraScript>();

		if (player1)
		{
			cameraScript.target = player1;
		}
		else if (player2)
		{
			cameraScript.target = player2;
		}

		pm1 = player1.GetComponent<PlayerMovement>();
		pm2 = player2.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (canSwitch)
		{
			if (Input.GetKeyDown(switchInput))
			{
				SwitchActivePlayer();
			}
		}
	}

	void SwitchActivePlayer()
	{
		if (activePlayer == 1)
		{
			activePlayer++;
		}
		else if (activePlayer == 2)
		{
			activePlayer--;
		}

		if (activePlayer == 1)
		{
			cameraScript.target = player1;
			pm1.canControl = true;
			pm2.canControl = false;
		}
		else
		{
			cameraScript.target = player2;
			pm1.canControl = false;
			pm2.canControl = true;
		}

		pm1.ResetSpeed();
		pm2.ResetSpeed();
	}

	void CanSwitch(bool canIt)
	{
		canSwitch = canIt;
	}
}
