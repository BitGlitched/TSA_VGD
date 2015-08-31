using UnityEngine;
using System.Collections;

[RequireComponent(typeof (InputScript))]
public class PlayerMovement : MonoBehaviour
{
	public LayerMask groundLayer;
	public LayerMask wallLayer;

	public bool canMove = true;

	public float walkSpeed = 12f;
	public float speedUpDuration = 0.1f;
	public bool  airControl = false;
	public float airSpeed = 8f;
	public float jumpHeight = 3.5f;
	public float jumpSpeed = 5f;
	public float jumpTime = 1f;
	public float characterMass = 1f;

	private float targetHorizontalSpeed;
	private float horizontalSpeed;
	private float smoothDampVelX;
	public float velocityX;
	public float velocityY;
	private float jumpTimestamp;

	private bool canJump = false;
	private bool jumping = false;
	public bool grounded = false;

	private Vector3 currentScale;

	private InputScript inputScript;
	private Rigidbody2D rigidbody;
	private BoxCollider2D topCollider;
	private CircleCollider2D bottomCollider;

	void Awake()
	{
		inputScript = transform.GetComponent<InputScript>();
		rigidbody = transform.GetComponent<Rigidbody2D>();
		topCollider = transform.GetComponent<BoxCollider2D>();
		bottomCollider = transform.GetComponent<CircleCollider2D>();
	}

	// Update is called once per frame
	void Update ()
	{
		//Ground check section, comes first for frame by frame perect accuracy
		grounded = false;

		if (bottomCollider.IsTouchingLayers(groundLayer))
		{
			float castRadius = bottomCollider.bounds.extents.x;
			float yOffset = (bottomCollider.bounds.size.y - bottomCollider.offset.y) * 0.5f;
			Vector2 castVector = new Vector2 (transform.position.x, transform.position.y - yOffset);
			Collider2D[] colliders = Physics2D.OverlapCircleAll(castVector, castRadius, groundLayer);

			for (int count = 0; count < colliders.Length; count++)
			{
				if (colliders[count].gameObject != gameObject)
				{
					grounded = true;
					canJump = true;
					jumping = false;
					jumpTimestamp = 0;
				}
			}
		}
		else if ((topCollider.IsTouchingLayers(groundLayer))&&(jumping))
		{
			canJump = false;
			jumping = false;
			velocityY = 0;
		}
	}

	void FixedUpdate()
	{
		targetHorizontalSpeed = 0;
		currentScale = transform.localScale;


/*/////////////////////////////////////////////////////////////////////////////////////
///			HORIZONTAL MOVEMENT SECTION												///
/////////////////////////////////////////////////////////////////////////////////////*/

		//Regular movement intention section
		if ((airControl)||(grounded))
		{
			if (inputScript.rightInputPressed)
			{
				targetHorizontalSpeed++;
			}

			if (inputScript.leftInputPressed)
			{
				targetHorizontalSpeed--;
			}

			//Smoothing of horizontal speed
			if (horizontalSpeed != targetHorizontalSpeed)
			{
				horizontalSpeed = Mathf.SmoothDamp(horizontalSpeed, targetHorizontalSpeed, ref smoothDampVelX, speedUpDuration);
			}
		}

		//Actual movement instructions
		if (grounded == true)
		{
			velocityX = horizontalSpeed * walkSpeed;
		}
		else
		{
			velocityX = horizontalSpeed * airSpeed;
		}


		/*/////////////////////////////////////////////////////////////////////////////////////
		///			VERTICAL/JUMPING SECTION												///
		/////////////////////////////////////////////////////////////////////////////////////*/

		//Jumping section
		if ((inputScript.jumpInputPressed)&&(canJump)&&(jumpTimestamp < jumpTime))
		{
			jumping = true;
			jumpTimestamp += Time.fixedDeltaTime;

			velocityY = jumpHeight / jumpTime;
		}

		if ((inputScript.jumpInputUp)||((jumpTimestamp > jumpTime)&&(jumping)))
		{
			canJump = false;
			jumping = false;
		}


		/*/////////////////////////////////////////////////////////////////////////////////////
		///			GRAVITY SECTION												///
		/////////////////////////////////////////////////////////////////////////////////////*/
		if ((grounded)&&(!jumping))
		{
			velocityY = 0;
		}

		velocityY += (-9.8f * characterMass) * Time.fixedDeltaTime;

		velocityY = Mathf.Clamp(velocityY, -9.8f * characterMass, 1000);


		/*/////////////////////////////////////////////////////////////////////////////////////
		///			SPRITE FLIPPING SECTION													///
		/////////////////////////////////////////////////////////////////////////////////////*/

		//Function to flip the sprite
		if ((grounded)&&(horizontalSpeed != 0))
		{
			float absDirectionValue = horizontalSpeed / Mathf.Abs(horizontalSpeed);

			if ((currentScale.x != absDirectionValue))
			{
				Vector3 newScale = currentScale;
				newScale.x = absDirectionValue;
				transform.localScale = newScale;
			}
		}


		/*/////////////////////////////////////////////////////////////////////////////////////
		///			FINAL MOVEMENT SECTION												///
		/////////////////////////////////////////////////////////////////////////////////////*/

		Vector2 currentPosition = new Vector2 (transform.position.x, transform.position.y);
		Vector2 newPosition = new Vector2 (velocityX, velocityY);
		rigidbody.MovePosition (currentPosition + newPosition * Time.fixedDeltaTime);
	}
}
