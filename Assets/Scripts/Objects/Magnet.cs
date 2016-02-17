using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour
{
	public float timeToAttract = 0.5f;
	public float dist;
	public float animationDuration = 1;

	public bool magnetIsOn = true;

	private Vector3 velocity;

	private bool robotStuck = false;

	private bool hasSwitched = false;

	public Sprite[] stuckFrames;
	public GameObject robotPlayer;
	public int GirlPlayerNumber = 1;
	private SpriteAnimator spriteAnimator;
	
	// Update is called once per frame
	void Update ()
	{
		if (robotStuck && magnetIsOn)
		{
			dist = Vector2.Distance(robotPlayer.transform.position, transform.position);

			if (dist > 0.43f || dist < -0.43f)
			{
				robotPlayer.transform.position = Vector3.SmoothDamp(robotPlayer.transform.position, transform.position, ref velocity, timeToAttract);
			}
			else
			{
				if (!hasSwitched)
				{
					robotPlayer.transform.parent.SendMessage("SwitchActivePlayer");
					hasSwitched = true;
				}
			}

			if (!spriteAnimator.animating)
			{
				spriteAnimator.PlayAnimation(this, animationDuration, true, stuckFrames, false, true);
			}
		}
		else if (!magnetIsOn && robotStuck)
		{
			ReleaseRobot();
		}
	}

	void ObjectEnter(Collider2D collision)
	{
		if (collision.gameObject == robotPlayer && magnetIsOn)
		{
			robotPlayer.SendMessage("IsStuck", true);
			spriteAnimator = robotPlayer.GetComponent<SpriteAnimator>();

			robotStuck = true;
		}
	}

	void ReleaseRobot()
	{
		robotStuck = false;
		robotPlayer.SendMessage("IsStuck", false);
		hasSwitched = false;
	}

	public void MagnetToggle(bool OnOff)
	{
		magnetIsOn = OnOff;
	}

	public void TurnMagnetOff()
	{
		magnetIsOn = false;
	}
}
