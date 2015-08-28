using UnityEngine;
using System.Collections;

public class AnimationTester : MonoBehaviour
{
	public Sprite[] animationFrames;
	public bool loopAnimation = true;
	public float duration = 1f;

	private SpriteAnimator spriteAnimator;

	void Awake()
	{
		spriteAnimator = transform.GetComponent<SpriteAnimator>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			spriteAnimator.PlayAnimation(this, duration, loopAnimation, animationFrames, true);
		}
	}
}
