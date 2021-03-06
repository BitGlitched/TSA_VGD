﻿using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour
{
	public bool hasIdleAnimation = false;

	public Sprite[] idleAnimationFrames;

	public float idleAnimationDuration = 1.25f;

	public Sprite[] currentAnimationFrames;

	public float animationDuration = 0.5f;
	public float frameChangeInterval;
	public float timestamp = 0;

	public bool animationLengthTiming = false;
	public bool loopAnimation = false;
	public bool hangAnimation = false;
	public bool animating;

	public int frame;

	public Sprite defaultSprite;
	private SpriteRenderer spriteRenderer;
	public GameObject animationMessageOrigin;

	// Use this for initialization
	void Awake()
	{
		spriteRenderer = transform.GetComponent<SpriteRenderer>();
		defaultSprite = spriteRenderer.sprite;
	}

	public void PlayAnimation(Component origin, float duration, bool loop, Sprite[] animationFrames, bool hang, bool overideAnimation)
	{
		if (animating == false)
		{
			animating = true;
			defaultSprite = spriteRenderer.sprite;
			animationMessageOrigin = origin.gameObject;
			animationDuration = duration;
			loopAnimation = loop;
			hangAnimation = hang;
			currentAnimationFrames = animationFrames;
			frame = 0;
		}
		else if (overideAnimation)
		{
			//Finish animation before we set the animation origin object to prevent it from not recieving the finished mesage
			AnimationFinish();
			animating = true;
			animationMessageOrigin = origin.gameObject;
			animationDuration = duration;
			currentAnimationFrames = animationFrames;

		}
		else
		{
			Debug.LogWarning ("Something was already animating the sprite! The component that tried to animate was: " + origin + "on the object" + origin.gameObject);
		}
	}

	public void StopAnimation()
	{
		AnimationFinish();
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		//Checks if we are currently animating the object
		if (animating)
		{
			if (currentAnimationFrames.Length <= 0)
			{
				Debug.LogError("There are no frames for the animation!", this.GetComponent<SpriteAnimator>());
			}
			else
			{
				AnimationTiming();
			}
		}
		else if (hasIdleAnimation)
		{
			if (idleAnimationFrames.Length <= 0)
			{
				Debug.LogError("There are no frames for the animation!", this.GetComponent<SpriteAnimator>());
			}
			else
			{
				currentAnimationFrames = idleAnimationFrames;
				animationDuration = idleAnimationDuration;
				AnimationTiming();
			}
		}
	}

	void AnimationTiming()
	{
		frameChangeInterval = animationDuration / currentAnimationFrames.Length;

		if (timestamp >= frameChangeInterval)
		{
			timestamp = 0;
			ChangeFrame();
		}
		else
		{
			timestamp += Time.fixedDeltaTime;
		}
	}

	void ChangeFrame()
	{
		timestamp = 0;

		if (frame >= currentAnimationFrames.Length - 1 && !hangAnimation)
		{
			frame = 0;
			spriteRenderer.sprite = currentAnimationFrames[frame];

			if ((loopAnimation == false)&&(hangAnimation == false))
			{
				AnimationFinish();
			}
		}
		else if (!hangAnimation)
		{
			frame++;
			spriteRenderer.sprite = currentAnimationFrames[frame];
		}
		else if (!(frame >= currentAnimationFrames.Length - 1))
		{
			frame++;
			spriteRenderer.sprite = currentAnimationFrames[frame];
			
		}
	}

	void AnimationFinish()
	{
		frame = 0;

		spriteRenderer.sprite = defaultSprite;
		loopAnimation = false;
		hangAnimation = false;
		animating = false;

		if (animationMessageOrigin != null)
		{
			animationMessageOrigin.SendMessage("AnimationFinished", SendMessageOptions.DontRequireReceiver);
		}
	}
}