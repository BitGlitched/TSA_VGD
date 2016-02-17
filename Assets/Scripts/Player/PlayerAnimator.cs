using UnityEngine;
using System.Collections;

[RequireComponent(typeof (PlayerMovement))]
public class PlayerAnimator : MonoBehaviour
{
	public bool animationPlaying = false;

	public float jumpAnimationTime = 1;

	public Sprite[] walkCycleFrames;
	public Sprite[] runCycleFrames;
	public Sprite[] jumpFrames;
	public Sprite[] wallJumpFrames;

	private PlayerMovement pm;
	private PlayerGroundCheck gc;
	private SpriteAnimator spriteAnimator;

	// Use this for initialization
	void Awake ()
	{
		pm = this.gameObject.GetComponent<PlayerMovement>();
		gc = this.gameObject.GetComponent<PlayerGroundCheck>();
		spriteAnimator = this.gameObject.GetComponent<SpriteAnimator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!pm.jumping)
		{
			if (pm.horizontalSpeed != 0)
			{
				if (pm.running)
				{
					if (spriteAnimator.currentAnimationFrames == walkCycleFrames)
					{
						spriteAnimator.StopAnimation();
					}
					spriteAnimator.PlayAnimation(this, 1 / pm.runSpeed, true, runCycleFrames, false, false);
					animationPlaying = true;
				}
				else
				{
					if (spriteAnimator.currentAnimationFrames == runCycleFrames)
					{
						spriteAnimator.StopAnimation();
					}
					spriteAnimator.PlayAnimation(this, 1 / pm.walkSpeed, true, walkCycleFrames, false, false);
					animationPlaying = true;
				}
			}
			else if ((animationPlaying)&&(gc.grounded))
			{
				spriteAnimator.StopAnimation();
				animationPlaying = false;
				print("Stoping animation via walk stop");
			}
		}
		else if (gc.grounded)
		{
			animationPlaying = true;
			spriteAnimator.PlayAnimation(this, jumpAnimationTime, true, jumpFrames, true, true);
		}
	}
}
